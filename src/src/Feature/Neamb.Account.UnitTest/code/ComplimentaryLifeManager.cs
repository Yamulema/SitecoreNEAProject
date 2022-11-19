using System.Globalization;
using System.Reflection;
using Moq;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Feature.Account.UnitTest.Fakes;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using SUT = Neambc.Neamb.Feature.Account.Managers;

namespace Neambc.Neamb.Feature.Account.UnitTest {

	//// Some Test Guidelines
	////
	//// Arrange: Prep your objects & expectation
	////	parts of this that are setup one-time-only have a shortcut
	////	parts of this that are duplicated for every test have a shortcut

	//// Act: Call the Method-Under-Test (MUT)
	////	typically, this is a single call

	//// Assert: compare result to expectation (could be multiple)
	////	typically, this is one or more asserts (or compares/throw)
	////	when there's a failure, deliver good information
	////
	[TestFixture]
	public class ComplimentaryLifeManager {

		#region Fields

		//private BeneficiaryDTO _beneficiaryDto;

		//private ViewDataDictionary _viewData;

		// requires to instantiate
		private FakeSessionAuthenticationManager _sessionManager;
		private IProfileManager _profileManager;

		private Mock<IOracleDatabase> _oracleManagerMock;
		private IOracleDatabase _oracleManager;
		private IEmailValidationManager _emailValidationManager;
		private Mock<IExactTargetClient> _exactTargetClient;
		private ISessionAuthenticationManager _sessionAuthenticationManager;
        private Mock<IGtmService> _gtmServiceMock;
        private Mock<ICompIntroLifeService> _compIntroLifeServiceMock;

        private SUT.ComplimentaryLifeManager _sut;
        private Mock<IActionReminderService> _actionReminderService;
        private Mock<IComplimentaryLifeWizardService> _complimentaryLifeWizardService;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        //private Item _item;
        #endregion

        [SetUp]
		public void SetUp() {
			_sessionManager = new FakeSessionAuthenticationManager {
				AccountMembership = new AccountMembership() {
					Status = StatusEnum.Cold
				}
			};

			_exactTargetClient = new Mock<IExactTargetClient>();
			_sessionAuthenticationManager = new FakeSessionAuthenticationManager();
			_profileManager = new FakeProfileManager();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _oracleManagerMock = new Mock<IOracleDatabase>();
			_oracleManager = _oracleManagerMock.Object;
			_emailValidationManager = new FakeEmailValidationManager();
            _compIntroLifeServiceMock = new Mock<ICompIntroLifeService>();
            _gtmServiceMock = new Mock<IGtmService>();
            _actionReminderService = new Mock<IActionReminderService>();
            _complimentaryLifeWizardService = new Mock<IComplimentaryLifeWizardService>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _sut = new SUT.ComplimentaryLifeManager(
				_sessionManager,
                _profileManager,
				_oracleManager,
				_emailValidationManager,
				_exactTargetClient.Object,
				_sessionAuthenticationManager,
                _gtmServiceMock.Object,
                _actionReminderService.Object,
                _complimentaryLifeWizardService.Object,
                _accountRepositoryMock.Object,
                _globalConfigurationManagerMock.Object,
                _compIntroLifeServiceMock.Object
            );
        }

		
		//	#region Tests
		//	[Test, Isolated]
		//	public void WasMoreAboutYouChanged_ReturnsAppropriateValue() {
		//		var am = new AccountMembership() {
		//			Profile = new Profile(),
		//			AdditionalInfo = new AdditionalInfo()
		//		};
		//		var may = new MoreAboutYou();

		//		am.AdditionalInfo.Employment = "emp";
		//		Assert.IsTrue( _mgr.WasMoreAboutYouChanged(am, may), "Employment should report change");
		//		may.Employment = new MbcDbOption { SelectedValue = "emp" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "Employment should report same");

		//		am.AdditionalInfo.FamilyIncome = "family";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "FamilyIncome should report change");
		//		may.FamilyIncome = new MbcDbOption { SelectedValue = "family" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "FamilyIncome should report same");

		//		am.Profile.GenderCode = "M";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "GenderCode should report change");
		//		may.Gender = new MbcDbOption { SelectedValue = "M" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "GenderCode should report same");

		//		am.AdditionalInfo.Housing = "house";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "Housing should report change");
		//		may.Housing = new MbcDbOption { SelectedValue = "house" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "Housing should report same");

		//		am.AdditionalInfo.MajorWageEarner = "mwe";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "MajorWageEarner should report change");
		//		may.MajorWageEarner = new MbcDbOption { SelectedValue = "mwe" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "MajorWageEarner should report same");

		//		am.AdditionalInfo.MaritalStatus = "married";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "MaritalStatus should report change");
		//		may.MaritalStatus = new MbcDbOption { SelectedValue = "married" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "MaritalStatus should report same");

