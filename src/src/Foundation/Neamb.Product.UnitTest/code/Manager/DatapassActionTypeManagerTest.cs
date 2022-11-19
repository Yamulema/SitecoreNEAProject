using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;
using NUnit.Framework;
using MN = Neambc.Neamb.Foundation.Product.Manager;


namespace Neambc.Neamb.Foundation.Product.UnitTest.Manager {
	[TestFixture]
	public class DatapassActionTypeManagerTest {
		#region Fields
		private Mock<IUserStatusHandler> _userStatusHandler;
		private Mock<IEligibilityManager> _eligibilityManager;
		private Mock<ISessionManager> _sessionManager;
		private Mock<IPipelineService> _pipelineService;
		#endregion

		[SetUp]
		public void SetUp() {
			_userStatusHandler = new Mock<IUserStatusHandler>();
			_eligibilityManager = new Mock<IEligibilityManager>();
			_sessionManager = new Mock<ISessionManager>();
			_pipelineService = new Mock<IPipelineService>();
		}

		#region Private Methods

		private DatapassModel SetupDataPassModel(string mdsid, string productCode, string actionPrimary, int componentType) {
			var accountUserBase = new AccountUserBase { Mdsid = mdsid, Username = "Jenny", Profile = new Profile() };

			var datapassModel =
				new DatapassModel { AccountUser = accountUserBase, ProductCode = productCode, PrimarySecondaryActionType = actionPrimary, ComponentType = componentType};
			return datapassModel;
		}

		#endregion

		[Test]
		public void Return_Unforbidden_When_UserNotEligible() {
			//Arrange
			var expectedValue = new OperationResult { ResultUrl = ResultUrlEnum.UnForbidden };
			var datapassModel = SetupDataPassModel("991", "2", "", (int) ComponentTypeEnum.None);

			_userStatusHandler.Setup(x => x.GetUrlUserNotAuthenticated()).Returns("");
			var datapassManager = new MN.DatapassActionTypeManager(_eligibilityManager.Object, _userStatusHandler.Object, _sessionManager.Object, _pipelineService.Object);

			//Act
			var result = datapassManager.GetUrlDatapass(datapassModel);

			//Assert
			Assert.AreEqual(result.ResultUrl, expectedValue.ResultUrl);
		}

		[Test]
		public void Return_None_When_UserEligible() {
			//Arrange
			var expectedValue = new OperationResult {ResultUrl = ResultUrlEnum.None, Url = ""};
			var datapassModel = SetupDataPassModel("991", "2", "datapass-first-", (int)ComponentTypeEnum.None);
			_userStatusHandler.Setup(x => x.GetUrlUserNotAuthenticated()).Returns("");
			_eligibilityManager.
				Setup(x => x.IsMemberEligible(datapassModel.AccountUser.Mdsid, datapassModel.ProductCode, 12)).
				Returns(EligibilityResultEnum.Eligible);
			_pipelineService.
				Setup(x => x.RunProcessPipelines(datapassModel.ProductCode,
					datapassModel.AccountUser,
					ConstantsNeamb.PipelineNameDatapass, "")).
				Returns(new ResultPipeline{ActionPrimary = "datapass-first-", ActionSecondary = "datapass-first-"});

			var datapassManager = new MN.DatapassActionTypeManager(
				_eligibilityManager.Object,
				_userStatusHandler.Object,
				_sessionManager.Object,
				_pipelineService.Object);

			//Act
			var result = datapassManager.GetUrlDatapass(datapassModel);

			//Assert
			Assert.AreEqual(result.ResultUrl, expectedValue.ResultUrl);
		}

		[Test]
		public void Return_ActionSecondary_When_UserEligible() {
			//Arrange
			var expectedValue = new OperationResult {ResultUrl = ResultUrlEnum.None, Url = "datapass-first-"};
			var datapassModel = SetupDataPassModel("991", "2", "datapass", (int)ComponentTypeEnum.SpecialOffer);
			_userStatusHandler.Setup(x => x.GetUrlUserNotAuthenticated()).Returns("");
			_eligibilityManager.
				Setup(x => x.IsMemberEligible(datapassModel.AccountUser.Mdsid, datapassModel.ProductCode, 12)).
				Returns(EligibilityResultEnum.Eligible);
			_pipelineService.
				Setup(x => x.RunProcessPipelines(datapassModel.ProductCode,
					datapassModel.AccountUser,
					ConstantsNeamb.PipelineNameDatapass, "")).
				Returns(new ResultPipeline{ActionPrimary = "datapass-first-", ActionSecondary = "datapass-first-"});

			var datapassManager = new MN.DatapassActionTypeManager(
				_eligibilityManager.Object,
				_userStatusHandler.Object,
				_sessionManager.Object,
				_pipelineService.Object);

			//Act
			var result = datapassManager.GetUrlDatapass(datapassModel);

			//Assert
			Assert.AreEqual(result.ResultUrl, expectedValue.ResultUrl);
			Assert.AreEqual(result.Url, expectedValue.Url);
		}

		[Test]
		public void Return_NoUrl_When_No_Action() {
			//Arrange
			var expectedValue = new OperationResult {ResultUrl = ResultUrlEnum.NoUrl, Url = ""};
			var datapassModel = SetupDataPassModel("991", "2", "datapass", (int)ComponentTypeEnum.SpecialOffer);
			_userStatusHandler.Setup(x => x.GetUrlUserNotAuthenticated()).Returns("");
			_eligibilityManager.
				Setup(x => x.IsMemberEligible(datapassModel.AccountUser.Mdsid, datapassModel.ProductCode, 12)).
				Returns(EligibilityResultEnum.Eligible);
			_pipelineService.
				Setup(x => x.RunProcessPipelines(datapassModel.ProductCode,
					datapassModel.AccountUser,
					ConstantsNeamb.PipelineNameDatapass, "")).
				Returns(new ResultPipeline{ActionPrimary = "", ActionSecondary = ""});

			var datapassManager = new MN.DatapassActionTypeManager(
				_eligibilityManager.Object,
				_userStatusHandler.Object,
				_sessionManager.Object,
				_pipelineService.Object);

			//Act
			var result = datapassManager.GetUrlDatapass(datapassModel);

			//Assert
			Assert.AreEqual(result.ResultUrl, expectedValue.ResultUrl);
		}


	}
}
