using System.Collections.Generic;
using System.Threading.Tasks;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Feature.Account.UnitTest.Fakes {

	public class FakeExactTargetManager : IExactTargetManager {

		#region IExactTargetManager
		public string SendExactTargetService(string customerDefinition, string username, List<KeyValuePair<string, string>> exactTargetParameters,
			string subscriberKey = null) {
			throw new System.NotImplementedException();
		}

		public IEnumerable<APIObject> RetrieveAllSubscriptions(string subscriberKey) {
			throw new System.NotImplementedException();
		}

		public bool UpdateSubscriberList(string subscriberKey, int listId, SubscriberStatus newStatus) {
			throw new System.NotImplementedException();
		}

		public bool AddUpdateSubscriberList(string subscriberKey, int listId, string email, SubscriberStatus newStatus) {
			throw new System.NotImplementedException();
		}

		public bool AddUpdateDataExtension(string customerKey, Dictionary<string, string> properties) {
			throw new System.NotImplementedException();
		}

		public bool UnsubscribeListMail(string subscriberkey, int listid) {
			throw new System.NotImplementedException();
		}

		public Task<bool> TriggeredSendAsync(ExactTargetEmail exactTargetEmail) {
			throw new System.NotImplementedException();
		}

		public bool TriggeredSend(ExactTargetEmail exactTargetEmail) {
			throw new System.NotImplementedException();
		}
		#endregion
	}
}
