using System;
using System.Linq;
using System.Net;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.DeleteUser;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Model.ValidateEmailDomain;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services.DeleteUser;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.DeleteUser
{
    [TestFixture]
    public class DeleteUserTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private SUT.DeleteUserService _sut;
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
            _sut = new SUT.DeleteUserService(_accessTokenService, _globalConfigurationManager, _mbcRestfulService, _log, _responseHandler);
        }
        #endregion

        [Test]
        public void DeleteUser_WithNoAccessToken()
        {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.DeleteUserStatus("nea.sally@gmail.com", 1));
        }

        [Test]
        public void DeleteUser_WitInvalidInput()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            var response = new DeleteUserResponse
            {
                Success = false,
                Error = new RestError { Code = 10002, Messages = new[] { "Input validation failed" } }
            };
            var resultService = new RestResultDto<DeleteUserResponse>();
            resultService.Result = response;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<DeleteUserResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            _responseHandlerMock.Setup(x => x.LogErrorResponse(response.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            var result = _sut.DeleteUserStatus("nea.sally", 1);
            var entriesInfo = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesInfo, 2);
        }
        [Test]
        public void DeleteUser_WithEmptyFields()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.DeleteUserStatus("", 1));
        }
        [Test]
        public void deleteUser_WithResponseOk()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var deleteUserModel = new DeleteUserResponseData
            {
                deleted = true
            };

            var response = new DeleteUserResponse
            {
                Success = true,
                Data = deleteUserModel
            };
            var resultService = new RestResultDto<DeleteUserResponse>();
            resultService.Result = response;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<DeleteUserResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.DeleteUserStatus("nea.sally@gmail.com", 1);
            Assert.AreEqual(result.Data, deleteUserModel);
        }
    }
}
