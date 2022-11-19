using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Seiumb.Feature.Forms.Constants;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Web.Mvc;
using Neambc.Seiumb.Foundation.Sitecore.Utility;
using Sitecore.Diagnostics;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class LoginMobileFormController : BaseController {
		private const string LOGIN_MOBILE_FORM = "/Views/Forms/LoginMobileForm.cshtml";
		private readonly IAuthenticationManager _authenticationManager;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IUserRepository _userRepository;

        public LoginMobileFormController(IAuthenticationManager authenticationManager, ISeiumbProfileManager seiumbProfileManager, IUserRepository userRepository) {
            _authenticationManager = authenticationManager;
            _seiumbProfileManager = seiumbProfileManager;
            _userRepository = userRepository;
        }

		public ActionResult LoginMobileForm(string id, string actionurl, string actionkind, string actiontitle, string actionprocedurepar1, string actionprocedurepar2, string materialid, string actiondatapass,string productname, string actiontype, string contextitemid,string calllinkid,string actiontarget,string postparameterid) {
			var model = new LoginMobileFormModel();
            var seiuProfile = _seiumbProfileManager.GetProfile();
            if (_userRepository.GetUserStatus().Equals(UserStatusCons.WARM_HOT)) model.Username = seiuProfile.Email;
            var alreadyRegistered = HttpContext.Session[SessionConstants.ALREADY_REGISTERED] != null && 
                bool.Parse(HttpContext.Session[SessionConstants.ALREADY_REGISTERED].ToString());
			if (alreadyRegistered) model.Errors = new List<LoginErrors> { LoginErrors.ALREADY_REGISTERED };
            model.IsMobile = true;
			model.RememberMe = true;
			model.Initialize(RenderingContext.Current.Rendering);
            var novendorParameter = Request.QueryString[ConstantsNeamb.NoVendorRakuten];
            var storeUrlParameter = Request.QueryString[ConstantsNeamb.StoreUrlParameter];
            var ctaText = Request.QueryString[ConstantsNeamb.StoreCtaTextParameter];
            var hotdeal = Request.QueryString[ConstantsNeamb.StoreHotDealParameter];
            var redirectPageLogin = Request.QueryString[ConstantsNeamb.RedirectUrlLoginParameter];
            if (!string.IsNullOrEmpty(redirectPageLogin)) model.RedirectUrlId = redirectPageLogin;
            if (!string.IsNullOrEmpty(hotdeal)) model.HotDeal = hotdeal;
            if (!string.IsNullOrEmpty(ctaText)) model.CtaText = ctaText;
            if (!string.IsNullOrEmpty(novendorParameter)) model.LoginAjaxProcess = LoginAjaxEnum.RakutenNoVendor;
            if (!string.IsNullOrEmpty(storeUrlParameter)) {
                model.LoginAjaxProcess = LoginAjaxEnum.RakutenStore;
                model.StoreId = storeUrlParameter;
            }
            var homeItem = Sitecore.Context.Database.GetItem(Templates.Home.ID);
            
            if (Request != null && Request.UrlReferrer != null) {
                var requestPage = Request.UrlReferrer.OriginalString;
                model.PreviousPage = QueryStringHelper.RemoveQueryStringByKey(requestPage, "ref");
            } else {
                var redirectUrlHome = LinkManager.GetItemUrl(homeItem);

                model.PreviousPage = redirectUrlHome;
            }
            if (string.IsNullOrEmpty(id)) return View(LOGIN_MOBILE_FORM, model);
            var currentItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(id));
            if (currentItem.IsDerived(Templates.ProductDetailPageType.ID) && !_userRepository.GetUserStatus().Equals(UserStatusCons.HOT)) {
                model.LoginAjaxProcess = LoginAjaxEnum.Product;
            }
            model.ActionKind = actionkind;
            model.ActionUrl = actionurl;
            model.ActionTitle = actiontitle;
            model.ActionProcedurePar1 = actionprocedurepar1;
            model.ActionProcedurePar2 = actionprocedurepar2;
            model.MaterialId = materialid;
            model.Action = actiondatapass;
            model.ProductName = productname;
            model.ActionType = actiontype;
            model.Contextitemid = contextitemid;
            model.Calllinkid = calllinkid;
            model.Actiontarget = actiontarget;
            model.Postparameterid = postparameterid;
            
            
           
           
            return View(LOGIN_MOBILE_FORM, model);
		}

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult LoginMobileForm(LoginMobileFormModel model) {

            var formModel = new LoginMobileFormModel();
            formModel.Initialize(RenderingContext.Current.Rendering);

            if (!ModelState.IsValid)
            {
                formModel.Errors = new List<LoginErrors>();
                formModel.Errors.Add(LoginErrors.INVALID_USERNAME_PASSWORD);

                return View(LOGIN_MOBILE_FORM, formModel);
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                string userName = model != null ? model.Username : "";
                Log.Error($"method: LoginMobileForm, model password is empty userName {userName}", this);
            }
            
            formModel.PreviousPage = model.PreviousPage;
            var cellCode = HttpContext.Session[ConstantsSeiumb.SourceCode] != null ? HttpContext.Session[ConstantsSeiumb.SourceCode].ToString() : string.Empty;
            var seiuProfile = _seiumbProfileManager.GetProfile();
            Log.Info($"Registration cellCode: {cellCode}", this);

            var response = _authenticationManager.AuthenticateUser(seiuProfile, model.Username, model.Password, cellCode);
            if (response.IsAuthenticated) {
                Sitecore.Diagnostics.Log.Info($"Response mobile 0 (Login method) login{model.Username}", this);
                var duplicateRegistrationPage = Sitecore.Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID);
                var duplicateRegistrationUrl = LinkManager.GetItemUrl(duplicateRegistrationPage);

                if (response.Data.RegistrationCount > 1) {
                    HttpContext.Session[ConstantsSeiumb.DuplicateLogin] = "1";
                    HttpContext.Session[ConstantsSeiumb.ReferredUrl] = Request.UrlReferrer?.OriginalString;
                    var profile = _seiumbProfileManager.GetProfile();
                    profile.DuplicateEmail = model.Username.ToLower();
                    Sitecore.Diagnostics.Log.Info($"RegistrationCount mobile > 1 (Login method) login{model.Username}", this);
                    return Redirect(duplicateRegistrationUrl);
                }
                _authenticationManager.FillUserData(seiuProfile, response.Data.MdsIdAsString, FormConstants.NEA_COOKIE_MDSID, true, response.Data.RegistrationDuplicateOldFormat);
                Sitecore.Diagnostics.Log.Info($"RegistrationCount mobile =0 (Login method) login{model.Username}", this);

                string redirectUrl;
                var registrationPage = Sitecore.Context.Database.GetItem(Templates.RegistrationPage.ID);
                var registrationUrl = LinkManager.GetItemUrl(registrationPage);

                redirectUrl = string.IsNullOrEmpty(model.PreviousPage) || model.PreviousPage.Contains(duplicateRegistrationUrl) || 
                    model.PreviousPage.Contains(registrationUrl) ? LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.Home.ID))
                        : model.PreviousPage;
                return Redirect(redirectUrl);
            }
            formModel.Errors = new List<LoginErrors>();
            switch (response.ErrorCodeResponse) {
                case LoginUserErrorCodeEnum.FailedLogin:
                    formModel.Errors.Add(LoginErrors.INVALID_USERNAME_PASSWORD);
                    break;
                case LoginUserErrorCodeEnum.AccountLocked:
                    formModel.Errors.Add(LoginErrors.ACCOUNT_LOCKED_SENT_MAIL);
                    break;
                case LoginUserErrorCodeEnum.UsernameNotExist:
                    formModel.Errors.Add(LoginErrors.USERNAME_DOES_NOT_EXIST);
                    break;
                case LoginUserErrorCodeEnum.InvalidDataRequiredField:
                case LoginUserErrorCodeEnum.InvalidDataValidationField:
                    formModel.Errors.Add(LoginErrors.INVALID_USERNAME_PASSWORD);
                    break;
                default:
                    var message = $"Response webservice LoginUser email:{model.Username} code:{response.ErrorCodeResponse}";
                    Sitecore.Diagnostics.Log.Error(message, this);
                    formModel.Errors.Add(LoginErrors.TIME_OUT);
                    break;
            }
            return View(LOGIN_MOBILE_FORM, formModel);
        }
	}
}