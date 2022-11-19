using Moq;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.Product.UnitTest.Repositories
{
    [TestFixture]
    public class CtaActionRepositoryTest {
        #region Fields
        private Mock<ISessionManager> _sessionManager;
        private Mock<IProductGtmManager> _gtmManager;
        private Mock<IBaseUrlActionLink> _baseUrlActionLink;
        private Mock<IBaseUrlActionDatapass> _baseUrlActionDatapass;
        private Mock<IBaseUrlActionEfulfillment> _baseUrlActionEfulfillment;
        private Mock<IBaseUrlColdUser> _baseUrlColdUser;
        private Mock<IOracleDatabase> _oracleManager;
        private ICtaActionRepository _sut;
        private FakeLog _log;
        private Mock<IBaseUrlActionOmni> _baseUrlActionOmni;
        private Mock<IPageSitecoreContext> _pageSitecoreContext;

        #endregion

        #region Instrumentation

        [SetUp]
        public void Setup() {
            _sessionManager = new Mock<ISessionManager>();
            _gtmManager = new Mock<IProductGtmManager>();
            _baseUrlActionLink = new Mock<IBaseUrlActionLink>();
            _baseUrlActionDatapass = new Mock<IBaseUrlActionDatapass>();
            _baseUrlActionEfulfillment = new Mock<IBaseUrlActionEfulfillment>();
            _baseUrlColdUser = new Mock<IBaseUrlColdUser>();
            _oracleManager = new Mock<IOracleDatabase>();
            _log = new FakeLog();
            _baseUrlActionOmni = new Mock<IBaseUrlActionOmni>();
            _pageSitecoreContext = new Mock<IPageSitecoreContext>();

            _sut = new CtaActionRepository(
                _sessionManager.Object,
                _gtmManager.Object,
                _baseUrlActionLink.Object,
                _baseUrlActionDatapass.Object,
                _baseUrlActionEfulfillment.Object,
                _baseUrlColdUser.Object,
                _oracleManager.Object,
                _log, _baseUrlActionOmni.Object, 
                _pageSitecoreContext.Object);

        }

        #endregion

        #region Test

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckSetAnonymousData_ReturnsAnonymousData()
        {
            const string productName = "product name";
            const string baseUrl = "https://neamb.local/login";
            const string loginText = "Sign in to check eligibility";
            const string idItemLogin = "5EA33232-AC25-42E5-A550-6C9232F318EC";
            const string gtmAction = "gtm Action";
            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) {
                    Fields = {
                        new DbField("anonymousItemId", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EE}")) {
                            Value =
                                $"<link text=\"{loginText}\" linktype=\"internal\" url=\"\" anchor=\"\" title=\"\" class=\"\" querystring=\"\" target=\"\" id=\"{idItemLogin}\" />"
                        }
                    }
                },
                new DbItem("loginItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) { }
            })
            {
                _baseUrlColdUser.Setup(x => x.GetBaseUrlPartner()).Returns(baseUrl);
                _gtmManager.Setup(x => x.GetGtmFunction(ComponentTypeEnum.Anonymous,
                        db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")),
                        It.IsAny<string>(),
                        It.IsAny<ProductCtaBase>(), StatusEnum.Cold))
                    .Returns(gtmAction);

                var result = _sut.SetAnonymousData(new ID("{5EA33232-AC25-42E5-A550-6C9232F318EE}"),
                    db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")),
                    productName);
                Assert.AreEqual(loginText, result.Text);
                Assert.IsNotNull(result.Url);
                Assert.AreEqual(gtmAction, result.GtmAction);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckGetActionData_ReturnsError_WhenNull() {
            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) {
                    Fields = {
                        new DbField("CtaLinkItemId", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EE}")) { }
                    }
                },
                new DbItem("loginItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) { }
            }) {
                ActionRequest actionRequest = new ActionRequest {
                    RenderingItem = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")),
                    CtaLinkItemId = new ID("{5EA33232-AC25-42E5-A550-6C9232F318EA}")
                };
                var result = _sut.GetActionData(StatusEnum.Cold, actionRequest);
                Assert.IsTrue(result.HasError);
            }

        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckGetActionData_ReturnsEmpty_WhenNoTarget() {
            const string contextId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string ctaLinkItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";

            using (var db = new Db {
                new DbItem("renderingItem", new ID(contextId)) {
                    Fields = {
                        new DbField("CtaLinkItemId", new ID(ctaLinkItemId)) {
                            Value = $"<link text=\"test\" linktype=\"internal\" url=\"\" anchor=\"\" title=\"\" class=\"\" querystring=\"\" target=\"\" />"
                        }
                    }
                },
                new DbItem("loginItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) { }
            })
            {
                ActionRequest actionRequest = new ActionRequest
                {
                    RenderingItem = db.GetItem(new ID(contextId)),
                    CtaLinkItemId = new ID(ctaLinkItemId)
                };
                var result = _sut.GetActionData(StatusEnum.Cold, actionRequest);
                Assert.IsEmpty(result.Target);
                Assert.IsFalse(result.HasError);
            }

        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void CheckGetActionData_ReturnsTarget_WhenHasTarget()
        {
            const string contextId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string ctaLinkItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";

            using (var db = new Db {
                new DbItem("renderingItem", new ID(contextId)) {
                    Fields = {
                        new DbField("CtaLinkItemId", new ID(ctaLinkItemId)) {
                            Value = $"<link text=\"test\" linktype=\"internal\" url=\"\" anchor=\"\" title=\"\" class=\"\" querystring=\"\" target=\"blank\" />"
                        }
                    }
                },
                new DbItem("loginItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318EC}")) { }
            })
            {
                ActionRequest actionRequest = new ActionRequest
                {
                    RenderingItem = db.GetItem(new ID(contextId)),
                    CtaLinkItemId = new ID(ctaLinkItemId)
                };
                var result = _sut.GetActionData(StatusEnum.Cold, actionRequest);
                Assert.IsNotEmpty(result.Target);
                Assert.AreEqual(result.Target, "_blank");
                Assert.IsFalse(result.HasError);
            }

        }

        [Test]
        public void GetActionResult_ReturnsError_WhenRenderingItemIsNull() {
            ActionRequest actionRequest = new ActionRequest();
            var result = _sut.GetActionResult(StatusEnum.Cold, actionRequest);
            Assert.IsTrue(result.HasError);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetActionResult_ReturnsError_WhenNoStatusUser()
        {
            const string contextId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string ctaLinkItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";
            const string ctaTypeItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EF}";
            const string ctaTypeItemIdRef = "{5EA33232-AC25-42E5-A550-6C9232F318EC}";
            const string PostDataItemId = "{fd6c04b3-ad4a-4411-b569-147bb695ed82}";

            using (var db = new Db {
                new DbItem("renderingItem", new ID(contextId)) {
                    Fields = {
                        new DbField("CtaLinkItemId", new ID(ctaLinkItemId)) {
                            Value = $"<link text=\"test\" linktype=\"internal\" url=\"\" anchor=\"\" title=\"\" class=\"\" querystring=\"\" target=\"blank\" />"
                        },
                        new DbField("CtaTypeItemId", new ID(ctaTypeItemId)) {
                            Value = $"{ctaTypeItemIdRef}"
                        },
                        new DbField("PostDataItemId", new ID(PostDataItemId)) {
                            Value = $"p1:v1\r\nmds_id:[mdsid_clear]"
                        }
                    }
                },
                new DbItem("ctaTypeItemIdRef", new ID(ctaTypeItemIdRef)) {
                    Fields = {
                        new DbField("CtaLinkTypeValue", new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}")) {
                            Value = $"Link"
                        }
                }
            }}) {
                ActionRequest actionRequest = new ActionRequest {
                    CtaLinkItemId = new ID(ctaLinkItemId),
                    RenderingItem = db.GetItem(new ID(contextId)),
                    CtaTypeItemId = new ID(ctaTypeItemId),
                    Model = new ProductDetailDTO{HasCheckEligibility = true},
                    PostDataItemId = new ID(PostDataItemId)
                };
                var result = _sut.GetActionResult(StatusEnum.Duplicated, actionRequest);
                Assert.IsTrue(result.HasError);
            }
            
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetActionResult_ReturnsActionClick_WhenStatusUserHot()
        {
            const string contextId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string ctaLinkItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";
            const string ctaTypeItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EF}";
            const string ctaTypeItemIdRef = "{5EA33232-AC25-42E5-A550-6C9232F318EC}";
            const string PostDataItemId = "{fd6c04b3-ad4a-4411-b569-147bb695ed82}";
            const string componentId = "50DFADA8382944D9A9EB2B835EB28624";
            const string productCode = "705 07";
            const string baseUrl = "http://www.amig.com/insurance/collector-car/";
            const string gtmResult = "dataLayerPush({'event':'product cta','productName':'NEA Antique & Classic Auto Insurance','ctaText':'Get a Quote Now','clickHref':'http://www.amig.com/insurance/collector-car/'},this);";
            const string clickAction =
                "trackingGoalProduct('{5EA33232-AC25-42E5-A550-6C9232F318ED}','');executelink50DFADA8382944D9A9EB2B835EB28624('{5EA33232-AC25-42E5-A550-6C9232F318EE}','{5EA33232-AC25-42E5-A550-6C9232F318ED}','705 07','','0', {p1: 'v1',mds_id: '[mdsid_clear]'});operationprocedureactioncta('705 07');return false";
            string expectedActionOnClick = $"{gtmResult}{clickAction}";

            using (var db = new Db {
                new DbItem("NEA Antique and Classic Auto Insurance", new ID(contextId)) {
                    Fields = {
                        new DbField("CtaLinkItemId", new ID(ctaLinkItemId)) {
                            Value = $"<link text=\"Get a Quote Now\" linktype=\"external\" url=\"http://www.amig.com/insurance/collector-car/ \" anchor=\"\" target=\"_blank\" />"
                        },
                        new DbField("CtaTypeItemId", new ID(ctaTypeItemId)) {
                            Value = $"{ctaTypeItemIdRef}"
                        },
                        new DbField("PostDataItemId", new ID(PostDataItemId)) {
                            Value = $"p1:v1\r\nmds_id:[mdsid_clear]"
                        }
                    }
                },
                new DbItem("ctaTypeItemIdRef", new ID(ctaTypeItemIdRef)) {
                    Fields = {
                        new DbField("CtaLinkTypeValue", new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}")) {
                            Value = $"Link"
                        }
                    }
                }})
            {
                ActionRequest actionRequest = new ActionRequest {
                    CtaLinkItemId = new ID(ctaLinkItemId),
                    RenderingItem = db.GetItem(new ID(contextId)),
                    CtaTypeItemId = new ID(ctaTypeItemId),
                    Model = new ProductDetailDTO { HasCheckEligibility = true },
                    ProductCode = productCode,
                    ComponentId = componentId,
                    ComponentType = ComponentTypeEnum.Cta,
                    PostDataItemId = new ID(PostDataItemId)
                };
                _baseUrlActionLink.Setup(x => x.GetBaseUrlPartner(db.GetItem(new ID(contextId)), new ID(ctaLinkItemId))).Returns(baseUrl);
                _gtmManager.Setup(x => x.GetGtmFunction(ComponentTypeEnum.Cta, db.GetItem(new ID(contextId)), It.IsAny<string>(), It.IsAny<ProductCtaBase>(), StatusEnum.Hot))
                    .Returns(gtmResult);
                var result = _sut.GetActionResult(StatusEnum.Hot, actionRequest);
                Assert.AreEqual(expectedActionOnClick,result.ActionClick);
                Assert.IsFalse(result.HasError);
            }

        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetActionResult_ReturnsActionEfulfillment_WhenStatusUserHot()
        {
            const string contextId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string ctaLinkItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";
            const string ctaTypeItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EF}";
            const string ctaTypeItemIdRef = "{5EA33232-AC25-42E5-A550-6C9232F318EC}";
            const string PostDataItemId = "{fd6c04b3-ad4a-4411-b569-147bb695ed82}";
            const string componentId = "772235CC0972465BAD331FC0D1AB00DA";
            const string productCode = "600 01";
            const string baseUrl = "/api/ProductRoute/DownloadEfulfillmentPdfMultirow";
            const string gtmResult = "dataLayerPush({'event':'product cta','productName':'NEA Home Financing Program','ctaText':'Print Membership Form','clickHref':'/api/ProductRoute/DownloadEfulfillmentPdfMultirow'},this);";
            const string clickAction =
                "trackingGoalProduct('{5EA33232-AC25-42E5-A550-6C9232F318ED}','');downloadpdf772235CC0972465BAD331FC0D1AB00DA('120', '600 01','Efulfillment772235CC0972465BAD331FC0D1AB00DA','0');return false";
            string expectedActionOnClick = $"{gtmResult}{clickAction}";

            using (var db = new Db {
                new DbItem("NEA Home Financing Program", new ID(contextId)) {
                    Fields = {
                        new DbField("CtaLinkItemId", new ID(ctaLinkItemId)) {
                            Value = $"<link text=\"Print Membership Form\" linktype=\"external\" url=\"\" anchor=\"\" target=\"_blank\" />"
                        },
                        new DbField("CtaTypeItemId", new ID(ctaTypeItemId)) {
                            Value = $"{ctaTypeItemIdRef}"
                        },
                        new DbField("PostDataItemId", new ID(PostDataItemId)) {
                            Value = $"p1:v1\r\nmds_id:[mdsid_clear]"
                        }
                    }
                },
                new DbItem("ctaTypeItemIdRef", new ID(ctaTypeItemIdRef)) {
                    Fields = {
                        new DbField("CtaLinkTypeValue", new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}")) {
                            Value = $"Efulfillment"
                        }
                    }
                }})
            {
                ActionRequest actionRequest = new ActionRequest
                {
                    CtaLinkItemId = new ID(ctaLinkItemId),
                    RenderingItem = db.GetItem(new ID(contextId)),
                    CtaTypeItemId = new ID(ctaTypeItemId),
                    Model = new ProductDetailDTO { HasCheckEligibility = true },
                    ProductCode = productCode,
                    ComponentId = componentId,
                    ComponentType = ComponentTypeEnum.Cta,
                    PostDataItemId = new ID(PostDataItemId)
                };
                _baseUrlActionLink.Setup(x => x.GetBaseUrlPartner(db.GetItem(new ID(contextId)), new ID(ctaLinkItemId))).Returns(baseUrl);
                _gtmManager.Setup(x => x.GetGtmFunction(ComponentTypeEnum.Cta, db.GetItem(new ID(contextId)), It.IsAny<string>(), It.IsAny<ProductCtaBase>(), StatusEnum.Hot))
                    .Returns(gtmResult);
                _oracleManager.Setup(x => x.SelectItemCodeForProductCode(productCode)).Returns("120");
                var result = _sut.GetActionResult(StatusEnum.Hot, actionRequest);
                Assert.AreEqual(expectedActionOnClick, result.ActionClick);
                Assert.IsFalse(result.HasError);
            }

        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void GetActionResult_ReturnsErrorEfulfillment_WhenStatusUserHot()
        {
            const string contextId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string ctaLinkItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EE}";
            const string ctaTypeItemId = "{5EA33232-AC25-42E5-A550-6C9232F318EF}";
            const string ctaTypeItemIdRef = "{5EA33232-AC25-42E5-A550-6C9232F318EC}";
            const string PostDataItemId = "{fd6c04b3-ad4a-4411-b569-147bb695ed82}";

            const string componentId = "772235CC0972465BAD331FC0D1AB00DA";
            const string productCode = "600 01";
            const string baseUrl = "/api/ProductRoute/DownloadEfulfillmentPdfMultirow";
            const string gtmResult = "dataLayerPush({'event':'product cta','productName':'NEA Home Financing Program','ctaText':'Print Membership Form','clickHref':'/api/ProductRoute/DownloadEfulfillmentPdfMultirow'},this);";
            const string clickAction =
                "downloadpdf772235CC0972465BAD331FC0D1AB00DA('120', '600 01','Efulfillment772235CC0972465BAD331FC0D1AB00DA');return false";
            string expectedActionOnClick = $"{gtmResult}{clickAction}";

            using (var db = new Db {
                new DbItem("NEA Home Financing Program", new ID(contextId)) {
                    Fields = {
                        new DbField("CtaLinkItemId", new ID(ctaLinkItemId)) {
                            Value = $"<link text=\"Print Membership Form\" linktype=\"external\" url=\"\" anchor=\"\" target=\"_blank\" />"
                        },
                        new DbField("CtaTypeItemId", new ID(ctaTypeItemId)) {
                            Value = $"{ctaTypeItemIdRef}"
                        },
                        new DbField("PostDataItemId", new ID(PostDataItemId)) {
                            Value = $"p1:v1\r\nmds_id:[mdsid_clear]"
                        }
                    }
                },
                new DbItem("ctaTypeItemIdRef", new ID(ctaTypeItemIdRef)) {
                    Fields = {
                        new DbField("CtaLinkTypeValue", new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}")) {
                            Value = $"Efulfillment"
                        }
                    }
                }})
            {
                ActionRequest actionRequest = new ActionRequest
                {
                    CtaLinkItemId = new ID(ctaLinkItemId),
                    RenderingItem = db.GetItem(new ID(contextId)),
                    CtaTypeItemId = new ID(ctaTypeItemId),
                    Model = new ProductDetailDTO { HasCheckEligibility = true },
                    ProductCode = productCode,
                    ComponentId = componentId,
                    ComponentType = ComponentTypeEnum.Cta,
                    PostDataItemId = new ID(PostDataItemId)
                };
                _baseUrlActionLink.Setup(x => x.GetBaseUrlPartner(db.GetItem(new ID(contextId)), new ID(ctaLinkItemId))).Returns(baseUrl);
                _gtmManager.Setup(x => x.GetGtmFunction(ComponentTypeEnum.Cta, db.GetItem(new ID(contextId)), It.IsAny<string>(), It.IsAny<ProductCtaBase>(), StatusEnum.Hot))
                    .Returns(gtmResult);
                var result = _sut.GetActionResult(StatusEnum.Hot, actionRequest);
                Assert.IsTrue(result.HasError);
            }

        }

        #endregion
    }

}

