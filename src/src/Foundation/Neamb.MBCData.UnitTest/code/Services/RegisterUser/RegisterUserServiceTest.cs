using System;
using System.Linq;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.RegisterUser;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.RegisterUser
{
    [TestFixture]
    public class RegisterUserServiceTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private RegisterUserService _sut;
        private FakeLog _log;
        private UserDataRegister _userDataRegister;
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
            _sut = new RegisterUserService(_accessTokenService,_globalConfigurationManager, _mbcRestfulService,_log, _responseHandler);
            _userDataRegister = new UserDataRegister();
        }

        public void SetUserDataRegister() {
            _userDataRegister.FirstName = "Nadine";
            _userDataRegister.LastName = "Newt";
            _userDataRegister.Dob = "05291964";
            _userDataRegister.City = "Huntsville";
            _userDataRegister.Password = "pwd";
            _userDataRegister.Phone = "3012519600";
            _userDataRegister.StateCode = "AL";
            _userDataRegister.StreetAddress = "891 Martins Creek Road";
            _userDataRegister.UnionId = 1;
            _userDataRegister.Username = "nea.nadine@gmail.com";
            _userDataRegister.ZipCode = "35801";
            _userDataRegister.Password = "pwd";
        }
        #endregion

        [Test]
        public void RegisterUserData_WithNoAccessToken() {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.RegisterUserData(_userDataRegister.FirstName, _userDataRegister.LastName,
                _userDataRegister.StreetAddress, _userDataRegister.City, _userDataRegister.StateCode, _userDataRegister.ZipCode,
                _userDataRegister.Dob, _userDataRegister.Phone, _userDataRegister.Username, _userDataRegister.Password, _userDataRegister.PermissionIndicator,
                _userDataRegister.Campcode, _userDataRegister.CellCode, _userDataRegister.UnionId, _userDataRegister.Webusersource));
        }

        [Test]
        public void RegisterUserData_WithErrorInputParameter()
        {
            TokenResponse tokenResponse = new TokenResponse{Data = new TokenModel{AccessToken = "texttoken"},Success = true};
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.RegisterUserData(_userDataRegister.FirstName, _userDataRegister.LastName,
                _userDataRegister.StreetAddress, _userDataRegister.City, _userDataRegister.StateCode, _userDataRegister.ZipCode,
                _userDataRegister.Dob, _userDataRegister.Phone, _userDataRegister.Username, _userDataRegister.Password, _userDataRegister.PermissionIndicator,
                _userDataRegister.Campcode, _userDataRegister.CellCode, _userDataRegister.UnionId, _userDataRegister.Webusersource));
        }

        [Test]
        public void RegisterUserData_ReturnsSucess()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var registerUserModel = new RegisterUserModel {
                Registered = true,
                WebuserId = "123"
            };

            RegisterUserResponse registerUserResponse = new RegisterUserResponse
            {
                Success = true,
                Data = registerUserModel
            };
            RestResultDto<RegisterUserResponse> restResultDto = new RestResultDto<RegisterUserResponse> {
                Result = registerUserResponse,
                Success = true
            };
            

            _mbcRestfulServiceMock.Setup(x => x.Post<RegisterUserResponse>(It.IsAny<RestRequestDto>())).Returns(restResultDto);
            SetUserDataRegister();
            var result = _sut.RegisterUserData(_userDataRegister.FirstName, _userDataRegister.LastName,
                _userDataRegister.StreetAddress, _userDataRegister.City, _userDataRegister.StateCode, _userDataRegister.ZipCode,
                _userDataRegister.Dob, _userDataRegister.Phone, _userDataRegister.Username, _userDataRegister.Password, _userDataRegister.PermissionIndicator,
                _userDataRegister.Campcode, _userDataRegister.CellCode, _userDataRegister.UnionId, _userDataRegister.Webusersource);
            Assert.AreEqual(result.Data, registerUserModel);
        }

        [Test]
        public void RegisterUserData_ReturnsErrorResult()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            
            RegisterUserResponse registerUserResponse = new RegisterUserResponse
            {
                Success = false,
                Error = new RestError { Code = 123,Messages = new []{"error"}}
            };

            RestResultDto<RegisterUserResponse> restResultDto = new RestResultDto<RegisterUserResponse>
            {
                Result = registerUserResponse,
                Success = true,
            };

            _responseHandlerMock.Setup(x => x.LogErrorResponse(registerUserResponse.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            _mbcRestfulServiceMock.Setup(x => x.Post<RegisterUserResponse>(It.IsAny<RestRequestDto>())).Returns(restResultDto);
            SetUserDataRegister();
            _sut.RegisterUserData(_userDataRegister.FirstName, _userDataRegister.LastName,
                _userDataRegister.StreetAddress, _userDataRegister.City, _userDataRegister.StateCode, _userDataRegister.ZipCode,
                _userDataRegister.Dob, _userDataRegister.Phone, _userDataRegister.Username, _userDataRegister.Password, _userDataRegister.PermissionIndicator,
                _userDataRegister.Campcode, _userDataRegister.CellCode, _userDataRegister.UnionId, _userDataRegister.Webusersource);
            Assert.AreEqual(_log.Entries.Count(item => item.EntryType == "info"), 2);
        }
        [Test]
        public void RegisterUserData_ReturnsErrorPost()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            
            RestResultDto<RegisterUserResponse> restResultDto = new RestResultDto<RegisterUserResponse>
            {
                Result = null,
                Success = false,
            };

            _mbcRestfulServiceMock.Setup(x => x.Post<RegisterUserResponse>(It.IsAny<RestRequestDto>())).Returns(restResultDto);
            SetUserDataRegister();
            _sut.RegisterUserData(_userDataRegister.FirstName, _userDataRegister.LastName,
                _userDataRegister.StreetAddress, _userDataRegister.City, _userDataRegister.StateCode, _userDataRegister.ZipCode,
                _userDataRegister.Dob, _userDataRegister.Phone, _userDataRegister.Username, _userDataRegister.Password, _userDataRegister.PermissionIndicator,
                _userDataRegister.Campcode, _userDataRegister.CellCode, _userDataRegister.UnionId, _userDataRegister.Webusersource);
            Assert.AreEqual(_log.Entries.Count(item=> item.EntryType== "error"), 1);
        }
    }
}
