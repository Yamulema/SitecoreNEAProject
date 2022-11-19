using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Forms.Constants;
using Neambc.Seiumb.Feature.Forms.Enums;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Feature.Forms.Repositories;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Registration.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.WebServices.Managers;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class RegistrationFormController : BaseController
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IUserRepository _userRepository;

        public RegistrationFormController(IAuthenticationManager authenticationManager, IAuthenticationRepository authenticationRepository,
            IRegistrationRepository registrationRepository, ISeiumbProfileManager seiumbProfileManager, IUserRepository userRepository)
        {
            _authenticationManager = authenticationManager;
            _authenticationRepository = authenticationRepository;
            _registrationRepository = registrationRepository;
            _seiumbProfileManager = seiumbProfileManager;
            _userRepository = userRepository;
        }

        public ActionResult RegistrationForm()
        {
            var registrationModel = new RegistrationFormModel { SendInformation = true };
            registrationModel.Initialize(RenderingContext.Current.Rendering);
            var keyControllerFormName = Request.Form.AllKeys.FirstOrDefault(item => item.Equals("fhController"));
            var keyControllerValue = keyControllerFormName != null ? Request.Form.Get(keyControllerFormName) : string.Empty;
            var loginMobilePage = Context.Database.GetItem(Templates.LoginMobilePage.ID);
            var loginMobileUrl = LinkManager.GetItemUrl(loginMobilePage);
            var userStatus = _userRepository.GetUserStatus();
            if (userStatus == UserStatusCons.HOT)
            {
                var homeItem = Context.Database.GetItem(Templates.Home.ID);
                return Redirect(LinkManager.GetItemUrl(homeItem));
            }

            if ((userStatus.Equals(UserStatusCons.WARM_HOT) || userStatus.Equals(UserStatusCons.HOT)) &&
                (string.IsNullOrEmpty(keyControllerValue) || !keyControllerValue.Equals("LoginFormController")))
            {
                LogoutUser();
            }
            var seiuProfile = _seiumbProfileManager.GetProfile();
            var zipValidation = HttpContext.Session[SessionConstants.ZIP_VALIDATION]?.ToString();
            if (zipValidation != null)
            {
                registrationModel.FirstName = seiuProfile.FirstName;
                registrationModel.LastName = seiuProfile.LastName;
                registrationModel.Address = seiuProfile.StreetAddress;
                registrationModel.City = seiuProfile.City;
                registrationModel.State = seiuProfile.StateCode;
                registrationModel.ZipCode = seiuProfile.ZipCode;
                registrationModel.DateOfBirth = seiuProfile.DateOfBirth;
                registrationModel.Phone = seiuProfile.Phone;
                registrationModel.Email = seiuProfile.Email;
                HttpContext.Session[SessionConstants.ZIP_VALIDATION] = null;
            }
            else
            {
                if (!_userRepository.GetUserStatus().Equals(UserStatusCons.WARM_COLD)) return View("/Views/Forms/RegistrationForm.cshtml", registrationModel);
                var zipCodeValidationPage = Context.Database.GetItem(Templates.ZipCodeValidationPage.ID);
                var zipCodeValidationUrl = LinkManager.GetItemUrl(zipCodeValidationPage);
                return Redirect(zipCodeValidationUrl);
            }
            return View("/Views/Forms/RegistrationForm.cshtml", registrationModel);
        }

        [HttpPost]
        [ValidateFormHandler]
        [ValidateInput(false)]
        public ActionResult RegistrationForm(RegistrationFormModel model)
        {
            if (ModelState.IsValid)
            {
                model.Email = model.Email.ToLower();
                //Debug data
                Log.Info($"RegisterUser method login:{model.Email}", this);
                Log.Info($"RegisterUser method address:{model.Address}", this);
                Log.Info($"RegisterUser method city:{model.City}", this);
                Log.Info($"RegisterUser method date of birth:{model.DateOfBirth}", this);
                Log.Info($"RegisterUser method firstname:{model.FirstName}", this);
                Log.Info($"RegisterUser method lastname:{model.LastName}", this);
                Log.Info($"RegisterUser method phone:{model.Phone}", this);
                Log.Info($"RegisterUser method state:{model.State}", this);
                Log.Info($"RegisterUser method zipcode:{model.ZipCode}", this);

                var validCode = _authenticationRepository.IsUsernameAvailable(model.Email);

                if (validCode == 1)
                {  // available for registration
                    var registrationPage = Context.Database.GetItem(Templates.RegistrationPage.ID);
                    Item thankYouPageItem = null;
                    Item errorPageItem = null;
                    if (registrationPage != null && registrationPage.HasChildren)
                    {
                        thankYouPageItem = registrationPage.GetChildren().FirstOrDefault(item => item.TemplateID.Equals(Templates.RegistrationThankYouTemplate.ID));
                        errorPageItem = registrationPage.GetChildren().FirstOrDefault(item => item.TemplateID.Equals(Templates.ErrorPageTemplate.ID));
                    }
                    Log.Info($"Calling RegisterUser method login{model.Email}", this);

                    var cellCode = HttpContext.Session[ConstantsSeiumb.SourceCode] != null ? HttpContext.Session[ConstantsSeiumb.SourceCode].ToString() : string.Empty;
                    var campaignCode = HttpContext.Session[ConstantsSeiumb.CampaignCode] != null ? HttpContext.Session[ConstantsSeiumb.CampaignCode].ToString() : string.Empty;
                    var seiuProfile = _seiumbProfileManager.GetProfile();
                    Log.Info($"Registration cellCode: {cellCode}", this);
                    Log.Info($"Registration campaignCode: {campaignCode}", this);

                    var response = _registrationRepository.RegisterUser(model.FirstName, model.LastName, model.Address, model.City, model.State,
                        model.ZipCode, Utilities.Utilities.FormatDate(model.DateOfBirth), Utilities.Utilities.FormatPhone(model.Phone), model.Email, model.Password,
                        Utilities.Utilities.FormatPermission(model.SendInformation), campaignCode, cellCode);
                    if (response != null && response.Success && response.Data.Registered && thankYouPageItem != null)
                    {
                        Log.Info($"Response 0 RegisterUser login{model.Email}", this);
                        var loginResponse = _authenticationManager.AuthenticateUser(seiuProfile, model.Email, model.Password, cellCode);
                        if (loginResponse.IsAuthenticated)
                        {
                            _authenticationManager.FillUserData(seiuProfile, loginResponse.Data.MdsIdAsString, FormConstants.NEA_COOKIE_MDSID, true, loginResponse.Data.RegistrationDuplicateOldFormat);
                            if (loginResponse.Data.RegistrationCount > 1)
                            {
                                HttpContext.Session[ConstantsSeiumb.DuplicateLogin] = "1";
                                Log.Info($"RegistrationCount > 1 (Registration method) login{model.Email}", this);
                                //Section to send the email of registration
                                var mdsIndvId = loginResponse.Data.MdsIdAsString.PadLeft(9, '0');
                                SendRegistrationMail(model, null, mdsIndvId, model.FirstName, model.LastName);
                                seiuProfile.DuplicateEmail = model.Email.ToLower();
                                seiuProfile.Status = UserStatusCons.HOT;
                                _seiumbProfileManager.SaveProfile(seiuProfile);
                                //Sitecore.Context.User.Profile.Save();
                                //Sitecore.Context.User.Profile.Reload();
                                var duplicateRegistrationPage = Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID);
                                var duplicateRegistrationUrl = !string.IsNullOrEmpty(model.RedirectAction) ?
                                    $"{LinkManager.GetItemUrl(duplicateRegistrationPage)}?redirect={HttpUtility.UrlEncode(model.RedirectAction)}" :
                                    LinkManager.GetItemUrl(duplicateRegistrationPage);
                                return Redirect(duplicateRegistrationUrl);
                            }
                            else
                            {
                                Log.Info($"RegistrationCount =0 (Registration method)  login{model.Email}", this);
                                //AuthenticationManager.Instance.FillUserData(loginResponse.MdsId, FormConstants.NEA_COOKIE_MDSID, true);
                                Session[SessionConstants.REGISTRATION_IS_MEMBER] = !string.IsNullOrEmpty(seiuProfile.SeiuCurrentMember) &&
                                    seiuProfile.SeiuCurrentMember.Equals("Y");
                                //Section to send the email of registration
                                SendRegistrationMail(model, seiuProfile.SeiuCurrentMember, seiuProfile.MdsId.PadLeft(9, '0'),
                                    seiuProfile.FirstName, seiuProfile.LastName);
                                var thankYouUrl = !string.IsNullOrEmpty(model.RedirectAction) ?
                                    $"{LinkManager.GetItemUrl(thankYouPageItem)}?redirect={HttpUtility.UrlEncode(model.RedirectAction)}"
                                    : LinkManager.GetItemUrl(thankYouPageItem);
                                return Redirect(thankYouUrl);
                            }
                        }
                    }
                    else
                    {
                        Log.Info($"Response error RegisterUser login{model.Email}", this);
                        var errorPageUrl = LinkManager.GetItemUrl(errorPageItem);
                        return Redirect(errorPageUrl);
                    }
                }
                else if (validCode == 0)
                {   // username already registered
                    Log.Info($"RegisterUser method USERNAME_NOT_AVAILABLE  login{model.Email}", this);
                    if (model.Errors == null) model.Errors = new List<RegistrationErrors> { RegistrationErrors.USERNAME_NOT_AVAILABLE };
                    model.Initialize(RenderingContext.Current.Rendering);
                    return View("/Views/Forms/RegistrationForm.cshtml", model);
                }
                else  // username invalid
                {
                    Log.Info(string.Format("RegisterUser method USERNAME_INVALID  login{0}", model.Email), this);
                    if (model.Errors == null) //INVALID_USERNAME  MBREQ-1394
                        model.Errors = new List<RegistrationErrors> { RegistrationErrors.INVALID_USERNAME };
                    model.Initialize(RenderingContext.Current.Rendering);
                    return View("/Views/Forms/RegistrationForm.cshtml", model);
                }
            }
            else
            {
                if (model.Errors == null) model.Errors = new List<RegistrationErrors>();

                model.Initialize(RenderingContext.Current.Rendering);
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
                modelStateVal = ViewData.ModelState[nameof(model.Address)];
                model.HasErrorAddress = modelStateVal.Errors.Count > 0;
                if (model.HasErrorAddress)
                {
                    model.HasErrorAddressInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
                    model.HasErrorAddressLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                }

                modelStateVal = ViewData.ModelState[nameof(model.City)];
                model.HasErrorCity = modelStateVal.Errors.Count > 0;
                if (model.HasErrorCity)
                {
                    model.HasErrorCityInvalidCharacters = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Special characters not allowed")) != null;
                    model.HasErrorCityLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                }
                modelStateVal = ViewData.ModelState[nameof(model.State)];
                model.HasErrorState = modelStateVal.Errors.Count > 0;
                modelStateVal = ViewData.ModelState[nameof(model.ZipCode)];
                model.HasErrorZipcode = modelStateVal.Errors.Count > 0;
                if (model.HasErrorZipcode) model.HasErrorZipcodeLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                modelStateVal = ViewData.ModelState[nameof(model.DateOfBirth)];
                model.HasErrorBirthDate = modelStateVal.Errors.Count > 0;
                if (model.HasErrorBirthDate) //verify the age greater than 16 years
                    model.HasErrorDateOfBirthAge = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("DateRangeError")) != null;
                modelStateVal = ViewData.ModelState[nameof(model.Phone)];
                model.HasErrorPhone = modelStateVal.Errors.Count > 0;
                if (model.HasErrorPhone) model.HasErrorPhoneLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                modelStateVal = ViewData.ModelState[nameof(model.Email)];
                model.HasErrorEmail = modelStateVal.Errors.Count > 0;
                if (model.HasErrorEmail) model.HasErrorEmailLength = modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals("Error Length")) != null;
                return View("/Views/Forms/RegistrationForm.cshtml", model);
            }
            return null;
        }

        private void SendRegistrationMail(RegistrationFormModel model, string isMember, string mdsIndvId, string firstName, string lastName)
        {
            var site = Context.Site;
            var cookieKey = site.GetCookieKey("lang");
            var cellcode = !string.IsNullOrEmpty(isMember) && isMember.Equals("Y") ? string.IsNullOrEmpty(WebUtil.GetCookieValue(cookieKey)) ||
                WebUtil.GetCookieValue(cookieKey).Equals("en") ? Settings.GetSetting("ExacttargetRegistrationCellCodeCurrYesEN") :
                Settings.GetSetting("ExacttargetRegistrationCellCodeCurrYesES") :
                string.IsNullOrEmpty(WebUtil.GetCookieValue(cookieKey)) || WebUtil.GetCookieValue(cookieKey).Equals("en") ?
                    Settings.GetSetting("ExacttargetRegistrationCellCodeCurrNoEN") :
                    Settings.GetSetting("ExacttargetRegistrationCellCodeCurrNoES");
            var seiuProfile = _seiumbProfileManager.GetProfile();

            var resultRestrictedLocals = OracleProvider.ExecuteQueryRestrictedLocals(seiuProfile.SeiuLocalNumber);
            if (!string.IsNullOrEmpty(resultRestrictedLocals))
                cellcode = string.IsNullOrEmpty(WebUtil.GetCookieValue(cookieKey)) || WebUtil.GetCookieValue(cookieKey).Equals("en") ?
                    Settings.GetSetting("ExacttargetRegistrationCellCodeRestricedLocalEN") : Settings.GetSetting("ExacttargetRegistrationCellCodeRestricedLocalES");
            else
                Log.Info($"ExecuteQueryRestrictedLocals doesn't return data. Email {model.Email}, local number {seiuProfile.SeiuLocalNumber}", this);
            ExactTargetServiceManager.SendExactTargetRegistration(model.Email, mdsIndvId, firstName, lastName, cellcode, "TGS01132",
                seiuProfile.EmailPermission, Settings.GetSetting("ExacttargetRegistrationCustomerDefinition"));
        }

        private void LogoutUser()
        {
            CookieStore.SetCookie(FormConstants.NEA_COOKIE_MDSID, string.Empty, TimeSpan.FromDays(System.Convert.ToInt32(-7)));
            CookieStore.SetCookie(FormConstants.SEIUMBUsername, string.Empty, TimeSpan.FromDays(System.Convert.ToInt32(-7)));
            _userRepository.LogoutUser();
            _seiumbProfileManager.DeleteProfile();
        }
    }
}