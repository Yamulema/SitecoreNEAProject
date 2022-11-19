using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Neambc.Neamb.Foundation.Rakuten.Model;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.Sitecore.Utility;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Convert = System.Convert;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class LoginFormController : BaseController
    {
        #region Fields
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILockedAccountService _lockedAccountService;
        private readonly IProductRestManagerSeiumb _productRestManagerSeiumb;
        private readonly IStoreManager _storeManager;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IUserRepository _userRepository;
        #endregion

        #region Constructors
        public LoginFormController(IAuthenticationManager authenticationManager, ILockedAccountService lockedAccountService, 
            IProductRestManagerSeiumb productRestManagerSeiumb, IStoreManager storeManager,ISeiumbProfileManager seiumbProfileManager,
            IUserRepository userRepository)
        {
            _authenticationManager = authenticationManager;
            _lockedAccountService = lockedAccountService;
            _productRestManagerSeiumb = productRestManagerSeiumb;
            _storeManager = storeManager;
            _seiumbProfileManager = seiumbProfileManager;
            _userRepository = userRepository;
        }
        #endregion

        #region Properties
        public object AuthenticationController { get; private set; }
        #endregion

        #region Public Methods
        public ActionResult LoginForm()
        {
            var model = new LoginFormModel { RememberMe = true };
            var profile = _seiumbProfileManager.GetProfile();
            var userStatus = _userRepository.GetUserStatus();
            
            if (userStatus == UserStatusCons.WARM_HOT) model.Username = profile.Email;
            if (userStatus.Equals(UserStatusCons.WARM_COLD) && Request.UrlReferrer != null &&
                RedirectionHelper.CanRedirectPreviousPage(Request.UrlReferrer.AbsolutePath))
                HttpContext.Session[ConstantsSeiumb.ReferredUrl] = Request.UrlReferrer.AbsolutePath;
            model.IsMobile = false;
            model.UserStatus = userStatus;
            model.Initialize(RenderingContext.Current.Rendering);
            return View("/Views/Forms/LoginForm.cshtml", model);
        }

        [HttpPost]

        public ActionResult LoginFormAjax(LoginFormModel model)
        {
            var seiuProfile = _seiumbProfileManager.GetProfile();

            if (ModelState.IsValid)
            {
                var emailWarm = seiuProfile.Email;
                var cellCode = HttpContext.Session[ConstantsSeiumb.SourceCode] != null ? HttpContext.Session[ConstantsSeiumb.SourceCode].ToString() : string.Empty;
                if (string.IsNullOrEmpty(model.Password))
                {
                    string userName = model != null ? model.Username : "";
                    Log.Error($"method: LoginFormAjax, password is empty. Username:{userName}", this);
                }

                var response = _authenticationManager.AuthenticateUser(seiuProfile, model.Username, model.Password, cellCode);
                if (response.IsAuthenticated)
                {
                    var duplicateRegistrationPage = Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID);
                    var duplicateRegistrationUrl = LinkManager.GetItemUrl(duplicateRegistrationPage);
                    Log.Info($"Response 0 (Login method) login{model.Username}", this);
                    if (response.Data.RegistrationCount > 1)
                    {
                        HttpContext.Session[ConstantsSeiumb.DuplicateLogin] = "1";
                        HttpContext.Session[ConstantsSeiumb.ReferredUrl] = Request.UrlReferrer.OriginalString;
                        seiuProfile.DuplicateEmail = model.Username.ToLower();
                        _seiumbProfileManager.SaveProfile(seiuProfile);
                        Log.Info($"RegistrationCount > 1 (Login method) login{model.Username}", this);
                        return Json(new { result = "duplicated", url = duplicateRegistrationUrl }, JsonRequestBehavior.AllowGet);
                    }
                    _authenticationManager.FillUserData(seiuProfile, response.Data.MdsIdAsString, FormConstants.NEA_COOKIE_MDSID, true, response.Data.RegistrationDuplicateOldFormat);
                    if (model.LoginAjaxProcess == LoginAjaxEnum.RakutenStore || model.LoginAjaxProcess == LoginAjaxEnum.RakutenNoVendor)
                    {
                        var loginResultStores = new List<LoginResultEnum>();
                        VerifyUserLoggedIn(model, emailWarm, loginResultStores);
                        VerifyRakutenMember(model, loginResultStores);
                        VerifyEligibility(model, response.Data.MdsIdAsString, loginResultStores);
                        var resultredirection = GetResultRedirection(loginResultStores);
                        //get the redirect url
                        string redirectUrl = "";
                        if (!string.IsNullOrEmpty(model.RedirectUrlId))
                        {
                            var redirectItemUrl = Context.Database.GetItem(new ID(model.RedirectUrlId));
                            redirectUrl = LinkManager.GetItemUrl(redirectItemUrl);
                        }
                        var gtmRakuten = "";

                        if (!string.IsNullOrEmpty(model.StoreId)) {
                            gtmRakuten = _storeManager.GetGtmFunction(string.IsNullOrEmpty(model.HotDeal) ? GtmRakutenType.Action : GtmRakutenType.HotDeal, model.StoreId, model.CtaText);
                        }

                        return Json(new { url = redirectUrl, result = resultredirection, type = model.LoginAjaxProcess.ToString(), store = model.StoreId, gtm = gtmRakuten }, JsonRequestBehavior.AllowGet);
                    }

                    //Verify if user logged is different than the warm user currently logged
                    if (string.IsNullOrEmpty(emailWarm) || model.Username.Equals(emailWarm)) return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                    var homeItem = Context.Database.GetItem(Templates.Home.ID);
                    var redirectUrlHome = LinkManager.GetItemUrl(homeItem);
                    return Json(new { result = "duplicated", url = redirectUrlHome }, JsonRequestBehavior.AllowGet);
                }
                var errorType = string.Empty;
                var errorMessage = string.Empty;
                Item datasourceItem = null;
                if (!string.IsNullOrEmpty(model.ContextItemId))
                {
                    var contextItem = Context.Database.GetItem(new ID(model.ContextItemId));
                    var renderingLogin = RenderingHelper.GetRenderingsByPlaceholder("login", contextItem);
                    datasourceItem = renderingLogin != null ? Context.Database.GetItem(renderingLogin.DataSource) : contextItem;
                }

                if (datasourceItem == null) datasourceItem = Context.Database.GetItem(Templates.LoginGlobal.ID);

                //Get messages
                var invalidCredentials = datasourceItem[Foundation.Authentication.Templates.LoginForm.Fields.InvalidCredentials];
                var recentAccountLocked = datasourceItem[Foundation.Authentication.Templates.LoginForm.Fields.AccountLocked];
                var accountLocked = datasourceItem[Foundation.Authentication.Templates.LoginForm.Fields.AccountAlreadyLocked];
                var timeout = datasourceItem[Foundation.Authentication.Templates.LoginForm.Fields.TimeOut];

                var resultcode = response.ErrorCodeResponse;

                switch (resultcode)
                {
                    case LoginUserErrorCodeEnum.FailedLogin:
                        errorType = LoginErrors.INVALID_USERNAME_PASSWORD.ToString();
                        errorMessage = invalidCredentials;
                        break;
                    case LoginUserErrorCodeEnum.UsernameNotExist:
                        errorType = LoginErrors.USERNAME_DOES_NOT_EXIST.ToString();
                        errorMessage = invalidCredentials;
                        break;
                    case LoginUserErrorCodeEnum.InvalidDataRequiredField:
                    case LoginUserErrorCodeEnum.InvalidDataValidationField:
                        errorType = LoginErrors.INVALID_DATA.ToString();
                        errorMessage = invalidCredentials;
                        break;
                    case LoginUserErrorCodeEnum.AccountLocked:
                        var lockedAccountError = _lockedAccountService.HandleLockedAccount(model.Username,
                                datasourceItem.Fields[Foundation.Authentication.Templates.LoginForm.Fields.CancelLink],
                                datasourceItem.Fields[Foundation.Authentication.Templates.LoginForm.Fields.ResetLink],
                                out var isUsernameValid, true);
                        if (lockedAccountError == LoginErrors.ACCOUNT_LOCKED_SENT_MAIL)
                        {
                            var retrievePasswordModel = new RetrievePasswordModel
                            {
                                Submitted = false,
                                Item = datasourceItem,
                                UserName = model.Username
                            };

                            retrievePasswordModel.IsUsernameValid = isUsernameValid;
                            errorType = LoginErrors.ACCOUNT_LOCKED_SENT_MAIL.ToString();
                            errorMessage = recentAccountLocked;
                        }
                        else
                        {
                            errorType =
                                LoginErrors.ACCOUNT_LOCKED_NOT_SENT_MAIL.ToString();
                            errorMessage = accountLocked;
                        }
                        break;
                    default:
                        var message = $"Response webservice LoginUser email:{model.Username} code:{response.ErrorCodeResponse}";
                        Log.Error(message, this);
                        errorType = LoginErrors.TIME_OUT.ToString();
                        errorMessage = timeout;
                        break;
                }
                return Json(new { result = "error", errorType, errorMessage },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                var datasourceItem = Context.Database.GetItem(Templates.LoginGlobal.ID);
                var errorType = LoginErrors.EMPTY_FIELDS.ToString();
                var errorMessage = datasourceItem[Foundation.Authentication.Templates.LoginForm.Fields.EmptyField];

                return Json(new { result = "error", errorType, errorMessage },
                    JsonRequestBehavior.AllowGet);
            }
        }

        private string GetResultRedirection(List<LoginResultEnum> loginResultStores)
        {
            var resultredirection = "ok";
            if (loginResultStores.Contains(LoginResultEnum.ErrorEligibilityRakuten))
                resultredirection = "errorRakutenEligible";
            else if (loginResultStores.Contains(LoginResultEnum.ErrorRakutenMember))
                resultredirection = "errorRakutenMember";
            else if (loginResultStores.Contains(LoginResultEnum.ErrorLogin)) resultredirection = "error";
            return resultredirection;
        }

        private void VerifyEligibility(LoginFormModel model, string mdsid, List<LoginResultEnum> loginResultStores)
        {
            int.TryParse(mdsid, out var mdsidInt);
            var resultEligibility = _productRestManagerSeiumb.GetEligibility(mdsidInt);

            //Check the result of the webservice call
            if (!resultEligibility && (model.LoginAjaxProcess == LoginAjaxEnum.RakutenStore || model.LoginAjaxProcess == LoginAjaxEnum.RakutenNoVendor))
                loginResultStores.Add(LoginResultEnum.ErrorEligibilityRakuten);
        }

        private void VerifyRakutenMember(LoginFormModel model, List<LoginResultEnum> loginResultStores) {
            if ((model.LoginAjaxProcess == LoginAjaxEnum.RakutenStore || model.LoginAjaxProcess == LoginAjaxEnum.RakutenNoVendor) &&
                !_seiumbProfileManager.IsRakutenMember())
                loginResultStores.Add(LoginResultEnum.ErrorRakutenMember);
        }

        private void VerifyUserLoggedIn(LoginFormModel model, string emailWarm, List<LoginResultEnum> loginResultStores)
        {
            if (!string.IsNullOrEmpty(emailWarm) && !model.Username.Equals(emailWarm)) loginResultStores.Add(LoginResultEnum.ErrorLogin);
        }

        [HttpPost]
        [ValidateFormHandler]
        public ActionResult LoginForm(LoginFormModel model) 
        {
            var seiuProfile = _seiumbProfileManager.GetProfile();
            var formModel = new LoginFormModel();
            formModel.Initialize(RenderingContext.Current.Rendering);

            if (!ModelState.IsValid)
            {
                formModel.Errors = new List<LoginErrors>();
                formModel.Errors.Add(LoginErrors.EMPTY_FIELDS);

                return View("/Views/Forms/LoginForm.cshtml", formModel);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                string userName = model != null ? model.Username : "";
                Log.Error($"method: LoginForm, model password is empty userName {userName}", this);
            }            
            
            var cellCode = HttpContext.Session[ConstantsSeiumb.SourceCode] != null ? HttpContext.Session[ConstantsSeiumb.SourceCode].ToString() : string.Empty;
            Log.Info($"Registration cellCode: {cellCode}", this);
            var response = _authenticationManager.AuthenticateUser(seiuProfile, model.Username, model.Password, cellCode);
            if (response.IsAuthenticated)
            {
                var duplicateRegistrationPage = Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID);
                var duplicateRegistrationUrl = LinkManager.GetItemUrl(duplicateRegistrationPage);

                Log.Info($"Response 0 (Login method) login{model.Username}", this);
                if (response.Data.RegistrationCount > 1)
                {
                    HttpContext.Session[ConstantsSeiumb.DuplicateLogin] = "1";
                    HttpContext.Session[ConstantsSeiumb.ReferredUrl] = Request.UrlReferrer?.OriginalString;
                    seiuProfile.DuplicateEmail = model.Username.ToLower();
                    _seiumbProfileManager.SaveProfile(seiuProfile);
                    Log.Info($"RegistrationCount > 1 (Login method) login{model.Username}", this);
                    return Redirect(duplicateRegistrationUrl);
                }
                _authenticationManager.FillUserData(seiuProfile, response.Data.MdsIdAsString, FormConstants.NEA_COOKIE_MDSID, true,
                    response.Data.RegistrationDuplicateOldFormat);
                Log.Info(
                    $"RegistrationCount =0 (Login method) login{model.Username}", this);
                var registrationPage = Context.Database.GetItem(Templates.RegistrationPage.ID);
                var registrationUrl = LinkManager.GetItemUrl(registrationPage);

                if (!Request.UrlReferrer.OriginalString.Contains(duplicateRegistrationUrl) &&
                    !Request.UrlReferrer.OriginalString.Contains(registrationUrl))
                {
                    if (Request.UrlReferrer.OriginalString.Contains("?notyou=true"))
                    {
                        var site = Request.UrlReferrer.OriginalString.Replace("?notyou=true", string.Empty);
                        return Redirect(site);
                    }
                    var requestPage = Request.UrlReferrer.OriginalString;
                    var depuredUrl = QueryStringHelper.RemoveQueryStringByKey(requestPage, "ref");

                    return Redirect(depuredUrl);
                }
                var homeItem = Context.Database.GetItem(Templates.Home.ID);
                var redirectUrl = LinkManager.GetItemUrl(homeItem);
                return Redirect(redirectUrl);
            }
            formModel.Errors = new List<LoginErrors>();
            switch (response.ErrorCodeResponse)
            {
                case LoginUserErrorCodeEnum.FailedLogin:
                    formModel.Errors.Add(LoginErrors.INVALID_USERNAME_PASSWORD);
                    break;
                case LoginUserErrorCodeEnum.UsernameNotExist:
                    formModel.Errors.Add(LoginErrors.USERNAME_DOES_NOT_EXIST);
                    break;
                case LoginUserErrorCodeEnum.InvalidDataValidationField:
                case LoginUserErrorCodeEnum.InvalidDataRequiredField:
                    formModel.Errors.Add(LoginErrors.INVALID_DATA);
                    break;
                case LoginUserErrorCodeEnum.AccountLocked:
                    var retrievePasswordModel = new RetrievePasswordModel();
                    retrievePasswordModel.Initialize(RenderingContext.Current.Rendering);
                    retrievePasswordModel.UserName = model.Username;
                    var lockedAccountError = _lockedAccountService.HandleLockedAccount(model.Username,
                        RenderingContext.Current.Rendering.Item.Fields[Foundation.Authentication.Templates.LoginForm.Fields.CancelLink],
                        RenderingContext.Current.Rendering.Item.Fields[Foundation.Authentication.Templates.LoginForm.Fields.ResetLink],
                        out var isUsernameValid, true);
                    retrievePasswordModel.IsUsernameValid = isUsernameValid;
                    formModel.Errors.Add(lockedAccountError);
                    break;

                default:
                    var message = $"Response webservice LoginUser email:{model.Username} code:{response.ErrorCodeResponse}";
                    Log.Error(message, this);
                    formModel.Errors.Add(LoginErrors.TIME_OUT);
                    break;
            }
            return View("/Views/Forms/LoginForm.cshtml", formModel);
        }
        
        [HttpPost]
        [ValidateFormHandler]

        public ActionResult LogoutForm()
        {
            LogoutUser();
            var duplicateRegistrationPage = Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID);
            var duplicateRegistrationUrl = LinkManager.GetItemUrl(duplicateRegistrationPage);
            if (!Request.UrlReferrer.AbsolutePath.Equals(duplicateRegistrationUrl)) return Redirect(Request.UrlReferrer.AbsolutePath);
            var homeItem = Context.Database.GetItem(Templates.Home.ID);
            var redirectUrl = LinkManager.GetItemUrl(homeItem);
            return Redirect(redirectUrl);
        }

        [HttpPost]
        [ValidateFormHandler]

        public ActionResult NotYouForm()
        {
            LogoutUser();
            CookieStore.SetCookie(FormConstants.SEIUMBUsername, string.Empty, TimeSpan.FromDays(Convert.ToInt32(-7)));
            return Redirect(Request.UrlReferrer.AbsolutePath + "?notyou=true");
        }

        [HttpPost]
        [ValidateFormHandler]

        public ActionResult NotYouFormMobile()
        {
            LogoutUser();
            CookieStore.SetCookie(FormConstants.SEIUMBUsername, string.Empty, TimeSpan.FromDays(Convert.ToInt32(-7)));
            var loginItem = Context.Database.GetItem(Templates.LoginMobilePage.ID);
            var loginurl = LinkManager.GetItemUrl(loginItem);
            return Redirect(loginurl);
        }
        #endregion

        #region Private Methods
        private void LogoutUser()
        {
            CookieStore.SetCookie(FormConstants.NEA_COOKIE_MDSID, string.Empty, TimeSpan.FromDays(Convert.ToInt32(-7)));
            _userRepository.LogoutUser();
        }
        #endregion
    }
} 