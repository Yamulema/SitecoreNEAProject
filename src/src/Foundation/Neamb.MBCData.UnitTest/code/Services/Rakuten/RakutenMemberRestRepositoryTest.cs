using System;
using System.Collections.Generic;
using System.Net;
using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Seiumb.Foundation.Sitecore;
using NUnit.Framework;
using Neambc.Neamb.Foundation.MBCData.Services.Rakuten;
using Sitecore.Configuration;
using System.IO;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Services.Rakuten
{
    [TestFixture]
    public class RakutenMemberRestRepositoryTest
    {
        #region Fields
        private Mock<ILog> _log;
        private Mock<IRakutenRestfulService> _rakutenRestfulService;
        private Mock<IOracleDatabase> _oracleManager;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManager;
        #endregion

       [SetUp]
        public void SetUp() {
            State.HttpRuntime.AppDomainAppPath = Directory.GetCurrentDirectory();
            _log = new Mock<ILog>();
            _rakutenRestfulService = new Mock<IRakutenRestfulService>();
            _oracleManager = new Mock<IOracleDatabase>();
            _globalConfigurationManager = new Mock<IGlobalConfigurationManager>();
    }

         [Test]
       public void RakutenMemberRestRepository_Post_InvalidParameter()
        {
            var sut = new RakutenMemberRestRepository(_log.Object, _rakutenRestfulService.Object, _oracleManager.Object, _globalConfigurationManager.Object);

            var request1 = new RestRequestDto
            {
                Server = null
            };
            var request2 = new RestRequestDto
            {
                Body = null
            };
            var expected = new RestResultDto<MemberCreationResponse>
            {
                Success = false
            };

            string logMsg = "No Log Message!";
            _log.Setup(x => x.Error(It.IsAny<string>(), It.IsAny<Object>()))
                .Callback<string, object>((s, o) => logMsg = s);

            var result1 = sut.Post(request1);
            Assert.AreEqual(expected.Success, result1.Success);
            Assert.IsNotNull(result1.ExceptionDetail);
            _log.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(1));
            Console.WriteLine(logMsg);

            var result2 = sut.Post(request2);
            Assert.AreEqual(expected.Success, result2.Success);
            Assert.IsNotNull(result2.ExceptionDetail);
            _log.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
            Console.WriteLine(logMsg);
        }

        [Test]
        public void RakutenMemberRestRepository_Post_Created()
        {
            var myRequest = new RakutenRestRequestDto(new List<KeyValuePair<string, string>>());
            myRequest.Server = "DummyServer";
            myRequest.Body = "DummyBody";
            myRequest.MdsId = "12345";
            myRequest.UnionId = "1";
            myRequest.CellCode = "cellcode";

            var response = new RestResultDto<MemberCreationResponse>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Headers = new List<RestSharp.Parameter>() { new RestSharp.Parameter("ebtoken", "testToken", RestSharp.ParameterType.HttpHeader) },
                Result = new MemberCreationResponse {
                    EmailAddress = "TestEmail",
                    Id = "storeId"
                }
            };

            _rakutenRestfulService.Setup(x => x.Post<MemberCreationResponse>(myRequest)).Returns(response);
            _oracleManager.Setup(x => x.Rakuten_Registration(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            var sut = new RakutenMemberRestRepository(_log.Object, _rakutenRestfulService.Object, _oracleManager.Object, _globalConfigurationManager.Object);
            var res = sut.Post(myRequest);
                
            Assert.IsTrue(res.Success);
            Assert.AreEqual(res.Result.EBtoken, "testToken");
            Assert.AreEqual(res.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(res.Result.EmailAddress, response.Result.EmailAddress);
            Assert.AreEqual(res.Result.Id, response.Result.Id);

            _oracleManager.Verify( x=>x.Rakuten_Registration(myRequest.MdsId, response.Result.EmailAddress,
                response.Result.Id, "testToken", myRequest.UnionId, myRequest.CellCode));
        }

        [Test]
        public void RakutenMemberRestRepository_Post_ErrorStatus()
        {
            var sut = new RakutenMemberRestRepository(_log.Object, _rakutenRestfulService.Object, _oracleManager.Object, _globalConfigurationManager.Object);

            var myRequest = new RakutenRestRequestDto(new List<KeyValuePair<string, string>>());
            myRequest.Server = "DummyServer";
            myRequest.Body = "DummyBody";

            string logMsg="No Log Message!";
            _log.Setup(x => x.Error(It.IsAny<string>(), It.IsAny<Object>()))
                .Callback<string, object>((s,o)=>logMsg = s);

            var StatusCodes = new HttpStatusCode[] { HttpStatusCode.BadRequest, HttpStatusCode.Forbidden,
                     HttpStatusCode.MethodNotAllowed, HttpStatusCode.HttpVersionNotSupported,
                    HttpStatusCode.InternalServerError };

            int i = 0;
            foreach(var code in StatusCodes )
            {
                i++;
                var response = new RestResultDto<MemberCreationResponse> { StatusCode = code };
                _rakutenRestfulService.Setup(x => x.Post<MemberCreationResponse>(myRequest)).Returns(response);
                var res = sut.Post(myRequest);
                Assert.IsFalse(res.Success);
                Assert.AreEqual(res.StatusCode, response.StatusCode);
                _log.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(i));
                Console.WriteLine(logMsg);
            }
        }
    }
}
