using System;
using System.Linq;
using System.Net;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.CreateResetToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.CreateResetToken
{
    [TestFixture]
    public class CreateResetTokenServiceTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private SUT.CreateResetTokenService _sut;
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
            _sut = new SUT.CreateResetTokenService(_accessTokenService,_globalConfigurationManager, _mbcRestfulService,_log,_responseHandler);
            
        }

        #endregion

        [Test]
        public void CreateResetToken_WithNoAccessToken() {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.CreateResetToken("",0));
        }

        [Test]
        public void CreateResetToken_WithErrorInputParameter()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.CreateResetToken("", 0));
        }

        [Test]
        public void CreateResetToken_WithResponseWsNull()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var result =_sut.CreateResetToken("nea.jessica@gmail.com", 1);
            Assert.IsNull(result);
            var entriesError= _log.Entries.Count(x => x.EntryType == FakeLog.ERROR);
            Assert.AreEqual(entriesError,1);
        }

        [Test]
        public void CreateResetToken_WithErrorResponseWsExecution()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            RestResultDto<CreateResetTokenResponse> resultService = new RestResultDto<CreateResetTokenResponse> {
                ExceptionDetail = "Not found",
                StatusCode = HttpStatusCode.NotFound,
                Success = false
            };
            _mbcRestfulServiceMock.Setup(x => x.Post<CreateResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _sut.CreateResetToken("nea.jessica@gmail.com", 1);
            Assert.IsNull(result);
            var entriesError = _log.Entries.Count(x => x.EntryType == FakeLog.ERROR);
            Assert.AreEqual(entriesError, 1);
        }

        [Test]
        public void CreateResetToken_WithResponseErrorFromWs()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            
            CreateResetTokenResponse createResetTokenResponse = new CreateResetTokenResponse
            {
                Success = false,
                Error = new RestError { Code = 12002, Messages = new []{ "Username not found" } }
            };
            RestResultDto<CreateResetTokenResponse> resultService = new RestResultDto<CreateResetTokenResponse>();
            resultService.Result = createResetTokenResponse;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<CreateResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            _responseHandlerMock.Setup(x => x.LogErrorResponse(createResetTokenResponse.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            var result = _sut.CreateResetToken("nea.jessica@gmail.com", 1);
            Assert.AreEqual(createResetTokenResponse, result);
            var entriesInfo = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesInfo, 2);
        }

        [Test]
        public void CreateResetToken_WithResponseWsNotNull()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var createResetTokenModel = new CreateResetTokenModel {
                FirstName = "Jessica",
                NewToken = true,
                ResetToken = "abc"
            };

            CreateResetTokenResponse createResetTokenResponse = new CreateResetTokenResponse
            {
                Success = true,
                Data = createResetTokenModel
            };
            RestResultDto<CreateResetTokenResponse> resultService = new RestResultDto<CreateResetTokenResponse>();
            resultService.Result = createResetTokenResponse;
            resultService.Success = true;
            
            _mbcRestfulServiceMock.Setup(x => x.Post<CreateResetTokenResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.CreateResetToken("nea.jessica@gmail.com", 1);
            Assert.AreEqual(createResetTokenResponse, result);
            Assert.AreEqual(_log.Entries.Count, 0); //no errors
        }

    }
}
