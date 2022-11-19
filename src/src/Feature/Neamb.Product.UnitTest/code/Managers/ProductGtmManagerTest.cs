using Moq;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Product.Manager;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.Product.UnitTest.Managers
{
	[TestFixture]
	public class ProductGtmManagerTest {
		#region Fields

		private Mock<IGtmService> _gtmService;
        private FakePageSitecoreContext _pageSitecoreContext;

        #endregion

        [SetUp]
		public void SetUp() {
			_gtmService = new Mock<IGtmService>();
            _pageSitecoreContext = new FakePageSitecoreContext();
		}

        [Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_ClickAction_ComponentTypeCta_WhenProductCtaNotNull() {
            using (var db = new Db {
                new DbItem("itemTest", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"))
            }) {
                _pageSitecoreContext.Current = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var productGtmManager = new ProductGtmManager(_gtmService.Object, _pageSitecoreContext);
                var expectedValue = "dataLayerPush({'event':'product cta','productName':'productamerican','ctaText':'American fidelity'})";

                ProductCtaBase productCta = new ProductCtaBase
                {
                    CtaText = "American fidelity",
                    Event = "product cta",
                    ProductName = "productamerican"
                };
                _gtmService.Setup(x => x.GetGtmEvent(productCta)).Returns(expectedValue);
                var result = productGtmManager.GetGtmFunction(ComponentTypeEnum.Cta, null, "", productCta);
                Assert.AreEqual(result, expectedValue);
            }
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_ClickActionEmpty_ComponentTypeCta_WhenProductCtaIsNull()
		{
            var templateId = new ID("5FF6A507-7D14-4EE5-B85B-12082E2C85D7");
            using (var db = new Db {
                new DbItem("itemTest", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"),templateId)
            }) {
                _pageSitecoreContext.Current = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var productGtmManager = new ProductGtmManager(_gtmService.Object, _pageSitecoreContext);
                var expectedValue = "";
                var result = productGtmManager.GetGtmFunction(ComponentTypeEnum.Cta, null, null);
                Assert.AreEqual(result, expectedValue);
            }
        }

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_ClickAction_ComponentTypeAnonymous_WhenProductCtaNotNull() {
            using (var db = new Db {
                new DbItem("itemTest", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"))
            }) {
                _pageSitecoreContext.Current = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"));
                var productGtmManager = new ProductGtmManager(_gtmService.Object, _pageSitecoreContext);
                var expectedValue = "dataLayerPush({'event':'login cta','productName':'productamerican','ctaText':'Sign in'})";

                ProductCtaBase productCta = new ProductCtaBase
                {
                    CtaText = "Sign in",
                    Event = "login cta",
                    ProductName = "productamerican"
                };
                _gtmService.Setup(x => x.GetGtmEvent(productCta)).Returns(expectedValue);
                var result = productGtmManager.GetGtmFunction(ComponentTypeEnum.Anonymous, null, "", productCta);
                Assert.AreEqual(result, expectedValue);
            }
        }

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_ClickAction_ComponentTypeMultirow_WhenProductCtaNotNull()
		{
			var expectedValue = "dataLayerPush({'event':'multi-offer card','cardTitle':'Click & save|American fidelity','ctaText':'description'})";
			using (var db = new Db {
                new DbItem("itemTest", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")),
                new DbItem("product1", new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")) {
					Name = "productamerican",
					Fields = {
						new DbField("Link", new ID("{D6DAB03D-D771-4C76-A6C0-7C48F10FE0BE}")) { Value="<link text=\"American fidelity\" linktype=\"external\" url=\"\" anchor=\"\" target=\"_blank\" />" },
						new DbField("Title", new ID("{759B52DE-E620-4A28-8498-B9965B37216B}")){ Value="Click & save"},
						new DbField("Description", new ID("{9E269846-70E9-4A47-BF53-87B0262E933B}")){ Value="description"},
					}
				}
			}) {
                _pageSitecoreContext.Current = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"));
                _gtmService.Setup(x => x.GetGtmEvent(It.IsAny<object>())).Returns(expectedValue);

				var productGtmManager = new ProductGtmManager(_gtmService.Object,_pageSitecoreContext);
				Sitecore.Data.Items.Item itemTest = db.GetItem(new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")); //("/sitecore/content/product1");

				var result = productGtmManager.GetGtmFunction(ComponentTypeEnum.MultiRow, itemTest, null);
				Assert.AreEqual(result, expectedValue);
			}
			
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_ClickAction_ComponentTypeOfferLink_WhenProductCtaNotNull()
		{
			var expectedValue = "dataLayerPush({'event':'account','accountSection':'manage products and services','accountAction':'product','ctaText':'Test american'})";
			using (var db = new Db {
                new DbItem("itemTest", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")),
				new DbItem("product1", new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")) {
					Name = "productamerican",
					Fields = {
						new DbField("Link", new ID("{D6DAB03D-D771-4C76-A6C0-7C48F10FE0BE}")) { Value="<link text=\"American fidelity\" linktype=\"external\" url=\"\" anchor=\"\" target=\"_blank\" />" },
						new DbField("Title", new ID("{759B52DE-E620-4A28-8498-B9965B37216B}")){ Value="Click & save"},
						new DbField("Description", new ID("{9E269846-70E9-4A47-BF53-87B0262E933B}")){ Value="description"},
					}
				}
			})
			{
                _pageSitecoreContext.Current = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"));
                _gtmService.Setup(x => x.GetGtmEvent(It.IsAny<object>())).Returns(expectedValue);
				
				var productGtmManager = new ProductGtmManager(_gtmService.Object,_pageSitecoreContext);
				Sitecore.Data.Items.Item itemTest = db.GetItem(new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")); //("/sitecore/content/product1");

				var result = productGtmManager.GetGtmFunction(ComponentTypeEnum.OfferLink, itemTest, null);
				Assert.AreEqual(result, expectedValue);
			}

		}
	}
}
