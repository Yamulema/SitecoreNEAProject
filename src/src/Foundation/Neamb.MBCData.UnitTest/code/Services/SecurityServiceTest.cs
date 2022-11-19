using System;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.ApiSecurity;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.SecurityManagement;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services
{
    public class SecurityServiceTest
    {
        #region Fields
        private SecurityService _sut;
        private Mock<IAccessTokenService>  _accessTokenServiceMock;
        private Mock<IGlobalConfigurationManager>  _configMock;
        private Mock<IMBCRestfulService> _mbcRestfulServiceMock;

        private FakeLog _log;
        #endregion

        [SetUp]
        public void Setup()
        {
            _log = new FakeLog();
            _accessTokenServiceMock = new Mock<IAccessTokenService>();
            _configMock = new Mock<IGlobalConfigurationManager>();
            _mbcRestfulServiceMock = new Mock<IMBCRestfulService>();

            _sut = new SecurityService(_accessTokenServiceMock.Object, _configMock.Object, _mbcRestfulServiceMock.Object);
        }
        [Test]
        public void RaiseExceptionInExecuteEncryptionWhenMdsidIsEmpty() {
            Assert.Throws<ArgumentNullException>(() => _sut.AesEncrypt("", Token.Afinium));
        }

        [Test]
        public void RaiseExceptionWhenTokenResponseIsNull() {
            ApiSecurityResponse response = new ApiSecurityResponse {
                Success = true
            };
            RestResultDto<ApiSecurityResponse> result = new RestResultDto<ApiSecurityResponse>();
            result.Result = response;
            _mbcRestfulServiceMock.Setup(x => x.Post<ApiSecurityResponse>(It.IsAny<RestRequestDto>())).Returns(result);
            Assert.Throws<ArgumentException>(() => _sut.AesEncrypt("995", Token.Afinium));
        }

        [Test]
        public void TestEncryptionAfiniumMdsid() {
            string encryptionString = "crypt";
            ApiSecurityResponse response = new ApiSecurityResponse
            {
                Success = true,
                Data = new ApiSecurityModel { EncryptedText = encryptionString }
            };
            TokenResponse tokenModel = new TokenResponse{Data = new TokenModel{AccessToken = "test"}};
            RestResultDto<ApiSecurityResponse> result = new RestResultDto<ApiSecurityResponse>{Success = true};
            result.Result = response;
            _mbcRestfulServiceMock.Setup(x => x.Post<ApiSecurityResponse>(It.IsAny<RestRequestDto>())).Returns(result);
            _accessTokenServiceMock.Setup(x => x.GetAccessTokenFromRedis()).Returns(tokenModel);
            var resultEncryption =_sut.AesEncrypt("995", Token.Afinium);
            Assert.AreEqual(resultEncryption, encryptionString);

        }
    }
}
