using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Forms.Constants;
using Neambc.Seiumb.Feature.Forms.Enums;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.DependencyInjection;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class ZipCodeValidationFormController : BaseController {
        public virtual ISeiumbProfileManager SeiumbProfileManager => (ISeiumbProfileManager)ServiceLocator.ServiceProvider.GetService(typeof(ISeiumbProfileManager));
        public virtual IUserRepository UserRepository => (IUserRepository)ServiceLocator.ServiceProvider.GetService(typeof(IUserRepository));

        private const string ZIPCODE_VALIDATION_FORM = "/Views/Forms/ZipCodeValidationForm.cshtml";
		public ActionResult ZipCodeValidationForm() {
            var seiuProfile = SeiumbProfileManager.GetProfile();
            var zipCodeValidationFormModel = new ZipCodeValidationFormModel();
			if (UserRepository.GetUserStatus().Equals(UserStatusCons.HOT))
			{
				var homeItem = Sitecore.Context.Database.GetItem(Templates.Home.ID);
				return Redirect(LinkManager.GetItemUrl(homeItem));
			}
			else if (UserRepository.GetUserStatus().Equals(UserStatusCons.WARM_COLD)) zipCodeValidationFormModel.FullName = $"{seiuProfile.FirstName} {seiuProfile.LastName}";
            zipCodeValidationFormModel.Initialize(RenderingContext.Current.Rendering);
			return View(ZIPCODE_VALIDATION_FORM, zipCodeValidationFormModel);
		}

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult ZipCodeValidationForm(ZipCodeValidationFormModel model) {
            var seiuProfile = SeiumbProfileManager.GetProfile();

            if (ModelState.IsValid) {
				model.FullName = $"{seiuProfile.FirstName} {seiuProfile.LastName}";
				if (ValidateZipCode(model.ZipCode) && !string.IsNullOrWhiteSpace(seiuProfile.ZipCode)) {
					if (seiuProfile.ZipCode.Equals(model.ZipCode)) {
						HttpContext.Session[SessionConstants.ZIP_VALIDATION] = "true";
						return Redirect(GetRegistrationPageUrl());
					}

					var attempt = HttpContext.Session[SessionConstants.ZIP_VALIDATION_ATTEMPT]?.ToString();
					if (!string.IsNullOrEmpty(attempt)) {
						var attemptCounter = int.Parse(attempt);
						if (attemptCounter > 1) {
							HttpContext.Session[SessionConstants.ZIP_VALIDATION_ATTEMPT] = null;
							UserRepository.LogoutUser();
							return Redirect(GetRegistrationPageUrl());
						} 

						attemptCounter++;
						HttpContext.Session[SessionConstants.ZIP_VALIDATION_ATTEMPT] = attemptCounter;
						
					} else {
						HttpContext.Session[SessionConstants.ZIP_VALIDATION_ATTEMPT] = "1";
					}

					if (model.Errors == null) {
						model.Errors = new List<ZipCodeValidationErrors> {
							ZipCodeValidationErrors.WRONG_ZIPCODE
						};
					}
					model.Initialize(RenderingContext.Current.Rendering);
					return View(ZIPCODE_VALIDATION_FORM, model);

				}
				var modelStateVal = ViewData.ModelState[nameof(model.ZipCode)];
				model.HasErrorZipcode = modelStateVal.Errors.Count > 0;
				if (model.HasErrorZipcode) {
					model.HasErrorZipcodeLength = modelStateVal.Errors.Any(i => i.ErrorMessage.Equals("Error Length"));
				}
				model.Initialize(RenderingContext.Current.Rendering);
				return View(ZIPCODE_VALIDATION_FORM, model);

			}
			return null;
		}

		private string GetRegistrationPageUrl() {
			var registrationPage = Sitecore.Context.Database.GetItem(Templates.RegistrationPage.ID);
			var registrationPageUrl = LinkManager.GetItemUrl(registrationPage);
			return registrationPageUrl;
		}

		private bool ValidateZipCode(string zipcode) {
			return !string.IsNullOrEmpty(zipcode) && int.TryParse(zipcode, out var i) && zipcode.Length == 5;
		}
	}
}