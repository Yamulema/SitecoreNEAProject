using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Product.Controllers {
	public class ReminderServiceController : BaseController {
		private readonly IReminderService _reminderService;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        protected ILoginHandlerPostAction _loginHandlerPostAction;

        public ReminderServiceController(IReminderService reminderService, ISessionAuthenticationManager sessionAuthenticationManager, ILoginHandlerPostAction loginHandlerPostAction) {
			_reminderService = reminderService;
			_sessionAuthenticationManager = sessionAuthenticationManager;
            _loginHandlerPostAction = loginHandlerPostAction;
        }

		[HttpPost]

        public ActionResult SetReminder(string reminderIdSweepstake,string componentIdSweepstake)
        {
            try
            {
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                switch (accountMembership.Status)
                {
                    case StatusEnum.Hot:
                        var success = _reminderService.SetReminder(reminderIdSweepstake, accountMembership.Mdsid);
                        if (success)
                        {
                            return Redirect(Request.UrlReferrer.AbsolutePath);
                        }
                        else
                        {
                            Log.Error($"Error while Setting User Reminder with reminderId:{reminderIdSweepstake}", this);
                            return new HttpError500Result();
                        }
                    default:
                        var siteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
                        var loginUrl = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(((LinkField)siteSettings.Fields[Templates.SiteSettings.Fields.SignIn])?.TargetID));
                        _loginHandlerPostAction.SaveTrackingPageToRedirection(LoginHandlerEnum.Sweepstake, componentIdSweepstake);
                        return Redirect(loginUrl);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error in NotifyProductAvailable action for reminderId:{reminderIdSweepstake}", e, this);
                return new HttpError500Result();
            }
        }
    }
}