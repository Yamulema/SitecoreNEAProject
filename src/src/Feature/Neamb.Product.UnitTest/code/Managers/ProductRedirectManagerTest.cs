using System.Collections.Generic;
using Moq;
using Neambc.Neamb.Feature.Product.Managers;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.Indexing.Models;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Manager;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using Sitecore.Common;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.Product.UnitTest.Managers
{
	[TestFixture]
	public class ProductRedirectManagerTest
    {
		#region Fields

		private Mock<ILinkActionTypeManager> _linkActionTypeManager;
        private Mock<IProductSearchManager> _productSearchManager;
        private Mock<IProductManager> _productManager;
        private FakeLog _log;
        private ProductRedirectManager _sut;

        #endregion

        [SetUp]
		public void SetUp() {
            _linkActionTypeManager = new Mock<ILinkActionTypeManager>();
            _productSearchManager = new Mock<IProductSearchManager>();
            _productManager = new Mock<IProductManager>();
            _log= new FakeLog();
            _sut= new ProductRedirectManager(_linkActionTypeManager.Object,_productSearchManager.Object,_log,_productManager.Object);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteProductRedirect_WithInputNull() {
            using (var db = new Db {
                new DbItem("home", new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}"))
            }) {
                var result= _sut.ExecuteProductRedirect(null, null);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.UrlRedirect);
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteProductRedirect_WithNullQueryParameter_AndNoErrorPage()
        {
            ProductRedirectRequest productRedirectRequest= new ProductRedirectRequest();

            using (var db = new Db {
                new DbItem("productRedirect", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")),
                new DbItem("home", new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}"))
            })
            {
                var result = _sut.ExecuteProductRedirect(productRedirectRequest, db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.UrlRedirect);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteProductRedirect_WithNullQueryParameter_WithErrorPage()
        {
            ProductRedirectRequest productRedirectRequest = new ProductRedirectRequest();

            using (var db = new Db {
                new DbItem("productRedirect", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")){
                    Name = "productamerican",
                    Fields = {
                        new DbField("Link", new ID("{011E4945-4005-480E-ACA2-75708BFAAA5E}")) {
                            Value = "<link text=\"American fidelity\" linktype=\"internal\" id=\"{E53FDA76-1219-4003-99BF-85314348B522}\" />"
                        }
                    }
                },
                new DbItem("home", new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}")),
                new DbItem("errorpage", new ID("{E53FDA76-1219-4003-99BF-85314348B522}"))
            })
            {
                var result = _sut.ExecuteProductRedirect(productRedirectRequest, db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.UrlRedirect);
                Assert.IsTrue(result.UrlRedirect.Contains("errorpage"));
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteProductRedirect_WithNoproductPagesFound() {
            string productCode = "710 01";
            ProductRedirectRequest productRedirectRequest = new ProductRedirectRequest {
                Mdsid = "999",
                ProductCode = productCode
            };

            using (var db = new Db {
                new DbItem("productRedirect", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")){
                    Name = "productamerican",
                    Fields = {
                        new DbField("Link", new ID("{011E4945-4005-480E-ACA2-75708BFAAA5E}")) {
                            Value = "<link text=\"American fidelity\" linktype=\"internal\" id=\"{E53FDA76-1219-4003-99BF-85314348B522}\" />"
                        }
                    }
                },
                new DbItem("home", new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}")),
                new DbItem("errorpage", new ID("{E53FDA76-1219-4003-99BF-85314348B522}"))
            })
            {
                ProductResult resultSearch= null;
                _productSearchManager.Setup(x => x.GetContentPages(productCode)).Returns(resultSearch);
                var result = _sut.ExecuteProductRedirect(productRedirectRequest, db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.UrlRedirect);
                Assert.IsTrue(result.UrlRedirect.Contains("errorpage"));
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteProductRedirect_WithProductPagesFoundButNoUrl()
        {
            string productCode = "710 01";
            ProductRedirectRequest productRedirectRequest = new ProductRedirectRequest
            {
                Mdsid = "999",
                ProductCode = productCode
            };

            using (var db = new Db {
                new DbItem("productRedirect", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")){
                    Name = "productRedirect",
                    Fields = {
                        new DbField("Link", new ID("{011E4945-4005-480E-ACA2-75708BFAAA5E}")) {
                            Value = "<link text=\"American fidelity\" linktype=\"internal\" id=\"{E53FDA76-1219-4003-99BF-85314348B522}\" />"
                        }
                    }
                },
                new DbItem("home", new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}")),
                new DbItem("errorpage", new ID("{E53FDA76-1219-4003-99BF-85314348B522}")),
                new DbItem("productmercer", new ID("{E53FDA76-1219-4003-99BF-85314348B523}"))
            })
            {
                var resultSearch= new ProductResult{ItemId = new ID("{E53FDA76-1219-4003-99BF-85314348B523}") };
                _productSearchManager.Setup(x => x.GetContentPages(productCode)).Returns(resultSearch);
                var result = _sut.ExecuteProductRedirect(productRedirectRequest, db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.UrlRedirect);
                Assert.IsTrue(result.UrlRedirect.Contains("errorpage"));
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteProductRedirect_WithProductPagesFound_WithNoLinkType()
        {
            string productCode = "710 01";
            string urlPartner = "https://partners.rentalcar.com/nea/";
            ProductRedirectRequest productRedirectRequest = new ProductRedirectRequest
            {
                Mdsid = "999",
                ProductCode = productCode
            };
            OperationResult operationResult = new OperationResult {
                Url = urlPartner
            };

            using (var db = new Db {
                new DbItem("productRedirect", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")){
                    Name = "productRedirect",
                    Fields = {
                        new DbField("Link", new ID("{011E4945-4005-480E-ACA2-75708BFAAA5E}")) {
                            Value = "<link text=\"American fidelity\" linktype=\"internal\" id=\"{E53FDA76-1219-4003-99BF-85314348B522}\" />"
                        }
                    }
                },
                new DbItem("home", new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}")),
                new DbItem("errorpage", new ID("{E53FDA76-1219-4003-99BF-85314348B522}")),
                new DbItem("productmercer", new ID("{E53FDA76-1219-4003-99BF-85314348B523}"))
            })
            {
                var resultSearch = new ProductResult { ItemId = new ID("{E53FDA76-1219-4003-99BF-85314348B523}") };
                _productSearchManager.Setup(x => x.GetContentPages(productCode)).Returns(resultSearch);
                _linkActionTypeManager.Setup(x => x.GetUrlLink(It.IsAny<LinkModel>())).Returns(operationResult);
                var result = _sut.ExecuteProductRedirect(productRedirectRequest, db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.UrlRedirect);
                Assert.IsTrue(result.UrlRedirect.Contains("errorpage"));
            }
        }
        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteProductRedirect_WithProductPagesFound_WithLinkType()
        {
            string productCode = "710 01";
            string urlPartner = "https://partners.rentalcar.com/nea/";
            ProductRedirectRequest productRedirectRequest = new ProductRedirectRequest
            {
                Mdsid = "999",
                ProductCode = productCode
            };
            OperationResult operationResult = new OperationResult
            {
                Url = urlPartner
            };

            using (var db = new Db {
                new DbItem("productRedirect", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")){
                    Name = "productRedirect",
                    Fields = {
                        new DbField("Link", new ID("{011E4945-4005-480E-ACA2-75708BFAAA5E}")) {
                            Value = "<link text=\"American fidelity\" linktype=\"internal\" id=\"{E53FDA76-1219-4003-99BF-85314348B522}\" />"
                        },
                        new DbField("Type", new ID("{011E4945-4005-480E-ACA2-75708BFAAA52}")) {
                            Value = "{AFD10856-2F11-4819-AD60-EEE6D184055F}"
                        }
                    }
                },
                new DbItem("home", new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}")),
                new DbItem("errorpage", new ID("{E53FDA76-1219-4003-99BF-85314348B522}")),
                new DbItem("productmercer", new ID("{E53FDA76-1219-4003-99BF-85314348B523}")){
                    Name = "productmercer",
                    Fields = {
                        new DbField("Type", new ID("{31379df6-1740-444f-a62b-d926ea9511ec}")) {
                            Value = "{AFD10856-2F11-4819-AD60-EEE6D184055F}"
                        }
                    }
                },
                new DbItem("catalogo", new ID("{AFD10856-2F11-4819-AD60-EEE6D184055F}")){
                    Fields = {
                        new DbField("Link", new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}")) {
                            Value = "Link"
                        }
                    }
                },
            })
            {
                var resultSearch = new ProductResult { ItemId = new ID("{E53FDA76-1219-4003-99BF-85314348B523}") };
                _productSearchManager.Setup(x => x.GetContentPages(productCode)).Returns(resultSearch);
                _linkActionTypeManager.Setup(x => x.GetUrlLink(It.IsAny<LinkModel>())).Returns(operationResult);
                var result = _sut.ExecuteProductRedirect(productRedirectRequest, db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")));
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.UrlRedirect);
                Assert.AreSame(result.UrlRedirect,urlPartner);
            }
        }
    }
}
