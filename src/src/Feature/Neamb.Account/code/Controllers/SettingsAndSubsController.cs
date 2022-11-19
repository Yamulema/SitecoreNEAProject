using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers {
	public class SettingsAndSubsController : BaseController {
		private readonly ISessionAuthenticationManager _sessionManager;
		private readonly ISubscriptionsManager _subscriptionsManager;
		private readonly IActionReminderService _actionReminderService;

		public SettingsAndSubsController(ISessionAuthenticationManager sessionManager, ISubscriptionsManager subscriptionsManager, ICacheManager cacheManager, ISessionAuthenticationManager sessionAuthenticationManager, IActionReminderService actionReminderService) {
			_sessionManager = sessionManager;
			_subscriptionsManager = subscriptionsManager;
			_actionReminderService = actionReminderService;
		}
		public ActionResult SettingsAndSubs() {
			var model = new SettingsAndSubsDTO();
			model.Initialize(RenderingContext.Current.Rendering);
			var datasource = model.Item ?? model.PageItem;
			var accountMembership = _sessionManager.GetAccountMembership();
			var mdsid = Sitecore.Context.PageMode.IsExperienceEditor
				? string.Empty
				: accountMembership?.Mdsid?.PadLeft(9, '0');

			model.Newsletters = GetNewsletters(datasource, mdsid);
			model.UserStatus = accountMembership.Status;

			var postbackResult = Request.QueryString[Configuration.SubscriptionsResultParameterName];

			if (bool.TryParse(postbackResult, out var isSuccessful)) {
				model.HasGeneralError = !isSuccessful;
			} else {
				model.HasGeneralError = false;
			}

			if (accountMembership.Status == StatusEnum.Hot) {
				var pageItem = PageContext.Current.Item;
				if (pageItem.IsDerived(Templates.WizardStep.ID)) {
					_actionReminderService.SetVisited(PageType.Subscription, accountMembership.Username);
				}
			}
			return View("/Views/Neamb.Account/SettingsAndSubs.cshtml", model);
		}

		private List<Newsletter> GetNewsletters(Item datasource, string mdsid) {
			if (Sitecore.Context.PageMode.IsExperienceEditor) {
				var newsletterItems = ((MultilistField)datasource.Fields[Templates.SettingsAndSubscriptions.Fields.Newsletters]).GetItems();
				return newsletterItems.Select(GetNewsletter).ToList();
			} else {
				var accountMembership = _sessionManager.GetAccountMembership();
				if (accountMembership.Status != StatusEnum.Hot) {
					return new List<Newsletter>();
				}

				var newsletterItems =
					((MultilistField)datasource.Fields[Templates.SettingsAndSubscriptions.Fields.Newsletters])
					.GetItems();
				var newsletters = newsletterItems.Select(GetNewsletter).ToList();
				var subscriptions = _subscriptionsManager.GetAllSubscriptions(mdsid).ToList();

				foreach (var newsletter in newsletters) {
					var subscription = subscriptions.FirstOrDefault(x => x.ListID.Equals(newsletter.Id));
					newsletter.SubscriberStatus = subscription?.Status ?? SubscriberStatus.Unsubscribed;
                }

				return newsletters.ToList();
			}
		}

		private Newsletter GetNewsletter(Item item) {
			return new Newsletter() {
				Item = item,
				Id = int.TryParse(item.Fields[Templates.Newsletters.Fields.Id]?.Value, out var id) ? id : 0,
				Subscribe = item.Fields[Templates.Newsletters.Fields.Subscribe]?.Value,
				Unsubscribe = item.Fields[Templates.Newsletters.Fields.Unsubscribe]?.Value
			};
		}

		[HttpPost]
		
		public JsonResult ChangeSubscription(int listId, SubscriberStatus newStatus) {
			var accountMembership = _sessionManager.GetAccountMembership();

			switch (accountMembership.Status) {
				case StatusEnum.Hot when newStatus == SubscriberStatus.Active || newStatus == SubscriberStatus.Unsubscribed:
					var wasProcessed = _subscriptionsManager.AddUpdateSubscription(
						accountMembership.Mdsid.PadLeft(9, '0'),
						listId, accountMembership.Profile.Email,
						newStatus);
					if (wasProcessed) {
						//Response.StatusCode = (int)HttpStatusCode.OK;
						return Json(new { wasProcessed = true });
					} else {
						//Response.StatusCode = (int)HttpStatusCode.InternalServerError;
						return Json(new {
							wasProcessed = false,
							message = "There was an error while processing the request."
						});
					}
				case StatusEnum.Hot:
					//Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(new {
						wasProcessed = false,
						message = "Unsupported subscription status."
					});
				default:
					//Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					return Json(new { wasProcessed = false, message = "Unauthorized user." });
			}
		}
	}
}