		//		am.AdditionalInfo.SpouseEmployment = "working";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "SpouseEmployment should report change");
		//		may.SpouseEmployment = new MbcDbOption { SelectedValue = "working" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "SpouseEmployment should report same");

		//		am.AdditionalInfo.ChildYear1 = "child1";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "ChildYear1 should report change");
		//		may.ChildYear1 = new Option { SelectedValue = "child1" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "ChildYear1 should report same");

		//		am.AdditionalInfo.ChildYear2 = "child2";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "ChildYear2 should report change");
		//		may.ChildYear2 = new Option { SelectedValue = "child2" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "ChildYear2 should report same");

		//		am.AdditionalInfo.ChildYear3 = "child3";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "ChildYear3 should report change");
		//		may.ChildYear3 = new Option { SelectedValue = "child3" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "ChildYear3 should report same");

		//		am.AdditionalInfo.ChildYear4 = "child4";
		//		Assert.IsTrue(_mgr.WasMoreAboutYouChanged(am, may), "ChildYear4 should report change");
		//		may.ChildYear4 = new Option { SelectedValue = "child4" };
		//		Assert.IsFalse(_mgr.WasMoreAboutYouChanged(am, may), "ChildYear4 should report same");


		//	}
		//	[Test, Isolated]
		//	public void SaveBeneficiary_ColdMembershipStatusReturnsModel() {
		//		Arrange_GetAddBeneficiaryModel();
		//		// by default, _sessionManager.AccountMembership.Status is cold
		//		_viewData = new ViewDataDictionary();

		//		var expected = new BeneficiaryDTO();
		//		Isolate.WhenCalled(() => _mgr.GetAddBeneficiaryModel((Item)null)).WillReturn(expected);
		//		var result = _mgr.SaveBeneficiary(_beneficiaryDto, _viewData, _item);

		//		Assert.AreSame(expected, result);
		//	}
		//	[Test, Isolated]
		//	public void SaveBeneficiary_HotMembershipStatusReturnsDto_OtherEntity_ValidationError() {
		//		_beneficiaryDto = new BeneficiaryDTO();
		//		_viewData = new ViewDataDictionary();
		//		_sessionManager.AccountMembership.Status = StatusEnum.Hot;

		//		_beneficiaryDto = new BeneficiaryDTO {
		//			DatasourceId = _item.ID.ToGuid().ToString("B"),
		//			SelectedType = BeneficiaryType.OtherEntity.ToString(),
		//			Relationship = new MbcDbOption(),
		//			PayoutPercentage = 25
		//		};

		//		// will be the mock output for every call to ValidationFieldHelper.GetErrorStatus,
		//		// regardless of pass parameters
		//		// To get different values, one must stack the Isolate calls in-order
		//		Isolate.WhenCalled(() => ValidationFieldHelper.GetErrorStatus("Email", null, true, true, true))
		//			.WillReturn(ErrorStatusEnum.EmailInUse);

		//		var result = _mgr.SaveBeneficiary(_beneficiaryDto, _viewData, _item);

		//		Assert.IsInstanceOf<BeneficiaryDTO>(result);
		//		Assert.AreEqual(_item.ID, result.Item.ID);
		//		Assert.AreEqual(0, result.Relationship.Values.Count);
		//		Assert.AreEqual(ErrorStatusEnum.EmailInUse, result.PayoutPercentageErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.EmailInUse, _beneficiaryDto.EmailErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.EmailInUse, _beneficiaryDto.OtherEntityNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.FirstNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.LastNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.MiddleInitialErrorStatus);
		//	}
		//	[Test, Isolated]
		//	public void SaveBeneficiary_HotMembershipStatusReturnsDto_NamedIndividual_ValidationError() {
		//		_beneficiaryDto = new BeneficiaryDTO();
		//		_viewData = new ViewDataDictionary();
		//		_sessionManager.AccountMembership.Status = StatusEnum.Hot;

		//		_beneficiaryDto = new BeneficiaryDTO {
		//			DatasourceId = _item.ID.ToGuid().ToString("B"),
		//			SelectedType = BeneficiaryType.NamedIndividual.ToString(),
		//			Relationship = new MbcDbOption(),
		//			PayoutPercentage = 25
		//		};

		//		// will be the mock output for every call to ValidationFieldHelper.GetErrorStatus,
		//		// regardless of pass parameters
		//		// To get different values, one must stack the Isolate calls in-order
		//		Isolate.WhenCalled(() => ValidationFieldHelper.GetErrorStatus(
		//				"Email",
		//				null,
		//				true,
		//				true,
		//				true
		//			))
		//			.WillReturn(ErrorStatusEnum.EmailInUse);

