using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Neambc.Neamb.Foundation.Rakuten.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.Sitecore.Utility;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class AuthenticationAccountController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly ICookieManager _cookieManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;
        private readonly ISessionManager _sessionManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IStoreManager _storeManager;
        private readonly IBase64Service _base64Service;
        private readonly ILoginGtmManager _loginGtmManager;

        protected IEligibilityManager _eligibilityManager;
        protected ILoginHandlerPostAction _loginHandlerPostAction;

        public AuthenticationAccountController(IAccountRepository accountRepository, ISessionAuthenticationManager _sessionAuthenticationManager,
            ICookieManager cookieManager, IAuthenticationAccountManager authenticationAccountManager, ISessionManager sessionManager, IAuthenticationManager authenticationManager, IEligibilityManager eligibilityManager, ILoginHandlerPostAction loginHandlerPostAction, IGlobalConfigurationManager globalConfigurationManager, IStoreManager storeManager, IBase64Service base64Service, ILoginGtmManager loginGtmManager)
        {
            _accountRepository = accountRepository;
            this._sessionAuthenticationManager = _sessionAuthenticationManager;
            _cookieManager = cookieManager;
            _authenticationAccountManager = authenticationAccountManager;
            _sessionManager = sessionManager;
            _authenticationManager = authenticationManager;
            _eligibilityManager = eligibilityManager;
            _loginHandlerPostAction = loginHandlerPostAction;
            _globalConfigurationManager = globalConfigurationManager;
            _storeManager = storeManager;
            _base64Service = base64Service;
            _loginGtmManager = loginGtmManager;
        }

        /// <summary>
        /// Controller rendering Sign in page
        /// </summary>
        /// <returns></returns>
        public ActionResult SignIn()
        {
            RedirectPage();

            var model = new AccountDTO();
            model.Initialize(RenderingContext.Current.Rendering);

            SetReturnPageAfterRegistration(ref model);

            AccountMembership account = SetValuesFromQueryString(ref model);
            //Case the user is Warm-hot present with the username populated
            if (account.Status == StatusEnum.WarmHot || account.Status == StatusEnum.WarmCold)
            {
                model.Email = account.Username;
            }
            else
            {
                //Get the remember me cookie
                var rememberMe = _cookieManager.GetRememberMe();
                if (!string.IsNullOrEmpty(rememberMe))
                {
                    model.Email = _base64Service.Decode(rememberMe);
                }
            }

            var fromRegistration = RetreiveStringValueFromSession(ConstantsNeamb.FromRegistrationPage);

            if (!string.IsNullOrEmpty(fromRegistration))
            {
                model.IsAlreadyRegistered = account.Status == StatusEnum.WarmHot ||
                                            account.Status == StatusEnum.Hot;
                _sessionManager.Remove(ConstantsNeamb.FromRegistrationPage);
            }

            SetValuesFromSessionInModel(ref model);

            return View("/Views/Neamb.Account/SignIn.cshtml", model);
        }

        private AccountMembership SetValuesFromQueryString(ref AccountDTO model)
        {
            var storeUrlParameter = Request.QueryString[ConstantsNeamb.StoreUrlParameter];
            var ctaText = Request.QueryString[ConstantsNeamb.StoreCtaTextParameter];
            var hotdeal = Request.QueryString[ConstantsNeamb.StoreHotDealParameter];
            var account = _sessionAuthenticationManager.GetAccountMembership();
            var novendorParameter = Request.QueryString[ConstantsNeamb.NoVendorRakuten];

            model.Status = account.Status;

            if (!string.IsNullOrEmpty(novendorParameter))
            {
                model.LoginAjaxProcess = LoginAjaxEnum.RakutenNoVendor;
                if (account.Status == StatusEnum.WarmHot || account.Status == StatusEnum.WarmCold)
                {
                    if (!string.IsNullOrEmpty(account.Username))
                    {
                        _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, account.Username);
                    }
                }
                else
                {
                    _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, ConstantsNeamb.UserCold);
                }
            }
            if (!string.IsNullOrEmpty(storeUrlParameter) && !string.IsNullOrEmpty(ctaText))
            {
                model.LoginAjaxProcess = LoginAjaxEnum.RakutenStore;
                model.StoreId = storeUrlParameter;
                var productCode = _globalConfigurationManager.ProductCodeStores;
                model.ProductCode = productCode;
                model.CtaActionClick = $"operationprocedureactioncta('{productCode}');executestoreredirection('{model.StoreId}')";
                if (account.Status == StatusEnum.WarmHot || account.Status == StatusEnum.WarmCold)
                {
                    if (!string.IsNullOrEmpty(account.Username))
                    {
                        _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, account.Username);
                    }
                }
                else
                {
                    _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, ConstantsNeamb.UserCold);
                }
                if (string.IsNullOrEmpty(hotdeal))
                {
                    model.GtmAction = _storeManager.GetGtmFunction(GtmRakutenType.Action, storeUrlParameter, ctaText);
                }
                else
                {
                    model.GtmAction = _storeManager.GetGtmFunction(GtmRakutenType.HotDeal, storeUrlParameter, ctaText);
                }
            }

            return account;
        }

        private void SetValuesFromSessionInModel(ref AccountDTO model)
        {
            var idComponent = string.Empty;
            model.LoginText = RenderingContext.Current.Rendering.Item[Templates.Login.Fields.LoginButton];
            var actiontype = RetreiveStringValueFromSession(ConstantsNeamb.CtaActionType);

            if (!string.IsNullOrEmpty(actiontype))
            {
                model.LoginAjaxProcess = LoginAjaxEnum.Product;
                if (actiontype.Contains(ConstantsNeamb.Primary))
                {
                    idComponent = RetreiveStringValueFromSession(ConstantsNeamb.CtaActionType).Replace(ConstantsNeamb.Primary, string.Empty);
                    model.CtaAction = RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.CtaActionPrimary, idComponent)).Replace(idComponent, string.Empty);

                    model.CtaActionClick = RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.CtaActionPrimaryOnclick, idComponent)).Replace(idComponent, string.Empty).Replace("_sec", string.Empty);
                    model.CtaActionTargetBlank =
                        RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.CtaActionPrimaryTargetBlank, idComponent)).Replace(idComponent, string.Empty);
                    model.ProductCode =
                        RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.ProductComponent, idComponent));
                    model.GtmAction =
                        RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.ProductGtmActionPrimary, idComponent));
                    model.HasCheckEligibility =
                        RetreiveValueFromSession<bool>(string.Format("{0}{1}", ConstantsNeamb.ProductHasCheckEligibility, idComponent));
                    model.FromProduct = !string.IsNullOrEmpty(model.CtaAction) || !string.IsNullOrEmpty(model.CtaActionClick);
                    model.CtaFirstSecondAction = ConstantsNeamb.DataFirst;
                }
                else
                {
                    idComponent = RetreiveStringValueFromSession(ConstantsNeamb.CtaActionType).Replace(ConstantsNeamb.Secondary, string.Empty);
                    model.CtaAction = RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.CtaActionSecondary, idComponent)).Replace(idComponent, string.Empty);

                    model.CtaActionClick = RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.CtaActionSecondaryOnclick, idComponent)).Replace(idComponent, string.Empty).Replace("_sec", string.Empty);
                    model.CtaActionTargetBlank =
                        RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.CtaActionSecondaryTargetBlank, idComponent)).Replace(idComponent, string.Empty);
                    model.ProductCode =
                        RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.ProductComponent, idComponent));
                    model.GtmAction =
                        RetreiveStringValueFromSession(string.Format("{0}{1}", ConstantsNeamb.ProductGtmActionSecondary, idComponent));
                    model.FromProduct = !string.IsNullOrEmpty(model.CtaAction) || !string.IsNullOrEmpty(model.CtaActionClick);
                    model.CtaFirstSecondAction = ConstantsNeamb.DataSecond;
                }
            }


        }

        private string RetreiveStringValueFromSession(string sessionName)
        {
            string value = RetreiveValueFromSession<string>(sessionName);

            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value;
        }

        private T RetreiveValueFromSession<T>(string sessionName)
        {
            T value = default(T);

            value = _sessionManager.RetrieveFromSession<T>(sessionName);

            return value;
        }

        private void SetReturnPageAfterRegistration(ref AccountDTO model)
        {
            
            model.GtmLoadAction = _loginGtmManager.GetGtmFunction(LoginGtmStatus.Attempt);
            model.GtmLoginFailed = _loginGtmManager.GetGtmFunction(LoginGtmStatus.Failed);

            string[] hostNames = GetWebsiteHostNames();

            //Change for returning to the previous page after registration
            //----------------------------------------------------------
            Sitecore.Data.Fields.LinkField registrationLink = RenderingContext.Current.Rendering.Item.Fields[Templates.Login.Fields.CreateAccount];
            var pathRegistration = LinkManager.GetItemUrl(registrationLink.TargetItem);
            //Get item id of the previous page
            if (Request != null && Request.UrlReferrer != null && hostNames.Contains(Request.UrlReferrer.Host))
            {
                var fullPathDepuredUrl = RedirectionHelper.GetFullPath(Request.UrlReferrer);
                var itemToRedirect = Sitecore.Context.Database.GetItem(fullPathDepuredUrl);
                if (itemToRedirect != null)
                {
                    var pageToRedirect = itemToRedirect.ID.ToString();
                    _sessionManager.StoreInSession("PageToRedirect", pageToRedirect);
                }
            }
            model.RegistrationUrl = pathRegistration;
            model.RegistrationText = registrationLink.Text;
            //-----------------------------------------------------------------------
        }

        private void RedirectPage()
        {
            var pathResetPassword = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ResetPage.ID));

            var redirectPage = Request.QueryString[ConstantsNeamb.RedirectUrlLoginParameter];
            var multioffer = Request.QueryString[ConstantsNeamb.MultiofferLoginParameter];
            string[] hostNames = GetWebsiteHostNames();

            if(!string.IsNullOrEmpty(multioffer))
            {
                var requestedLogin=_sessionAuthenticationManager.GetRequestedPageLogin();
                var requestedAbsolutePathLogin=_sessionAuthenticationManager.GetRequestedPageLoginAbsolutePath();
                if (!string.IsNullOrEmpty(requestedLogin) && !string.IsNullOrEmpty(requestedAbsolutePathLogin))
                    return;
            }

            if (!string.IsNullOrEmpty(redirectPage))
            {
                Guid itemId = Guid.Empty;

                if (!Guid.TryParse(redirectPage, out itemId))
                {
                    SaveRedirectLogin();
                    return;
                }

                var redirectedPageItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(itemId));

                if (redirectedPageItem == null)
                {
                    SaveRedirectLogin();
                    return;
                }

                var redirectedUrl = LinkManager.GetItemUrl(redirectedPageItem);

                SaveRequestedPage(redirectedUrl, redirectedUrl);
            }
            else if (Request != null && Request.UrlReferrer != null && hostNames.Contains(Request.UrlReferrer.Host) && !Request.UrlReferrer.AbsolutePath.Equals(pathResetPassword, StringComparison.InvariantCultureIgnoreCase))
            {
                var requestPage = Request.UrlReferrer.OriginalString;
                var depuredUrl = QueryStringHelper.RemoveQueryStringByKey(requestPage, "ref");
                var requestPageAbsolutePath = Request.UrlReferrer.AbsolutePath;

                SaveRequestedPage(depuredUrl, requestPageAbsolutePath);
            }
            else
            {
                SaveRedirectLogin();
            }
        }

        private string[] GetWebsiteHostNames()
        {
            var urlHostName = Sitecore.Context.Site.HostName;

            var hostNames = urlHostName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return hostNames;
        }

        private void SaveRedirectLogin()
        {
            var startItemPath = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID));
            SaveRequestedPage(startItemPath, startItemPath);
        }

        private void SaveRequestedPage(string requestedPage, string pageWithAbsolutePath)
        {
            _sessionAuthenticationManager.SaveRequestedPageLogin(requestedPage);
            _sessionAuthenticationManager.SaveRequestedPageLoginAbsolutePath(pageWithAbsolutePath);
        }

        /// <summary>
        /// Login in product page, warm hot user and click on CTA product action
        /// </summary>
        /// <param name="model">Login and password data</param>
        /// <param name="ckbrememberme">Remember me option</param>
        /// <param name="hasCheckEligibility">Check Eligibility for user</param>
        /// <returns>Json with the authentication result</returns>
        [HttpPost]

        public ActionResult SignInAjax(
            AccountDTO model,
            string ckbrememberme,
            string productCode,
            bool hasCheckEligibility
        )
        {
            //Email that is currently as warm hot
            var emailWarm = RetreiveStringValueFromSession(ConstantsNeamb.CtaActionWarmUser);

            var contextItem = Sitecore.Context.Database.GetItem(Templates.LoginPage.ID);
            model.LoginText = contextItem[Templates.Login.Fields.LoginButton];
            model.GtmLoadAction = _loginGtmManager.GetGtmFunction(LoginGtmStatus.Attempt);
            var options = new ItemUrlBuilderOptions
            {
                AlwaysIncludeServerUrl = true,
            };

            Sitecore.Data.Fields.LinkField resetLink =
                contextItem.Fields[Templates.Login.Fields.ResetLink];
            var pathReset = resetLink != null && resetLink.TargetItem != null
                ? LinkManager.GetItemUrl(resetLink.TargetItem, options)
                : string.Empty;
            var loginPage = _sessionAuthenticationManager.GetRequestedPageLogin();

            var resultAuthentication = _authenticationManager.ExecuteAuthentication(ModelState, ckbrememberme, pathReset, model, ViewData);
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();

            if (!string.IsNullOrEmpty(accountMembership.Mdsid))
            {
                _authenticationAccountManager.RemoveSessionOffersMenu(accountMembership.Mdsid);
            }

            switch (resultAuthentication)
            {
                case AuthenticationResultEnum.Valid:
                    {
                        string resultredirection = "";
                        if (model.LoginAjaxProcess == LoginAjaxEnum.RakutenStore || model.LoginAjaxProcess == LoginAjaxEnum.RakutenNoVendor)
                        {
                            List<LoginResultEnum> loginResultStores = new List<LoginResultEnum>();
                            VerifyUserLoggedIn(model, emailWarm, loginResultStores);
                            VerifyRakutenMember(model, accountMembership, loginResultStores);
                            var productCodeStore = _globalConfigurationManager.ProductCodeStores;
                            VerifyEligibility(model, accountMembership.Mdsid, productCodeStore, emailWarm, loginResultStores);
                            resultredirection = GetResultRedirection(loginResultStores);
                        }
                        else if (model.LoginAjaxProcess == LoginAjaxEnum.Product)
                        {
                            List<LoginResultEnum> loginResultProducts = new List<LoginResultEnum>();
                            VerifyUserLoggedIn(model, emailWarm, loginResultProducts);
                            if (hasCheckEligibility &&
                                emailWarm != null &&
                                emailWarm.Equals(ConstantsNeamb.UserCold))
                            {
                                VerifyEligibility(model, accountMembership.Mdsid, productCode, emailWarm, loginResultProducts);
                            }
                            resultredirection = GetResultRedirection(loginResultProducts);
                        }

                        //Redirect to the page that calls the login
                        if (!string.IsNullOrEmpty(loginPage) && _authenticationAccountManager.IsValidRedirection(loginPage))
                        {
                            return Json(new
                            {
                                result = resultredirection,
                                url = loginPage
                            },
                                JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(
                                new
                                {
                                    result = resultredirection,
                                    url = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID))
                                },
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                case AuthenticationResultEnum.Duplicated:
                    {
                        _sessionAuthenticationManager.SaveRequestedPageLogin(loginPage);
                        return Json(new
                        {
                            result = "duplicated",
                            url = _accountRepository.GetDuplicateRegistrationPageUrl()
                        },
                            JsonRequestBehavior.AllowGet);
                    }
                case AuthenticationResultEnum.ErrorFromService:
                    {
                        var errorsFromService = new List<string>();
                        if (model.HasErrorInvalidCredentials)
                        {
                            errorsFromService.Add(contextItem[Templates.Login.Fields.InvalidCredentials]);
                        }
                        if (model.HasAlreadyLockedErrorTokenValid)
                        {
                            errorsFromService.Add(contextItem[Templates.Login.Fields.AccountAlreadyLockedTokenValid]);
                        }
                        //if (model.HasAlreadyLockedErrorTokenInvalid) { errorsFromService.Add(contextItem[Templates.Login.Fields.AccountAlreadyLockedTokenExpired]); }
                        if (model.HasErrorTimeout)
                        {
                            errorsFromService.Add(contextItem[Templates.Login.Fields.TimeOut]);
                        }
                        if (model.HasLockedError)
                        {
                            errorsFromService.Add(contextItem[Templates.Login.Fields.AccountLocked]);
                        }
                        if (model.IsAlreadyRegistered)
                        {
                            errorsFromService.Add(contextItem[Templates.Login.Fields.AlreadyRegistered]);
                        }
                        _sessionAuthenticationManager.SaveRequestedPageLogin(loginPage);

                        //Return the errors of the authentication
                        return Json(new
                        {
                            result = "error",
                            errorsfromservice = errorsFromService
                        },
                            JsonRequestBehavior.AllowGet);
                    }
                case AuthenticationResultEnum.ErrorNotValid:
                    {
                        _sessionAuthenticationManager.SaveRequestedPageLogin(loginPage);

                        return Json(new
                        {
                            result = "error",
                            errorsusername = contextItem[Templates.Login.Fields.EmptyLogin],
                            errorpassword = contextItem[Templates.Login.Fields.EmptyPassword]
                        },
                            JsonRequestBehavior.AllowGet);
                    }
                default:
                    {
                        _sessionAuthenticationManager.SaveRequestedPageLogin(loginPage);
                        return Json(new
                        {
                            result = "error"
                        },
                            JsonRequestBehavior.AllowGet);
                    }
            }
        }

        private string GetResultRedirection(List<LoginResultEnum> loginResultStores)
        {
            string resultredirection = "ok";
            if (loginResultStores.Contains(LoginResultEnum.ErrorEligibilityRakuten))
            {
                resultredirection = "errorRakutenEligible";
            }
            else if (loginResultStores.Contains(LoginResultEnum.ErrorRakutenMember))
            {
                resultredirection = "errorRakutenMember";
            }
            else if (loginResultStores.Contains(LoginResultEnum.ErrorLogin))
            {
                resultredirection = "errorlogin";
            }
            return resultredirection;
        }

        private void VerifyRakutenMember(AccountDTO model, AccountMembership accountMembership, List<LoginResultEnum> loginResultStores)
        {
            if ((model.LoginAjaxProcess == LoginAjaxEnum.RakutenStore || model.LoginAjaxProcess == LoginAjaxEnum.RakutenNoVendor) &&
                !accountMembership.Profile.IsRakutenMember)
            {
                loginResultStores.Add(LoginResultEnum.ErrorRakutenMember);
            }
        }
        private void VerifyUserLoggedIn(AccountDTO model, string emailWarm, List<LoginResultEnum> loginResultStores)
        {
            if (string.IsNullOrEmpty(emailWarm) || (!emailWarm.Equals(ConstantsNeamb.UserCold) && !emailWarm.Equals(model.Email)))
            {
                loginResultStores.Add(LoginResultEnum.ErrorLogin);
            }
        }

        private void VerifyEligibility(AccountDTO model, string mdsid, string productCode, string emailWarm, List<LoginResultEnum> loginResultStores)
        {
            var resultEligibility = _eligibilityManager.IsMemberEligible(mdsid, productCode);

            //Check the result of the webservice call
            if (resultEligibility != EligibilityResultEnum.Eligible && (model.LoginAjaxProcess == LoginAjaxEnum.RakutenStore || model.LoginAjaxProcess == LoginAjaxEnum.RakutenNoVendor))
            {
                loginResultStores.Add(LoginResultEnum.ErrorEligibilityRakuten);
            }
            if (resultEligibility != EligibilityResultEnum.Eligible && model.LoginAjaxProcess == LoginAjaxEnum.Product)
            {
                loginResultStores.Add(LoginResultEnum.ErrorLogin);
            }

        }

        /// <summary>
		/// Sign in post method to done the authentication
		/// </summary>
		/// <param name="model"></param>
		/// <param name="ckbrememberme"></param>
		/// <returns></returns>
		[HttpPost]

        [ValidateFormHandler]
        public ActionResult SignIn(AccountDTO model, string ckbrememberme)
        {
            model.Initialize(RenderingContext.Current.Rendering);
            model.LoginText = RenderingContext.Current.Rendering.Item[Templates.Login.Fields.LoginButton];
            Sitecore.Data.Fields.LinkField resetLink =
                RenderingContext.Current.Rendering.Item.Fields[Templates.Login.Fields.ResetLink];
            model.IsRememberMe = (ckbrememberme == "on");
            var options = LinkManager.GetDefaultUrlBuilderOptions();
            options.AlwaysIncludeServerUrl = true;

            var pathReset = (resetLink?.TargetItem != null)
                ? LinkManager.GetItemUrl(resetLink.TargetItem, options)
                : string.Empty;

            var loginPage = _sessionAuthenticationManager.GetRequestedPageLogin();
            var loginPageAbsolutePath = _sessionAuthenticationManager.GetRequestedPageLoginAbsolutePath();
            var resultAuthentication = _authenticationManager.ExecuteAuthentication(ModelState, ckbrememberme, pathReset, model, ViewData);
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            if (!string.IsNullOrEmpty(accountMembership.Mdsid))
            {
                _authenticationAccountManager.RemoveSessionOffersMenu(accountMembership.Mdsid);
            }

            switch (resultAuthentication)
            {
                case AuthenticationResultEnum.Valid:
                    {
                        //Redirect to the page that calls the login
                        if (!string.IsNullOrEmpty(loginPage) && !string.IsNullOrEmpty(loginPageAbsolutePath) && _authenticationAccountManager.IsValidRedirection(loginPageAbsolutePath))
                        {
                            //Get the component id from session that indicates that in the origin page that called the login it is required to execute some logic
                            var redirectionPage = _loginHandlerPostAction.GetPageToRedirection(loginPage);
                            return Redirect(redirectionPage);
                        }
                        _sessionManager.Remove(ConstantsNeamb.PostLoginAction);
                        return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID)));
                    }
                case AuthenticationResultEnum.Duplicated:
                    {
                        _sessionAuthenticationManager.SaveRequestedPageLogin(loginPage);
                        _sessionAuthenticationManager.SaveRequestedPageLoginAbsolutePath(loginPageAbsolutePath);
                        _sessionManager.Remove(ConstantsNeamb.PostLoginAction);
                        return Redirect(_accountRepository.GetDuplicateRegistrationPageUrl());
                    }
                default:
                    {
                        _sessionAuthenticationManager.SaveRequestedPageLogin(loginPage);
                        _sessionAuthenticationManager.SaveRequestedPageLoginAbsolutePath(loginPageAbsolutePath);
                        var postLoginAction = _sessionManager.RetrieveFromSession<bool>(ConstantsNeamb.PostLoginAction);
                        if (postLoginAction)
                        {
                            model.GtmLoginFailed = _loginGtmManager.GetGtmFunction(LoginGtmStatus.Failed);
                            model.IsPost = true;
                            _sessionManager.Remove(ConstantsNeamb.PostLoginAction);
                        }
                        return View("/Views/Neamb.Account/SignIn.cshtml", model);
                    }
            }

        }

        [HttpPost]

        public ActionResult LogoutForm(string linkpage)
        {
            _authenticationAccountManager.LogoutUser();

            _cookieManager.RemoveWarmUser();

            if (!string.IsNullOrEmpty(linkpage))
            {
                _cookieManager.RemoveRememberMe();
                return Redirect(linkpage);
            }
            if (_authenticationAccountManager.IsValidRedirection(Request.UrlReferrer.AbsolutePath))
            {
                return Redirect(Request.UrlReferrer.AbsolutePath);
            }
            return Redirect("/");
        }

        /// <summary>
        /// Redirect to the login page and save in session primary/secondary button clicked
        /// </summary>
        /// <param name="actiontype">primary/secondary</param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult RedirectWarmProduct(string actiontype)
        {
            var account = _sessionAuthenticationManager.GetAccountMembership();
            if (account.Status == StatusEnum.WarmCold)
            {
                var pathZicodePage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ZipCodeVerificationPage.ID));
                return Redirect(pathZicodePage);
            }
            else
            {
                _sessionManager.StoreInSession(ConstantsNeamb.CtaActionType, actiontype);
                var pathLoginPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));
                return Redirect(pathLoginPage);
            }
        }

        /// <summary>
        /// Save in session the component id when it is required to execute some action in source page that called the login
        /// </summary>
        /// <param name="componentIdAuthentication">Component id</param>
        /// <param name="loginUrlForAuthentication">Login url</param>
        /// <param name="featureType">Feature type: Sweepstakes, Seminar</param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult RedirectWarmColdAuthentication(string componentIdAuthentication, string loginUrlForAuthentication, string featureType)
        {
            if (featureType == LoginHandlerEnum.Sweepstake.ToString())
            {
                _loginHandlerPostAction.SaveTrackingPageToRedirection(LoginHandlerEnum.Sweepstake, componentIdAuthentication);
            }
            if (featureType == LoginHandlerEnum.Seminar.ToString())
            {
                _loginHandlerPostAction.SaveTrackingPageToRedirection(LoginHandlerEnum.Seminar, componentIdAuthentication);
            }
            return Redirect(loginUrlForAuthentication);
        }

        [HttpPost]

        public ActionResult SavePostLoginAction()
        {
            _sessionManager.StoreInSession<bool>(ConstantsNeamb.PostLoginAction, true);
            return Json(new { results = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}