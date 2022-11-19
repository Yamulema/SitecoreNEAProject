using System;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.ForgotUserName;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services.ForgotUserName;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.ForgotUserName
{
    [TestFixture]
    public class ForgotUserNameServiceTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private SUT.ForgotUserNameService _sut;
        private FakeLog _log;
        private ForgotUserNameData _forgotUserName;
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
            _sut = new SUT.ForgotUserNameService(_accessTokenService, _globalConfigurationManager, _mbcRestfulService, _log,_responseHandler);
            _forgotUserName = new ForgotUserNameData();
            SetUserDataForgotUserName();
        }

        public void SetUserDataForgotUserName() {
            _forgotUserName.FirstName = "Nadine";
            _forgotUserName.LastName = "Newt";
            _forgotUserName.ZipCode = "35801";
            _forgotUserName.Dob = "05291966";
            _forgotUserName.UnionId = 1;
        } 
        #endregion

        [Test]
        public void ForgotUserNameData_WithNoAccessToken() {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName, _forgotUserName.ZipCode,  _forgotUserName.Dob, _forgotUserName.UnionId));
        }

        [Test]
        public void ForgotUserNameData_WithErrorInputParameter()
        {
            TokenResponse tokenResponse = new TokenResponse{Data = new TokenModel{AccessToken = "texttoken"},Success = true};
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            _forgotUserName.FirstName = ""; // invalid firstname
            Assert.Throws<ArgumentException>(() => _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName,
               _forgotUserName.ZipCode, _forgotUserName.Dob, _forgotUserName.UnionId));
            SetUserDataForgotUserName();

            _forgotUserName.LastName = ""; // invalid lasttname
            Assert.Throws<ArgumentException>(() => _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName,
               _forgotUserName.ZipCode, _forgotUserName.Dob, _forgotUserName.UnionId));
            SetUserDataForgotUserName();

            _forgotUserName.ZipCode = ""; // invalid zip
            Assert.Throws<ArgumentException>(() => _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName,
               _forgotUserName.ZipCode, _forgotUserName.Dob, _forgotUserName.UnionId));
            SetUserDataForgotUserName();

            _forgotUserName.UnionId = -1; // invalid unionid
            Assert.Throws<ArgumentException>(() => _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName,
               _forgotUserName.ZipCode, _forgotUserName.Dob, _forgotUserName.UnionId));
            SetUserDataForgotUserName();

            _forgotUserName.Dob = ""; // invalid dob
            Assert.Throws<ArgumentException>(() => _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName,
               _forgotUserName.ZipCode, _forgotUserName.Dob, _forgotUserName.UnionId));
            SetUserDataForgotUserName();


        }

        [Test]
        public void ForgotUserNameData_ReturnsSucess()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var forgotUsernameModel = new ForgotUserNameResponseData {
                matchFound = true,
                username = "nea.nadine@gmail.com"
            
            };

            ForgotUserNameResponse forgotUsernameResponse = new ForgotUserNameResponse
            {
                Success = true,
                Data = forgotUsernameModel
            };
            RestResultDto<ForgotUserNameResponse> restResultDto = new RestResultDto<ForgotUserNameResponse> {
                Result = forgotUsernameResponse,
                Success = true
            };
            

            _mbcRestfulServiceMock.Setup(x => x.Post<ForgotUserNameResponse>(It.IsAny<RestRequestDto>())).Returns(restResultDto);
            SetUserDataForgotUserName();
            var result = _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName, _forgotUserName.ZipCode, _forgotUserName.Dob, _forgotUserName.UnionId);
            Assert.AreEqual(result.Data, forgotUsernameModel);
        }

        [Test]
        public void ForgotUserNameData_ReturnsErrorResult()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            ForgotUserNameResponse forgotUsernameResponse = new ForgotUserNameResponse
            {
                Success = false,
                Error = new RestError { Code = 123,Messages = new []{"error"}}
            };

            RestResultDto<ForgotUserNameResponse> restResultDto = new RestResultDto<ForgotUserNameResponse>
            {
                Result = forgotUsernameResponse,
                Success = true,
            };
            _responseHandlerMock.Setup(x => x.LogErrorResponse(forgotUsernameResponse.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            _mbcRestfulServiceMock.Setup(x => x.Post<ForgotUserNameResponse>(It.IsAny<RestRequestDto>())).Returns(restResultDto);
            SetUserDataForgotUserName();
            _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName, _forgotUserName.ZipCode, _forgotUserName.Dob, _forgotUserName.UnionId);
            Assert.AreEqual(_log.Entries.Count,2);
        }
        [Test]
        public void ForgotUserNameData_ReturnsErrorPost()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
      
            RestResultDto<ForgotUserNameResponse> restResultDto = new RestResultDto<ForgotUserNameResponse>
            {
                Result = null,
                Success = false,
            };


            _mbcRestfulServiceMock.Setup(x => x.Post<ForgotUserNameResponse>(It.IsAny<RestRequestDto>())).Returns(restResultDto);
            SetUserDataForgotUserName();
            var result = _sut.ForgotUserNameStatus(_forgotUserName.FirstName, _forgotUserName.LastName, _forgotUserName.ZipCode, _forgotUserName.Dob, _forgotUserName.UnionId);
            Assert.AreEqual(_log.Entries.Count, 1);
        }
    }
}
