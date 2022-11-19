using System;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers {
	[TestFixture]
	public class AmazonS3ProxyTests
    {

        #region Fields
        private Mock<IAmazonS3ClientFactory> _clientFactory;
        private Mock<IAmazonS3> _soapClient;
        private FakeLog _log;
        private IAmazonS3Proxy _sut;
        #endregion

        #region Instrumentation
        [SetUp]
		public void Setup() {
            _clientFactory = new Mock<IAmazonS3ClientFactory>();
			_log = new FakeLog();
            _soapClient = new Mock<IAmazonS3>();
            _clientFactory.Setup(x => x.CreateClient()).Returns(_soapClient.Object);

            _sut = new AmazonS3Proxy(_clientFactory.Object,_log);
		}
		#endregion

		#region Tests

		[Test]
		public void Constructor_ThrowsOnNullArgs() {
			Assert.Throws<ArgumentNullException>(() => new AmazonS3Proxy(null, null));
			Assert.Throws<ArgumentNullException>(() => new AmazonS3Proxy(null, _log));
			Assert.Throws<ArgumentNullException>(() => new AmazonS3Proxy(_clientFactory.Object,null));
		}
		[Test]
		public void CheckPutObjectS3_ReturnsTrue() {
            PutObjectRequest putObjectRequest= new PutObjectRequest();
            PutObjectResponse putObjectResponse = new PutObjectResponse();
            _soapClient.Setup(x => x.PutObject(putObjectRequest)).Returns(putObjectResponse);
            var result = _sut.PutObjectS3(putObjectRequest);
			Assert.IsTrue(result);
		}
        [Test]
        public void CheckPutObjectS3_ReturnsFalse() {
            var result = _sut.PutObjectS3(null);
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckPutObjectS3_ReturnsErrorException() {
            PutObjectRequest putObjectRequest = new PutObjectRequest();

            _soapClient.Setup(x => x.PutObject(putObjectRequest)).Throws(new Exception("Error putting some object"));
            var result = _sut.PutObjectS3(putObjectRequest);
            Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("error")));
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckGetObjectS3_ReturnsObjectResponse()
        {
            GetObjectResponse expectedResponse = new GetObjectResponse();
            GetObjectRequest objectRequest= new GetObjectRequest();
            _soapClient.Setup(x => x.GetObject(objectRequest)).Returns(expectedResponse);
            var result = _sut.GetObjectS3(objectRequest);
            Assert.AreEqual(result,expectedResponse);
        }

        [Test]
        public void CheckGetObjectS3_ReturnsException()
        {
            GetObjectResponse expectedResponse = new GetObjectResponse();
            GetObjectRequest objectRequest = new GetObjectRequest();
            _soapClient.Setup(x => x.GetObject(objectRequest)).Throws(new Exception("Error getting some object"));
            var result = _sut.GetObjectS3(objectRequest);
            Assert.IsNull(result);
            Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("debug")));
        }

        [Test]
        public void CheckGetObjectS3_ReturnsObjectResponse_WhenRequestNull()
        {
            GetObjectResponse expectedResponse = new GetObjectResponse();
            _soapClient.Setup(x => x.GetObject(null)).Returns(expectedResponse);
            var result = _sut.GetObjectS3(null);
            Assert.AreEqual(result, expectedResponse);
        }

        [Test]
        public void CheckCopyObjectS3_ReturnsObjectResponse()
        {
            CopyObjectResponse expectedResponse = new CopyObjectResponse();
            CopyObjectRequest objectRequest = new CopyObjectRequest();
            _soapClient.Setup(x => x.CopyObject(objectRequest)).Returns(expectedResponse);
            var result = _sut.CopyObjectS3(objectRequest);
            Assert.AreEqual(result, expectedResponse);
        }

        [Test]
        public void CheckCopyObjectS3_ReturnsException()
        {
            CopyObjectRequest objectRequest = new CopyObjectRequest();
            _soapClient.Setup(x => x.CopyObject(objectRequest)).Throws(new Exception("Error getting some object"));
            var result = _sut.CopyObjectS3(objectRequest);
            Assert.IsNull(result);
            Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("error")));
        }

        [Test]
        public void CheckCopyObjectS3ReturnsObjectResponse_WhenRequestNull()
        {
            CopyObjectResponse expectedResponse = new CopyObjectResponse();
            _soapClient.Setup(x => x.CopyObject(null)).Returns(expectedResponse);
            var result = _sut.CopyObjectS3(null);
            Assert.AreEqual(result, expectedResponse);
        }
        [Test]
        public void DeleteObjectS3_ReturnsObjectResponse()
        {
            bool expectedResponse = true;
            DeleteObjectRequest objectRequest = new DeleteObjectRequest();
            _soapClient.Setup(x => x.DeleteObject(objectRequest)).Returns(new DeleteObjectResponse());
            var result = _sut.DeleteObjectS3(objectRequest);
            Assert.AreEqual(expectedResponse,result);
        }

        [Test]
        public void DeleteObjectS3_ReturnsException()
        {
            DeleteObjectRequest objectRequest = new DeleteObjectRequest();
            _soapClient.Setup(x => x.DeleteObject(objectRequest)).Throws(new Exception("Error getting some object"));
            var result = _sut.DeleteObjectS3(objectRequest);
            Assert.IsFalse(result);
            Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("error")));
        }

        [Test]
        public void GetListObjectsS3_ReturnsObjectResponse()
        {
            ListObjectsResponse objectResponse = new ListObjectsResponse();
            ListObjectsRequest objectRequest = new ListObjectsRequest();
            _soapClient.Setup(x => x.ListObjects(objectRequest)).Returns(objectResponse);
            var result = _sut.GetListObjects(objectRequest);
            Assert.AreEqual(objectResponse, result);
        }

        [Test]
        public void GetListObjectsS3_ReturnsException()
        {
            ListObjectsRequest objectRequest = new ListObjectsRequest();
            _soapClient.Setup(x => x.ListObjects(objectRequest)).Throws(new Exception("Error getting some object"));
            var result = _sut.GetListObjects(objectRequest);
            Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("error")));
        }
        #endregion

    }
}
