using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Contest.Interfaces;
using Neambc.Neamb.Feature.Contest.Model;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Feature.Contest.Services
{
    [Service(typeof(IFallbackProvider))]
    public class S3FallbackProvider : IFallbackProvider
    {
        private readonly IAmazonS3Repository _amazonS3Repository;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;

        public S3FallbackProvider(IAmazonS3Repository amazonS3Repository, IGlobalConfigurationManager globalConfigurationManager)
        {
			_amazonS3Repository = amazonS3Repository;
            _globalConfigurationManager = globalConfigurationManager;
        }

        public IList<Vote> GetContestBackup(string contestId) {
			BaseRequestS3 baseRequest = new BaseRequestS3 {
				BucketName = _globalConfigurationManager.BucketNameContestImages,
				Key = $"{GetContestBackupPath(contestId)}/{Configuration.S3VoteFileName}"
			};

			return _amazonS3Repository.GetObjectS3<IList<Vote>>(baseRequest);
        }

        public void SetContestBackup(IList<Vote> votes)
        {
            if (votes == null || string.IsNullOrEmpty(votes?.FirstOrDefault()?.ContestId))
            {
                Log.Warn($"Unable to backup votes in S3 for contestId with the value of:{votes?.FirstOrDefault()?.ContestId}", this);
                return;
            }
			var serializedObject = JsonConvert.SerializeObject(votes);
			RequestS3 requestS3 = new RequestS3
			{
				BucketName = _globalConfigurationManager.BucketNameContestImages,
				Key = $"{GetContestBackupPath(votes.FirstOrDefault().ContestId)}/{Configuration.S3VoteFileName}",
				ContentType = "text/plain",
				ContentBody = serializedObject
			};
			_amazonS3Repository.CreateObjectS3(requestS3);
            Log.Info($"Backup contest {votes.FirstOrDefault().ContestId} in S3.", this);
        }

        private string GetContestBackupPath(string contestId)
        {
            return $"{contestId}/{Configuration.S3VoteFolder}";
        }
    }
}