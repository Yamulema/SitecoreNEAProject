using Moq;
using Neambc.Neamb.Feature.Avatar.Managers;
using Neambc.Neamb.Feature.Avatar.Models;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;

namespace Neambc.Neamb.Feature.Avatar.UnitTest.Manager
{
	[TestFixture]
	public class AvatarManagerTest {

		#region Fields

		private Mock<ISessionAuthenticationManager> _sessionAuthenticationManager;
		private Mock<IAmazonS3Repository> _amazonS3Repository;
		private Mock<IGlobalConfigurationManager> _globalConfigurationManager;
		private Mock<ICacheManager> _cacheManager;
		private Mock<IBase64Service> _base64Service;
		private string _mdsId;
        private Mock<IActionReminderService> _actionReminderService;
        #endregion

        [SetUp]
		public void SetUp() {
			_sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
			_amazonS3Repository = new Mock<IAmazonS3Repository>();
			_globalConfigurationManager = new Mock<IGlobalConfigurationManager>();
			_cacheManager = new Mock<ICacheManager>();
			_base64Service = new Mock<IBase64Service>();
			_mdsId = "997";
            _actionReminderService = new Mock<IActionReminderService>();
		}
		[Test]
		public void Return_NoImageUrl_WhenNoAvatarImage() {
			//Arrange
			AvatarDTO expectedResult = new AvatarDTO();

			//Act
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
				.Returns(new AccountMembership {
					Mdsid = _mdsId,
					Status = StatusEnum.Hot,
					Profile = new Profile {
						Webuserid = "123"
					}
				});

			var avatarManager = new AvatarManager(_amazonS3Repository.Object,
				_sessionAuthenticationManager.Object,
				_globalConfigurationManager.Object,
				_cacheManager.Object,
				_base64Service.Object,
                _actionReminderService.Object);
			var result = avatarManager.GetAvatarModel("", null);
			//Assert
			Assert.AreEqual(result.UserImageUrl, expectedResult.UserImageUrl);
		}

		[Test]
		public void Return_ImageUrl_WhenHasAvatarImage() {
			//Arrange
			AvatarDTO expectedResult = new AvatarDTO();
			string key = $"AvatarImage:123";
			//Act
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
				.Returns(new AccountMembership {
					Mdsid = _mdsId,
					Status = StatusEnum.Hot,
					Profile = new Profile {
						Webuserid = "123"
					}
				});
			_cacheManager.Setup(x => x.ExistInCache(key))
				.Returns(true);

			_cacheManager.Setup(x => x.RetrieveFromCache<byte[]>(key))
				.Returns(new byte[10]);

			var avatarManager = new AvatarManager(_amazonS3Repository.Object,
				_sessionAuthenticationManager.Object,
				_globalConfigurationManager.Object,
				_cacheManager.Object,
				_base64Service.Object,
                _actionReminderService.Object);
			var result = avatarManager.GetAvatarModel("", null);
			//Assert
			Assert.AreEqual(result.UserImageUrl, expectedResult.UserImageUrl);
		}

		[Test]
		public void Return_Error_SaveAvatarImage() {
			//Arrange
			AvatarResultOperation expectedResult = AvatarResultOperation.GeneralError;
			//Act
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
				.Returns(new AccountMembership());
			var avatarManager = new AvatarManager(_amazonS3Repository.Object,
				_sessionAuthenticationManager.Object,
				_globalConfigurationManager.Object,
				_cacheManager.Object,
				_base64Service.Object,
                _actionReminderService.Object);
			var result = avatarManager.SaveAvatar("");
			//Assert
			Assert.AreEqual(result, expectedResult);
		}

		[Test]
		public void Return_Ok_SaveAvatarImage()
		{
			//Arrange
			AvatarResultOperation expectedResult = AvatarResultOperation.Ok;
			//Act
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
				.Returns(new AccountMembership{ Status = StatusEnum.Hot});
            _amazonS3Repository.Setup(x => x.CreateObjectS3(It.IsAny<RequestS3>())).Returns(true);
			var avatarManager = new AvatarManager(_amazonS3Repository.Object,
				_sessionAuthenticationManager.Object,
				_globalConfigurationManager.Object,
				_cacheManager.Object,
				_base64Service.Object,
                _actionReminderService.Object);
			var result = avatarManager.SaveAvatar("");
			//Assert
			Assert.AreEqual(result, expectedResult);
		}

		[Test]
		public void Return_ErrorSize_SaveAvatarImage()
		{
			//Arrange
			AvatarResultOperation expectedResult = AvatarResultOperation.ErrorSize;
			//Act
			_sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
				.Returns(new AccountMembership { Status = StatusEnum.Hot });
			var avatarManager = new AvatarManager(_amazonS3Repository.Object,
				_sessionAuthenticationManager.Object,
				_globalConfigurationManager.Object,
				_cacheManager.Object,
				_base64Service.Object,
                _actionReminderService.Object);
			var result = avatarManager.SaveAvatar("test");
			//Assert
			Assert.AreEqual(result, expectedResult);
		}
	}

}
