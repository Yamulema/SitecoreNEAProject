using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
	public class UnsubscribeNeambController : BaseController
	{
        private readonly ISubscriptionsManager _subscriptionsManager;

        public UnsubscribeNeambController(ISubscriptionsManager subscriptionsManager) {
            _subscriptionsManager = subscriptionsManager;
        }

		/// <summary>
		/// Unsubscribe from Exact target
		/// </summary>
		/// <param name="listid">Subscription list id</param>
		/// <param name="mdsid">Mdsid user id</param>
		/// <param name="cellcode">Cell code</param>
		/// <returns></returns>
		public ActionResult ExecuteUnsubscribe(int listid, string mdsid, string cellcode)
		{
			var unsubscribeModel = new UnsubscribeDTO();
			unsubscribeModel.Initialize(RenderingContext.Current.Rendering);
            var subscriber = _subscriptionsManager.RetrieveSubscriber(mdsid);
            unsubscribeModel.IsSucess = subscriber == null || _subscriptionsManager.AddUpdateSubscription(subscriber.SubscriberKey, listid, subscriber.EmailAddress, SubscriberStatus.Unsubscribed);
			return View("/Views/Neamb.GeneralContent/Renderings/UnsubscribeNeamb.cshtml", unsubscribeModel);
		}
    }
}