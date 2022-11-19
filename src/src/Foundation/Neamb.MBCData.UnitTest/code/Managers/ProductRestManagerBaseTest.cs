using System;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility;
using NUnit.Framework;


namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers
{
    [TestFixture]
    public class ProductRestManagerBaseTest
    {
        #region Fields

        private Mock<IGlobalConfigurationManager> _configMock;
        private IProductRestBaseRepository _sut;
        private Mock<IMBCRestfulService> _mbcRestfulMock;

        #endregion

        #region Instrumentation

        [SetUp]
        public void SetUp()
        {
            _configMock = new Mock<IGlobalConfigurationManager>();
            _mbcRestfulMock = new Mock<IMBCRestfulService>();
            _sut = new ProductRestBaseRepository(_mbcRestfulMock.Object, _configMock.Object);
             
        }

        #endregion

        [Test]
        public void GetEligibility_WithInputValuesEmpty()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetEligibility(null, null, ""));
        }

        //[Test]
        //public void GetEligibility_WhenReturnFalse()
        //{
        //    APIResponse<ProductBaseApiResult> response = new APIResponse<ProductBaseApiResult>();
        //    response.Success = true;
        //    response.Result = new ProductBaseApiResult
        //    {
        //        Data = new ProductEligibilityModel
        //        {
        //            Eligible = false
        //        },
        //        Success = true
        //    };

        //    _restRepositoryMock.Setup(x => x.Post<ProductBaseApiResult>(It.IsAny<RestRequestDto>())).Returns(response);
        //    _sut = new ProductRestManagerBase(_restRepositoryMock.Object, _configMock.Object, _accessTokenServiceMock.Object);

        //    var result = _sut.GetEligibility(_tokenResponse, new ProductRestRequest(), "url");
        //    Assert.AreEqual(result, false);
        //}

        //[Test]
        //public void GetEligibility_WhenReturnFalseAndRetryNoToken()
        //{
        //    APIResponse<ProductBaseApiResult> response = new APIResponse<ProductBaseApiResult>();
        //    response.Success = false;
        //    response.Result = new ProductBaseApiResult();
        //    response.Result.Error = new TokenError();
        //    response.Result.Error.Code = 401;

        //    _restRepositoryMock.Setup(x => x.Post<ProductBaseApiResult>(It.IsAny<RestRequestDto>())).Returns(response);
        //    Assert.Throws<Exception>(() => _sut.GetEligibility(_tokenResponse, new ProductRestRequest(), "url"));
        //}

        //[Test]
        //public void GetEligibility_WhenReturnFalseAndRetryWithToken()
        //{
        //    APIResponse<ProductBaseApiResult> response = new APIResponse<ProductBaseApiResult>();
        //    response.Success = false;
        //    response.Result = new ProductBaseApiResult();
        //    response.Result.Error = new TokenError();
        //    response.Result.Error.Code = 401;

        //    _restRepositoryMock.Setup(x => x.Post<ProductBaseApiResult>(It.IsAny<RestRequestDto>())).Returns(response);
        //    _accessTokenServiceMock.Setup(x => x.GetAccessToken()).Returns(_tokenResponse);
        //    var result = _sut.GetEligibility(_tokenResponse, new ProductRestRequest(), "url");
        //    Assert.AreEqual(result, false);
        //}
    }
}
