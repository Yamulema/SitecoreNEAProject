using System;
using System.Linq;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.Rakuten;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;


namespace Neambc.Neamb.Foundation.Rakuten.UnitTest.Manager {
	[TestFixture]
	public class RakutenStoreRestRepositoryTest
    {
        #region Fields
        private IRakutenStoreRestRepository _sut;
        private FakeLog _log;
        private Mock<IRakutenRestfulService> _restRepositoryMock;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManagerMock;
        #endregion

        [SetUp]
        public void SetUp()
        {
            _log = new FakeLog();
            _restRepositoryMock = new Mock<IRakutenRestfulService>();
            _globalConfigurationManagerMock = new Mock<IGlobalConfigurationManager>();
            _sut = new RakutenStoreRestRepository(_log,_restRepositoryMock.Object, _globalConfigurationManagerMock.Object);
        }

        [Test]
        public void GetStoreWithResponse() {
           
            StoreResponse storeResponse= new StoreResponse();
            storeResponse.Etag = "123456";
            _globalConfigurationManagerMock.Setup(x => x.RakutenStoreApiUrl).Returns("test");
            _globalConfigurationManagerMock.Setup(x => x.RakutenServerApiUrl).Returns("test");

            RestResultDto<StoreResponse> resultStoreResponse = new RestResultDto<StoreResponse>();
            resultStoreResponse.Result = storeResponse;
            _restRepositoryMock.Setup(x => x.Get<StoreResponse>(It.IsAny<RestRequestDto>())).Returns(resultStoreResponse);
            var result= _sut.GetStore(It.IsAny<string>());
            Assert.AreEqual(result, resultStoreResponse);
        }
        [Test]
        public void GetStoreDetailWithResponse()
        {
            RestRequestDto restRequestDto = new RestRequestDto
            {
                Action = "action",
                Server = "server"
            };
            StoreDetailResponseTop storeResponseDetail = new StoreDetailResponseTop();

            RestResultDto<StoreDetailResponseTop> resultStoreResponse = new RestResultDto<StoreDetailResponseTop>();
            resultStoreResponse.Result = storeResponseDetail;
            _restRepositoryMock.Setup(x => x.Get<StoreDetailResponseTop>(restRequestDto)).Returns(resultStoreResponse);
            var result = _sut.GetStoreDetail(restRequestDto);
            Assert.AreEqual(result, resultStoreResponse);
        }

    }
}
