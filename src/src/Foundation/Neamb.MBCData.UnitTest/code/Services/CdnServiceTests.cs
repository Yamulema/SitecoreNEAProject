using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.CloudFront.Model;
using Moq;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.Aws;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services
{
    [TestFixture]
    public class CdnServiceTests
    {
        #region Fields
        private CdnService _sut;
        private Mock<ICloudFrontFactory> _cloudFrontFactoryMock;
        private Mock<ICloudFrontProxy> _cloudFrontProxyMock;
        private FakeLog _log;
        #endregion

        [SetUp]
        public void Setup() {
            _log = new FakeLog();
            _cloudFrontFactoryMock = new Mock<ICloudFrontFactory>();
            _cloudFrontProxyMock = new Mock<ICloudFrontProxy>();
            _cloudFrontFactoryMock
                .Setup(x => x.GetClient())
                .Returns(_cloudFrontProxyMock.Object);
            _sut = new CdnService(_cloudFrontFactoryMock.Object, _log);
        }

        [Test]
        public void InvalidateAsync_Should_Return_True_When_Paths_IsNull() {
            //Arrange
            List<string> paths = null;

            //Act
            var result = _sut.InvalidateAsync(paths).Result;

            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void InvalidateAsync_Should_Return_True_When_Paths_IsEmpty()
        {
            //Arrange
            var paths = new List<string>();

            //Act
            var result = _sut.InvalidateAsync(paths).Result;

            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void InvalidateAsync_Should_Return_False_When_Client_Throws_NeambAwsException()
        {
            //Arrange
            var paths = new List<string>() {
                "path1/",
                "path2/"
            };

            _cloudFrontProxyMock
                .Setup(x => x.CreateInvalidationAsync(It.IsAny<CreateInvalidationRequest>()))
                .Throws<NeambAwsException>();

            //Act
            var result = _sut.InvalidateAsync(paths).Result;

            //Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void InvalidateAsync_Should_Throw_AnException_When_An_UnhandledException_IsThrown()
        {
            //Arrange
            var paths = new List<string>() {
                "path1/",
                "path2/"
            };
            _cloudFrontFactoryMock
                .Setup(x => x.GetClient())
                .Throws(new Exception());

            //Act
            Assert.That(() => _sut.InvalidateAsync(paths).Result, Throws.Exception);
        }
        [Test]
        public void InvalidateAsync_Should_Return_False_When_Client_Returns_Null()
        {
            //Arrange
            var paths = new List<string>() {
                "path1/",
                "path2/"
            };

            _cloudFrontProxyMock
                .Setup(x => x.CreateInvalidationAsync(It.IsAny<CreateInvalidationRequest>()))
                .ReturnsAsync((CreateInvalidationResponse)null);

            //Act
            var result = _sut.InvalidateAsync(paths).Result;

            //Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void InvalidateAsync_Should_Return_False_When_Client_Returns_AnEmptyResponse()
        {
            //Arrange
            var paths = new List<string>() {
                "path1/",
                "path2/"
            };

            _cloudFrontProxyMock
                .Setup(x => x.CreateInvalidationAsync(It.IsAny<CreateInvalidationRequest>()))
                .ReturnsAsync(new CreateInvalidationResponse());

            //Act
            var result = _sut.InvalidateAsync(paths).Result;

            //Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void InvalidateAsync_Should_Return_True_When_Client_Returns_AnInvalidationId()
        {
            //Arrange
            var paths = new List<string>() {
                "path1/",
                "path2/"
            };

            _cloudFrontProxyMock
                .Setup(x => x.CreateInvalidationAsync(It.IsAny<CreateInvalidationRequest>()))
                .ReturnsAsync(new CreateInvalidationResponse() {
                    Invalidation = new Invalidation() {
                        Id = Guid.NewGuid().ToString()
                    }
                });

            //Act
            var result = _sut.InvalidateAsync(paths).Result;

            //Assert
            Assert.IsTrue(result);
        }
    }
}
