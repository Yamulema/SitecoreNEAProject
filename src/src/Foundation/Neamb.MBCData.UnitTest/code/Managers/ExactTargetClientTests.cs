using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers {
	[TestFixture]
	public class ExactTargetClientTests {

		#region Fields
		private Mock<IGlobalConfigurationManager> _manager;
		private Mock<IExactTargetProxy> _proxy;
		private IExactTargetClient _sut;
		#endregion

		#region Instrumentation
		[SetUp]
		public void Setup() {
			_manager = new Mock<IGlobalConfigurationManager>();
			_proxy = new Mock<IExactTargetProxy>();
			_sut = new ExactTargetClient(_manager.Object, _proxy.Object);
		}
		#endregion

		#region Tests

		[Test]
		public void Constructor_ThrowsOnNullArgs() {
			Assert.Throws<ArgumentNullException>(() => new ExactTargetClient(null, null));
			Assert.Throws<ArgumentNullException>(() => new ExactTargetClient(_manager.Object, null));
			Assert.Throws<ArgumentNullException>(() => new ExactTargetClient(null, _proxy.Object));
		}

		[Test]
		public void SendExactTargetService_SubscriberKey() {
			string capturedArg1 = null;
			Subscriber capturedArg2 = null;
			_proxy.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Subscriber>()))
				.Callback((string x, Subscriber y) => {
					capturedArg1 = x;
					capturedArg2 = y;
				})
				.Returns(true);

			var parameters = new List<KeyValuePair<string, string>>() {
				new KeyValuePair<string, string>("key1", "value1"),
				new KeyValuePair<string, string>("key2", "value2"),
				new KeyValuePair<string, string>("key3", "value3")
			};
			const string customerDef = "customerDefinition";
			const string subscriberKey = "subscriberKey";
			const string username = "username";
			_sut.SendExactTargetService(
				customerDef,
				username,
				parameters,
				subscriberKey
			);
			// check that the proper arguments were packages
			Assert.AreEqual(customerDef, capturedArg1);

			var expectedSubscriber = new Subscriber {
				SubscriberKey = subscriberKey,
				EmailAddress = username,
				Attributes = parameters.Select(x => new ExactTargetService.Attribute {
					Name = x.Key,
					Value = x.Value
				})
					.ToArray()
			};

			Assert.AreEqual(expectedSubscriber.SubscriberKey, capturedArg2.SubscriberKey);
			Assert.AreEqual(expectedSubscriber.EmailAddress, capturedArg2.EmailAddress);
			// varify same number of attributes in the same order
			Assert.AreEqual(expectedSubscriber.Attributes.Length, capturedArg2.Attributes.Length);
			for (var ndx = 0; ndx < expectedSubscriber.Attributes.Length; ndx++) {
				var expectedAttribute = expectedSubscriber.Attributes[ndx];
				var resultAttribute = capturedArg2.Attributes[ndx];
				Assert.AreEqual(expectedAttribute.Name, resultAttribute.Name);
				Assert.AreEqual(expectedAttribute.Value, resultAttribute.Value);
			}
		}
		[Test]
		public void SendExactTargetService_Username() {
			string capturedArg1 = null;
			Subscriber capturedArg2 = null;
			_proxy.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Subscriber>()))
				.Callback((string x, Subscriber y) => {
					capturedArg1 = x;
					capturedArg2 = y;
				})
				.Returns(true);

			var parameters = new List<KeyValuePair<string, string>>() {
				new KeyValuePair<string, string>("key1", "value1"),
				new KeyValuePair<string, string>("key2", "value2"),
				new KeyValuePair<string, string>("key3", "value3")
			};
			const string customerDef = "customerDefinition";
			const string username = "username";
			_sut.SendExactTargetService(
				customerDef,
				username,
				parameters
			);
			// check that the proper arguments were packages
			Assert.AreEqual(customerDef, capturedArg1);

			var expectedSubscriber = new Subscriber {
				// the entire purpose of this test is to use this username instead of subscriber key
				SubscriberKey = username,
				EmailAddress = username,
				Attributes = parameters.Select(x => new ExactTargetService.Attribute {
					Name = x.Key,
					Value = x.Value
				})
					.ToArray()
			};

			Assert.AreEqual(expectedSubscriber.SubscriberKey, capturedArg2.SubscriberKey);
			Assert.AreEqual(expectedSubscriber.EmailAddress, capturedArg2.EmailAddress);
			// varify same number of attributes in the same order
			Assert.AreEqual(expectedSubscriber.Attributes.Length, capturedArg2.Attributes.Length);
		}
		[Test]
		public void SendExactTargetService_NullAttributes() {
			string capturedArg1 = null;
			Subscriber capturedArg2 = null;
			_proxy.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Subscriber>()))
				.Callback((string x, Subscriber y) => {
					capturedArg1 = x;
					capturedArg2 = y;
				})
				.Returns(true);

			const string customerDef = "customerDefinition";
			const string username = "username";
			var expectedSubscriber = new Subscriber {
				SubscriberKey = username,
				EmailAddress = username
			};

			_sut.SendExactTargetService(
				customerDef,
				username,
				null
			);

			// check that the proper arguments were packages
			Assert.AreEqual(customerDef, capturedArg1);
			Assert.AreEqual(expectedSubscriber.SubscriberKey, capturedArg2.SubscriberKey);
			Assert.AreEqual(expectedSubscriber.EmailAddress, capturedArg2.EmailAddress);
			// varify same number of attributes in the same order
			Assert.AreEqual(0, capturedArg2.Attributes.Length);
		}

		[Test]
		public void RetrieveAllSubscriptions() {
			RetrieveRequest capturedArg = null;

			const int clientId = 123;
			const string subscriberKey = "subscriber";

			_manager.Setup(x => x.ExacttargetClientId).Returns(clientId);

			_proxy.Setup(x => x.Retrieve(It.IsAny<RetrieveRequest>()))
				.Callback((RetrieveRequest x) => {
					capturedArg = x;
				})
				.Returns(new APIObject[0]);

			_sut.RetrieveAllSubscriptions(subscriberKey);

			Assert.AreEqual(clientId, capturedArg.ClientIDs.First().ID);
			Assert.IsTrue(capturedArg.ClientIDs.First().IDSpecified);
			Assert.AreEqual(subscriberKey, ((SimpleFilterPart)capturedArg.Filter).Value.First());
		}

		[Test]
		public void UpdateSubscriberList() {
			UpdateRequest capturedArg = null;
			const string subscriberKey = "subscriber";
			const int listId = 1;
			const SubscriberStatus newStatus = SubscriberStatus.Unsubscribed;
			const int clientId = 123;
			_manager.Setup(x => x.ExacttargetClientId).Returns(clientId);
			_proxy.Setup(x => x.Update(It.IsAny<UpdateRequest>()))
				.Callback((UpdateRequest x) => {
					capturedArg = x;
				})
				.Returns(true);
			_sut.UpdateSubscriberList(subscriberKey,listId,newStatus);
			Assert.AreEqual(clientId, capturedArg.Objects.First().Client.ID);
			var subscriber = (Subscriber)capturedArg.Objects.First();
			Assert.AreEqual(subscriberKey, subscriber.SubscriberKey);
			var subscriberList = subscriber.Lists.First();
			Assert.AreEqual(listId, subscriberList.ID);
			Assert.AreEqual(newStatus, subscriberList.Status);
			Assert.IsTrue(subscriberList.IDSpecified);
			Assert.IsTrue(subscriberList.StatusSpecified);
		}

		[Test]
		public void AddUpdateSubscriberList() {
			CreateRequest capturedArg = null;
			const string subscriberKey = "subscriber";
			const int listId = 1;
			const SubscriberStatus newStatus = SubscriberStatus.Unsubscribed;
			const int clientId = 123;
			const string email = "mpancho@oshyn.com";
			_manager.Setup(x => x.ExacttargetClientId).Returns(clientId);
			_proxy.Setup(x => x.CreateCall(It.IsAny<CreateRequest>()))
				.Callback((CreateRequest x) => {
					capturedArg = x;
				})
				.Returns(new CreateResult[0]);
			_sut.AddUpdateSubscriberList(subscriberKey, listId,email,newStatus);
			Assert.AreEqual(clientId, capturedArg.Objects.First().Client.ID);
			var subscriber = (Subscriber)capturedArg.Objects.First();
			Assert.AreEqual(subscriberKey, subscriber.SubscriberKey);
			Assert.AreEqual(email, subscriber.EmailAddress);
			var subscriberList = subscriber.Lists.First();
			Assert.AreEqual(listId, subscriberList.ID);
			Assert.AreEqual(newStatus, subscriberList.Status);
			Assert.IsTrue(subscriberList.IDSpecified);
			Assert.IsTrue(subscriberList.StatusSpecified);
		}

		[Test]
		public void AddUpdateDataExtension()
		{
			UpdateRequest capturedArg = null;
			const string customerKey = "customerKey";
			var properties = new Dictionary<string, string>() {
					{"key1", "val1"},
				{"key2", "val2"},
				{"key3", "val3"},
			};

			var dataExtensionObjectExpected = new DataExtensionObject() {
				CustomerKey = customerKey,
				Properties = properties.Select(x => new APIProperty() {
							Name = x.Key,
							Value = x.Value
						}).ToArray()
			};

			_proxy.Setup(x => x.Update(It.IsAny<UpdateRequest>()))
				.Callback((UpdateRequest x) =>
				{
					capturedArg = x;
				})
				.Returns(true);
			_sut.AddUpdateDataExtension(customerKey,properties);
			var dataExtension = (DataExtensionObject)capturedArg.Objects.First();

			Assert.AreEqual(dataExtensionObjectExpected.CustomerKey, dataExtension.CustomerKey);
			var propertiesDataExtension = dataExtension.Properties;
			Assert.AreEqual(dataExtensionObjectExpected.Properties.Length, propertiesDataExtension.Length);
			for (var ndx = 0; ndx < dataExtensionObjectExpected.Properties.Length; ndx++)
			{
				var expectedAttribute = dataExtensionObjectExpected.Properties[ndx];
				var resultAttribute = dataExtension.Properties[ndx];
				Assert.AreEqual(expectedAttribute.Name, resultAttribute.Name);
				Assert.AreEqual(expectedAttribute.Value, resultAttribute.Value);
			}
		}

		[Test]
		public void UnsubscribeListMail() {
			UpdateRequest capturedArg = null;
			const string subscriberKey = "subscriberkey";
			const int listId = 1;
			const int clientId = 123;
			var unSubscriberStatus = SubscriberStatus.Unsubscribed;

			_manager.Setup(x => x.ExacttargetClientId).Returns(clientId);
			_proxy.Setup(x => x.Update(It.IsAny<UpdateRequest>()))
				.Callback((UpdateRequest x) =>
				{
					capturedArg = x;
				})
				.Returns(true);
			_sut.UnsubscribeListMail(subscriberKey, listId);
			var subscriber = (Subscriber)capturedArg.Objects.First();
			Assert.AreEqual(subscriberKey, subscriber.SubscriberKey);
			Assert.AreEqual(clientId, capturedArg.Objects.First().Client.ID);
			Assert.IsTrue(capturedArg.Objects.First().Client.IDSpecified);
			var subscriberList = subscriber.Lists.First();
			Assert.AreEqual(listId, subscriberList.ID);
			Assert.AreEqual(unSubscriberStatus, subscriberList.Status);
			Assert.IsTrue(subscriberList.IDSpecified);
			Assert.IsTrue(subscriberList.StatusSpecified);
		}

		[Test]
		public void TriggeredSendAsync() {
			const string customerKey = "customerkey";
			const string subscriberKey = "subscriberkey";
			const string emailTo = "marielapancho@hotmail.com";
			const int clientId = 123;
			var attributes = new Dictionary<string, string>(){
				{"key1", "val1"},
				{"key2", "val2"},
				{"key3", "val3"},
			};
			CreateRequest capturedArg = null;

			var exactTargetEmail = new ExactTargetEmail {
				CustomerKey = customerKey,
				EmailTo = emailTo,
				SubscriberKey = subscriberKey,
				Attributes = attributes
			};
			_manager.Setup(x => x.ExacttargetClientId).Returns(clientId);
			
			_proxy.Setup(x => x.CreateAsync(It.IsAny<CreateRequest>()))
				.Callback((CreateRequest x) =>
				{
					capturedArg = x;
				})
				.Returns(Task.FromResult(true));
			_sut.TriggeredSendAsync(exactTargetEmail);
			var triggeredSend = (TriggeredSend)capturedArg.Objects.First();
			var subscriber = triggeredSend.Subscribers.First();
			Assert.AreEqual(clientId, capturedArg.Objects.First().Client.ID);
			Assert.AreEqual(customerKey, triggeredSend.TriggeredSendDefinition.CustomerKey);
			Assert.AreEqual(subscriberKey, subscriber.SubscriberKey);
			Assert.AreEqual(emailTo, subscriber.EmailAddress);
			var attributesReturned = triggeredSend.Attributes;
			var counter = 0;
			foreach (var attributeItem in attributes) {
				var expectedAttribute = attributesReturned[counter];
				Assert.AreEqual(expectedAttribute.Name, attributeItem.Key);
				Assert.AreEqual(expectedAttribute.Value, attributeItem.Value);
				counter++;
			}
		}

		[Test]
		public void TriggeredSend() {
			const string customerKey = "customerkey";
			const string subscriberKey = "subscriberkey";
			const string emailTo = "marielapancho@hotmail.com";
			const int clientId = 123;
			var attributes = new Dictionary<string, string>(){
				{"key1", "val1"},
				{"key2", "val2"},
				{"key3", "val3"},
			};
			CreateRequest capturedArg = null;

			var exactTargetEmail = new ExactTargetEmail
			{
				CustomerKey = customerKey,
				EmailTo = emailTo,
				SubscriberKey = subscriberKey,
				Attributes = attributes
			};
			_manager.Setup(x => x.ExacttargetClientId).Returns(clientId);

			_proxy.Setup(x => x.Create(It.IsAny<CreateRequest>()))
				.Callback((CreateRequest x) =>
				{
					capturedArg = x;
				})
				.Returns(true);
			_sut.TriggeredSend(exactTargetEmail);
			var triggeredSend = (TriggeredSend)capturedArg.Objects.First();
			var subscriber = triggeredSend.Subscribers.First();
			Assert.AreEqual(clientId, capturedArg.Objects.First().Client.ID);
			Assert.AreEqual(customerKey, triggeredSend.TriggeredSendDefinition.CustomerKey);
			Assert.AreEqual(subscriberKey, subscriber.SubscriberKey);
			Assert.AreEqual(emailTo, subscriber.EmailAddress);
			var attributesReturned = triggeredSend.Attributes;
			var counter = 0;
			foreach (var attributeItem in attributes)
			{
				var expectedAttribute = attributesReturned[counter];
				Assert.AreEqual(expectedAttribute.Name, attributeItem.Key);
				Assert.AreEqual(expectedAttribute.Value, attributeItem.Value);
				counter++;
			}
		}
		#endregion
	}
}
