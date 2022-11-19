using System;
using System.IO;
using Neambc.Neamb.Feature.Avatar.Interfaces;
using Neambc.Neamb.Feature.Avatar.Models;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Avatar.Managers
{
	[Service(typeof(IAvatarManager))]
	public class AvatarManager: IAvatarManager
	{
		private readonly IAmazonS3Repository _amazonS3Repository;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly ICacheManager _cacheManager;
		private readonly IBase64Service _base64Service;
		private readonly string _cacheKeyGroup = "AvatarImage";
        private readonly IActionReminderService _actionReminderService;
        public AvatarManager(IAmazonS3Repository amazonS3Repository, ISessionAuthenticationManager sessionAuthenticationManager,
			IGlobalConfigurationManager globalConfigurationManager, ICacheManager cacheManager, IBase64Service base64Service,
            IActionReminderService actionReminderService
        ) {
			_amazonS3Repository = amazonS3Repository;
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_globalConfigurationManager = globalConfigurationManager;
			_cacheManager = cacheManager;
			_base64Service = base64Service;
            _actionReminderService = actionReminderService;
        }
		public AvatarDTO GetAvatarModel(string uploadParameter, Rendering rendering) {
			var model = new AvatarDTO();
			model.Initialize(rendering);
			//var uploadParameter = Request.QueryString[ConstantsNeamb.UploadResult];
			if (!string.IsNullOrEmpty(uploadParameter) && uploadParameter.Equals("ok"))
			{
				model.IsProcessedSucessfully = true;
			}
			//Get the user authentication object
			var account = _sessionAuthenticationManager.GetAccountMembership();
			model.UserStatus = account.Status;
			if (account.Status == StatusEnum.Hot)
			{
				//Try to get from caching
				var key = $"{_cacheKeyGroup}:{account.Profile.Webuserid}";
				BaseRequestS3 baseRequest = new BaseRequestS3 {
					BucketName = _globalConfigurationManager.BucketNameAvatarImages,
					Key = account.Profile.Webuserid,
					IsEncrypted = true
				};
			
				var retrievedFile = _cacheManager.ExistInCache(key)
					? _cacheManager.RetrieveFromCache<byte[]>(key)
					: _amazonS3Repository.GetObjectS3<byte[]>(baseRequest);
				if (retrievedFile != null && retrievedFile.Length > 0)
				{
					_cacheManager.StoreInCache(key, retrievedFile, DateTime.Now.AddDays(3));
					//Build the image url
					model.UserImageUrl = _base64Service.EncodeImage(retrievedFile);
				}
			}
			return model;
		}
		public AvatarResultOperation SaveAvatar(string data) {
			string mdsId = "";
			try {
				//Get the data of the user authenticated
				var account = _sessionAuthenticationManager.GetAccountMembership();

				//Convert from base 64 string to byte []
				var base64 = data.Substring(data.IndexOf(',') + 1);
				base64 = base64.Trim('\0');
				var chartData = Convert.FromBase64String(base64);

				//Check the size
				if (chartData.Length > _globalConfigurationManager.MaxImageSizeAvatar) {
					return AvatarResultOperation.ErrorSize;
				}
				//Check the user status = Hot
				if (account.Status == StatusEnum.Hot) {
					mdsId = account.Mdsid;
					var outputStream = new MemoryStream(chartData);
                    outputStream.Seek(0, SeekOrigin.Begin);
                    //Upload the image in S3
                    RequestS3 requestS3 = new RequestS3
					{
						BucketName = _globalConfigurationManager.BucketNameAvatarImages,
						Key = account.Profile.Webuserid,
						ContentType = "image/png",
						InputStream = outputStream,
						IsEncrypted = true
					};
					var response=_amazonS3Repository.CreateObjectS3(requestS3);
					if (!response) {
						return AvatarResultOperation.GeneralError;
					}
					var key = $"{_cacheKeyGroup}:{account.Profile.Webuserid}";
					_cacheManager.StoreInCache(key, chartData, DateTime.Now.AddDays(3));
                    _actionReminderService.SetVisited(PageType.Profile, account.Username);
                    return AvatarResultOperation.Ok;
				}
				return AvatarResultOperation.GeneralError;
			}
			catch (Exception e)
			{
				Log.Error($"Error in Uploading the avatar image for mdsid:{mdsId}", e, this);
				return AvatarResultOperation.GeneralError;
			}

		}
	}
}