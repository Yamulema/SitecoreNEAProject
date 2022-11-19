using System;
using Moq;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services.Efulfillment;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using MN = Neambc.Neamb.Foundation.Product.Manager;

namespace Neambc.Neamb.Foundation.Product.UnitTest.Manager {
	[TestFixture]
	public class ProductManagerTest {

		#region Fields

		private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
		private Mock<IAccountServiceProxy> _serviceManagerMock;
		private Mock<ISessionAuthenticationManager> _sessionAuthenticationManagerMock;
		private Mock<IOracleDatabase> _oracleManagerMock;
		private Mock<ISessionManager> _sessionManagerMock;
        private Mock<IAmazonS3Repository> _amazonS3RepositoryMock;
        private Mock<ICacheManager> _cacheManagerMock;
        private Mock<ICacheManagerSeiumb> _cacheManagerSeiumbMock;
        private Mock<IEfulfillmentService> _efulfillmentMock;
        private Mock<IMdsLoggingManager> _mdsLoggingManager;
        private Mock<IAnalyticsManager> _analyticsManager;
        #endregion

        #region Pre/Post Actions

        [SetUp]
		public void SetUp() {
			// before each test
			_globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
			_serviceManagerMock = new Mock<IAccountServiceProxy>();
			_sessionAuthenticationManagerMock = new Mock<ISessionAuthenticationManager>();
			_oracleManagerMock = new Mock<IOracleDatabase>();
			_sessionManagerMock = new Mock<ISessionManager>();
            _amazonS3RepositoryMock = new Mock<IAmazonS3Repository>();
            _cacheManagerMock = new Mock<ICacheManager>();
            _cacheManagerSeiumbMock = new Mock<ICacheManagerSeiumb>();
            _efulfillmentMock = new Mock<IEfulfillmentService>();
            _mdsLoggingManager = new Mock<IMdsLoggingManager>();
            _analyticsManager = new Mock<IAnalyticsManager>();
        }

		[TearDown]
		public void TearDown() {
			// after each test
		}

		#endregion

		#region Tests

