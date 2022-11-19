using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Seiumb.Feature.ContactUs.Extensions;
using Neambc.Seiumb.Feature.ContactUs.Models;
using Neambc.Seiumb.Feature.ContactUs.Repositories;
using Neambc.Seiumb.Foundation.Analytics.GTM;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System.Linq;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.ContactUs.Controllers
{
    public class ContactUsController : BaseController {

		private const string CONTACT_US_VIEW = "/Views/ContactUs/Renderings/ContactUs.cshtml";
        private readonly IGTMServiceSeiumb _gtmService;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
		private readonly ICaptchaManager _captchaManager;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;

		public ContactUsController(IGTMServiceSeiumb gtmService, ISeiumbProfileManager seiumbProfileManager, ICaptchaManager captchaManager, IGlobalConfigurationManager globalConfigurationManager) {
            _gtmService = gtmService;
            _seiumbProfileManager = seiumbProfileManager;
			_captchaManager = captchaManager;
			_globalConfigurationManager = globalConfigurationManager;
		}

        public ActionResult ContactUs() {
			var contactUsModel = new ContactUsModel();
			contactUsModel.Initialize(RenderingContext.Current.Rendering);
            var profile = _seiumbProfileManager.GetProfile();
			contactUsModel.CaptchaKey = _globalConfigurationManager.CaptchaKeySeiumb;

			if (string.IsNullOrEmpty(profile.MdsId)) return View(CONTACT_US_VIEW, contactUsModel);
            contactUsModel.FillModel(profile);
			return View(CONTACT_US_VIEW, contactUsModel);
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateFormHandler]
		
#pragma warning disable RECS0154 // Parameter is never used
		public ActionResult ContactUs(ContactUsModel model, FormCollection formCollection)
		{
			model.Initialize(RenderingContext.Current.Rendering);
			var resultCaptcha = _captchaManager.ExecutePostRecaptcha(formCollection["g-recaptcha-response"], _globalConfigurationManager.CaptchaSecretSeiumb);

			if (resultCaptcha)
			{
#pragma warning restore RECS0154 // Parameter is never used
				if (ModelState.IsValid)
				{
					var profile = _seiumbProfileManager.GetProfile();

					if (string.IsNullOrEmpty(profile.MdsId)) profile.MdsId = "000000000";
					var response = ContactUsRepository.Instance.ContactUsCall(model, profile.MdsId);

					var contactUsPage = RenderingContext.Current.Rendering.Item;
					Item thankYouPageItem = null;
					Item errorPageItem = null;
					if (contactUsPage != null && contactUsPage.HasChildren)
					{
						thankYouPageItem = contactUsPage.GetChildren().FirstOrDefault(item => item.TemplateID.Equals(Templates.ThankYouPageTemplate.ID));
						errorPageItem = contactUsPage.GetChildren().FirstOrDefault(item => item.TemplateID.Equals(Templates.ErrorPageTemplate.ID));
					}
					if (response)
					{

						var dataLayerData = _gtmService.GetGtmEvent(new AccountSeiumb
						{
							Event = "account",
							AccountSection = "contact us",
							AccountAction = RenderingContext.Current.Rendering.Item.Fields[Templates.ContactUs.Fields.FormTitle]?.Value ?? string.Empty,
							CTAText = RenderingContext.Current.Rendering.Item.Fields[Templates.ContactUs.Fields.SubmitButton]?.Value ?? string.Empty
						});

						HttpContext.Session["GTMAction"] = dataLayerData;
						HttpContext.Session["EventSuccess"] = true;

						var thankYouUrl = LinkManager.GetItemUrl(thankYouPageItem);
						return Redirect(thankYouUrl);
					}

					var errorPageUrl = LinkManager.GetItemUrl(errorPageItem);
					return Redirect(errorPageUrl);
				}

				model.Initialize(RenderingContext.Current.Rendering);

				var modelStateVal = ViewData.ModelState["FirstName"];

				model.HasErrorFirstName = modelStateVal.Errors.Count > 0;
				if (model.HasErrorFirstName)
				{
					model.HasErrorFirstNameInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
					model.HasErrorFirstNameLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
				}

				modelStateVal = ViewData.ModelState["LastName"];

				model.HasErrorLastName = modelStateVal.Errors.Count > 0;
				if (model.HasErrorLastName)
				{
					model.HasErrorLastNameInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
					model.HasErrorLastNameLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
				}
				modelStateVal = ViewData.ModelState[nameof(model.Phone)];
				model.HasErrorPhone = modelStateVal.Errors.Count > 0;
				if (model.HasErrorPhone) model.HasErrorPhoneLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;

				modelStateVal = ViewData.ModelState[nameof(model.Email)];
				model.HasErrorEmail = modelStateVal.Errors.Count > 0;
				if (model.HasErrorEmail)
				{
					model.HasErrorEmailLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
					model.HasErrorEmailFormat = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Email Format")) != null;
				}

				model.HasErrorState = ViewData.ModelState["State"].Errors.Count > 0;
				model.HasErrorTopic = ViewData.ModelState["Topic"].Errors.Count > 0;
				model.HasErrorMessage = ViewData.ModelState["Message"].Errors.Count > 0;
				model.HasErrorLocalUnion = ViewData.ModelState["LocalUnion"].Errors.Count > 0;
				modelStateVal = ViewData.ModelState["Message"];
				if (model.HasErrorMessage) model.HasErrorMessageInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("EH")) != null;
				return View(CONTACT_US_VIEW, model);
			}
            else
            {
				model.HasCaptchaError = true;
				model.CaptchaKey = _globalConfigurationManager.CaptchaKeySeiumb;
				return View(CONTACT_US_VIEW, model);
			}
		}
	}
}