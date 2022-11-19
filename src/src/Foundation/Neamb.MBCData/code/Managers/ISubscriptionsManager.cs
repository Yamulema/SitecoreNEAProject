using System.Collections.Generic;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Model;
namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    public interface ISubscriptionsManager
    {
        /// <summary>
        /// Gets All Subscriptions a Subscriber is On given its Mdsid
        /// </summary>
        /// <param name="mdsid"></param>
        /// <returns></returns>
        IEnumerable<ListSubscriber> GetAllSubscriptions(string mdsid);
        bool UpdateSubscription(string mdsid, int listId, SubscriberStatus newStatus);
        bool AddUpdateSubscription(string mdsid, int listId, string email, SubscriberStatus newStatus);
        Subscriber RetrieveSubscriber(string subscriberKey);
    }
}