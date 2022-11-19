using System;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.SecurityManagement;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using NUnit.Framework;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.FakeDb.Sites;
using MN = Neambc.Neamb.Foundation.Product.Manager;


namespace Neambc.Neamb.Foundation.Product.UnitTest.Manager {
	[TestFixture]
	public class LinkActionTypeManagerTest {

		#region Fields
		private Mock<IUserStatusHandler> _userStatusHandler;
		private Mock<IEligibilityManager> _eligibilityManager;
		private Mock<ISessionAuthenticationManager> _sessionAuthenticationManager;
		private Mock<IAccountServiceProxy> _accountServiceProxy;
        private Mock<IAccessTokenService> _accessTokenService;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManager;
        private Db _database;
		private MN.LinkActionTypeManager _sut;
		private ID _contextItemId;
		private ID _eligibilityItemId;
		private ID _ctaLinkItemId;
		private string _mdsid;
        private Mock<ISecurityService> _securityService;
		private Mock<ITokenManager> _tokenManagerMock;
		#endregion

		#region Pre/Post Actions
		[SetUp]
		public void SetUp() {
			_userStatusHandler = new Mock<IUserStatusHandler>();
			_eligibilityManager = new Mock<IEligibilityManager>();
			_sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
			_accountServiceProxy = new Mock<IAccountServiceProxy>();
            _accessTokenService = new Mock<IAccessTokenService>();
            _globalConfigurationManager = new Mock<IGlobalConfigurationManager>();
            _securityService = new Mock<ISecurityService>();
			_tokenManagerMock = new Mock<ITokenManager>();
			_database = new Db("web");
			_contextItemId = ID.NewID;
			_eligibilityItemId = ID.NewID;
			_ctaLinkItemId = ID.NewID;
			_mdsid = "991";

		}
		[TearDown]
		public void TearDown() {
			// after each test
			_database.Dispose();
		}

		#endregion

		#region PrivateMethods

		private LinkModel SetupLinkModel() {
			var linkmodel = new LinkModel {
				AccountUser = new AccountUserBase{Mdsid = _mdsid},
				ContextItem = _contextItemId.ToString(),
				EligibilityItemId = _eligibilityItemId.ToString(),
				ProductCodeLink = "www.test.com",
				CtaLinkItemId = _ctaLinkItemId.ToString()
			};
			return linkmodel;
		}

		#endregion

