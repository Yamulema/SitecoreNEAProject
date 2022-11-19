using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUser;
using Neambc.Seiumb.Feature.Forms.Enums;
using Neambc.Seiumb.Feature.Forms.Extensions;
using Neambc.Seiumb.Feature.Forms.GTM;
using Neambc.Seiumb.Feature.Forms.GTM.Model;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Repositories;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.WebServices;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class ProfileFormController : BaseController
    {
        private const string PROFILE_FORM_VIEW = "/Views/Forms/ProfileForm.cshtml";

        private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly IUpdateUserService _updateUserService;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IUserRepository _userRepository;

        public ProfileFormController(IWebServicesConfiguration webServicesConfiguration, IUpdateUserService updateUserService, ISeiumbProfileManager seiumbProfileManager,
            IUserRepository userRepository
        )
        {
            _webServicesConfiguration = webServicesConfiguration;
            _updateUserService = updateUserService;
            _seiumbProfileManager = seiumbProfileManager;
            _userRepository = userRepository;
        }
        public ActionResult ProfileForm()
        {
            var model = new ProfileFormModel();
            model.Initialize(RenderingContext.Current.Rendering);
            var profile = _seiumbProfileManager.GetProfile();

            if (!_userRepository.GetUserStatus().Equals(UserStatusCons.HOT)) return View(PROFILE_FORM_VIEW, model);
            model.FillModel(profile);
            model.SendInformation = (profile.EmailPermission ?? string.Empty).Equals(((int)MyAccountProfile.EMAIL_PERMISSION_CHEKED).ToString());
            return View(PROFILE_FORM_VIEW, model);
        }

        [HttpPost]
        [ValidateFormHandler]
        [ValidateInput(false)]

        public ActionResult ProfileForm(ProfileFormModel model)
        {
            if (ModelState.IsValid)
            {
                model.Initialize(RenderingContext.Current.Rendering);
                var profilePage = Sitecore.Context.Database.GetItem(Templates.ProfilePage.ID);
                Item errorPageItem = null;
                if (profilePage != null && profilePage.HasChildren)
                    errorPageItem = profilePage.GetChildren().FirstOrDefault(item =>
                        item.TemplateID.Equals(Templates.ErrorPageTemplate.ID));

                var profile = _seiumbProfileManager.GetProfile();
                var response = _updateUserService.UpdateUser(profile.Email, Convert.ToInt32(profile.Webuserid), model.FirstName, model.LastName,
                  model.Address, model.City, model.State, model.ZipCode, Utilities.Utilities.FormatDate(model.DateOfBirth),
                  Utilities.Utilities.FormatPhone(model.Phone), Utilities.Utilities.FormatPermission(model.SendInformation),
                  Convert.ToInt32(_webServicesConfiguration.UnionId));

                if (response != null && response.Success)
                {
                    _seiumbProfileManager.SaveProfile(profile.ToProfile(model));
                    Sitecore.Context.User.Profile.Save();
                    Sitecore.Context.User.Profile.Reload();

                    var gtmService = new GTMServiceSeiumb();
                    var dataLayerData = gtmService.GetGtmEvent(new Account
                    {
                        Event = "account",
                        AccountSection = "profile",
                        AccountAction = "update profile",
                        CTAText = RenderingContext.Current.Rendering.Item.Fields[Templates.ProfileTemplate.Fields.ProfileSubmitButton]?.Value ?? string.Empty
                    });
                    HttpContext.Session["GTMAction"] = dataLayerData;
                    HttpContext.Session["EventSuccess"] = true;

                    LinkField thankYouField = model.Item.Fields[Templates.ProfileTemplate.Fields.ThankYouPage_Profile];
                    var thankYouUrl = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(thankYouField.TargetItem.ID));
                    return Redirect(thankYouUrl);
                }

                var errorPageUrl = LinkManager.GetItemUrl(errorPageItem);
                if (!response.Success && errorPageItem != null) return Redirect(errorPageUrl);
                return View(PROFILE_FORM_VIEW, model);
            }
            else
            {
                if (model.Errors == null)
                    model.Errors = new List<ProfileErrors> { ProfileErrors.VALUES_REQUIRED };
                var modelStateVal = ViewData.ModelState[nameof(model.FirstName)];

                model.HasErrorFirstName = modelStateVal.Errors.Count > 0;
                if (model.HasErrorFirstName)
                {
                    model.HasErrorFirstNameInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
                    model.HasErrorFirstNameLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                }
                modelStateVal = ViewData.ModelState[nameof(model.LastName)];

                model.HasErrorLastName = modelStateVal.Errors.Count > 0;
                if (model.HasErrorLastName)
                {
                    model.HasErrorLastNameInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
                    model.HasErrorLastNameLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                }
                modelStateVal = ViewData.ModelState[nameof(model.City)];

                model.HasErrorCity = modelStateVal.Errors.Count > 0;
                if (model.HasErrorCity)
                {
                    model.HasErrorCityInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
                    model.HasErrorCityLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                }

                modelStateVal = ViewData.ModelState[nameof(model.Address)];

                model.HasErrorAddress = modelStateVal.Errors.Count > 0;
                if (model.HasErrorAddress)
                {
                    model.HasErrorAddressInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
                    model.HasErrorAddressLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                }

                modelStateVal = ViewData.ModelState[nameof(model.Phone)];
                model.HasErrorPhone = modelStateVal.Errors.Count > 0;
                if (model.HasErrorPhone) model.HasErrorPhoneLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;

                modelStateVal = ViewData.ModelState[nameof(model.DateOfBirth)];
                model.HasErrorBirthDate = modelStateVal.Errors.Count > 0;

                if (model.HasErrorBirthDate)//verify the age greater than 16 years
                    model.HasErrorDateOfBirthAge = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("DateRangeError")) != null;

                model.Initialize(RenderingContext.Current.Rendering);
                return View(PROFILE_FORM_VIEW, model);
            }
        }
    }
}