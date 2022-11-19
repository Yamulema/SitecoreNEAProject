using System.Linq;
using Moq;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Cache.Managers;
using NUnit.Framework;
using Neambc.UnitTesting.Base.Fakes;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.Product.UnitTest.Repositories
{
    [TestFixture]
    public class ComingSoonRepositoryTest {
        #region Fields

        private Mock<IOracleDatabase> _oracleManager;
        private Mock<IProductGtmManager> _gtmManager;
        private Mock<IBaseUrlColdUser> _baseUrlColdUser;
        private Mock<IBaseUrlComingSoon> _baseUrlComingSoon;
        private Mock<ISessionManager> _sessionManager;
        private IComingSoonRepository _sut;
        private FakeLog _log;

        #endregion

        #region Instrumentation

        [SetUp]
        public void Setup() {
            _oracleManager = new Mock<IOracleDatabase>();
            _gtmManager = new Mock<IProductGtmManager>();
            _baseUrlColdUser = new Mock<IBaseUrlColdUser>();
            _baseUrlComingSoon = new Mock<IBaseUrlComingSoon>();
            _sessionManager = new Mock<ISessionManager>();
            _log = new FakeLog();
            _sut = new ComingSoonRepository(_oracleManager.Object,
                _gtmManager.Object,
                _baseUrlColdUser.Object,
                _sessionManager.Object,
                _baseUrlComingSoon.Object,
                _log);

        }

        #endregion

        #region Test

        [Test]
        public void CheckVerifyAlreadyNotified_ReturnsTrue() {
            ComingSoonRequest request = new ComingSoonRequest {
                ProductCode = "600",
                Mdsid = "995"
            };
            int logCount = 1;
            _oracleManager.Setup(x => x.ReminderLogCount(request.ProductCode, request.Mdsid)).Returns(logCount);
            var result = _sut.VerifyAlreadyNotified(request);
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckVerifyAlreadyNotified_ReturnsFalse() {
            ComingSoonRequest request = new ComingSoonRequest {
                ProductCode = "600",
                Mdsid = "995"
            };
            int logCount = 0;
            _oracleManager.Setup(x => x.ReminderLogCount(request.ProductCode, request.Mdsid)).Returns(logCount);
            var result = _sut.VerifyAlreadyNotified(request);
            Assert.IsFalse(result);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckSetPropertiesColdUser_ReturnsColdData() {
            StatusEnum statusEnum = StatusEnum.Cold;
            const string baseUrlCold =
                "dataLayerPush({'event':'login cta','productName':'productcomingsoon','ctaText':'Notify Me When Available','clickHref':'http://neamb.local/productcomingsoon'},this);";
            const string reminderCtaId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";
            const string renderingId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            ComponentTypeEnum componentType = ComponentTypeEnum.Cta;
            string actionResult = "action result";
            const string productName = "product name";

            using (var db = new Db {
                new DbItem("login", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) { },
                new DbItem("renderingItem", new ID(renderingId)) {
                    Fields = {
                        new DbField("CtaId", new ID(reminderCtaId)) {
                            Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"\" anchor=\"\" target=\"_blank\" />"
                        }
                    }
                }
            })
            {
                ComingSoonRequest comingSoonRequest = new ComingSoonRequest
                {
                    ProductName = productName,
                    RenderingItem = db.GetItem(new ID(renderingId)),
                    ReminderCtaId = new ID(reminderCtaId),
                    ComponentType = componentType
                };
                ComponentTypeEnum capturedArg1 = ComponentTypeEnum.Cta;
                Item capturedArg2 = null;
                string capturedArg3 = "";
                ProductCtaBase capturedArg4 = null;
                StatusEnum capturedArg5 = StatusEnum.Cold;

                _baseUrlComingSoon.Setup(x => x.GetBaseUrlPartner(It.IsAny<string>())).Returns(actionResult);
                _gtmManager.Setup(x => x.GetGtmFunction(componentType,
                        comingSoonRequest.RenderingItem,
                        It.IsAny<string>(),
                        It.IsAny<ProductCtaBase>(), statusEnum))
                    .Callback((
                        ComponentTypeEnum componentTypeEnum,
                        Item item,
                        string input,
                        ProductCtaBase productCta,
                        StatusEnum userStatus
                    ) => {
                        capturedArg1 = componentTypeEnum;
                        capturedArg2 = item;
                        capturedArg3 = input;
                        capturedArg4 = productCta;
                        capturedArg5 = userStatus;
                    })
                    .Returns(baseUrlCold);
                var result = _sut.GetPropertiesUser(statusEnum, comingSoonRequest);
                var expectedBaseUrl = $"executeloginwarm('primary');{baseUrlCold}";
                Assert.AreEqual(expectedBaseUrl, result.Action);
                Assert.IsFalse(result.HasError);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckSetPropertiesWarmUser_ReturnsWarmData() {
            StatusEnum statusEnum = StatusEnum.WarmHot;
            const string componentId = "ED54B67592894817A6BED007D0C7C2AA";
            const string baseUrlWarm =
                "dataLayerPush({'event':'login cta','productName':'productcomingsoon','ctaText':'Notify Me When Available','clickHref':'http://neamb.local/productcomingsoon'},this);";
            const string reminderCtaId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";
            const string renderingId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            ComponentTypeEnum componentType = ComponentTypeEnum.Cta;
            string actionResult = "action result";
            const string productName = "product name";

            using (var db = new Db {
                new DbItem("login", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) { },
                new DbItem("renderingItem", new ID(renderingId)) {
                    Fields = {
                        new DbField("CtaId", new ID(reminderCtaId)) {
                            Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"\" anchor=\"\" target=\"_blank\" />"
                        }
                    }
                }
            }) {
                ComingSoonRequest comingSoonRequest = new ComingSoonRequest {
                    ProductName = productName,
                    RenderingItem = db.GetItem(new ID(renderingId)),
                    ReminderCtaId = new ID(reminderCtaId),
                    ComponentType = componentType,
                    UserName = "nea.owen@gmail.com",
                    ComponentId = componentId
                };
                ComponentTypeEnum capturedArg1 = ComponentTypeEnum.None;
                Item capturedArg2 = null;
                string capturedArg3 = "";
                ProductCtaBase capturedArg4 = null;
                StatusEnum capturedArg5 = StatusEnum.WarmHot;

                _baseUrlComingSoon.Setup(x => x.GetBaseUrlPartner(It.IsAny<string>())).Returns(actionResult);
                _gtmManager.Setup(x => x.GetGtmFunction(componentType,
                        comingSoonRequest.RenderingItem,
                        It.IsAny<string>(),
                        It.IsAny<ProductCtaBase>(), statusEnum))
                    .Callback((
                        ComponentTypeEnum componentTypeEnum,
                        Item item,
                        string input,
                        ProductCtaBase productCta,
                        StatusEnum userStatus
                    ) => {
                        capturedArg1 = componentTypeEnum;
                        capturedArg2 = item;
                        capturedArg3 = input;
                        capturedArg4 = productCta;
                        capturedArg5 = userStatus;
                    })
                    .Returns(baseUrlWarm);
                var result = _sut.GetPropertiesUser(statusEnum, comingSoonRequest);
                var expectedBaseUrl = $"executeloginwarm('primary{componentId}');{baseUrlWarm}";
                Assert.AreEqual(expectedBaseUrl, result.Action);
                Assert.AreEqual(componentType, capturedArg1);
                Assert.AreEqual(db.GetItem(new ID(renderingId)), capturedArg2);
                Assert.AreEqual(actionResult, capturedArg3);
                Assert.AreEqual(productName, capturedArg4.ProductName);
                Assert.IsFalse(result.HasError);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckSetPropertiesHotUser_ReturnsHotData() {
            StatusEnum statusEnum = StatusEnum.Hot;
            const string componentId = "ED54B67592894817A6BED007D0C7C2AA";
            const string baseUrlHot =
                "dataLayerPush({'event':'product cta','productName':'productcomingsoon','ctaText':'Notify Me When Available','clickHref':'http://neamb.local/productcomingsoon'},this);";
            const string reminderCtaId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";
            const string renderingId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string eligibilityId = "{5EA33232-AC25-42E5-A550-6C9232F318EF}";
            ComponentTypeEnum componentType = ComponentTypeEnum.Cta;
            string actionResult = "action result";
            const string productName = "product name";

            using (var db = new Db {
                new DbItem("login", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) { },
                new DbItem("renderingItem", new ID(renderingId)) {
                    Fields = {
                        new DbField("CtaId", new ID(reminderCtaId)) {
                            Value = "<link text=\"American fidelity\" linktype=\"external\" url=\"\" anchor=\"\" target=\"_blank\" />"
                        },
                        new DbField("Eligibility", new ID(eligibilityId)) {
                            Value = "1"
                        }
                    }
                }
            }) {
                ComingSoonRequest comingSoonRequest = new ComingSoonRequest {
                    ProductName = productName,
                    RenderingItem = db.GetItem(new ID(renderingId)),
                    ReminderCtaId = new ID(reminderCtaId),
                    ComponentType = componentType,
                    UserName = "nea.owen@gmail.com",
                    ComponentId = componentId,
                    EligibilityItemId= new ID(eligibilityId)
                };
                ComponentTypeEnum capturedArg1 = ComponentTypeEnum.None;
                Item capturedArg2 = null;
                string capturedArg3 = "";
                ProductCtaBase capturedArg4 = null;
                StatusEnum capturedArg5 = StatusEnum.Hot;

                _baseUrlComingSoon.Setup(x => x.GetBaseUrlPartner(It.IsAny<string>())).Returns(actionResult);
                _gtmManager.Setup(x => x.GetGtmFunction(componentType,
                        comingSoonRequest.RenderingItem,
                        It.IsAny<string>(),
                        It.IsAny<ProductCtaBase>(), statusEnum))
                    .Callback((
                        ComponentTypeEnum componentTypeEnum,
                        Item item,
                        string input,
                        ProductCtaBase productCta, 
                        StatusEnum userStatus
                    ) => {
                        capturedArg1 = componentTypeEnum;
                        capturedArg2 = item;
                        capturedArg3 = input;
                        capturedArg4 = productCta;
                        capturedArg5 = userStatus;
                    })
                    .Returns(baseUrlHot);
                var result = _sut.GetPropertiesUser(statusEnum, comingSoonRequest);
                var expectedBaseUrl = $"notifyproductavailable{componentId}('','{renderingId}','{eligibilityId}');return false;{baseUrlHot}";
                Assert.AreEqual(expectedBaseUrl, result.Action);
                Assert.AreEqual(componentType, capturedArg1);
                Assert.AreEqual(db.GetItem(new ID(renderingId)), capturedArg2);
                Assert.AreEqual(actionResult, capturedArg3);
                Assert.AreEqual(productName, capturedArg4.ProductName);
                Assert.IsFalse(result.HasError);
            }
        }

        [Test]
        public void CheckSetPropertiesUserUnknown_ReturnsError() {
            StatusEnum statusEnum = StatusEnum.Duplicated;
            var result = _sut.GetPropertiesUser(statusEnum, new ComingSoonRequest());
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(_log.Entries.Count, 1);
            Assert.IsNotNull(_log.Entries.First(x=> x.EntryType== "warn"));
        }

        #endregion

    }
}
