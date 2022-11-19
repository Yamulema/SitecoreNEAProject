using System;
using System.Linq;
using System.Net;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.CancelResetToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services.CancelResetToken;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.CancelResetToken
{
    [TestFixture]
    public class CancelResetTokenServiceTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private SUT.CancelResetTokenService _sut;
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
            _responseHandlerMock = new Mock<IResponseHandler>();
            _responseHandler = _responseHandlerMock.Object;
            _log = new FakeLog();
            _sut = new SUT.CancelResetTokenService(_accessTokenService,_globalConfigurationManager, _mbcRestfulService,_log, _responseHandler);
            
        }

        #endregion

        [Test]
        public void CancelResetToken_WithNoAccessToken() {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.CancelResetToken("",0));
        }

        [Test]
        public void CancelResetToken_WithErrorInputParameter()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.CancelResetToken("", 0));
        }

        [Test]
        public void CancelResetToken_WithResponseWsNull()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var result =_sut.CancelResetToken("nea.jessica@gmail.com", 1);
            Assert.IsFalse(result);
        }

        [Test]
        public void CancelResetToken_WithErrorResponseWsExecution()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            RestResultDto<CancelResetTokenResponse> resultService = new RestResultDto<CancelResetTokenResponse> {
                ExceptionDetail = "Not found",
                StatusCode = HttpStatusCode.NotFound,
                Success = false
            };
            _mbcRestfulServiceMock.Setup(x => x.Post<CancelResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _sut.CancelResetToken("nea.jessica@gmail.com", 1);
            Assert.IsFalse(result);
        }

        [Test]
        public void CancelResetToken_WithResponseErrorFromWs()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            
            CancelResetTokenResponse cancelResetTokenResponse = new CancelResetTokenResponse
            {
                Success = false,
                Error = new RestError { Code = 12002, Messages = new []{ "Username not found" } }
            };
            RestResultDto<CancelResetTokenResponse> resultService = new RestResultDto<CancelResetTokenResponse>();
            resultService.Result = cancelResetTokenResponse;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<CancelResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            _responseHandlerMock.Setup(x => x.LogErrorResponse(cancelResetTokenResponse.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            var result = _sut.CancelResetToken("nea.jessica@gmail.com", 1);
            Assert.IsFalse(result);
            var entriesError = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesError, 2);
        }

        [Test]
        public void CancelResetToken_WithResponseWsNotNull()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var cancelResetTokenModel = new CancelResetTokenModel {
                Canceled = true
            };

            CancelResetTokenResponse cancelResetTokenResponse = new CancelResetTokenResponse
            {
                Success = true,
                Data = cancelResetTokenModel
            };
            RestResultDto<CancelResetTokenResponse> resultService = new RestResultDto<CancelResetTokenResponse>();
            resultService.Result = cancelResetTokenResponse;
            resultService.Success = true;
            
            _mbcRestfulServiceMock.Setup(x => x.Post<CancelResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.CancelResetToken("nea.jessica@gmail.com", 1);
            Assert.IsTrue(result);
            Assert.AreEqual(_log.Entries.Count, 0); //no errors
        }

    }
}
