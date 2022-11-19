using System;
using System.Linq;
using System.Net;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Efulfillment;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services.Efulfillment;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.Efulfillment
{
    [TestFixture]
    public class EfulfillmentServiceTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private IBase64Service _base64Service;
        private Mock<IBase64Service> _base64ServiceMock;
        private SUT.EfulfillmentService _sut;
        private FakeLog _log;
        private Mock<IResponseHandler> _responseHandlerMock;
        private IResponseHandler _responseHandler;
        #endregion

        #region Instrumentation

        [SetUp]
        public void SetUp()
        {
            _accessTokenServiceMock = new Mock<IAccessTokenService>();
            _accessTokenService = _accessTokenServiceMock.Object;
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _globalConfigurationManager = _globalConfigurationManagerMock.Object;
            _mbcRestfulServiceMock = new Mock<IMBCRestfulService>();
            _mbcRestfulService = _mbcRestfulServiceMock.Object;
            _base64ServiceMock = new Mock<IBase64Service>();
            _base64Service = _base64ServiceMock.Object;
            _log = new FakeLog();
            _responseHandlerMock = new Mock<IResponseHandler>();
            _responseHandler = _responseHandlerMock.Object;
            _sut = new SUT.EfulfillmentService(_accessTokenService,_globalConfigurationManager, _mbcRestfulService,_log, _base64Service, _responseHandler);
            
        }

        #endregion

        [Test]
        public void GetPdf_WithNoAccessToken() {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.GetPdfFile(null));
        }

        [Test]
        public void GetPdf_WithErrorInputParameter()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.GetPdfFile(null));
        }
        private PdfRequest GetPdfData() {
            var materialId = "552";
            var mdsid = "991";
            var username = "Jessica";
            var custom1 = "";
            var custom2 = "";
            string firstName = "Jessica";
            string lastName = "Jones";
            string dateOfBirth = "02031994";
            string streetAddress = "Street1";
            string city = "New York";
            string stateCode = "NY";
            string zipCode = "32007";
            string neaMembershipType = "Premium";


            int.TryParse(mdsid, out int mdsidInt);
            var pdfRequest = new PdfRequest
            {
                ProductIemId = materialId,
                Email = username,
                PdTransDate = DateTime.Now.ToString("MM/dd/yyyy"),
                PdFirstName = firstName,
                PdLastName = lastName,
                PdDob = dateOfBirth,
                PdMdsid = mdsidInt,
                PdAddress = streetAddress,
                PdCity = city,
                PdState = stateCode,
                PdZip = zipCode,
                PdMemberType = neaMembershipType,
                Custom1 = custom1,
                Custom2 = custom2
            };
            return pdfRequest;
        }

        [Test]
        public void GetPdf_WithResponseWsNull()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            var result = _sut.GetPdfFile(GetPdfData());
            var entriesError = _log.Entries.Count(x => x.EntryType == FakeLog.ERROR);
            Assert.AreEqual(entriesError, 1);
            Assert.IsNull(result);
        }

        [Test]
        public void GetPdf_WithErrorResponseWsExecution()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            RestResultDto<EfulfillmentResponse> resultService = new RestResultDto<EfulfillmentResponse>
            {
                ExceptionDetail = "Not found",
                StatusCode = HttpStatusCode.NotFound,
                Success = false
            };
            _mbcRestfulServiceMock.Setup(x => x.Post<EfulfillmentResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.GetPdfFile(GetPdfData());
            var entriesError = _log.Entries.Count(x => x.EntryType == FakeLog.ERROR);
            Assert.AreEqual(entriesError, 1);
            Assert.IsNull(result);

        }

        [Test]
        public void GetPdf_WithResponseErrorFromWs()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            EfulfillmentResponse response = new EfulfillmentResponse {
                Success = false,
                Error = new RestError {
                    Code = 12002,
                    Messages = new[] {
                        "Username not found"
                    }
                }
            };

            RestResultDto <EfulfillmentResponse> resultService = new RestResultDto<EfulfillmentResponse>
            {
                Result = response,
                Success = true
            };
            _responseHandlerMock.Setup(x => x.LogErrorResponse(response.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            _mbcRestfulServiceMock.Setup(x => x.Post<EfulfillmentResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.GetPdfFile(GetPdfData());
            var entriesInfo = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesInfo, 2);
            Assert.IsNull(result);
        }

        [Test]
        public void GetPdf_WithResponseOkFromWs() {
            const string encodedStr = "DEgMCBvYmoKPDwvRmlsdGVyL0ZsYXRlRGVjb2RlL0xlbmd0aCAxMD4+c3RyZWF";
            byte[] document= new byte[120];
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            EfulfillmentResponse response = new EfulfillmentResponse
            {
                Success = true,
                Data = new EfulfillmentModel {
                    EncodedString = encodedStr,
                    EncodedType = "Base64",
                    FileType = "pdf"
                }
            };

            RestResultDto<EfulfillmentResponse> resultService = new RestResultDto<EfulfillmentResponse>
            {
                Result = response,
                Success = true
            };
            _mbcRestfulServiceMock.Setup(x => x.Post<EfulfillmentResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            _base64ServiceMock.Setup(x => x.ConvertBytes(encodedStr)).Returns(document);
            var result = _sut.GetPdfFile(GetPdfData());
            Assert.AreEqual(result, document);
        }
    }
}
