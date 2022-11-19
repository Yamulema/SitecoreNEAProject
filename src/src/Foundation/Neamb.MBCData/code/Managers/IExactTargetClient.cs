using System.Collections.Generic;
using System.Threading.Tasks;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	public interface IExactTargetClient {
		bool SendExactTargetService(
			string customerDefinition,
			string username,
			List<KeyValuePair<string, string>> exactTargetParameters,
			string subscriberKey = null
		);
		IEnumerable<APIObject> RetrieveAllSubscriptions(string subscriberKey);
		bool UpdateSubscriberList(string subscriberKey, int listId, SubscriberStatus newStatus);
		bool AddUpdateSubscriberList(
			string subscriberKey,
			int listId,
			string email,
			SubscriberStatus newStatus
		);
		bool AddUpdateDataExtension(string customerKey, Dictionary<string, string> properties);
		/// <summary>
		/// Unsubscribe a email list in Eact target
		/// </summary>
		/// <param name="subscriberKey">Mdsid</param>
		/// <param name="listId">Subscription list id</param>
		/// <returns></returns>
		bool UnsubscribeListMail(string subscriberKey, int listId);
		Task<bool> TriggeredSendAsync(ExactTargetEmail exactTargetEmail);
		bool TriggeredSend(ExactTargetEmail exactTargetEmail);
        IEnumerable<Subscriber> RetrieveSubscriber(string subscriberKey);
    }
}