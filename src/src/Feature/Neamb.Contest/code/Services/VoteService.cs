using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Contest.Interfaces;
using Neambc.Neamb.Feature.Contest.Model;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.Membership.Managers;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;

namespace Neambc.Neamb.Feature.Contest.Services {
	[Service(typeof(IVoteService))]
	public class VoteService : IVoteService {
		#region Properties
		private readonly ICacheManager _cacheManager;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IFallbackProvider _fallbackProvider;
		private readonly IAmazonS3Repository _amazonS3Repository;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly IBase64Service _base64Service;
		private readonly IOracleDatabase _oracleManager;
		#endregion

		#region Ctor
		public VoteService(ICacheManager cacheManager, ISessionAuthenticationManager sessionAuthenticationManager, IFallbackProvider fallbackProvider, IAmazonS3Repository amazonS3Repository, IGlobalConfigurationManager globalConfigurationManager, IBase64Service base64Service, IOracleDatabase oracleManager) {
			_cacheManager = cacheManager;
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_fallbackProvider = fallbackProvider;
			_amazonS3Repository = amazonS3Repository;
			_globalConfigurationManager = globalConfigurationManager;
			_base64Service = base64Service;
			_oracleManager = oracleManager;
		}
		#endregion

		#region Public Methods

		public void PerformIntegritySync(string contestId) {
			if (string.IsNullOrEmpty(contestId)) {
				throw new Exception("Invalid contestId");
			}

			var backedVotes = _fallbackProvider.GetContestBackup(contestId);
			var redisVotes = GetVotes(contestId);

			// Restore Redis votes if votes are corrupted.
			if (IntegrityCheck(backedVotes, redisVotes)) {
				// Creates a backup.
				_fallbackProvider.SetContestBackup(redisVotes);
				Log.Debug($"Backup performed from Redis.", this);
			} else {
				Sync(backedVotes, redisVotes);
			}
		}
		public void AddUserVote(UserVote userVote) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			if (string.IsNullOrEmpty(accountMembership?.Profile?.Webuserid)) {
				throw new AccessDeniedException();
			}
			var existingVote = GetUserVote(userVote.ContestId);
			//Checks if user has already voted.
			if (existingVote != null) {
				return;
			}

			// Checks if submission exists in S3.
			BaseRequestS3 baseRequest = new BaseRequestS3
			{
				BucketName = _globalConfigurationManager.BucketNameContestImages,
				Key = $"{GetSubmissionPath(userVote.ContestId)}/{GetSubmissionFileName(userVote.SubmissionId)}"
			};

			var submission = _amazonS3Repository.GetObjectS3<ContestFileItem>(baseRequest);
			
			if (submission == null) {
				return;
			}

			AddVote(new Vote() {
				ContestId = userVote.ContestId,
				SubmissionId = userVote.SubmissionId,
				Total = 1
			});

			//Puts user's voting ability in cooldown.
			var userVoteKey = GetUserVoteKey(userVote.ContestId, accountMembership.Profile.Webuserid);
			_cacheManager.StoreInCache(userVoteKey, userVote, GetUserVoteExpiration());

			// Logs vote in Oracle.
			ExecuteContestLoggingProcess(GetContestVoteCode(userVote.ContestId), accountMembership?.Mdsid);
		}

