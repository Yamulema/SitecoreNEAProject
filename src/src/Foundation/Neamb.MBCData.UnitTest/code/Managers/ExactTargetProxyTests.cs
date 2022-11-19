using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers {
	[TestFixture]
	public class ExactTargetProxyTests {

		#region Classes
		/// <summary>
		/// Used only to make protected methods into public methods for unit tests
		/// </summary>
		public class TestExactTargetProxy : ExactTargetProxy {
			public TestExactTargetProxy(ILog log, IGlobalConfigurationManager manager, IExactTargetSoapClientFactory factory, IExactTargetLogger exactTargetLog)
				: base(log, manager, factory, exactTargetLog) { }

			public new bool CheckStatus(string status, Result result) {
				return base.CheckStatus(status, result);
			}
		}
		#endregion

		#region Fields
		private Mock<IGlobalConfigurationManager> _manager;
		private Mock<IExactTargetSoapClientFactory> _factory;
		private Mock<IExactTargetSoapClient> _soapClient;
		private IExactTargetProxy _sut;
		private TestExactTargetProxy _sutProtected;
		private FakeLog _log;
        private Mock<IExactTargetLogger> _exactTargetLog;
        #endregion

        #region Instrumentation
        [SetUp]
		public void Setup() {
			_manager = new Mock<IGlobalConfigurationManager>();
			_factory = new Mock<IExactTargetSoapClientFactory>();
            _exactTargetLog = new Mock<IExactTargetLogger>();
			_soapClient = new Mock<IExactTargetSoapClient>();
			_factory.Setup(x => x.CreateClient()).Returns(_soapClient.Object);
			_log = new FakeLog();
			_sut = new ExactTargetProxy(_log, _manager.Object, _factory.Object, _exactTargetLog.Object);
			_sutProtected = new TestExactTargetProxy(_log, _manager.Object, _factory.Object, _exactTargetLog.Object);
		}
		#endregion

		#region Tests

		[Test]
		public void Constructor_ThrowsOnNullArgs() {
			Assert.Throws<ArgumentNullException>(() => new ExactTargetProxy(null, null, null, _exactTargetLog.Object));
			Assert.Throws<ArgumentNullException>(() => new ExactTargetProxy(_log, null, null, _exactTargetLog.Object));
			Assert.Throws<ArgumentNullException>(() => new ExactTargetProxy(_log, _manager.Object, null, _exactTargetLog.Object));
			Assert.Throws<ArgumentNullException>(() => new ExactTargetProxy(_log, null, _factory.Object, _exactTargetLog.Object));
			Assert.Throws<ArgumentNullException>(() => new ExactTargetProxy(null, _manager.Object, _factory.Object, _exactTargetLog.Object));
			Assert.Throws<ArgumentNullException>(() => new ExactTargetProxy(null, null, _factory.Object, _exactTargetLog.Object));
		}
		[Test]
		public void CheckStatus_ReturnsTrue() {
			const string inputStatus = "OK";
			var result = _sutProtected.CheckStatus(inputStatus, null);
			Assert.IsTrue(result);
		}
		[Test]
		public void CheckStatus_ReturnsLogsAndReturnsFalse() {
			const string inputStatus = "ERROR";
			const string statusMessage = "Status";
			string warn1 = $"No handle for status {inputStatus} after preforming a ExactTarget SOAP request.";
			string warn2 = $"ExactTarget status message: {statusMessage}";

			Result result = new Result {
				StatusMessage = statusMessage
			};
			var response = _sutProtected.CheckStatus(inputStatus, result);
			Assert.AreEqual(_log.Entries.Count,2);
			var firstWarn= _log.Entries.First();
			var lastWarn = _log.Entries.Last();
			Assert.AreEqual(firstWarn.Message,warn1);
			Assert.AreEqual(lastWarn.Message, warn2);
			foreach (var logEntry in _log.Entries) {
				Assert.AreEqual(logEntry.EntryType, "error");
			}
			Assert.IsFalse(response);
		}

		[Test]
		public void Retrieve_ReturnsEmptySet() {
			List<APIObject> expectedResult = new List<APIObject>();
			RetrieveRequest retrieveRequest= new RetrieveRequest();
			var result=_sut.Retrieve(retrieveRequest);
			Assert.IsFalse(result.Any());
			Assert.AreEqual(expectedResult, result);
		}
		[Test]
		public void Retrieve_ReturnsNonemptySet() {
			RetrieveRequest retrieveRequest = new RetrieveRequest();
			string requesId;
			APIObject[] result = new APIObject[] {
				new APIObject(),
				new APIObject()
			};
			string status="OK";
			_soapClient.Setup(x => x.Retrieve(retrieveRequest, out requesId, out result)).Returns(status);
			var resultResponse = _sut.Retrieve(retrieveRequest);
			Assert.AreSame(resultResponse,result);
			Assert.IsTrue(resultResponse.Any());
		}
		[Test]
		public void Retrieve_CatchesExceptions() {
			string requestId;
			List<APIObject> expectedResult = new List<APIObject>();
			APIObject[] result = new APIObject[] {
				new APIObject(),
				new APIObject()
			};
			_soapClient.Setup(x => x.Retrieve(It.IsAny<RetrieveRequest>(), out requestId, out result)).Throws(new Exception());
			var resultResponse = _sut.Retrieve(null);
			Assert.AreEqual(expectedResult, resultResponse);
			Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("error")));
		}

		[Test]
		public void Update_ReturnsFalse() {
			var result=_sut.Update(new UpdateRequest());
			Assert.IsFalse(result);
		}
		[Test]
		public void Update_ReturnsTrue() {
			var updateRequest = new UpdateRequest();
			string requestId;
			string status="OK";
			UpdateResult[] updateResults = new UpdateResult[] {
				new UpdateResult(),
				new UpdateResult()
			};
			_soapClient.Setup(x => x.Update(updateRequest.Options, updateRequest.Objects, out requestId, out status)).Returns(updateResults);
			var result = _sut.Update(new UpdateRequest());
			Assert.IsTrue(result);
		}
		[Test]
		public void Update_CatchesExceptions() {
			var result = _sut.Update(null);
			Assert.IsFalse(result);
			Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("error")));
		}

		[Test]
		public void Create_ReturnsFalse() {
			var result = _sut.Create(new CreateRequest());
			Assert.IsFalse(result);
		}
		[Test]
		public void Create_ReturnsTrue() {
			var createRequest = new CreateRequest();
			string requestId;
			string status = "OK";
			CreateResult[] createResults = new CreateResult[] {
				new CreateResult(),
				new CreateResult()
			};
			_soapClient.Setup(x => x.Create(createRequest.Options, createRequest.Objects, out requestId, out status)).Returns(createResults);
			var result = _sut.Create(createRequest);
			Assert.IsTrue(result);
		}
		[Test]
		public void Create_CatchesExceptions() {
			var result = _sut.Create(null);
			Assert.IsFalse(result);
			Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("error")));
		}

		[Test]
		public void CreateAsync_ReturnsFalse() {
			Task<bool> result = _sut.CreateAsync(new CreateRequest());
			Assert.IsFalse(result.Result);
		}
		
		[Test]
		public void CreateAsync_ReturnsTrue() {
			var createRequest = new CreateRequest();
			
			Task<CreateResponse> createResponse = Task.FromResult<CreateResponse>(new CreateResponse {
				OverallStatus = "OK",
				Results = new CreateResult[] {
					new CreateResult()
				}
			}
		    ); 
			_soapClient.Setup(x => x.CreateAsync(createRequest)).Returns(createResponse);
			var result = _sut.CreateAsync(createRequest);
			Assert.IsTrue(result.Result);
		}
		[Test]
		public void CreateAsync_CatchesExceptions() {
			_soapClient.Setup(x => x.CreateAsync(It.IsAny<CreateRequest>())).Throws(new Exception());
			var result= _sut.CreateAsync(null);
			Assert.IsFalse(result.Result);
			Assert.IsNotNull(_log.Entries.First(x => x.EntryType.Equals("error")));
		}
		
		[Test]
		public void Send_ReturnsTrue() {
			var requestId = "requestId";
			var overallStatus = "OK";
			var results = new CreateResult[0];
			_soapClient.Setup(x => x.Create(It.IsAny<CreateOptions>(), It.IsAny<APIObject[]>(), out requestId, out overallStatus))
				.Returns(results);

			Assert.IsTrue(_sut.Send("customerKey", new Subscriber()));
		}
		[Test]
		public void Send_ReturnsFalse() {
			var requestId = "requestId";
			var overallStatus = "BAD";
			var results = new CreateResult[0];
			_soapClient.Setup(x => x.Create(It.IsAny<CreateOptions>(), It.IsAny<APIObject[]>(), out requestId, out overallStatus))
				.Returns(results);

			Assert.IsFalse(_sut.Send("customerKey", new Subscriber()));
		}
		[Test]
		public void Send_CatchesException() {
			var requestId = "requestId";
			var overallStatus = "BAD";
			var results = new CreateResult[0];
			_soapClient.Setup(x => x.Create(It.IsAny<CreateOptions>(), It.IsAny<APIObject[]>(), out requestId, out overallStatus))
				.Callback(() => throw new Exception("boom!"))
				.Returns(results);

			Assert.IsFalse(_sut.Send("customerKey", new Subscriber()));
		}
		#endregion

	}
}