		//		var result = _mgr.SaveBeneficiary(_beneficiaryDto, _viewData, _item);
		//		Assert.IsInstanceOf<BeneficiaryDTO>(result);
		//		Assert.AreEqual(_item.ID, result.Item.ID);
		//		Assert.AreEqual(0, result.Relationship.Values.Count);
		//		Assert.AreEqual(ErrorStatusEnum.EmailInUse, result.PayoutPercentageErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.EmailInUse, _beneficiaryDto.EmailErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.OtherEntityNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.EmailInUse, _beneficiaryDto.FirstNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.EmailInUse, _beneficiaryDto.LastNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.EmailInUse, _beneficiaryDto.MiddleInitialErrorStatus);
		//	}
		//	[Test, Isolated]
		//	public void SaveBeneficiary_HotMembershipStatusReturnsDto_OtherEntity_NoError() {
		//		_beneficiaryDto = new BeneficiaryDTO();
		//		_viewData = new ViewDataDictionary();
		//		_sessionManager.AccountMembership.Status = StatusEnum.Hot;

		//		_beneficiaryDto = new BeneficiaryDTO {
		//			DatasourceId = _item.ID.ToGuid().ToString("B"),
		//			SelectedType = BeneficiaryType.OtherEntity.ToString(),
		//			Relationship = new MbcDbOption(),
		//			PayoutPercentage = 25
		//		};

		//		// will be the mock output for every call to ValidationFieldHelper.GetErrorStatus,
		//		// regardless of pass parameters
		//		// To get different values, one must stack the Isolate calls in-order
		//		Isolate.WhenCalled(() => ValidationFieldHelper.GetErrorStatus("Email", null, true, true, true))
		//			.WillReturn(ErrorStatusEnum.None);

		//		var result = _mgr.SaveBeneficiary(_beneficiaryDto, _viewData, _item);
		//		Assert.IsInstanceOf<BeneficiaryDTO>(result);
		//		Assert.AreEqual(_item.ID, result.Item.ID);
		//		Assert.AreEqual(0, result.Relationship.Values.Count);
		//		Assert.AreEqual(ErrorStatusEnum.None, result.PayoutPercentageErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.EmailErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.OtherEntityNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.FirstNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.LastNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.MiddleInitialErrorStatus);
		//	}
		//	[Test, Isolated]
		//	public void SaveBeneficiary_HotMembershipStatusReturnsDto_NamedIndividual_NoError() {
		//		_beneficiaryDto = new BeneficiaryDTO();
		//		_viewData = new ViewDataDictionary();
		//		_sessionManager.AccountMembership.Status = StatusEnum.Hot;

		//		_beneficiaryDto = new BeneficiaryDTO {
		//			DatasourceId = _item.ID.ToGuid().ToString("B"),
		//			SelectedType = BeneficiaryType.NamedIndividual.ToString(),
		//			Relationship = new MbcDbOption(),
		//			PayoutPercentage = 25
		//		};

		//		// will be the mock output for every call to ValidationFieldHelper.GetErrorStatus,
		//		// regardless of pass parameters
		//		// To get different values, one must stack the Isolate calls in-order
		//		Isolate.WhenCalled(() => ValidationFieldHelper.GetErrorStatus(
		//				"Email",
		//				null,
		//				true,
		//				true,
		//				true
		//			))
		//			.WillReturn(ErrorStatusEnum.None);

		//		var result = _mgr.SaveBeneficiary(_beneficiaryDto, _viewData,_item);
		//		Assert.IsInstanceOf<BeneficiaryDTO>(result);
		//		Assert.AreEqual(_item.ID, result.Item.ID);
		//		Assert.AreEqual(0, result.Relationship.Values.Count);
		//		Assert.AreEqual(ErrorStatusEnum.None, result.PayoutPercentageErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.EmailErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.OtherEntityNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.FirstNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.LastNameErrorStatus);
		//		Assert.AreEqual(ErrorStatusEnum.None, _beneficiaryDto.MiddleInitialErrorStatus);
		//	}
		//	#endregion

		//	#region Private Methods
		//private Item Arrange_FakeItem(Guid fakeGuid) {
		//	var id = new ID(fakeGuid.ToString("B"));
		//	_fakeSitecoreDatabase.Add(
		//		new SF.DbItem(string.Format("fakeDS{0}", id.ToShortID()), id) {
		//				new SF.DbItem(
		//					string.Format("datasource{0}",id.ToShortID()),
		//					Templates.Beneficiary.Fields.Back
		//				)
		//		}
		//	);
		//	return _fakeSitecoreDatabase.GetItem(id);
		//}
		//	private void Arrange_SetDraft(StatusEnum status = StatusEnum.Cold) {
		//		_sessionManager.AccountMembershipDraft = new AccountMembership() {
		//			Status = status,
		//			Mdsid = "Mdsid"
		//		};
		//	}
		//	private void Arrange_GetAddBeneficiaryModel() {
		//		var fakeItem = Isolate.Fake.Instance<Item>();
		//		Isolate.WhenCalled(() => Sitecore.Context.Item).WillReturn(fakeItem);
		//		Arrange_SetDraft();
		//	}
		//	#endregion
	}

}
