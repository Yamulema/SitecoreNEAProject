using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Reminder.Models;
using Neambc.Seiumb.Feature.Reminder.Repositories;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Reminder.Controllers
{
    public class ReminderController : BaseController {

		private const string REMINDER_VIEW = "/Views/Reminder/Renderings/Reminder.cshtml";

		public ActionResult Reminder() {
			var reminderModel = new ReminderModel();
			reminderModel.Initialize(RenderingContext.Current.Rendering);
			return View(REMINDER_VIEW, reminderModel);
		}


		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult Reminder(ReminderModel model) {
			var currentRendering = RenderingContext.Current.Rendering;
			var item = currentRendering.Item;
			var reminderId = item["Code"];
            var seiumbProfileManager = (ISeiumbProfileManager)ServiceLocator.ServiceProvider.GetService(typeof(ISeiumbProfileManager));
            var seiuProfile = seiumbProfileManager.GetProfile();
			//call Oracle DB: insert into reminderlog using reminderId and mdsIndvId
			var result = ReminderRepository.Instance.InsertReminder(reminderId, seiuProfile.MdsId);
			model.Initialize(RenderingContext.Current.Rendering);
			return View(REMINDER_VIEW, model);
		}
	}
}