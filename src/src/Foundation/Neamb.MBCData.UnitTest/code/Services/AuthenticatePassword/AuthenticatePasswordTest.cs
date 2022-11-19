using System;
using System.Linq;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.AuthenticatePassword;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.AuthenticatePassword;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.AuthenticatePassword
{
    [TestFixture]
    public class AuthenticatePasswordTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private AuthenticatePasswordService _sut;
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
            _sut = new AuthenticatePasswordService(_accessTokenService, _globalConfigurationManager, _mbcRestfulService, _log, _responseHandler);
        }
        #endregion

        [Test]
        public void AuthenticatePassword_WithNoAccessToken()
        {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.AuthenticatePasswordStatus("nea.sally@gmail.com", "secret12", 1));
        }

        [Test]
        public void AuthenticatePassword_WitInvalidInput()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            var response = new AuthenticatePasswordResponse
            {
                Success = false,
                Error = new RestError { Code = 10002, Messages = new[] { "Input validation failed" } }
            };
            var resultService = new RestResultDto<AuthenticatePasswordResponse>();
            resultService.Result = response;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<AuthenticatePasswordResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            _responseHandlerMock.Setup(x => x.LogErrorResponse(response.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            _sut.AuthenticatePasswordStatus("nea.sally", "secret12", 1);
            var entriesInfo = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesInfo, 2);
        }
        [Test]
        public void AuthenticatePassword_WithEmptyFields()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.AuthenticatePasswordStatus("", "", 1));
        }
        [Test]
        public void AuthenticatePassword_WithResponseOk()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var AuthenticatePasswordModel = new AuthenticatePasswordResponseData
            {
                authenticated = true
            };

            var response = new AuthenticatePasswordResponse
            {
                Success = true,
                Data = AuthenticatePasswordModel
            };
            var resultService = new RestResultDto<AuthenticatePasswordResponse>();
            resultService.Result = response;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<AuthenticatePasswordResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.AuthenticatePasswordStatus("nea.sally@gmail.com", "secret12", 1);
            Assert.AreEqual(result.Data, AuthenticatePasswordModel);
        }
    }
}
