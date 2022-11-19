using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.S3.Model;
using Moq;
using Neambc.Neamb.Feature.Contest.Model;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers {
    [TestFixture]
    public class AmazonS3RepositoryTests {

        #region Fields

        private Mock<IAmazonS3Proxy> _proxy;
        private IAmazonS3Repository _sut;

        #endregion

        #region Instrumentation

        [SetUp]
        public void Setup() {
            _proxy = new Mock<IAmazonS3Proxy>();
            _sut = new AmazonS3Repository(_proxy.Object);
        }

        #endregion

        #region Tests

        [Test]
        public void Constructor_ThrowsOnNullArgs() {
            Assert.Throws<ArgumentNullException>(() => new AmazonS3Repository(null));
        }
        [Test]
        public void CheckCreateObjectS3Text_ReturnsTrue() {
            const string bucketName = "neamb-contest-dev";
            const string key = "{39D922D0-F379-49B4-9F06-27FDD5557410}";
            const string content = "test json text";
            const string contentType = "text/plain";

            RequestS3 request = new RequestS3 {
                BucketName = bucketName,
                ContentBody = content,
                ContentType = contentType,
                Key = key
            };
            PutObjectRequest capturedArg1 = null;
            const bool expectedResult = true;
            _proxy.Setup(x => x.PutObjectS3(It.IsAny<PutObjectRequest>()))
                .Callback((PutObjectRequest requestObject) => {
                    capturedArg1 = requestObject;
                })
                .Returns(expectedResult);
            var result = _sut.CreateObjectS3(request);
            Assert.IsTrue(result);
            Assert.AreEqual(request.BucketName, capturedArg1.BucketName);
            Assert.AreEqual(request.ContentBody, capturedArg1.ContentBody);
            Assert.AreEqual(request.ContentType, capturedArg1.ContentType);
            Assert.AreEqual(request.Key, capturedArg1.Key);

        }

        [Test]
        public void CheckCreateObjectS3Image_ReturnsTrue()
        {
            const string bucketName = "neamb-contest-dev";
            const string key = "{39D922D0-F379-49B4-9F06-27FDD5557410}";
            const string contentType = "image/png";
            byte[] bytesInput = Encoding.UTF8.GetBytes("anystream");
            MemoryStream ms = new MemoryStream(bytesInput);

            RequestS3 request = new RequestS3
            {
                BucketName = bucketName,
                InputStream = ms,
                ContentType = contentType,
                Key = key
            };
            PutObjectRequest capturedArg1 = null;
            const bool expectedResult = true;
            _proxy.Setup(x => x.PutObjectS3(It.IsAny<PutObjectRequest>()))
                .Callback((PutObjectRequest requestObject) => {
                    capturedArg1 = requestObject;
                })
                .Returns(expectedResult);
            var result = _sut.CreateObjectS3(request);
            Assert.IsTrue(result);
            Assert.AreEqual(request.BucketName, capturedArg1.BucketName);
            Assert.AreEqual(request.ContentBody, capturedArg1.ContentBody);
            Assert.AreEqual(request.ContentType, capturedArg1.ContentType);
            Assert.AreEqual(request.Key, capturedArg1.Key);

        }
        [Test]
        public void CheckGetObjectS3_ReturnsDefaultValueWithString() {
            const string expectedResult = default(string);
            BaseRequestS3 baseRequestS3 = new BaseRequestS3 {
                BucketName = "neamb-contest-dev",
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}",
                IsEncrypted = false
            };
            var result = _sut.GetObjectS3<string>(baseRequestS3);
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void CheckGetObjectS3_ReturnsDefaultValueWithByteArray() {
            const string expectedResult = null;
            BaseRequestS3 baseRequestS3 = new BaseRequestS3 {
                BucketName = "neamb-contest-dev",
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}",
                IsEncrypted = false
            };
            var result = _sut.GetObjectS3<byte[]>(baseRequestS3);
            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void CheckDeleteObjectS3_ReturnsFalse() {
            DeleteObjectRequest capturedArg1 = null;
            BaseRequestS3 baseRequestS3 = new BaseRequestS3 {
                BucketName = "neamb-contest-dev",
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}"

            };

            _proxy.Setup(x => x.DeleteObjectS3(It.IsAny<DeleteObjectRequest>()))
                .Callback((DeleteObjectRequest requestObject) => {
                    capturedArg1 = requestObject;
                })
                .Returns(false);
            var result = _sut.DeleteObjectS3(baseRequestS3);
            Assert.IsFalse(result);
            Assert.AreEqual(capturedArg1.BucketName, baseRequestS3.BucketName);
            Assert.AreEqual(capturedArg1.Key, baseRequestS3.Key);
        }

        [Test]
        public void CheckDeleteObjectS3_ReturnsTrue() {
            DeleteObjectRequest capturedArg1 = null;
            BaseRequestS3 baseRequestS3 = new BaseRequestS3 {
                BucketName = "neamb-contest-dev",
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}"

            };

            _proxy.Setup(x => x.DeleteObjectS3(It.IsAny<DeleteObjectRequest>()))
                .Callback((DeleteObjectRequest requestObject) => {
                    capturedArg1 = requestObject;
                })
                .Returns(true);
            var result = _sut.DeleteObjectS3(baseRequestS3);
            Assert.IsTrue(result);
            Assert.AreEqual(capturedArg1.BucketName, baseRequestS3.BucketName);
            Assert.AreEqual(capturedArg1.Key, baseRequestS3.Key);
        }

        [Test]
        public void CheckCopyObjectS3_ReturnsFalse() {
            CopyObjectRequest capturedArg1 = null;
            CopyObjectResponse response = null;
            CopyRequestS3 baseRequestS3 = new CopyRequestS3 {
                SourceBucket = "sourceBucket",
                SourceKey = "sourcekey",
                DestinationBucket = "destinationBucket",
                DestinationKey = "destinationkey"
            };

            _proxy.Setup(x => x.CopyObjectS3(It.IsAny<CopyObjectRequest>()))
                .Callback((CopyObjectRequest requestObject) => {
                    capturedArg1 = requestObject;
                })
                .Returns(response);
            var result = _sut.CopyObjectS3(baseRequestS3);
            Assert.IsFalse(result);
            Assert.AreEqual(capturedArg1.SourceBucket, baseRequestS3.SourceBucket);
            Assert.AreEqual(capturedArg1.SourceKey, baseRequestS3.SourceKey.CalculateMD5Hash());
            Assert.AreEqual(capturedArg1.DestinationBucket, baseRequestS3.DestinationBucket);
            Assert.AreEqual(capturedArg1.DestinationKey, baseRequestS3.DestinationKey.CalculateMD5Hash());
        }

        [Test]
        public void CheckCopyObjectS3_ReturnsTrue() {
            CopyObjectRequest capturedArg1 = null;
            CopyObjectResponse response = new CopyObjectResponse();
            CopyRequestS3 baseRequestS3 = new CopyRequestS3 {
                SourceBucket = "sourceBucket",
                SourceKey = "sourcekey",
                DestinationBucket = "destinationBucket",
                DestinationKey = "destinationkey"
            };

            _proxy.Setup(x => x.CopyObjectS3(It.IsAny<CopyObjectRequest>()))
                .Callback((CopyObjectRequest requestObject) => {
                    capturedArg1 = requestObject;
                })
                .Returns(response);
            var result = _sut.CopyObjectS3(baseRequestS3);
            Assert.IsTrue(result);
            Assert.AreEqual(capturedArg1.SourceBucket, baseRequestS3.SourceBucket);
            Assert.AreEqual(capturedArg1.SourceKey, baseRequestS3.SourceKey.CalculateMD5Hash());
            Assert.AreEqual(capturedArg1.DestinationBucket, baseRequestS3.DestinationBucket);
            Assert.AreEqual(capturedArg1.DestinationKey, baseRequestS3.DestinationKey.CalculateMD5Hash());
        }

        [Test]
        public void CheckGetFiles_ReturnsListEmptyFiles() {
            GetObjectResponse response = null;
            FilesS3 filesS3 = new FilesS3 {
                BucketName = "neamb-contest-dev",
                IsEncrypted = false,
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}",
                TypeFilter = S3ObjectTypeFilter.File
            };
            _proxy.Setup(x => x.GetObjectS3(It.IsAny<GetObjectRequest>())).Returns(response);
            var result = _sut.GetFiles(filesS3);
            Assert.IsFalse(result.Any());
        }

        [Test]
        public void CheckGetObjectS3_ReturnsValueByteArray() {
            byte[] bytesInput = Encoding.UTF8.GetBytes("anystream");
            MemoryStream ms = new MemoryStream(bytesInput);

            GetObjectResponse response = new GetObjectResponse {
                ResponseStream = ms
            };
            BaseRequestS3 baseRequestS3 = new BaseRequestS3 {
                BucketName = "neamb-contest-dev",
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}",
                IsEncrypted = false
            };
            _proxy.Setup(x => x.GetObjectS3(It.IsAny<GetObjectRequest>())).Returns(response);
            var result = _sut.GetObjectS3<byte[]>(baseRequestS3);
            Assert.AreEqual(result, bytesInput);
        }

        [Test]
        public void CheckGetObjectS3_ReturnsValueTypeContestFileItem() {
            var contestFileItemTobeAdded = new ContestFileItem {
                Key = new Guid("09f4ca07-6093-4ebd-a534-2cea4ed933b5"),
                FileName = "Two cats",
                Webuserid = "1479557",
                Mdsid = "000000940"
            };
            //Get the json content to be saved in S3
            var resultJson = JsonConvert.SerializeObject(contestFileItemTobeAdded);

            byte[] bytesInput = Encoding.ASCII.GetBytes(resultJson);
            MemoryStream ms = new MemoryStream(bytesInput);

            GetObjectResponse response = new GetObjectResponse {
                ResponseStream = ms
            };
            BaseRequestS3 baseRequestS3 = new BaseRequestS3 {
                BucketName = "neamb-contest-dev",
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}",
                IsEncrypted = false
            };
            _proxy.Setup(x => x.GetObjectS3(It.IsAny<GetObjectRequest>())).Returns(response);
            var result = _sut.GetObjectS3<ContestFileItem>(baseRequestS3);
            Assert.AreEqual(result.Key, contestFileItemTobeAdded.Key);
            Assert.AreEqual(result.FileName, contestFileItemTobeAdded.FileName);
            Assert.AreEqual(result.Webuserid, contestFileItemTobeAdded.Webuserid);
            Assert.AreEqual(result.Mdsid, contestFileItemTobeAdded.Mdsid);
        }

        [Test]
        public void CheckGetListFiles_ReturnsListEmptyFiles()
        {
            const string bucketName = "neamb-contest-dev";

            FilesS3 filesS3 = new FilesS3
            {
                BucketName = bucketName,
                IsEncrypted = false,
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}/Curated",
                TypeFilter = S3ObjectTypeFilter.File
            };
            List<S3Object> listS3Objects = new List<S3Object>();
            S3Object s3Object = new S3Object
            {
                BucketName = bucketName,
                Key = "09f4ca07-6093-4ebd-a534-2cea4ed933b5.jpg"
            };
            listS3Objects.Add(s3Object);
            ListObjectsResponse listObjectsResponse = null;
           _proxy.Setup(x => x.GetListObjects(It.IsAny<ListObjectsRequest>())).Returns(listObjectsResponse);
            var result = _sut.GetFiles(filesS3);
            Assert.IsFalse(result.Any());
        }

        [Test]
        public void CheckGetListFiles_ReturnsListFiles() {
            const string bucketName = "neamb-contest-dev";
            FilesS3 filesS3 = new FilesS3
            {
                BucketName = bucketName,
                IsEncrypted = false,
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}/Curated",
                TypeFilter = S3ObjectTypeFilter.File
            };
            List<S3Object> listS3Objects = new List<S3Object>();
            S3Object s3Object = new S3Object
            {
                BucketName = bucketName,
                Key = "09f4ca07-6093-4ebd-a534-2cea4ed933b5.jpg",
                Size = 1000
            };
            listS3Objects.Add(s3Object);
            ListObjectsResponse listObjectsResponse = new ListObjectsResponse{ S3Objects = listS3Objects};
            _proxy.Setup(x => x.GetListObjects(It.IsAny<ListObjectsRequest>())).Returns(listObjectsResponse);
            var result = _sut.GetFiles(filesS3);
            Assert.AreEqual(result.Count(),listS3Objects.Count);
            for (var ndx = 0; ndx < listS3Objects.Count; ndx++) {
                Assert.AreEqual(listS3Objects[ndx].Key,result.ToList()[ndx].Key);
                Assert.AreEqual(listS3Objects[ndx].Key.GetExtension(), result.ToList()[ndx].Key.GetExtension());
            }
        }

        [Test]
        public void CheckGetListFiles_ReturnsListFolders()
        {
            const string bucketName = "neamb-contest-dev";
            FilesS3 filesS3 = new FilesS3
            {
                BucketName = bucketName,
                IsEncrypted = false,
                Key = "{39D922D0-F379-49B4-9F06-27FDD5557410}/Curated",
                TypeFilter = S3ObjectTypeFilter.Folder
            };
            List<S3Object> listS3Objects = new List<S3Object>();
            S3Object s3Object = new S3Object
            {
                BucketName = bucketName,
                Key = "Curated",
                Size = 0
            };
            listS3Objects.Add(s3Object);

            ListObjectsResponse listObjectsResponse = new ListObjectsResponse { S3Objects = listS3Objects };
            
            _proxy.Setup(x => x.GetListObjects(It.IsAny<ListObjectsRequest>())).Returns(listObjectsResponse);
            var result = _sut.GetFiles(filesS3);
            Assert.AreEqual(result.Count(), listS3Objects.Count);
            for (var ndx = 0; ndx < listS3Objects.Count; ndx++)
            {
                Assert.AreEqual(listS3Objects[ndx].Key, result.ToList()[ndx].Key);
                Assert.AreEqual(listS3Objects[ndx].Key.GetExtension(), result.ToList()[ndx].Key.GetExtension());
            }
        }

        #endregion

    }
}
