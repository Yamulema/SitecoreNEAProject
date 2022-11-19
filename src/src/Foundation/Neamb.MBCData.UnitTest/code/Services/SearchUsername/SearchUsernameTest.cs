using System;
using System.Linq;
using System.Net;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Model.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.SearchUsername
{
    [TestFixture]
    public class SearchUsernameTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private SearchUserNameService _searchUserNameService;
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
            _searchUserNameService = new SearchUserNameService(
                _accessTokenService, 
                _globalConfigurationManager, 
                _mbcRestfulService, 
                _log,_responseHandler);
        }

        #endregion

        [Test]
        public void SearchUsername_WithNoAccessToken()
        {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _searchUserNameService.SearchUserName("mail@hotmail.com"));
        }

        [Test]
        public void SearchUsername_WithErrorInputParameter()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _searchUserNameService.SearchUserName(""));
        }

        [Test]
        public void SearchUsername_ReturnsErrorPost()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var resultService = new RestResultDto<SearchUserNameResponse>
            {
                Result = null,
                Success = false
            };
            _mbcRestfulServiceMock.Setup(x => x.Post<SearchUserNameResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _searchUserNameService.SearchUserName("nea.jessica@gmail.com");
            Assert.IsNull(result);
            Assert.AreEqual(_log.Entries.Count(x => x.EntryType == FakeLog.ERROR), 1);
        }

        [Test]
        public void SearchUsername_ReturnsErrorResult()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            var resultService = new RestResultDto<SearchUserNameResponse>
            {
                Result = new SearchUserNameResponse {
                    Error = new RestError { Code = 10002, Messages = new[] { "error" } }
                },
                Success = true
            };
            _responseHandlerMock.Setup(x => x.LogErrorResponse(resultService.Result.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            _mbcRestfulServiceMock.Setup(x => x.Post<SearchUserNameResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _searchUserNameService.SearchUserName("nea.jessica@gmail.com");

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.ErrorCode == SearchUsernameErrorCodes.InputDataValidationError);
            var entriesInfo = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesInfo, 2);
        }

        [Test]
        public void UpdateUserStatus_ReturnsSuccess()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            var resultService = new RestResultDto<SearchUserNameResponse> {
                Result = new SearchUserNameResponse {
                    Success = true,
                    Data = new SearchUserNameModel {
                        WebUserId = "",
                        MdsId = 999,
                        Registered = true
                    }
                },
                Success = true
            };

            _mbcRestfulServiceMock.Setup(x => x.Post<SearchUserNameResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _searchUserNameService.SearchUserName("nea.jessica@gmail.com");
            //Assert.IsTrue(result);
        }
    }
}
