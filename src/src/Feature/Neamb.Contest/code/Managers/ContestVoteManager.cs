using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Contest.Enums;
using Neambc.Neamb.Feature.Contest.Interfaces;
using Neambc.Neamb.Feature.Contest.Model;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Contest.Managers {
	[Service(typeof(IContestVoteManager))]
	public class ContestVoteManager : IContestVoteManager {
		private readonly IVoteService _voteService;
		private readonly IContestSubmissionService _contestSubmissionService;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly ISessionManager _sessionManager;
        public ContestVoteManager(IVoteService voteService, IContestSubmissionService contestSubmissionService, ISessionAuthenticationManager sessionAuthenticationManager,
            ISessionManager sessionManager
        ) {
			_voteService = voteService;
			_contestSubmissionService = contestSubmissionService;
			_sessionAuthenticationManager = sessionAuthenticationManager;
            _sessionManager = sessionManager;
        }

		public ContestVote GetContestModel(Rendering rendering, int page) {
            _sessionManager.StoreInSession("returnUrl", LinkManager.GetItemUrl(Sitecore.Context.Item));
            var contestId = PageContext.Current.Item.ParentID.ToString();
			var result = new ContestVote(rendering) {
				ContestId = contestId
			};
			var status = GetStatus(result.Datasource);
            result.Status = status;
            var pageSize = GetPageSize(result.Datasource);

            switch (status) {
				case ContestStatus.Active:
					result.SocialShare = GetSocialShare();
					result.SubmissionVotes = GetSubmissionVotes(contestId, page, pageSize, PageContext.Current.Item.Parent, out var totalPages)
						.OrderByDescending(x => x.Item2)
						.ToList();
                    result.Pagination = new Pagination()
                    {
                        CurrentPage = page,
                        Pages = totalPages
                    };
					break;
				case ContestStatus.IsExperienceEditor:
					result.SocialShare = GetSocialShare();
					result.SubmissionVotes = new List<Tuple<Submission, int>>();
                    result.Pagination = new Pagination()
                    {
                        CurrentPage = 0,
                        Pages = 1
                    };
                    break;
				case ContestStatus.RequiresAuthentication:
					break;
				case ContestStatus.NotStarted:
					break;
				case ContestStatus.Closed:
					break;
				default:
					break;
			}
			return result;
		}

		private ContestStatus GetStatus(Item datasource) {
			var accountUser = _sessionAuthenticationManager.GetAccountMembership();
			if (Sitecore.Context.PageMode.IsExperienceEditor || Sitecore.Context.PageMode.IsPreview) {
				return ContestStatus.IsExperienceEditor;
			}
			switch (accountUser.Status) {
				case StatusEnum.Hot:
					if (!HasContestStarted(datasource)) {
						return ContestStatus.NotStarted;
					}
					if (HasContestEnded(datasource)) {
						return ContestStatus.Closed;
					}
					return ContestStatus.Active;
				default:
					return ContestStatus.RequiresAuthentication;
			}
		}

		/// <summary>
		/// Item 1: Submission, Item 2: Total Votes.
		/// </summary>
		/// <param name="contestId"></param>
		/// <returns></returns>
		private IList<Tuple<Submission, int>> GetSubmissionVotes(string contestId, int page, int pageSize, Item voteParent, out int totalPages) {
			var result = new List<Tuple<Submission, int>>();

            var submissions = _voteService.GetSubmissions(contestId, _contestSubmissionService.GetAllowedTypes(voteParent))
                                .Where(x => x.Metadata != null);
            totalPages = (int)Math.Ceiling((double)submissions.Count() / pageSize);
            var pagedSubmissions = submissions
                                .Skip(pageSize * page)
                                .Take(pageSize);
            
			var votes = _voteService.GetVotes(contestId).ToList();

			foreach (var submission in pagedSubmissions) {
				var submissionVote = new Tuple<Submission, int>(submission, 0);
                var itemFound= votes.FirstOrDefault(x => x.SubmissionId.Equals(submission.Id, StringComparison.InvariantCultureIgnoreCase));
                if (itemFound != null) {
                    submissionVote = new Tuple<Submission, int>(submission, itemFound.Total);
                }
                result.Add(submissionVote);
			}
			return result;
		}

		private Item GetContestPage(string contestId) {
			var contestFolder = Sitecore.Context.Database.GetItem(new ID(contestId));
			return contestFolder.Axes.GetDescendants()
				.FirstOrDefault(x => x.TemplateID == Templates.ConstestSubmission.ID);
		}

		private HtmlString GetSocialShare() {
			var siteSettings = Sitecore.Context.Database.GetItem(Configuration.SiteSettingsId);
			var socialShareContent = siteSettings.Fields[Templates.SiteSettings.Fields.InlineButtons].Value;
			if (string.IsNullOrEmpty(socialShareContent)) {
				return new HtmlString(string.Empty);
			}
			return new HtmlString(socialShareContent);
		}

		private bool HasContestStarted(Item datasource) {
			var startDateField = (DateField)datasource.Fields[Templates.ConstestVote.Fields.StartVoteDate];
			var startDateTime = Sitecore.DateUtil.IsoDateToDateTime(startDateField.Value);
			if (startDateTime == DateTime.MinValue || startDateTime == DateTime.MaxValue) {
				return true;
			}

			return DateTime.Now >= startDateTime;
		}
		private bool HasContestEnded(Item datasource) {
			var endDateField = (DateField)datasource.Fields[Templates.ConstestVote.Fields.EndVoteDate];
			var endDateTime = Sitecore.DateUtil.IsoDateToDateTime(endDateField.Value);
			if (endDateTime == DateTime.MinValue || endDateTime == DateTime.MaxValue) {
				return false;
			}
			return DateTime.Now >= endDateTime;
		}
        private int GetPageSize(Item datasource)
        {
            var pageSize = datasource.Fields[Templates.ConstestVote.Fields.Pagination].Value;
            bool success = int.TryParse(pageSize, out var number);
            return success ? number : Configuration.SubmissionPageSize;
        }
        private bool IsContestActive(Item datasource) {
			return HasContestStarted(datasource) && !HasContestEnded(datasource);
		}
	}
}