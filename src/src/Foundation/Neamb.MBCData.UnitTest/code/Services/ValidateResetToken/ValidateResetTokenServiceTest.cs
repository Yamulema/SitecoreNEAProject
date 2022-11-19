using System;
using System.Linq;
using System.Net;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Model.ValidateResetToken;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services.ValidateResetToken;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.ValidateResetToken
{
    [TestFixture]
    public class ValidateResetTokenServiceTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private SUT.ValidateResetTokenService _sut;
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
            _log = new FakeLog();
            _responseHandlerMock = new Mock<IResponseHandler>();
            _responseHandler = _responseHandlerMock.Object;
            _sut = new SUT.ValidateResetTokenService(_accessTokenService,_globalConfigurationManager, _mbcRestfulService,_log, _responseHandler);
        }

        #endregion

        [Test]
        public void ValidateResetToken_WithNoAccessToken() {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.ValidateResetToken("test","", (int)Union.NEA));
        }

        [Test]
        public void ValidateResetToken_WithErrorInputParameter()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.ValidateResetToken("", "", (int)Union.NEA));
        }

        [Test]
        public void ValidateResetToken_WithErrorResponseWsExecution()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var resultService = new RestResultDto<ValidateResetTokenResponse>
            {
                ExceptionDetail = "Not found",
                StatusCode = HttpStatusCode.NotFound,
                Success = false
            };
            _mbcRestfulServiceMock.Setup(x => x.Post<ValidateResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _sut.ValidateResetToken("nea.jessica@gmail.com", "tokenTest", (int)Union.NEA);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateResetToken_WithResponseErrorFromWs()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            var response = new ValidateResetTokenResponse
            {
                Success = false,
                Error = new RestError { Code = 12002, Messages = new[] { "Username not found" } }
            };
            var resultService = new RestResultDto<ValidateResetTokenResponse>();
            resultService.Result = response;
            resultService.Success = true;
            _responseHandlerMock.Setup(x => x.LogErrorResponse(resultService.Result.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            _mbcRestfulServiceMock.Setup(x => x.Post<ValidateResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _sut.ValidateResetToken("nea.jessica@gmail.com", "testToken", (int)Union.NEA);
            Assert.IsFalse(result);
            var entriesInfo = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesInfo, 2);
        }

        [Test]
        public void ValidateResetToken_WithResponseOk()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var validateResetTokenModel = new ValidateResetTokenModel
            {
                Valid = true
            };

            var response = new ValidateResetTokenResponse
            {
                Success = true,
                Data = validateResetTokenModel
            };
            var resultService = new RestResultDto<ValidateResetTokenResponse>();
            resultService.Result = response;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<ValidateResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.ValidateResetToken("nea.jessica@gmail.com", "textToken", (int)Union.NEA);
            Assert.IsTrue(result);
        }

    }
}