		#region Tests
		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void RemoveAllEmptyParameters_Should_Return_SameUrlWithNonEmptyParameters() {
			string inputUrl = "www.google.com?camp_code=1&camp_code=2&medium=3";
			var linkActionTypeManager = new MN.LinkActionTypeManager(_eligibilityManager.Object, _sessionAuthenticationManager.Object, _accountServiceProxy.Object, _securityService.Object, _tokenManagerMock.Object);
			var resultUrl = linkActionTypeManager.RemoveAllEmptyParameters(inputUrl);
			//Assert
			Assert.AreEqual(inputUrl, resultUrl);
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void RemoveAllEmptyParameters_Should_Return_OtherUrlWithEmptyParameters() {
			string inputUrl = "www.google.com?cell_code=&camp_code=&medium=3";
			string expectedUrl = "www.google.com?medium=3";
			var linkActionTypeManager = new MN.LinkActionTypeManager(_eligibilityManager.Object, _sessionAuthenticationManager.Object, _accountServiceProxy.Object, _securityService.Object, _tokenManagerMock.Object);
			var resultUrl = linkActionTypeManager.RemoveAllEmptyParameters(inputUrl);
			//Assert
			Assert.AreEqual(expectedUrl, resultUrl);
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void GetUrlLink_Should_Return_Url_Mdsid_Cellcode() {
			//Arrange
			var expectedResult = "http://www.neamb.com?mdsid=991&code=1234";
			var contextItem = new DbItem("ContextItem", _contextItemId) {
				Name = "productamerican",
				Fields = {
					new DbField("Link", _ctaLinkItemId) {
						Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"www.neamb.com?mdsid=[mdsid]&amp;code=[cellcode]\" anchor=\"\" target=\"_blank\" />"
					},
					new DbField("EligibilityItem", _eligibilityItemId) 
				}
			};
			_database.Add(contextItem);
			_sut = new MN.LinkActionTypeManager(_eligibilityManager.Object,
				_sessionAuthenticationManager.Object,
				_accountServiceProxy.Object,
				 _securityService.Object, _tokenManagerMock.Object);
			_accountServiceProxy.Setup(x => x.EncryptPartner(_mdsid, "mercer")).Returns("991");
			_sessionAuthenticationManager.Setup(x => x.GetCellCode()).Returns("1234");

			//Act
			var result = _sut.GetUrlLink(SetupLinkModel());

			//Assert
			Assert.AreEqual(expectedResult, result.Url);
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void GetUrlLink_Should_Return_Url_MdsidMercer_Campcode() {
			//Arrange
			var expectedResult = "http://www.neamb.com?mdsid=991&code=1234";
			var contextItem = new DbItem("ContextItem", _contextItemId) {
				Name = "productamerican",
				Fields = {
					new DbField("Link", _ctaLinkItemId) {
						Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"www.neamb.com?mdsid=[mdsid_mercer]&amp;code=[campcode]\" anchor=\"\" target=\"_blank\" />"
					},
					new DbField("EligibilityItem", _eligibilityItemId) 
				}
			};
			_database.Add(contextItem);
			_sut = new MN.LinkActionTypeManager(_eligibilityManager.Object,
				_sessionAuthenticationManager.Object,
				_accountServiceProxy.Object,
				 _securityService.Object, _tokenManagerMock.Object);
			_accountServiceProxy.Setup(x => x.EncryptPartner(_mdsid, "mercer")).Returns("991");
			_sessionAuthenticationManager.Setup(x => x.GetCampaignCode()).Returns("1234");

			//Act
			var result = _sut.GetUrlLink(SetupLinkModel());

			//Assert
			Assert.AreEqual(expectedResult, result.Url);
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void GetUrlLink_Should_Return_Url_Materialid_Medium() {
			//Arrange
			var expectedResult = "http://www.neamb.com?mdsid=991&code=1234";
			var contextItem = new DbItem("ContextItem", _contextItemId) {
				Name = "productamerican",
				Fields = {
					new DbField("Link", _ctaLinkItemId) {
						Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"www.neamb.com?mdsid=[materialid]&amp;code=[medium]\" anchor=\"\" target=\"_blank\" />"
					},
					new DbField("EligibilityItem", _eligibilityItemId) 
				}
			};
			_database.Add(contextItem);
			_sut = new MN.LinkActionTypeManager(_eligibilityManager.Object,
				_sessionAuthenticationManager.Object,
				_accountServiceProxy.Object,
				 _securityService.Object, _tokenManagerMock.Object);
			_sessionAuthenticationManager.Setup(x => x.GetMedium()).Returns("1234");

			//Act
			var result = _sut.GetUrlLink(SetupLinkModel());

			//Assert
			Assert.AreEqual(expectedResult, result.Url);
		}

        [Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void GetUrlLink_Should_Return_Url_Sob()
        {
            //Arrange
            var expectedResult = "http://www.neamb.com?sob=1235";
            var contextItem = new DbItem("ContextItem", _contextItemId)
            {
                Name = "productamerican",
                Fields = {
                    new DbField("Link", _ctaLinkItemId) {
                        Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"www.neamb.com?sob=[sob]\" anchor=\"\" target=\"_blank\" />"
                    },
                    new DbField("EligibilityItem", _eligibilityItemId)
                }
            };
            _database.Add(contextItem);
            _sut = new MN.LinkActionTypeManager(_eligibilityManager.Object,
                _sessionAuthenticationManager.Object,
                _accountServiceProxy.Object,
                 _securityService.Object, _tokenManagerMock.Object);
            _sessionAuthenticationManager.Setup(x => x.GetSob()).Returns("1235");

            //Act
            var result = _sut.GetUrlLink(SetupLinkModel());

            //Assert
            Assert.AreEqual(expectedResult, result.Url);
        }

        [Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void GetUrlLink_Should_Return_Url_Gclid()
        {
            //Arrange
            var expectedResult = "http://www.neamb.com?gclid=1236";
            var contextItem = new DbItem("ContextItem", _contextItemId)
            {
                Name = "productamerican",
                Fields = {
                    new DbField("Link", _ctaLinkItemId) {
                        Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"www.neamb.com?gclid=[gclid]\" anchor=\"\" target=\"_blank\" />"
                    },
                    new DbField("EligibilityItem", _eligibilityItemId)
                }
            };
            _database.Add(contextItem);
            _sut = new MN.LinkActionTypeManager(_eligibilityManager.Object,
                _sessionAuthenticationManager.Object,
                _accountServiceProxy.Object,
                _securityService.Object, _tokenManagerMock.Object);
            _sessionAuthenticationManager.Setup(x => x.GetGclid()).Returns("1236");

            //Act
            var result = _sut.GetUrlLink(SetupLinkModel());

            //Assert
            Assert.AreEqual(expectedResult, result.Url);
        }

        [Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void GetUrlLink_Should_Return_Url_UtmTerm()
        {
            //Arrange
            var expectedResult = "http://www.neamb.com?utm_term=1237";
            var contextItem = new DbItem("ContextItem", _contextItemId)
            {
                Name = "productamerican",
                Fields = {
                    new DbField("Link", _ctaLinkItemId) {
                        Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"www.neamb.com?utm_term=[term]\" anchor=\"\" target=\"_blank\" />"
                    },
                    new DbField("EligibilityItem", _eligibilityItemId)
                }
            };
            _database.Add(contextItem);
            _sut = new MN.LinkActionTypeManager(_eligibilityManager.Object,
                _sessionAuthenticationManager.Object,
                _accountServiceProxy.Object,
               _securityService.Object, _tokenManagerMock.Object);
            _sessionAuthenticationManager.Setup(x => x.GetUtmTerm()).Returns("1237");

            //Act
            var result = _sut.GetUrlLink(SetupLinkModel());

            //Assert
            Assert.AreEqual(expectedResult, result.Url);
        }


        [Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void GetUrlLink_Should_Return_Url_MdsidClear() {
			//Arrange
			var expectedResult = "http://www.neamb.com?mdsid=991";
			var contextItem = new DbItem("ContextItem", _contextItemId) {
				Name = "productamerican",
				Fields = {
					new DbField("Link", _ctaLinkItemId) {
						Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"www.neamb.com?mdsid=[mdsid_clear]\" anchor=\"\" target=\"_blank\" />"
					},
					new DbField("EligibilityItem", _eligibilityItemId) 
				}
			};
			_database.Add(contextItem);
			_sut = new MN.LinkActionTypeManager(_eligibilityManager.Object,
				_sessionAuthenticationManager.Object,
				_accountServiceProxy.Object,
				 _securityService.Object, _tokenManagerMock.Object);

			//Act
			var result = _sut.GetUrlLink(SetupLinkModel());

			//Assert
			Assert.AreEqual(expectedResult, result.Url);
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void GetUrlLink_Should_Return_Url_MdsidAfinium() {
			//Arrange
			var expectedResult = "http://www.neamb.com?mdsid=334";
			var contextItem = new DbItem("ContextItem", _contextItemId) {
				Name = "productamerican",
				Fields = {
					new DbField("Link", _ctaLinkItemId) {
						Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"www.neamb.com?mdsid=[mdsid_afinium]\" anchor=\"\" target=\"_blank\" />"
					},
					new DbField("EligibilityItem", _eligibilityItemId) 
				}
			};
			_database.Add(contextItem);
			_sut = new MN.LinkActionTypeManager(_eligibilityManager.Object,
				_sessionAuthenticationManager.Object,
				_accountServiceProxy.Object,
				 _securityService.Object, _tokenManagerMock.Object);
			_accountServiceProxy.Setup(x => x.EncryptPartner(_mdsid, "afinium")).Returns("334");

			//Act
			var result = _sut.GetUrlLink(SetupLinkModel());

			//Assert
			Assert.AreEqual(expectedResult, result.Url);
		}
		#endregion

	}
}
