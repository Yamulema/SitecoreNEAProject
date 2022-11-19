using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.Sitecore.Utility;
using Sitecore.Analytics;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class RegistrationController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly ISessionManager _sessionManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;

        public RegistrationController(IAccountRepository accountRepository, ISessionAuthenticationManager sessionAuthenticationManager, ISessionManager sessionManager, IAuthenticationAccountManager authenticationAccountManager
        )
        {
            _accountRepository = accountRepository;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _sessionManager = sessionManager;
            _authenticationAccountManager = authenticationAccountManager;
        }

        public ActionResult Registration()
        {
            var userAccount = _sessionAuthenticationManager.GetAccountMembership();

            if (userAccount.Status == StatusEnum.Hot)
            {
                return Redirect(GetReturnUrlHotState());
            }
            else if (userAccount.Status == StatusEnum.WarmHot || userAccount.Status == StatusEnum.Hot)
            {
                _sessionManager.StoreInSession<string>(ConstantsNeamb.FromRegistrationPage, "1");
                return Redirect(GetUrlFromSitecoreID(Templates.LoginPage.ID));
            }

            var pathZipCodeValidation = GetUrlFromSitecoreID(Templates.ZipCodeVerificationPage.ID);
            var pathMemberVerification = GetUrlFromSitecoreID(Items.MemberVerification);
            var reqPath = Request?.UrlReferrer?.AbsolutePath;

            var resultValidationSucess= _sessionAuthenticationManager.GetZipCodeValidationSuccess();
            if (userAccount.Status == StatusEnum.WarmCold &&
                !resultValidationSucess)
            {
                return Redirect(pathZipCodeValidation);
            }

            //Save in session the page to redirect after registration is done. (GUID as Query String)
            var redirectToPage = _sessionManager.RetrieveFromSession<string>("PageToRedirect");
            var redirectPageQueryString = Request.QueryString[ConstantsNeamb.RedirectUrlLoginParameter];
            var pathRegistration = GetUrlFromSitecoreID(Templates.RegistrationPage.ID);
            string redirectToUrl;

            var urlHostName = Sitecore.Context.Site.HostName;
            var hostNames = urlHostName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            //query string
            if(!string.IsNullOrEmpty(redirectPageQueryString))
            {
                redirectToUrl = GetUrlFromSitecoreID(new Sitecore.Data.ID(redirectPageQueryString));
            }
            //from session
            else if (!string.IsNullOrEmpty(redirectToPage))
            {
                redirectToUrl = GetUrlFromSitecoreID(new Sitecore.Data.ID(redirectToPage));
            }
            //postback
            else if (Request != null && Request.UrlReferrer != null && hostNames.Contains(Request.UrlReferrer.Host) && !Request.UrlReferrer.AbsolutePath.Equals(pathRegistration))
            {
                var fullPathDepuredUrl = RedirectionHelper.GetFullPath(Request.UrlReferrer);
                var itemToRedirect = Sitecore.Context.Database.GetItem(fullPathDepuredUrl);
                redirectToUrl = GetUrlFromSitecoreID(itemToRedirect.ID);
            }
            //if the request comes from an external url, should be redirected to the homepage
            else
            {
                redirectToUrl = GetUrlFromSitecoreID(Templates.HomePage.ID);
            }
            _sessionManager.Remove("PageToRedirect");
            _sessionAuthenticationManager.SaveRequestedPageRegister(redirectToUrl);

            //Populate registration form is Warm Cold
            RegistrationDTO registrationModel = new RegistrationDTO();
            if (userAccount.Status == StatusEnum.WarmCold)
            {
                registrationModel.Address = userAccount.Profile.StreetAddress;
                registrationModel.Month = !string.IsNullOrEmpty(userAccount.Profile.DateOfBirth) ? userAccount.Profile.DateOfBirth.Substring(0, 2) : string.Empty;
                registrationModel.Day = !string.IsNullOrEmpty(userAccount.Profile.DateOfBirth) ? userAccount.Profile.DateOfBirth.Substring(2, 2) : string.Empty;
                registrationModel.Year = !string.IsNullOrEmpty(userAccount.Profile.DateOfBirth) ? userAccount.Profile.DateOfBirth.Substring(4, 4) : string.Empty;
                registrationModel.City = userAccount.Profile.City;
                registrationModel.Email = userAccount.Username;
                registrationModel.FirstName = userAccount.Profile.FirstName;
                registrationModel.LastName = userAccount.Profile.LastName;
                registrationModel.Phone = userAccount.Profile.Phone;
                registrationModel.State = userAccount.Profile.StateCode;
                registrationModel.Zip = userAccount.Profile.ZipCode;
            }
            registrationModel.Initialize(RenderingContext.Current.Rendering);
            SetRequestedPage(registrationModel);

            if (Request != null &&
                Request.UrlReferrer != null &&
                !Request.UrlReferrer.AbsolutePath.Equals(pathRegistration) &&
                Tracker.Current != null &&
                !string.IsNullOrEmpty(registrationModel.RequestedPage))
            {
                var pagesVisited = Tracker.Current.Interaction.Pages;
                var itemRequestRegistration = pagesVisited.FirstOrDefault(item => item.Url.Path == HttpUtility.UrlDecode(registrationModel.RequestedPage));
                if (itemRequestRegistration != null)
                {
                    _sessionManager.StoreInSession<Guid>(ConstantsNeamb.ItemIdRequestRegistration, itemRequestRegistration.Item.Id);
                }
            }

            registrationModel.SubmitText =
                RenderingContext.Current.Rendering.Item[Templates.RegistrationSteps.Fields.NextStepButton];

            var contextItem = RenderingContext.Current.Rendering.Item;
            registrationModel.HasTooltipCity = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.CityTooltip]);
            registrationModel.HasTooltipAddress = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.AddressTooltip]);
            registrationModel.HasTooltipBirthDate = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.BirthDateTooltip]);
            registrationModel.HasTooltipEmail = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.EmailTooltip]);
            registrationModel.HasTooltipPassword = !string.IsNullOrEmpty(contextItem[Templates.Password.Fields.Tooltip]);
            registrationModel.HasTooltipFirstName = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.FirstNameTooltip]);
            registrationModel.HasTooltipLastName = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.LastNameTooltip]);
            registrationModel.HasTooltipPhone = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.PhoneTooltip]);
            registrationModel.HasTooltipState = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.StateTooltip]);
            registrationModel.HasTooltipZip = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.ZipTooltip]);

            return View("/Views/Neamb.Account/Registration.cshtml", registrationModel);
        }

        private void SetRequestedPage(RegistrationDTO registrationDTOModel)
        {
            if (string.IsNullOrEmpty(registrationDTOModel.RequestedPage))
            {
                var pathRedirectionRegister = _sessionAuthenticationManager.GetRequestedPageRegister();
                var pathRedirectionLogin = _sessionAuthenticationManager.GetRequestedPageLoginAbsolutePath();
                var pathLogin = GetUrlFromSitecoreID(Templates.LoginPage.ID);

                if (_authenticationAccountManager.IsValidRedirection(pathRedirectionLogin) && Request != null && Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.Equals(pathLogin, StringComparison.InvariantCultureIgnoreCase))
                {
                    registrationDTOModel.RequestedPage = pathRedirectionLogin;
                }
                else if (_authenticationAccountManager.IsValidRedirection(pathRedirectionRegister))
                {
                    registrationDTOModel.RequestedPage = pathRedirectionRegister;
                }
                else
                {
                    registrationDTOModel.RequestedPage = GetUrlFromSitecoreID(Templates.HomePage.ID);
                }
            }
        }

        /// <summary>
		/// Post Register form
		/// </summary>
		/// <param name="model">Register data</param>
		/// <returns></returns>
		[HttpPost]
        public JsonResult Registration(RegistrationDTO model)
        {
            if (string.IsNullOrEmpty(model.Emailconfirmation))
            {
                model.PostInitialize();
                model.IsSubmitted = true;
                model.BirthDate = model.TransformBirthDate(model);
                SetRequestedPage(model);

                var urlRedirection = _accountRepository.ExecuteRegistration(model, ViewData, ModelState.IsValid);
                _accountRepository.SetGtmActionRegistration(model);

                var response = (!ModelState.IsValid || !model.IsValid || !model.IsSubmitted || !model.HasGeneralError) && model.ProcessedSucessfully;
                var viewModel = GetRegistrationViewModel(model);
                var errors = GetViewModelErrorsFromModel(model);
                viewModel.UrlRedirection = urlRedirection;

                return Json(new
                {
                    Success = response,
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        Model = viewModel,
                        Errors = errors,
                    }
                });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return Json(new
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.Conflict
                });
            }
        }

        private string GetUrlFromSitecoreID(Sitecore.Data.ID sitecoreId)
        {
            return LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(sitecoreId));
        }

        private RegistrationViewModel GetRegistrationViewModel(RegistrationDTO dtoModel)
        {
            var viewModel = new RegistrationViewModel
            {
                FirstName = dtoModel.FirstName,
                LastName = dtoModel.LastName,
                Address = dtoModel.Address,
                Day = dtoModel.Day,
                Month = dtoModel.Month,
                Year = dtoModel.Year,
                ConfirmPassword = dtoModel.ConfirmPassword,
                CurrentPassword = dtoModel.CurrentPassword,
                Email = dtoModel.Email,
                EmailPermission = dtoModel.EmailPermission,
                Phone = dtoModel.Phone,
                OptIn = dtoModel.OptIn,
                City = dtoModel.City,
                State = dtoModel.State,
                Zip = dtoModel.Zip,
                GtmAction = dtoModel.GtmAction,
                RequestedPage = dtoModel.RequestedPage,
                UserFullName = dtoModel.UserFullName,
                ImageAvatar = dtoModel.ImageAvatar,
                UpdateAvatarLink = dtoModel.UpdateAvatarLink,
                IsSubmitted = dtoModel.IsSubmitted,
                IsValid = dtoModel.IsValid
            };
            return viewModel;
        }

        private ErrorsViewModel GetViewModelErrorsFromModel(RegistrationDTO model)
        {
            var errorList = new List<ErrorItem>
            {
                new ErrorItem() { FieldName = "firstName", ErrorEnumList = model.ErrorsFirstName },
                new ErrorItem() { FieldName = "lastName", ErrorEnumList = model.ErrorsLastName },
                new ErrorItem() { FieldName = "email", ErrorEnumList = model.ErrorsEmail },
                new ErrorItem() { FieldName = "password", ErrorEnumList = model.ErrorsPassword },
                new ErrorItem() { FieldName = "confirmPassword", ErrorEnumList = model.ErrorsConfirmPassword },
                new ErrorItem() { FieldName = "address", ErrorEnumList = model.ErrorsAddress },
                new ErrorItem() { FieldName = "city", ErrorEnumList = model.ErrorsCity },
                new ErrorItem() { FieldName = "phone", ErrorEnumList = model.ErrorsPhone },
                new ErrorItem() { FieldName = "state", ErrorEnumList = model.ErrorsState },
                new ErrorItem() { FieldName = "zip", ErrorEnumList = model.ErrorsZip },
                new ErrorItem() { FieldName = "birthDate", ErrorEnumList = model.ErrorsBirthDate }
            };

            var errors = new ErrorsViewModel
            {
                ErrorList = TransformErrors(errorList),
                HasGeneralError = model.HasGeneralError,
                HasDuplicateAccount = model.HasDuplicateAccount,
                HasErrorPassword = model.HasErrorPassword,
                HasErrorUsername = model.HasErrorUsername,
                IsProcessedSucessfully = model.ProcessedSucessfully
            };
            return errors;
        }

        private List<ErrorItem> TransformErrors(List<ErrorItem> errors)
        {
            var errorList = new List<ErrorItem>();

            foreach (var errorItem in errors)
            {
                if (errorItem.ErrorEnumList.Count == 0)
                {
                    continue;
                }

                var error = new ErrorItem()
                {
                    FieldName = errorItem.FieldName,
                    ErrorType = new List<string>()
                };
                foreach (var value in errorItem.ErrorEnumList)
                {
                    error.ErrorType.Add(Enum.GetName(typeof(ErrorStatusEnum), value));
                }
                errorList.Add(error);
            }
            return errorList;
        }

        private string GetReturnUrlHotState()
        {
            var redirectToPage = Request != null ? Request.QueryString[ConstantsNeamb.RedirectUrlRegisterParameter] : "";
            string redirectToUrl;

            if (!string.IsNullOrEmpty(redirectToPage))
            {
                redirectToUrl = GetUrlFromSitecoreID(new Sitecore.Data.ID(redirectToPage));
            }
            else
            {
                redirectToUrl = GetUrlFromSitecoreID(Templates.HomePage.ID);
            }
            return redirectToUrl;
        }
    }
}