		/// <summary>
		/// Gets the current user vote.
		/// </summary>
		/// <param name="contestId"></param>
		/// <returns>Null if no vote was submitted.</returns>
		public UserVote GetUserVote(string contestId) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			if (string.IsNullOrEmpty(accountMembership?.Profile?.Webuserid)) {
				throw new AccessDeniedException();
			}
			var key = GetUserVoteKey(contestId, accountMembership.Profile.Webuserid);
			return _cacheManager.RetrieveFromCache<UserVote>(key);
		}

		public IList<Vote> GetVotes(string contestId) {
			var key = GetVotesKey(contestId);
			var result = _cacheManager.RetrieveFromCache<IList<Vote>>(key);

			// Initializes a new voting List if there is not a current one.
			result = result ?? new List<Vote>();
			return result;
		}
		public Vote GetVote(string contestId, string submissionId) {
			var votes = GetVotes(contestId);
			//Initializes an Empty vote if null.
			return votes.FirstOrDefault(x => x.SubmissionId.Equals(submissionId, StringComparison.InvariantCultureIgnoreCase)) ?? new Vote() {
				ContestId = contestId,
				SubmissionId = submissionId,
				Total = 0
			};
		}

		public IEnumerable<Submission> GetSubmissions(string contestId, IEnumerable<string> allowedTypes) {
			var cacheKey = GetSubmssionKey(contestId);
			var result = _cacheManager.RetrieveFromCache<IEnumerable<Submission>>(cacheKey);
			if (result == null) {
				FilesS3 filesS3 = new FilesS3 {
					BucketName = _globalConfigurationManager.BucketNameContestImages,
					Key = GetVotePath(contestId),
					TypeFilter = S3ObjectTypeFilter.File
				};
				var submissionFiles =_amazonS3Repository.GetFiles(filesS3)
						.Where(x => allowedTypes.Any(y => y.Equals(x.Extension, StringComparison.InvariantCultureIgnoreCase)));

				result = submissionFiles.Select(x => new Submission() {
					ContestId = contestId,
					Id = x.Name,
					ImageSrc = _base64Service.EncodeImage(x.Content),
					Metadata = GetFile(contestId, $"{x.Name}.{Configuration.S3SubmissionExtension}")
				})
					.Where(x => x.Metadata != null)
					.Where(x => x.ImageSrc != null).ToList();

                foreach (var submission in result) {
                    submission.Metadata.FileName = FirstCharToUpper(submission.Metadata.FileName);
                }

				if (result.Any()) {
					_cacheManager.StoreInCache(cacheKey, result,
						DateTime.Now.AddHours(Configuration.SubmissionsCacheDuration));
				}
			}
			return result;
		}

		private ContestFileItem GetFile(string contestId, string submissionId) {
			BaseRequestS3 baseRequest = new BaseRequestS3
			{
				BucketName = _globalConfigurationManager.BucketNameContestImages,
				Key = $"{GetVotePath(contestId)}/{submissionId}"
			};

			return _amazonS3Repository.GetObjectS3<ContestFileItem>(baseRequest);
		}
		/// <summary>
		/// Insert the submission/vote data in Oracle (SP  MDS_UTIL.ORDER_FULFILL@MBCDB)
		/// </summary>
		/// <param name="itemcode">Submission or vote code</param>
		/// <param name="mdsid">Mdsid</param>
		/// <returns>Execution result true(executed) otherwise false</returns>
		public bool ExecuteContestLoggingProcess(string itemcode, string mdsid) {
			try {
				if (!string.IsNullOrEmpty(itemcode)) {
					var cellCode = _sessionAuthenticationManager.GetCellCode();
					Log.Debug($"Starting logging contest submission mdsid: {mdsid}, itemcode: {itemcode}, cellCode:{cellCode}", this);
					var result = _oracleManager.SelectOrderFulfill(
						Convert.ToInt32(mdsid),
						itemcode,
						cellCode,
						"WEB-MB"
					);
					Log.Debug($"Ending logging contest submission mdsid: {mdsid}, itemcode: {itemcode}, cellCode:{cellCode}", this);
					return result;
				}
				return true;
			} catch (Exception e) {
				Log.Error($"Error while calling ExecuteContestLoggingProcess for itemcode: {itemcode} with mdsid:{mdsid}", e, this);
				return false;
			}
		}

		#endregion

		#region Private Methods
		private void AddVote(Vote vote) {
			var votes = GetVotes(vote.ContestId);
			votes.Add(vote);
			SaveVotes(votes);
		}
		private void SaveVotes(IList<Vote> votes) {
			var contestId = votes.Select(x => x.ContestId).FirstOrDefault();
			if (string.IsNullOrEmpty(contestId)) {
				Log.Warn($"Invalid contestId with the value of {contestId}. No votes were saved.", this);
			} else {
				var key = GetVotesKey(contestId);
				votes = ConsolidateVotes(votes);
				_cacheManager.StoreInCache(key, votes);
			}
		}
		private string GetUserVoteKey(string contestId, string webUserId) {
			return $"{Configuration.ContestKeyGroup}:{contestId}:{Configuration.UserVoteKeyGroup}:{webUserId}";
		}
		private string GetVotesKey(string contestId) {
			return $"{Configuration.ContestKeyGroup}:{contestId}:{Configuration.VoteKeyGroup}";
		}
		private string GetSubmssionKey(string contestId) {
			return $"{Configuration.ContestKeyGroup}:{contestId}:{Configuration.SubmissionGroup}";
		}
		private DateTime GetUserVoteExpiration() {
			return DateTime.Now.AddHours(Configuration.UserVoteCooldown);
		}
		private IList<Vote> ConsolidateVotes(IList<Vote> votes) {
			return votes.GroupBy(x => x.SubmissionId)
				.Select(x => new Vote() {
					ContestId = x.First().ContestId,
					SubmissionId = x.Key,
					Total = x.Sum(y => y.Total)
				}).ToList();
		}

		/// <summary>
		/// Checks if Redis votes are not corrupted.
		/// </summary>
		/// <param name="backedVotes"></param>
		/// <param name="redisVotes"></param>
		/// <returns>False if votes are corrupted.</returns>
		private bool IntegrityCheck(IList<Vote> backedVotes, IList<Vote> redisVotes) {
			if (backedVotes == null || !backedVotes.Any()) {
				Log.Warn($"Cannot check vote integrity from an empty backup.", this);
				return false;
			}

			// Restore Redis if backup has more registers.
			if (backedVotes.Count > redisVotes.Count) {
				Log.Warn($"Vote corruption on contestId:{backedVotes?.FirstOrDefault()?.ContestId}. Backup has more registers.", this);
				return false;
			}

			foreach (var backedVote in backedVotes) {
				var corruptedVotes = redisVotes.Where(x => x.SubmissionId.Equals(backedVote.SubmissionId, StringComparison.InvariantCultureIgnoreCase))
					.Where(x => backedVote.Total > x.Total);

				// Restore Redis if a single submission from backup has more votes.
				if (corruptedVotes.Any()) {
					Log.Warn($"Vote corruption on contestId:{backedVotes?.FirstOrDefault()?.ContestId}. Corrupted votes:{JsonConvert.SerializeObject(corruptedVotes)}", this);
					return false;
				}
			}

			Log.Debug($"Vote integrity passed for contest:{backedVotes?.FirstOrDefault()?.ContestId}", this);
			// If this code is reached means that Redis votes are not corrupted.
			return true;
		}
		private void Sync(IList<Vote> backedVotes, IList<Vote> redisVotes) {
			Log.Warn("Running vote Synchronization", this);
			if (backedVotes == null || !backedVotes.Any()) {
				// Creates a backup.
				_fallbackProvider.SetContestBackup(redisVotes);
				Log.Debug($"Backup performed from Redis.", this);
				return;
			}

			// Restores Redis with backup.
			_cacheManager.StoreInCache(GetVotesKey(backedVotes?.FirstOrDefault()?.ContestId), backedVotes);
			Log.Warn($"Redis votes overwritten:{JsonConvert.SerializeObject(redisVotes)}", this);
		}
		private string GetSubmissionPath(string contestId) {
			return $"{contestId}/{Configuration.S3VoteFolder}";
		}

		private string GetSubmissionFileName(string submissionId) {
			return $"{submissionId}.{Configuration.S3SubmissionExtension}";
		}
		private string GetVotePath(string contestId) {
			return $"{contestId}/{Configuration.S3VoteFolder}";
		}
		public string GetContestVoteCode(string contestId) {
			var contestFolder = Sitecore.Context.Database.GetItem(new ID(contestId));
			var contestPage = contestFolder.Axes.GetDescendants()
				.FirstOrDefault(x => x.TemplateID == Templates.ConstestVote.ID);
			return contestPage?.Fields[Templates.ConstestVote.Fields.ContestVoteCode].Value;
		}
        private static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": return string.Empty;
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
        #endregion
    }
}