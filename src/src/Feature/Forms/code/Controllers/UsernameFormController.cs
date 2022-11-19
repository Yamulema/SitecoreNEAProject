using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Forms.Enums;
using Neambc.Seiumb.Feature.Forms.GTM;
using Neambc.Seiumb.Feature.Forms.GTM.Model;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Feature.Forms.Repositories;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class UsernameFormController : BaseController
    {
        private const string USERNAME_FORM_VIEW = "/Views/Forms/UsernameForm.cshtml";
        private readonly IFormsRepository _formsRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IUserRepository _userRepository;
        public UsernameFormController(IFormsRepository formsRepository, IProfileRepository profileRepository, ISeiumbProfileManager seiumbProfileManager,
            IUserRepository userRepository
        )
        {
            _profileRepository = profileRepository;
            _seiumbProfileManager = seiumbProfileManager;
            _userRepository = userRepository;
            _formsRepository = formsRepository;
        }

        public ActionResult UsernameForm()
        {
            var model = new UsernameFormModel();
            model.Initialize(RenderingContext.Current.Rendering);
            if (_userRepository.GetUserStatus().Equals(UserStatusCons.HOT))
                model.CurrentUsername = _seiumbProfileManager.GetProfile().Email;
            return View(USERNAME_FORM_VIEW, model);
        }

        [HttpPost]
        [ValidateFormHandler]

        public ActionResult UsernameForm(UsernameFormModel model)
        {
            var seiuProfile = _seiumbProfileManager.GetProfile();
            if (ModelState.IsValid)
            {
                model.Initialize(RenderingContext.Current.Rendering);
                model.CurrentUsername = seiuProfile.Email;
                var profilePage = Sitecore.Context.Database.GetItem(Templates.ProfilePage.ID);
                Item errorPageItem = null;
                if (profilePage != null && profilePage.HasChildren)
                    errorPageItem = profilePage.GetChildren().FirstOrDefault(item => item.TemplateID.Equals(Templates.ErrorPageTemplate.ID));
                if (model.CurrentUsername.Equals(model.NewUsername) &&
                    model.CurrentUsername.Equals(model.ConfirmNewUsername))
                {
                    if (model.Errors == null) model.Errors = new List<ProfileErrors> { ProfileErrors.USERNAME_NOT_AVAILABLE };
                }
                else
                {
                    var validCode = _profileRepository.IsUsernameAvailable(model.NewUsername);
                    switch (validCode)
                    {
                        case 0: //  username already registered
                            if (model.Errors == null)
                                model.Errors = new List<ProfileErrors> { ProfileErrors.USERNAME_NOT_AVAILABLE };
                            break;
                        case -1: // invalid username MBREQ-1394
                            model.Errors = new List<ProfileErrors> { ProfileErrors.INVALID_USERNAME };
                            break;
                        default:
                            {
                                var response = _profileRepository.UpdateUsername(model.CurrentUsername, model.NewUsername, model.ConfirmNewUsername);
                                if (response)
                                {
                                    var site = Sitecore.Context.Site;
                                    var cookieKey = site.GetCookieKey("lang");
                                    var newCell = string.IsNullOrEmpty(Sitecore.Web.WebUtil.GetCookieValue(cookieKey)) ||
                                        Sitecore.Web.WebUtil.GetCookieValue(cookieKey).Equals("en") ?
                                            Sitecore.Configuration.Settings.GetSetting("ExacttargetChangeUsernameNewCellCodeEN") :
                                            Sitecore.Configuration.Settings.GetSetting("ExacttargetChangeUsernameNewCellCodeES");
                                    var oldCell = string.IsNullOrEmpty(Sitecore.Web.WebUtil.GetCookieValue(cookieKey)) ||
                                        Sitecore.Web.WebUtil.GetCookieValue(cookieKey).Equals("en") ?
                                            Sitecore.Configuration.Settings.GetSetting("ExacttargetChangeUsernameOldCellCodeEN") :
                                            Sitecore.Configuration.Settings.GetSetting("ExacttargetChangeUsernameOldCellCodeES");

                                    _formsRepository.ChangeUsername(model, seiuProfile.MdsId, seiuProfile.FirstName, seiuProfile.LastName, newCell, string.Empty, "TGS01131", true);
                                    _formsRepository.ChangeUsername(model, seiuProfile.MdsId, seiuProfile.FirstName, seiuProfile.LastName, oldCell, string.Empty, "TGS01131", false);
                                    seiuProfile.Email = model.NewUsername;
                                    _seiumbProfileManager.SaveProfile(seiuProfile);
                                    Sitecore.Context.User.Profile.Save();
                                    var gtmService = new GTMServiceSeiumb();
                                    var dataLayerData = gtmService.GetGtmEvent(new Account
                                    {
                                        Event = "account",
                                        AccountSection = "profile",
                                        AccountAction = "update username",
                                        CTAText = RenderingContext.Current.Rendering.Item.Fields[Templates.ProfileTemplate.Fields.UsernameSubmitButton]?.Value ?? string.Empty
                                    });
                                    HttpContext.Session["GTMAction"] = dataLayerData;
                                    HttpContext.Session["EventSuccess"] = true;

                                    LinkField thankYouField = model.Item.Fields[Templates.ProfileTemplate.Fields.ThankYouPage_Username];
                                    var thankYouUrl = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(thankYouField.TargetItem.ID));
                                    return Redirect(thankYouUrl);
                                }
                                else if (errorPageItem != null)
                                {
                                    var errorPageUrl = LinkManager.GetItemUrl(errorPageItem);
                                    return Redirect(errorPageUrl);
                                }
                                break;
                            }
                    }
                }
                model.CurrentUsername = seiuProfile.Email;
                return View(USERNAME_FORM_VIEW, model);
            }
            else
            {
                var modelStateVal = ViewData.ModelState[nameof(model.NewUsername)];
                model.HasErrorNewUsername = modelStateVal.Errors.Count > 0;
                if (model.HasErrorNewUsername) model.HasErrorNewUsernameLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                model.Initialize(RenderingContext.Current.Rendering);
                if (_userRepository.GetUserStatus().Equals(UserStatusCons.HOT))
                    model.CurrentUsername = seiuProfile.Email;
                return View(USERNAME_FORM_VIEW, model);
            }
        }
    }
}