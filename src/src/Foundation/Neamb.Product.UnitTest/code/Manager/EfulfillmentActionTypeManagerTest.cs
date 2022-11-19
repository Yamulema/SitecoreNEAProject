using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Manager;
using Neambc.Neamb.Foundation.Product.Model;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Product.UnitTest.Manager {
	[TestFixture]
	public class EfulfillmentActionTypeManagerTest {

		#region Fields

		private Mock<IEligibilityManager> _eligibilityManagerMock;
		private Mock<IProductManager> _productManagerMock;
		private EfulfillmentActionTypeManager _sut;
		private EfulfillmentModel _model;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private Mock<IPdfManager> _pdfManagerMock;
        #endregion

        #region Instrumentation

        [SetUp]
		public void SetUp() {
			_eligibilityManagerMock = new Mock<IEligibilityManager>();
			_productManagerMock = new Mock<IProductManager>();
            _pdfManagerMock= new Mock<IPdfManager>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
		}

		[TearDown]
		public void Teardown() {
			HttpContext.Current = null;
		}

		#endregion

		#region Private Methods
		private EfulfillmentModel SetupEfulfillmentModel(AccountUserBase aub, bool checkEligibility, string materialId, string productCode) {
			return new EfulfillmentModel {
				AccountUser = aub,
				CheckEligibility = checkEligibility,
				CheckLogin = false,
				MaterialId = materialId,
				ProductCode = productCode
			};
		}
		private AccountUserBase SetupAccountUserBase(string mdsid, Profile profile = null) {
			return new AccountUserBase {
				Mdsid = mdsid,
				Username = "Jenny",
				Profile = profile ?? new Profile()
			};
		}
		private EfulfillmentActionTypeManager SetupGetPdfFile(string mdsId, string productCode, int months, EligibilityResultEnum eligibility = EligibilityResultEnum.Eligible) {
			var ret = new EfulfillmentActionTypeManager(_eligibilityManagerMock.Object, _productManagerMock.Object,_pdfManagerMock.Object,_globalConfigurationManagerMock.Object);
			_eligibilityManagerMock.Setup(x => x.IsMemberEligible(mdsId, productCode, months)).Returns(eligibility);
			return ret;
		}
		
		private EfulfillmentResult SetupAll_GetPdfFile(
            string mdsId,
            string productCode,
            string materialId,
            int month,
            bool checkEligibility,
            string pdfUrl,
            ResultUrlEnum result,
            byte[] fileBytes,
            Profile profile = null,
            EligibilityResultEnum eligibility = EligibilityResultEnum.Eligible
        ) {
            _sut = SetupGetPdfFile(mdsId, productCode, month, eligibility);
            _model = SetupEfulfillmentModel(SetupAccountUserBase(mdsId, profile), checkEligibility, materialId, productCode);
            var uniqueName = $"Efulfillment:{mdsId}-{materialId}";
            
            _pdfManagerMock.Setup(x => x.VerifyExistencePdfFile(uniqueName,"testBucket",true)).Returns(fileBytes);
            _pdfManagerMock.Setup(x => x.GetPdfUrl(uniqueName,true)).Returns(pdfUrl);
            return new EfulfillmentResult
            {
                PdfSucessUrl = pdfUrl,ResultUrl = result
            };
        }
        #endregion

        #region GetPdfFile

        [Test]
        public void GetPdfFile_Should_Return_Pdf_File_When_User_Eligible_And_Exists_Previously()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "0013",
                12,
                true,
                "test.pdf",
                ResultUrlEnum.None,
                new byte[10]
            );
            _pdfManagerMock.Setup(x => x.GetPdfUrl(It.IsAny<string>(), It.IsAny<bool>())).Returns("test.pdf");
            _pdfManagerMock.Setup(x => x.VerifyExistencePdfFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new byte[10]);

            //Act
            var result = _sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
            Assert.AreEqual(expectedResult.Url, result.Url);
        }

        [Test]
        public void GetPdfFile_Should_Return_New_Pdf_File_When_User_Eligible_And_Not_Exists_Previously()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "0013",
                12,
                true,
                "test.pdf",
                ResultUrlEnum.None,
                new byte[10]
            );
            byte[] resultFile = null;
            _pdfManagerMock.Setup(x => x.GetPdfUrl(It.IsAny<string>(), It.IsAny<bool>())).Returns("test.pdf");
            _pdfManagerMock.Setup(x => x.VerifyExistencePdfFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(resultFile);
            _pdfManagerMock.Setup(x => x.GetPdfFile(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<PdfRequest>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>())).Returns(new byte[10]);

            //Act
            var result = _sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
        }

        [Test]
        public void GetPdfFile_Should_Return_New_Pdf_File_When_User_Eligible_And_Not_ExistsPreviously_625()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "625",
                12,
                true,
                "test.pdf",
                ResultUrlEnum.None,
                new byte[10],
                new Profile
                {
                    DateOfBirth = "03241994"
                }
            );
            byte[] resultFile = null;
            _pdfManagerMock.Setup(x => x.GetPdfUrl(It.IsAny<string>(), It.IsAny<bool>())).Returns("test.pdf");
            _pdfManagerMock.Setup(x => x.VerifyExistencePdfFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(resultFile);
            _pdfManagerMock.Setup(x => x.GetPdfFile(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<PdfRequest>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>())).Returns(new byte[10]);

            //Act
            var result = _sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
        }

        [Test]
        public void GetPdfFile_Should_Return_New_Pdf_File_When_User_Eligible_And_Not_ExistsPreviously_022()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "022",
                12,
                true,
                null,
                ResultUrlEnum.NoUrl,
                new byte[10]
            );

            //_pdfManagerMock.Setup(x => x.GetPdfFile(It.IsAny<string>(),
            //    It.IsAny<string>(),
            //    It.IsAny<PdfRequest>(),
            //    It.IsAny<string>(),
            //    It.IsAny<string>(),
            //    It.IsAny<string>(),
            //    It.IsAny<bool>())).Returns(new byte[10]);
            //Act
            var result = _sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
            Assert.AreEqual(expectedResult.Url, result.Url);
        }

        [Test]
        public void GetPdfFile_Should_Return_New_Pdf_File_When_User_Eligible_And_Not_ExistsPreviously_883()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "883",
                12,
                true,
                null,
                ResultUrlEnum.NoUrl,
                new byte[10],
                new Profile
                {
                    DateOfBirth = "09241994"
                }
            );

            //Act
            var result = _sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
            Assert.AreEqual(expectedResult.Url, result.Url);
        }

        [Test]
        public void GetPdfFile_Should_Return_UnForbidden_When_User_NotEligible()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "883",
                12,
                true,
                null,
                ResultUrlEnum.UnForbidden,
                new byte[10],
                new Profile
                {
                    DateOfBirth = "09241994"
                },
                EligibilityResultEnum.NotEligible
            );
            expectedResult = new EfulfillmentResult
            {
                ResultUrl = expectedResult.ResultUrl
            };

            //Act
            var result = _sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
            Assert.AreEqual(expectedResult.Url, result.Url);
        }

        [Test]
        public void GetPdfFile_Should_Return_NoUrl_When_User_File_Length_Not_Greater_than_0()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "883",
                12,
                false,
                null,
                ResultUrlEnum.NoUrl,
                new byte[0],
                new Profile
                {
                    DateOfBirth = "09241994"
                },
                EligibilityResultEnum.NotEligible
            );
            expectedResult = new EfulfillmentResult
            {
                PdfSucessUrl = expectedResult.PdfSucessUrl,
                ResultUrl = expectedResult.ResultUrl
            };

            //Act
            var result = _sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
            Assert.AreEqual(expectedResult.Url, result.Url);
        }

        [Test]
        public void GetPdfFile_Should_Return_Pdf_File_When_Length_Greater_than_0()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "883",
                12,
                false,
                null,
                ResultUrlEnum.NoUrl,
                new byte[] { 1, 2, 3, 4, 5 },
                new Profile
                {
                    DateOfBirth = "09241994"
                },
                EligibilityResultEnum.NotEligible
            );
            expectedResult = new EfulfillmentResult
            {
                PdfSucessUrl = expectedResult.PdfSucessUrl,
                ResultUrl = ResultUrlEnum.NoUrl
            };

            //Act
            var result = _sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
            Assert.AreEqual(expectedResult.Url, result.Url);
        }

        private class FakeEfulfillmentActionTypeManager : EfulfillmentActionTypeManager
        {
            public FakeEfulfillmentActionTypeManager(
                IEligibilityManager eligibilityManager,
                IProductManager productmanager,IPdfManager pdfManager, IGlobalConfigurationManager globalConfigurationManager) : base(eligibilityManager, productmanager,pdfManager,globalConfigurationManager)
            {
            }
            public DateTime CurrentLocalNowReturns { get; set; }
            protected override DateTime CurrentLocalNow()
            {
                return CurrentLocalNowReturns;
            }
        }
        [Test]
        public void GetPdfFile_Should_Return_Pdf_File_When_Date_Greater_Than_August()
        {
            //Arrange
            var expectedResult = SetupAll_GetPdfFile(
                "991",
                "2",
                "883",
                12,
                false,
                null,
                ResultUrlEnum.NoUrl,
                null,
                new Profile
                {
                    DateOfBirth = "09241994"
                },
                EligibilityResultEnum.NotEligible
            );
            expectedResult = new EfulfillmentResult
            {
                PdfSucessUrl = expectedResult.PdfSucessUrl,
                ResultUrl = ResultUrlEnum.NoUrl,
            };

            var sut = new FakeEfulfillmentActionTypeManager(
                _eligibilityManagerMock.Object,
                _productManagerMock.Object,_pdfManagerMock.Object,_globalConfigurationManagerMock.Object)
            {
                CurrentLocalNowReturns = new DateTime(DateTime.Now.Year, 10, DateTime.Now.Day)
            };

            //Act
            var result = sut.GetPdfFile(_model);

            //Assert
            Assert.AreEqual(expectedResult.PdfSucessUrl, result.PdfSucessUrl);
            Assert.AreEqual(expectedResult.ResultUrl, result.ResultUrl);
            Assert.AreEqual(expectedResult.Url, result.Url);
        }


        #endregion

    }

}


