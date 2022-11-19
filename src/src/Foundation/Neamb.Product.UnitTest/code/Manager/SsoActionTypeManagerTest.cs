using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Manager;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Product.UnitTest.Manager {
	[TestFixture]
	public class SsoActionTypeManagerTest {

		#region Fields

		private Mock<IEligibilityManager> _eligibilityManagerMock;
		private Mock<IUserStatusHandler> _userStatusHandlerMock;
		private Mock<ISessionManager> _sessionManagerMock;
		private Mock<IPipelineService> _pipelineServiceMock;

		#endregion

		#region Pre/Post Actions

		[SetUp]
		public void SetUp() {
			// before each test
			_eligibilityManagerMock = new Mock<IEligibilityManager>();
			_userStatusHandlerMock = new Mock<IUserStatusHandler>();
			_sessionManagerMock = new Mock<ISessionManager>();
			_pipelineServiceMock = new Mock<IPipelineService>();

		}

		[TearDown]
		public void TearDown() {
			// after each test
		}

		#endregion

		#region Private Methods

		private SsoModel SetupSsoModel (string mdsid, string productCode, int months, int componentType){
			var username = "Jessica";
			var profile = new Profile {
				FirstName = "Jessica",
				LastName = "Jones",
				StreetAddress = "Street 1",
				City = "New York",
				StateCode = "NY",
				ZipCode = "31001",
				Phone = "18001231231",
				DateOfBirth = "21/12/1233"
			};
			var accountUser = new AccountUserBase {Mdsid = mdsid,
				Profile = profile,
				Username = username
			};
			var ssoModel = new SsoModel {
				AccountUser = accountUser,
				ComponentType = componentType, 
				ProductCode = productCode
			};
			return ssoModel;
		}

		#endregion

		#region Tests
		[Test]
		public void GetUrlSso_Should_Return_Url_When_Eligible_And_Result_Pipeline_Not_Null() {
			//Arrange
			var expectedResult = new OperationResult{ResultUrl = ResultUrlEnum.None, Url = "www.google.com"};
			var mdsid = "991";
			var productCode = "1231";
			var months = 12;
			var ssoModel = SetupSsoModel(mdsid, productCode, months, (int) ComponentTypeEnum.SpecialOffer);

			var resultPipeline = new ResultPipeline {ActionPrimary = "www.google.com"};
			_eligibilityManagerMock.Setup(x => x.IsMemberEligible(ssoModel.AccountUser.Mdsid, ssoModel.ProductCode, months)).Returns(EligibilityResultEnum.Eligible);
			_pipelineServiceMock.Setup(x => x.RunProcessPipelines(ssoModel.ProductCode, ssoModel.AccountUser, "neambProductCTASingleSignOn", "")).Returns(resultPipeline);
			var ssoActionTypeManager = new SsoActionTypeManager(
				_eligibilityManagerMock.Object, 
				_userStatusHandlerMock.Object, 
				_sessionManagerMock.Object, 
				_pipelineServiceMock.Object);
		

			//Act
			var result = ssoActionTypeManager.GetUrlSso(ssoModel);

			//Assert
			Assert.AreEqual(expectedResult.Url, result.Url );
			Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl );
		}

		[Test]
		public void GetUrlSso_Should_Return_Action_Primary_When_Not_Special_Offer() {
			//Arrange
			var expectedResult = new OperationResult{ResultUrl = ResultUrlEnum.None, Url = "www.google.com"};
			var mdsid = "991";
			var productCode = "1231";
			var months = 12;
			var ssoModel = SetupSsoModel(mdsid, productCode, months, (int) ComponentTypeEnum.Anonymous);

			var resultPipeline = new ResultPipeline {ActionPrimary = "www.google.com"};
			_eligibilityManagerMock.Setup(x => x.IsMemberEligible(ssoModel.AccountUser.Mdsid, ssoModel.ProductCode, months)).Returns(EligibilityResultEnum.Eligible);
			_pipelineServiceMock.Setup(x => x.RunProcessPipelines(ssoModel.ProductCode, ssoModel.AccountUser, "neambProductCTASingleSignOn", "")).Returns(resultPipeline);
			var ssoActionTypeManager = new SsoActionTypeManager(
				_eligibilityManagerMock.Object, 
				_userStatusHandlerMock.Object, 
				_sessionManagerMock.Object, 
				_pipelineServiceMock.Object);
		

			//Act
			var result = ssoActionTypeManager.GetUrlSso(ssoModel);

			//Assert
			Assert.AreEqual(expectedResult.Url, result.Url );
			Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl );
		}

		[Test]
		public void GetUrlSso_Should_Return_NoUrl_When_Action_Primary_Null() {
			//Arrange
			var expectedResult = new OperationResult{ResultUrl = ResultUrlEnum.NoUrl, Url = null};
			var mdsid = "991";
			var productCode = "1231";
			var months = 12;
			var ssoModel = SetupSsoModel(mdsid, productCode, months, (int) ComponentTypeEnum.Anonymous);

			var resultPipeline = new ResultPipeline {ActionPrimary = ""};
			_eligibilityManagerMock.Setup(x => x.IsMemberEligible(ssoModel.AccountUser.Mdsid, ssoModel.ProductCode, months)).Returns(EligibilityResultEnum.Eligible);
			_pipelineServiceMock.Setup(x => x.RunProcessPipelines(ssoModel.ProductCode, ssoModel.AccountUser, "neambProductCTASingleSignOn", "")).Returns(resultPipeline);
			var ssoActionTypeManager = new SsoActionTypeManager(
				_eligibilityManagerMock.Object, 
				_userStatusHandlerMock.Object, 
				_sessionManagerMock.Object, 
				_pipelineServiceMock.Object);
		

			//Act
			var result = ssoActionTypeManager.GetUrlSso(ssoModel);

			//Assert
			Assert.AreEqual(expectedResult.Url, result.Url );
			Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl );
		}

		[Test]
		public void GetUrlSso_Should_Return_Unforbidn_When_NotEligible() {
			//Arrange
			var expectedResult = new OperationResult{ResultUrl = ResultUrlEnum.UnForbidden, Url = null};
			var mdsid = "991";
			var productCode = "1231";
			var months = 12;
			var ssoModel = SetupSsoModel(mdsid, productCode, months, (int) ComponentTypeEnum.Anonymous);

			var resultPipeline = new ResultPipeline {ActionPrimary = ""};
			_eligibilityManagerMock.Setup(x => x.IsMemberEligible(ssoModel.AccountUser.Mdsid, ssoModel.ProductCode, months)).Returns(EligibilityResultEnum.NotEligible);
			_pipelineServiceMock.Setup(x => x.RunProcessPipelines(ssoModel.ProductCode, ssoModel.AccountUser, "neambProductCTASingleSignOn", "")).Returns(resultPipeline);
			var ssoActionTypeManager = new SsoActionTypeManager(
				_eligibilityManagerMock.Object, 
				_userStatusHandlerMock.Object, 
				_sessionManagerMock.Object, 
				_pipelineServiceMock.Object);
		

			//Act
			var result = ssoActionTypeManager.GetUrlSso(ssoModel);

			//Assert
			Assert.AreEqual(expectedResult.Url, result.Url );
			Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl );
		}
		#endregion
	}
}
