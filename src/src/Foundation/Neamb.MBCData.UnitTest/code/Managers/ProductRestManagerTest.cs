using System;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility;
using NUnit.Framework;


namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers
{
    [TestFixture]
    public class ProductRestManagerTest
    {
        #region Fields

        private Mock<IProductRestBaseRepository> _productRestManagerBaseMock;
        private Mock<IAccessTokenService> _accessTokenServiceMock;
        private Mock<IGlobalConfigurationManager> _configMock;
        private IProductEligibilityService _sut;

        #endregion

        #region Instrumentation

        [SetUp]
        public void SetUp() {
            _productRestManagerBaseMock = new Mock<IProductRestBaseRepository>();
            _accessTokenServiceMock = new Mock<IAccessTokenService>();
            _configMock = new Mock<IGlobalConfigurationManager>();
            _sut = new ProductEligibilityService(_accessTokenServiceMock.Object, _configMock.Object,_productRestManagerBaseMock.Object);
        }

        #endregion

        [Test]
        public void GetEligibility_WithInputValuesEmpty() {
            Assert.Throws<ArgumentException>(() => _sut.GetEligibility(-1, ""));
        }

        [Test]
        public void GetEligibility_WhenReturnFalse() {
            //TokenResponse tokenResponse = new TokenResponse {
            //    Data = new TokenModel {
            //        AccessToken =
            //            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJORUEgTWVtYmVyIEJlbmVmaXRzIiwiYWRtaW4iOnRydWUsImV4cCI6MTU5NDkyMDc3N30.f-1KsXXqMivzteA5XFbYpksGyEkKNBT0Unmv1w7-FOc"
            //    },
            //    Success = true
            //};
            //_accessTokenServiceMock.Setup(x=> x.GetAccessToken()).Returns(tokenResponse);
            //var result= _sut.GetEligibility(995, "486 01");
            //Assert.AreEqual(result, false);
        }

        [Test]
        public void GetEligibility_WhenReturnTrue() {
            //TokenResponse tokenResponse = new TokenResponse
            //{
            //    Data = new TokenModel
            //    {
            //        AccessToken =
            //            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJORUEgTWVtYmVyIEJlbmVmaXRzIiwiYWRtaW4iOnRydWUsImV4cCI6MTU5NDkyMDc3N30.f-1KsXXqMivzteA5XFbYpksGyEkKNBT0Unmv1w7-FOc"
            //    },
            //    Success = true
            //};
            //_accessTokenServiceMock.Setup(x => x.GetAccessToken()).Returns(tokenResponse);
            //_productRestManagerBaseMock.Setup(x => x.GetEligibility(tokenResponse, It.IsAny<object>(), It.IsAny<string>())).Returns(true);
            //var result = _sut.GetEligibility(995, "486 01");
            //Assert.AreEqual(result, true);
        }



    }
}
