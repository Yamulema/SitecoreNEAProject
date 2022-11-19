using System;
using Neambc.Seiumb.Feature.ContactUs.Models;
using Neambc.Seiumb.Foundation.WebServices.Managers;
using Sitecore;

namespace Neambc.Seiumb.Feature.ContactUs.Repositories {
	public class ContactUsRepository {
		private static ContactUsRepository instance;

		private ContactUsRepository() {
		}

		public static ContactUsRepository Instance {
			get {
				if (instance == null) {
					instance = new ContactUsRepository();
				}
				return instance;
			}
		}

		public bool ContactUsCall(ContactUsModel model, string individualId) {
			var site = Context.Site;

			var cookieKey = site.GetCookieKey("lang");
			var languageSelected = Sitecore.Web.WebUtil.GetCookieValue(cookieKey);
			if (string.IsNullOrEmpty(languageSelected)) {
				languageSelected = "en";
			}

			//variable settings
			var cellcodeOne = languageSelected.Equals("en") ? Sitecore.Configuration.Settings.GetSetting("ExacttargetCellcodeEn") : Sitecore.Configuration.Settings.GetSetting("ExacttargetCellcodeEs");
			var cellcodeTwo = languageSelected.Equals("en") ? Sitecore.Configuration.Settings.GetSetting("ExacttargetCellcodeMscEn") : Sitecore.Configuration.Settings.GetSetting("ExacttargetCellcodeMscEs");

			var campaignCd = Sitecore.Configuration.Settings.GetSetting("ExacttargetCampaignCd");

			var customerDefinitionOne = Sitecore.Configuration.Settings.GetSetting("ExacttargetCustomerDefinitionOne");
			var customerDefinitionTwo = Sitecore.Configuration.Settings.GetSetting("ExacttargetCustomerDefinitionTwo");
			var localUnion = string.IsNullOrEmpty(model.LocalUnion) ? string.Empty : model.LocalUnion;
			//Mail to the user
			var responseOne = ExactTargetServiceManager.SendExactTarget(
				model.FirstName,
				model.LastName,
				cellcodeOne,
				campaignCd,
				individualId,
				DateTime.Now.ToShortDateString(),
				localUnion,
				model.State,
				model.Topic,
				model.Email?.ToLower(),
				model.Message,
				customerDefinitionOne,
				true
			);

			//Mail to the admin
			var responseTwo = ExactTargetServiceManager.SendExactTarget(
				model.FirstName,
				model.LastName,
				cellcodeTwo,
				campaignCd,
				individualId,
				DateTime.Now.ToShortDateString(),
				localUnion,
				model.State,
				model.Topic,
				model.Email?.ToLower(),
				model.Message,
				customerDefinitionTwo,
				false
			);

			return responseOne && responseTwo;
		}
	}
}