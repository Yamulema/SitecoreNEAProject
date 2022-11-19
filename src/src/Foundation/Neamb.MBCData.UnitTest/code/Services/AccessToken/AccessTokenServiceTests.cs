using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.AccessToken
{
    [TestFixture]
    public class AccessTokenServiceTests
    {
        #region Fields

        //private Mock<IRestRepository> _restRepositoryMock;
        private Mock<IGlobalConfigurationManager> _configMock;
        private Mock<ICacheManager> _cacheManagerMock;

        #endregion

        #region Instrumentation

        [SetUp]
        public void SetUp()
        {
            //_restRepositoryMock = new Mock<IRestRepository>();
            _configMock = new Mock<IGlobalConfigurationManager>();
            _cacheManagerMock = new Mock<ICacheManager>();
        }

        #endregion


        [Test]
        public void GetAccessToken_ReturnsTrue()
        {
            //APIResponse<TokenResponse> response = new APIResponse<TokenResponse>();
            //response.Success = true;
            //response.Result = new TokenResponse
            //{
            //    Data = new TokenModel
            //    {
            //        AccessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJORUEgTWVtYmVyIEJlbmVmaXRzIiwiYWRtaW4iOnRydWUsImV4cCI6MTU5NDkyMDc3N30.f-1KsXXqMivzteA5XFbYpksGyEkKNBT0Unmv1w7-FOc"
            //    },
            //    Success = true
            //};

            //_restRepositoryMock.Setup(x => x.Post<TokenResponse>(It.IsAny<RestRequestDto>())).Returns(response);

            //_accessTokenService = new AccessTokenService(
            //    _restRepositoryMock.Object,
            //    _configMock.Object,
            //    _cacheManagerMock.Object);

            //var tokenResponse = _accessTokenService.GetAccessToken();
            //Assert.AreEqual(tokenResponse.Success, true);
        }

        [Test]
        public void GetAccessToken_ReturnsFalse()
        {
            //APIResponse<TokenResponse> response = new APIResponse<TokenResponse>();
            //response.Success = false;
            //response.Result = new TokenResponse();
            //response.Result.Error = new RestError();
            //response.Result.Error.Code = 401;
            //_restRepositoryMock.Setup(x => x.Post<TokenResponse>(It.IsAny<RestRequestDto>())).Returns(response);

            //_accessTokenService = new AccessTokenService(
            //    _restRepositoryMock.Object,
            //    _configMock.Object,
            //    _cacheManagerMock.Object);

            //var tokenResponse = _accessTokenService.GetAccessToken();
            //Assert.AreEqual(tokenResponse.Success, false);
        }
    }
}
