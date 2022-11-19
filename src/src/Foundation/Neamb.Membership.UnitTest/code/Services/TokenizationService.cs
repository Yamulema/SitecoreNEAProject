using System;
using System.Collections.Generic;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.IceDollars;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Neamb.Foundation.MBCData.Services.IceDollars;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using SUT = Neambc.Neamb.Foundation.Membership.Services;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Services
{

    [TestFixture]
    public class TokenizationService
    {

        #region Fields
        protected ITokenizationService _tokenizationService;
        protected Mock<ISessionAuthenticationManager> _sessionAuthenticationManagerMock;
        protected ISessionAuthenticationManager _sessionAuthenticationManager;
        protected SUT.IConvertMdsToRewards _rewardService;
        protected IResourcesService _resourcesService;
        protected Mock<ISessionService> _sessionService;
        protected Mock<IIceDollarsService> _iceDollarsService;
        private Mock<ISeminarRepository> _seminaryRepository;
        protected Mock<IGlobalConfigurationManager> _globalConfiguration;
        protected Mock<IEligibilityManager> _eligibilityManager;
        protected Mock<IEligibilityOmni> _eligibilityOmni;
        protected Mock<IProductUtilityManager> _productUtilityManagerMock;
        private FakeRenderingSitecoreContext _renderingSitecoreContext;
        protected Mock<ITokenManager> _tokenManagerMock;
        #endregion

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            // set up default mock objects once 
            // tests can still create their own, but 
            // defaults are available, and kept if used
            _rewardService = new Mock<SUT.IConvertMdsToRewards>().Object;
            _resourcesService = new Mock<IResourcesService>().Object;
            _sessionService = new Mock<ISessionService>();
            _iceDollarsService = new Mock<IIceDollarsService>();
            _seminaryRepository = new Mock<ISeminarRepository>();
            _globalConfiguration = new Mock<IGlobalConfigurationManager>();
            _eligibilityManager = new Mock<IEligibilityManager>();
            _eligibilityOmni = new Mock<IEligibilityOmni>();
            _productUtilityManagerMock = new Mock<IProductUtilityManager>();
            _renderingSitecoreContext = new FakeRenderingSitecoreContext();
            _tokenManagerMock = new Mock<ITokenManager>();
        }

        [SetUp]
        public void SetUp()
        {
            _sessionAuthenticationManagerMock = new Mock<ISessionAuthenticationManager>();
            _sessionAuthenticationManager = _sessionAuthenticationManagerMock.Object;

            _tokenizationService = new Membership.Managers.TokenizationService(
                _rewardService,
                _resourcesService,
                _sessionAuthenticationManager,
                _sessionService.Object,
                _iceDollarsService.Object,
                _seminaryRepository.Object,
                _globalConfiguration.Object,
                _eligibilityManager.Object,
                _eligibilityOmni.Object,
                _productUtilityManagerMock.Object,
                _renderingSitecoreContext,
                _tokenManagerMock.Object
            );
        }

        [Test]
        public void Unmatched_Elements_Returns_Input()
        {
            // test all the possible negative inputs
            const string rawText = "<g34><h1>Test</h1></g33>";
            var result = _tokenizationService.DeTokenize(rawText);
            Assert.AreEqual(result, rawText);
        }

        [Test]
        public void Null_Returns_Input()
        {
            Assert.IsNull(_tokenizationService.DeTokenize(null));
        }

        [Test]
        public void Empty_Returns_Input()
        {
            Assert.IsEmpty(_tokenizationService.DeTokenize(string.Empty));
        }

        [Test]
        public void NoTokens_Returns_Input()
        {
            const string rawText = "<h1>Test</h1>";
            var result = _tokenizationService.DeTokenize(rawText);
            Assert.AreEqual(result, rawText);
        }

        [Test]
        public void EmptyTokens_Returns_Input()
        {
            const string rawText = "<h1>Test[]</h1>";
            var result = _tokenizationService.DeTokenize(rawText);
            Assert.AreEqual(result, rawText);
        }

        [Test]
        public void UnknownTokens_Returns_Input()
        {
            const string rawText = "<h1>Test[NoToken]</h1>";
            var result = _tokenizationService.DeTokenize(rawText);
            Assert.AreEqual(result, rawText);
        }

        [Test]
        public void FirstNameTokens_Replaced()
        {
            //Arrange
            var token = "[FirstName]";
            var firstName = "Andres";
            var rawText = $"<h1>Hello {token}</h1>";
            var expectedResult = $"<h1>Hello {firstName}</h1>";

            _sessionAuthenticationManagerMock
                .Setup(x => x.GetAccountMembership())
                .Returns(new AccountMembership()
                {
                    Profile = new Profile()
                    {
                        FirstName = firstName
                    }
                });

            //Act
            var result = _tokenizationService.DeTokenize(rawText);

            //Assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void ReturnUrlTokens_Replaced()
        {
            //Arrange
            var token = "[ReturnUrl]";
            var tokenValue = "/home";
            var rawText = $"<h1>Hello {token}</h1>";
            var expectedResult = $"<h1>Hello {tokenValue}</h1>";

            var mockSessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
            _sessionService
                .Setup(x => x.Get(Configuration.ReturnUrlArg))
                .Returns(tokenValue);

            //Act
            var result = _tokenizationService.DeTokenize(rawText);

            //Assert
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void IcePoints_Token_Replaced()
        {
            //Arrange
            var token = "[IcePoints]";
            var templateTable = "<table class=\"table table-responsive\" width=\"50%\"><thead><tr><th>Date</th><th>Action</th><th>Amount Earned</th></tr></thead><tbody></tbody></table>";
            var rawText = $"<h1>Hello {token}</h1>";

            _sessionAuthenticationManagerMock
                .Setup(x => x.GetAccountMembership())
                .Returns(new AccountMembership
                {
                    Mdsid = "123"
                });

            var mockRewardService = new Mock<SUT.IConvertMdsToRewards>();
            mockRewardService
                .Setup(x => x.Unredeemed("123"))
                .Returns(new List<Reward>()
                {
                    new Reward()
                    {
                        Date = new DateTime(2018,10,24),
                        Value = 100,
                        Name = "Earned",
                        Description = "Earned",
                        Mdsid = 1234
                    },
                    new Reward()
                    {
                        Date = new DateTime(2019,10,24),
                        Value = 200,
                        Name = "Earned",
                        Description = "Earned",
                        Mdsid = 1234
                    }
                });
            var mockResourcesService = new Mock<IResourcesService>();
            mockResourcesService
                .Setup(x => x.ReadTextResourceFromAssembly(string.Empty))
                .Returns(templateTable);

            var tokenizationService =
                new Membership.Managers.TokenizationService(
                    mockRewardService.Object,
                    mockResourcesService.Object,
                    _sessionAuthenticationManager,
                    _sessionService.Object,
                    _iceDollarsService.Object,
                    _seminaryRepository.Object,
                    _globalConfiguration.Object,
                    _eligibilityManager.Object,
                    _eligibilityOmni.Object,
                    _productUtilityManagerMock.Object,
                    _renderingSitecoreContext,
                    _tokenManagerMock.Object
                );

            //Act
            var result = tokenizationService.DeTokenize(rawText);

            //Assert: Check the entire result, since *any* behavior changes should trigger a test change

            Assert.AreEqual(result,
                "<h1>Hello <table class=\"table table-responsive\" width=\"50%\">" +
                "<thead><tr><th>Date</th><th>Action</th><th>Amount Earned</th></tr></thead>" +
                "<tbody><tr><td>&nbsp;10/24/2018</td><td>Earned</td><td style=\"text-align: center;\">100</td></tr>" +
                "<tr><td>&nbsp;10/24/2019</td><td>Earned</td><td style=\"text-align: center;\">200</td></tr>" +
                "<tr><td>&nbsp;Total</td><td>&nbsp;</td><td style=\"text-align: center;\">&nbsp;0</td></tr></tbody></table></h1>");
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void StateNames_Token_Replaced()
        {
            //Arrange
            var token = "[StateNames]";
            var rawText = $"<h1>Hello {token}</h1>";
            var expectedText = "<h1>Hello " +
                "<option value = \"AZ\">Arizona</option>" +
                Environment.NewLine +
                "<option value = \"MD\">Maryland</option>" +
                Environment.NewLine +
                "</h1>";

            using (var db = new Db {
                new DbItem("Global", new ID("{1C0CF0BA-D674-418E-A807-72EF1BA9359C}")) {
                    new DbItem("Arizona", new ID("{1C0CF0BA-D674-418E-A807-72EF1BA93591}")) {
                        {new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}"), "AZ"}
                    },
                    new DbItem("Maryland", new ID("{1C0CF0BA-D674-418E-A807-72EF1BA93592}")) {
                        {new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}"), "MD"}
                    },
                }
            })
            {
                //Act
                var result = _tokenizationService.DeTokenize(rawText);
                //Assert: Check the entire result, since *any* behavior changes should trigger a test change
                Assert.AreEqual(result, expectedText);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ReturnOmniExpirationDate_Replaced()
        {
            //Arrange
            var token = "[OmniExpirationDate]";
            var tokenValue = "30/06/2021";
            var rawText = $"<h1>Hello {token}</h1>";
            var expectedResult = $"<h1>Hello {tokenValue}</h1>";
            string mdsId = "999";
            string productCode = "486 01";
            IList<ViewOmni> omniResult = new List<ViewOmni>();
            omniResult.Add(new ViewOmni { WebEndDt = tokenValue, WebAppUrl = "https://www.firstnational.com/lynx/#/apply?promo=MP86ZT39VMF6W", WebSoctUrl = "https://www.firstnational.com/lynx/#/apply?promo=MP86ZT39VMF6W" });

            _eligibilityOmni.Setup(x => x.CheckEligibility(mdsId, productCode)).Returns(omniResult);
            _sessionService
                .Setup(x => x.Get(Configuration.ReturnUrlArg))
                .Returns(tokenValue);

            _sessionAuthenticationManagerMock
                .Setup(x => x.GetAccountMembership())
                .Returns(new AccountMembership
                {
                    Mdsid = mdsId
                });
            _productUtilityManagerMock.Setup(x => x.GetProductCode(It.IsAny<Item>(), It.IsAny<ID>())).Returns(productCode);
            using (var db = new Db {
                new DbItem("itemTest", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"))
            })
            {
                _renderingSitecoreContext.Current = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"));
                //Act
                var result = _tokenizationService.DeTokenize(rawText);
                //Assert
                Assert.AreEqual(result, expectedResult);
            }
        }

        [Test]
        public void IceDollars_Token_Replaced() {
            //Arrange
            var token = "[IceTravelDollars]";
            var tokenValue = "100";
            var rawText = $"<h1>Ice Travel Dollars {token}</h1>";
            var expectedResult = $"<h1>Ice Travel Dollars {tokenValue}</h1>";

            _sessionAuthenticationManagerMock
                .Setup(x => x.GetAccountMembership())
                .Returns(new AccountMembership
                {
                    Mdsid = "992"
                });

            var icePoints = new IceDollarsResponse()
            {
                Data = new IceDollarsModel {PointsBalance = 100},
                Success = true
            };
            _iceDollarsService.Setup(x => x.GetBalance(It.IsAny<int>()))
                .Returns(icePoints);

            //Act
            var result = _tokenizationService.DeTokenize(rawText);

            //Assert
            Assert.AreEqual(result, expectedResult);
        }
        [Test]
        public void NcesidTokens_Replaced()
        {
            //Arrange
            var token = "[NCESID]";
            var ncesId = "12345";
            var rawText = $"<h1>Hello {token}</h1>";
            var expectedResult = $"<h1>Hello {ncesId}</h1>";

            _tokenManagerMock
                .Setup(x => x.GetNcesId())
                .Returns(ncesId);

            //Act
            var result = _tokenizationService.DeTokenize(rawText);

            //Assert
            Assert.AreEqual(result, expectedResult);
        }
    }
}