		[Test]
		public void GetPdfFile_Should_Return_Pdf_File() {
			//Arrange
			var materialId = "552";
			var file = new byte[120];
			var uniqueName = "id-1020";
			var mdsid = "991";
			var username = "Jessica";
			var custom1 = "";
			var custom2 = "";
			var accountUser = new AccountUserBase { Mdsid = mdsid, 
				Username = username, 
				Profile = new Profile {FirstName = "Jessica", 
										LastName = "Jones", 
										DateOfBirth = "02031994",
										StreetAddress = "Street1", 
										City = "New York", 
										StateCode = "NY", 
										ZipCode = "32007", 
										NeaMembershipType = "Premium"
				}};
            int.TryParse(accountUser.Mdsid, out int mdsidInt);
            var pdfRequest = new PdfRequest {
				ProductIemId = materialId,
				Email = accountUser.Username,
				PdTransDate = DateTime.Now.ToString("MM/dd/yyyy"),
				PdFirstName = accountUser.Profile.FirstName,
				PdLastName = accountUser.Profile.LastName,
				PdDob = accountUser.Profile.DateOfBirth,
				PdMdsid = mdsidInt,
				PdAddress = accountUser.Profile.StreetAddress,
				PdCity = accountUser.Profile.City,
				PdState = accountUser.Profile.StateCode,
				PdZip = accountUser.Profile.ZipCode,
				PdMemberType = accountUser.Profile.NeaMembershipType,
				Custom1 = custom1,
				Custom2 = custom2
			};
            var efulfillmentService = _efulfillmentMock.Object;
            var pdfManager = new PdfManager(_cacheManagerMock.Object,_globalConfigurationManagerMock.Object,_serviceManagerMock.Object,_amazonS3RepositoryMock.Object,_cacheManagerSeiumbMock.Object, efulfillmentService);
			
            _efulfillmentMock.Setup(x => x.GetPdfFile(pdfRequest)).Returns(file);

			var expectedResult = file;

            _amazonS3RepositoryMock.Setup(x => x.CreateObjectS3(It.IsAny<RequestS3>())).Returns(true);
			//Act
			var result = pdfManager.GetPdfFile( materialId, uniqueName, pdfRequest,"testBucket");

			//Assert
			Assert.AreEqual(expectedResult, result);
		}
		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_Product_Dimensions_WithNoValueSelected() {
			////Arrange
			var expectedValue = new ProductCustomDimension() {
				ProductCategory = ConstantsNeamb.ProductDimensionNone,
				ProductSubgroup = ConstantsNeamb.ProductDimensionNone,
				ProductSubcategory = ConstantsNeamb.ProductDimensionNone
			};

			using (var db = new Db {
				new DbItem("product1", new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")) {
					Fields = {
						new DbField("Category", new ID("{AD8E3BA4-0806-4278-B131-DFFA69F84BC3}")) {
							Value = ""
						},
						new DbField("SubCategory", new ID("{9696566F-F13B-4CCE-B532-E94BFC8C2227}")) {
							Value = ""
						},
						new DbField("SubGroup", new ID("{F4A6A478-CD21-4E0F-ADA0-B3E065D55EC2}")) {
							Value = ""
						},
					}
				}
			}) {
				{
					Sitecore.Data.Items.Item itemTest = db.GetItem(new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")); //("/sitecore/content/product1");

					var productManager =
						new MN.ProductManager(
                            _sessionAuthenticationManagerMock.Object,
                            _oracleManagerMock.Object,
							_sessionManagerMock.Object, _mdsLoggingManager.Object,_analyticsManager.Object);
					//Act
					var result = productManager.GetProductDimensions(itemTest);
					//Assert
					Assert.AreNotEqual(result, expectedValue);
				}
			}
		}

		[Test]
		[Ignore("Error in Fakedb to be fixed")]
		public void Return_Product_Dimensions_WithValuesSelected() {
			////Arrange
			string categoryTest = "category test";
			string subCategoryTest = "subcategory test";
			string subGroupTest = "subgroup test";

			var expectedValue = new ProductCustomDimension() {
				ProductCategory = categoryTest,
				ProductSubgroup = subCategoryTest,
				ProductSubcategory = subGroupTest
			};

			using (var db = new Db {
				new DbItem("product1", new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")) {
					Fields = {
						new DbField("Category", new ID("{AD8E3BA4-0806-4278-B131-DFFA69F84BC3}")) {
							Value = "{1EA33232-AC25-42E5-A550-6C9232F318EC}"
						},
						new DbField("SubCategory", new ID("{9696566F-F13B-4CCE-B532-E94BFC8C2227}")) {
							Value = "{2EA33232-AC25-42E5-A550-6C9232F318EC}"
						},
						new DbField("SubGroup", new ID("{F4A6A478-CD21-4E0F-ADA0-B3E065D55EC2}")) {
							Value = "{3EA33232-AC25-42E5-A550-6C9232F318EC}"
						},
					}
				},
				new DbItem("CategoryItem", new ID("{1EA33232-AC25-42E5-A550-6C9232F318EC}")) {
					new DbField("Category", new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}")) {
						Value = categoryTest
					},
				},
				new DbItem("CategoryItem2", new ID("{2EA33232-AC25-42E5-A550-6C9232F318EC}")) {
					new DbField("Category", new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}")) {
						Value = subCategoryTest
					},
				},
				new DbItem("CategoryItem3", new ID("{3EA33232-AC25-42E5-A550-6C9232F318EC}")) {
					new DbField("Category", new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}")) {
						Value = subGroupTest
					},
				}

			}) {
				{
					Sitecore.Data.Items.Item itemTest = db.GetItem(new ID("{39AD27F7-EE49-43EA-A80A-5C315E2BEE0E}")); //("/sitecore/content/product1");

					var productManager =
						new MN.ProductManager(
                            _sessionAuthenticationManagerMock.Object,
                            _oracleManagerMock.Object,
							_sessionManagerMock.Object, _mdsLoggingManager.Object, _analyticsManager.Object);
					//Act
					var result = productManager.GetProductDimensions(itemTest);
					//Assert
					Assert.AreNotEqual(result, expectedValue);
				}
			}
		}


		#endregion

	}
}
