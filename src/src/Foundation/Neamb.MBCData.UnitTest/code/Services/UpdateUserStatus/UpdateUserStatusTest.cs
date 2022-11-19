using System;
using System.Linq;
using System.Net;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Model.UpdateUserStatus;
using Neambc.Neamb.Foundation.MBCData.Model.ValidateResetToken;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services.UpdateUserStatus;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.UpdateUserStatus
{
    [TestFixture]
    public class UpdateUserStatusTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private SUT.UpdateUserStatusService _sut;
        private FakeLog _log;
        private int statusCodeNumber = (int)UserStatus.Default;
        private string statusCode;
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
            _sut = new SUT.UpdateUserStatusService(_accessTokenService,_globalConfigurationManager, _mbcRestfulService,_log,_responseHandler);
            statusCode = statusCodeNumber.ToString();
    }

        #endregion

        [Test]
        public void UpdateUserStatus_WithNoAccessToken() {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.UpdateUserStatus("test", statusCode, (int)Union.NEA));
        }

        [Test]
        public void UpdateUserStatus_WithErrorInputParameter()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.UpdateUserStatus("", "", (int)Union.NEA));
        }

        [Test]
        public void UpdateUserStatus_WithErrorResponseWsExecution()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var resultService = new RestResultDto<UpdateUserStatusResponse>
            {
                ExceptionDetail = "Not found",
                StatusCode = HttpStatusCode.NotFound,
                Success = false
            };
            _mbcRestfulServiceMock.Setup(x => x.Post<UpdateUserStatusResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _sut.UpdateUserStatus("nea.jessica@gmail.com", statusCode, (int)Union.NEA);
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateUserStatus_WithResponseErrorFromWs()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            var response = new UpdateUserStatusResponse
            {
                Success = false,
                Error = new RestError { Code = 12002, Messages = new[] { "Username not found" } }
            };
            var resultService = new RestResultDto<UpdateUserStatusResponse>();
            resultService.Result = response;
            resultService.Success = true;
            _responseHandlerMock.Setup(x => x.LogErrorResponse(resultService.Result.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });

            _mbcRestfulServiceMock.Setup(x => x.Post<UpdateUserStatusResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _sut.UpdateUserStatus("nea.jessica@gmail.com", statusCode, (int)Union.NEA);
            Assert.IsFalse(result);
            var entriesInfo = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesInfo, 2);
        }

        [Test]
        public void UpdateUserStatusn_WithResponseOk()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var validateResetTokenModel = new UpdateUserStatusModel
            {
                Updated = true
            };

            var response = new UpdateUserStatusResponse
            {
                Success = true,
                Data = validateResetTokenModel
            };
            var resultService = new RestResultDto<UpdateUserStatusResponse>();
            resultService.Result = response;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<UpdateUserStatusResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.UpdateUserStatus("nea.jessica@gmail.com", statusCode, (int)Union.NEA);
            Assert.IsTrue(result);
        }
    }
}
