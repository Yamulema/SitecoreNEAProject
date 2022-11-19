using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Seiumb.Feature.Forms.Enums;
using Neambc.Seiumb.Feature.Forms.GTM;
using Neambc.Seiumb.Feature.Forms.GTM.Model;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class PasswordFormController : BaseController {
		public const string PASSWORD_FORM_VIEW = "/Views/Forms/PasswordForm.cshtml";
        private readonly IAccountManager _accountManager;
        private readonly IUserRepository _userRepository;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        public PasswordFormController(IAccountManager accountManager, IUserRepository userRepository, ISeiumbProfileManager seiumbProfileManager) {
            _accountManager = accountManager;
            _userRepository = userRepository;
            _seiumbProfileManager = seiumbProfileManager;
        }

		public ActionResult PasswordForm() {
			var model = new PasswordFormModel();
			model.Initialize(RenderingContext.Current.Rendering);
			return View(PASSWORD_FORM_VIEW, model);
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateFormHandler]
		public ActionResult PasswordForm(PasswordFormModel model) {
			if (ModelState.IsValid) {
				model.Initialize(RenderingContext.Current.Rendering);
				var profilePage = Sitecore.Context.Database.GetItem(Templates.ProfilePage.ID);
				Item errorPageItem = null;
				if (profilePage != null && profilePage.HasChildren)
                    errorPageItem = profilePage.GetChildren().FirstOrDefault(item => item.TemplateID.Equals(Templates.ErrorPageTemplate.ID));
                var username = _seiumbProfileManager.GetProfile().Email;
                var updatePwdResponse = _accountManager.UpdatePassword(username, model.CurrentPassword, model.NewPassword, (int)Union.SEIU);

                if (updatePwdResponse.Success && (updatePwdResponse.Data?.Updated ?? false)) {
                    LinkField thankYouField = model.Item.Fields[Templates.ProfileTemplate.Fields.ThankYouPage_Password];
                    var thankYouUrl = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(thankYouField.TargetItem.ID));
                    Response.Cookies.Add(new HttpCookie(FormConstants.NEA_COOKIE_MDSID, string.Empty));
                    _userRepository.LogoutUser();

                    var gtmService = new GTMServiceSeiumb();
                    var dataLayerData = gtmService.GetGtmEvent(new Account
                    {
                        Event = "account",
                        AccountSection = "profile",
                        AccountAction = "update password",
                        CTAText = RenderingContext.Current.Rendering.Item.Fields[Templates.ProfileTemplate.Fields.PasswordSubmitButton]?.Value ?? string.Empty
                    });
                    HttpContext.Session["GTMAction"] = dataLayerData;
                    HttpContext.Session["EventSuccess"] = true;

                    return Redirect(thankYouUrl);
                } else {
                    if (updatePwdResponse.ErrorCodeResponse == UserAccountErrorCodesEnum.UsernamePasswordCombinationNoMatch) {
                        if (model.Errors == null)
                            model.Errors = new List<ProfileErrors> {
                                ProfileErrors.PASSWORD_DONT_MATCH
                            };
                        return View(PASSWORD_FORM_VIEW, model);
                    }
                    if (errorPageItem != null) {
                        var errorPageUrl = LinkManager.GetItemUrl(errorPageItem);
                        return Redirect(errorPageUrl);
                    }
                }
                return View(PASSWORD_FORM_VIEW, model);
            } else {
				var modelStateVal = ViewData.ModelState["CurrentPassword"];

				model.HasErrorCurrentPassword = modelStateVal.Errors.Count > 0;
				if (model.HasErrorCurrentPassword) {
					model.HasErrorCurrentPasswordInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("EH")) != null;
				}

				if (model.Errors == null) {
					model.Errors = new List<ProfileErrors> {
						ProfileErrors.VALUES_REQUIRED
					};
				}
				model.Initialize(RenderingContext.Current.Rendering);
				return View(PASSWORD_FORM_VIEW, model);
			}
        }
	}
}