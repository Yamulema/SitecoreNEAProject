using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CloudFront;
using Amazon.CloudFront.Model;
using Moq;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Neamb.Foundation.MBCData.Services.Aws;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services
{
    [TestFixture]
    public class CloudFrontProxyTests
    {
        private ILog _log;
        private Mock<IAmazonCloudFront> _clientMock;
        private CloudFrontProxy _sut;

        [SetUp]
        public void Setup()
        {
            _log = new FakeLog();
            _clientMock = new Mock<IAmazonCloudFront>();
            _sut = new CloudFrontProxy(_clientMock.Object, _log);
        }
        [Test]
        public void CreateInvalidationAsync_Should_Throw_NeambAwsException_For_AnyException() {
            //Arrange
            var request = new CreateInvalidationRequest() {
                DistributionId = Guid.NewGuid().ToString(),
                InvalidationBatch = new InvalidationBatch() {
                    Paths = new Paths() {
                        Items = new List<string>(),
                        Quantity = 0
                    }
                }
            };
            _clientMock
                .Setup(x => x.CreateInvalidationAsync(It.IsAny<CreateInvalidationRequest>(), default(CancellationToken)))
                .ThrowsAsync(new Exception());

            //Act & Assert
            Assert.That(() => _sut.CreateInvalidationAsync(request), Throws.TypeOf<NeambAwsException>());
        }
        [Test]
        public void CreateInvalidationAsync_Should_Return_CreateInvalidationResponse()
        {
            //Arrange
            var request = new CreateInvalidationRequest()
            {
                DistributionId = Guid.NewGuid().ToString(),
                InvalidationBatch = new InvalidationBatch()
                {
                    Paths = new Paths()
                    {
                        Items = new List<string>(),
                        Quantity = 0
                    }
                }
            };

            _clientMock
                .Setup(x => x.CreateInvalidationAsync(It.IsAny<CreateInvalidationRequest>(), default(CancellationToken)))
                .ReturnsAsync(new CreateInvalidationResponse());

            //Act
            var response = _sut.CreateInvalidationAsync(request);
            
            //Assert
            Assert.IsNotNull(response.Result);
        }
    }
}
