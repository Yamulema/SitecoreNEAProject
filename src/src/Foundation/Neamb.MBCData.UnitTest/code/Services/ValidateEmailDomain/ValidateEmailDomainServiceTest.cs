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
using Neambc.Neamb.Foundation.MBCData.Model.ValidateEmailDomain;
using Neambc.Neamb.Foundation.MBCData.Model.ValidateResetToken;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.MBCData.Services.ValidateEmailDomain;
namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.ValidateEmailDomain
{
    [TestFixture]
    public class ValidateEmailDomainTest
    {
        #region Fields
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        private IGlobalConfigurationManager _globalConfigurationManager;
        private IAccessTokenService _accessTokenService;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private IMBCRestfulService _mbcRestfulService;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;
        private SUT.ValidateEmailDomainService _sut;
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
            _sut = new SUT.ValidateEmailDomainService(_accessTokenService, _globalConfigurationManager, _mbcRestfulService, _log, _responseHandler);
            
        }

        #endregion

        [Test]
        public void ValidateEmailDomain_WithNoAccessToken()
        {
            TokenResponse tokenResponse = null;
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.ValidateEmailDomainStatus ("nea.sally@gmail.com"));
        }

        [Test]
        public void ValidateEmailDomain_WitInvalidInput()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);

            var response = new ValidateEmailDomainResponse
            {
                Success = false,
                Error = new RestError { Code = 10002, Messages = new[] { "Input validation failed" } }
            };
            var resultService = new RestResultDto<ValidateEmailDomainResponse>();
            resultService.Result = response;
            resultService.Success = true;
            _responseHandlerMock.Setup(x => x.LogErrorResponse(resultService.Result.Error, It.IsAny<string>(), _log))
                .Callback((RestError errorResponse, string method, ILog logService) => {
                    logService.Info("Error 1", this);
                    logService.Info("Error 2", this);
                });
            _mbcRestfulServiceMock.Setup(x => x.Post<ValidateEmailDomainResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);
            var result = _sut.ValidateEmailDomainStatus("nea.sally");
            var entriesInfo = _log.Entries.Count(x => x.EntryType == FakeLog.INFO);
            Assert.AreEqual(entriesInfo, 2);
        }
        [Test]
        public void ValidateEmailDomain_WithEmptyInput()
        {
            var tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            Assert.Throws<ArgumentException>(() => _sut.ValidateEmailDomainStatus(""));
        }
        [Test]
        public void ValidateEmailDomain_WithResponseOk()
        {
            TokenResponse tokenResponse = new TokenResponse { Data = new TokenModel { AccessToken = "texttoken" }, Success = true };
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenResponse);
            var ValidateEmailDomainModel = new ValidateEmailDomainResponseData
            {
                valid = true
            };

            var response = new ValidateEmailDomainResponse
            {
                Success = true,
                Data = ValidateEmailDomainModel
            };
            var resultService = new RestResultDto<ValidateEmailDomainResponse>();
            resultService.Result = response;
            resultService.Success = true;

            _mbcRestfulServiceMock.Setup(x => x.Post<ValidateEmailDomainResponse>(It.IsAny<RestRequestDto>())).Returns(resultService);

            var result = _sut.ValidateEmailDomainStatus("nea.sally@gmail.com");
            Assert.AreEqual(result.Data, ValidateEmailDomainModel);
        }
    }
}
