using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Services.AuthenticatePassword;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers
{
    public class DuplicateController : BaseController {
		private readonly IAccountRepository _accountRepository;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IAuthenticationAccountManager _authenticationAccountManager;
        private readonly ISessionManager _sessionManager;
		private readonly IProfileManager _profileManager;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IAccountManager _accountManager;
        private readonly IAuthenticatePasswordService _authenticatePasswordService;
		private readonly ILog _log;
		public DuplicateController(
			IAccountRepository accountRepository,
			ISessionAuthenticationManager sessionAuthenticationManager,
			IAuthenticationAccountManager authenticationAccountManager,
            ISessionManager sessionManager,
			IProfileManager profileManager,
			IGlobalConfigurationManager globalConfigurationManager,
            IAccountManager accountManager,
            IAuthenticatePasswordService authenticatePasswordService,
			ILog log
		) {
			_accountRepository = accountRepository;
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_authenticationAccountManager = authenticationAccountManager;
            _sessionManager = sessionManager;
			_profileManager = profileManager;
			_globalConfigurationManager = globalConfigurationManager;
            _accountManager = accountManager;
            _authenticatePasswordService = authenticatePasswordService;
			_log = log;
        }

		/// <summary>
		/// Duplicate Registration page
		/// </summary>
		/// <returns></returns>
		public ActionResult DuplicateRegistration() {
			var model = new DuplicateRegistrationDTO();
			model.Initialize(RenderingContext.Current.Rendering);
			//Get the email logged in the system that is duplicated
			var emailLoggedIn = _sessionAuthenticationManager.GetDuplicateRegistrationEmail();
			if(emailLoggedIn==null || string.IsNullOrEmpty(emailLoggedIn))
            {
				return RedirectErrorDuplicateRegistration(model, "Error getting the email logged in the system that is duplicated");
			}
			model.CurrentEmail = emailLoggedIn.ToLower();
			//Get the email list of the duplicated
			model.EmailList = _accountRepository.GetDuplicateRegistrationEmails(model.CurrentEmail);
			if (model.EmailList == null || model.EmailList.Count==0)
			{
				return RedirectErrorDuplicateRegistration(model,$"Error getting the duplicated email list {model.CurrentEmail}");
			}
			var isExperienceEditor = Sitecore.Context.PageMode.IsExperienceEditor;
			if (isExperienceEditor || model.EmailList.Count > 0) {
				var selectedEmail = RenderingContext.Current.Rendering.Item[Templates.DuplicateRegistration.Fields.SelectedEmail];
				var selectedEmailArray = selectedEmail.Split('|');
				if (selectedEmailArray.Length == 2) {
					model.MessageSelectedEmailPart1 = selectedEmailArray[0];
					model.MessageSelectedEmailPart2 = selectedEmailArray[1];
				}
				var unselectedEmail =
					RenderingContext.Current.Rendering.Item[Templates.DuplicateRegistration.Fields.UnselectedEmails];
				var unselectedEmailArray = unselectedEmail.Split('|');
				if (unselectedEmailArray.Length == 2) {
					model.MessageUnSelectedEmailPart1 = unselectedEmailArray[0];
					model.MessageUnSelectedEmailPart2 = unselectedEmailArray[1];
				}

				//Set the thank you Model
				SetThankPageUrl(model);
				return View("/Views/Neamb.Account/DuplicateRegistration.cshtml", model);
			} else {
               return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID)));
               
            }
        }

		private ActionResult RedirectErrorDuplicateRegistration(DuplicateRegistrationDTO model, string errorMessage)
        {
			model.HasGeneralError = true;
			_log.Error(errorMessage, this);
			return View("/Views/Neamb.Account/DuplicateRegistration.cshtml", model);
		}

		/// <summary>
		/// Get the redirection for the thank you page according the session value saved from Login/Register page
		/// </summary>
		/// <param name="model"></param>
		private void SetThankPageUrl(DuplicateRegistrationDTO model) {
       
            var requestedRegisterPage = _sessionAuthenticationManager.GetRequestedPageRegister();
            var requestedLoginPage = _sessionAuthenticationManager.GetRequestedPageLoginAbsolutePath();
			var requestedProfilePage = _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.RequestedPageProfile);

			if (requestedProfilePage != null && !string.IsNullOrEmpty(requestedProfilePage) && _authenticationAccountManager.IsValidRedirection(requestedProfilePage))
			{
				model.ThankYouUrl = requestedProfilePage;
			}
			else if (requestedLoginPage != null && !string.IsNullOrEmpty(requestedLoginPage) && _authenticationAccountManager.IsValidRedirection(requestedLoginPage)) {
				model.ThankYouUrl = requestedLoginPage;
			}
			else if (requestedRegisterPage != null && !string.IsNullOrEmpty(requestedRegisterPage) && _authenticationAccountManager.IsValidRedirection(requestedRegisterPage)) {
				model.ThankYouUrl = requestedRegisterPage;
			}
			else {

                model.ThankYouUrl = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.RegistrationPage.ID));
            }

			LinkField linkRegistration =
				RenderingContext.Current.Rendering.Item.Fields[Templates.DuplicateRegistration.Fields.ContinueButton];
			if (linkRegistration != null && linkRegistration.TargetItem != null) {
				var urlregister = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(linkRegistration.TargetItem.ID));
				if (string.IsNullOrEmpty(model.ThankYouUrl) && _authenticationAccountManager.IsValidRedirection(urlregister)) {
					model.ThankYouUrl = urlregister;
				}

				model.ThankYouUrlText = linkRegistration.Text;
			} else {
				if (string.IsNullOrEmpty(model.ThankYouUrl)) {
					model.ThankYouUrl = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID));
				}

				model.ThankYouUrlText = ConstantsNeamb.ContinueButton;
			}
		}

		[HttpPost]
		
		public ActionResult DuplicateRegistration(string selectedid, string password) {
			var errors = string.Empty;
			var duplicateEmail = _sessionAuthenticationManager.GetDuplicateRegistrationEmail();
			try {
				if (!duplicateEmail.Equals(selectedid) && string.IsNullOrEmpty(password)) {
					errors = ConstantsNeamb.ErrorDuplicationPassword;
				} else {
					var emailsDeleted = new List<string>();
					//Call the service to delete the emails that the user hasn't selected
					var deleteDuplication = _accountRepository.DeleteDuplicateRegistrationEmails(selectedid, emailsDeleted);
					if (deleteDuplication) {
                        //Call the webservice method UpdateUserStatus with the email selected
                        var resultUpdateUserStatus = _accountManager.UpdateUserStatus(selectedid, (int)UserStatus.Default, (int)Union.NEA);
                        if (resultUpdateUserStatus) {
                            var accountMembership = new AccountMembership();
                            //Authenticate with the user selected
                            if (!duplicateEmail.Equals(selectedid) && !string.IsNullOrEmpty(password)) {
                                var responseService = _accountRepository.AuthenticateUser(accountMembership, selectedid, password, string.Empty, false);
                                if (responseService == null ||
                                    responseService.Data == null ||
                                    !responseService.Data.LoggedIn) {
                                    errors = ConstantsNeamb.ErrorDuplication;
                                }
                            } else {
                                var passwordDuplicate = _sessionAuthenticationManager.GetDuplicateRegistrationPassword();
                                if (!string.IsNullOrEmpty(passwordDuplicate)) {
                                    var responseService = _accountRepository.AuthenticateUser(accountMembership, selectedid, passwordDuplicate, string.Empty, false);
                                    if (responseService == null ||
                                        responseService.Data == null ||
                                        !responseService.Data.LoggedIn) {
                                        errors = ConstantsNeamb.ErrorDuplication;
                                    }
                                } else {
                                    var newMdsid = _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.MdsidDuplication);
                                    accountMembership = _profileManager.CreateNewAccountMembership(newMdsid);
                                    _sessionManager.Remove(ConstantsNeamb.RequestedPageProfile);
                                }
                            }

                            if (accountMembership.Status != StatusEnum.Hot && accountMembership.Status != StatusEnum.WarmCold) {
                                errors = ConstantsNeamb.ErrorDuplication;
                            } else {
                                //Send email 
                                //var emails = string.Join(", ", emailsDeleted);
                                //_accountRepository.SendExactTargetDuplicateRegistrationEmail(accountMembership, emails, selectedid);
                                _sessionAuthenticationManager.RemoveDuplicateRegistration();
                                _sessionAuthenticationManager.RemoveDuplicateRegistrationEmail();
                                _sessionAuthenticationManager.RemoveDuplicateRegistrationPassword();
                            }
                        } else {
                            errors = ConstantsNeamb.ErrorDuplication;
                        }
                    } else {
						errors = ConstantsNeamb.ErrorDuplication;
					}
				}
			} catch {
				errors = ConstantsNeamb.ErrorDuplication;
			}

			return Json(new {
				errors,
				selectedUser = selectedid
			}, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Check the password of the email selected against the webservice AuthenticatePassword
		/// </summary>
		/// <param name="username">Email</param>
		/// <param name="password">Password</param>
		/// <returns></returns>
		[HttpPost]
		
		public JsonResult CheckPassword(string username, string password) {
            var serviceResponse = _authenticatePasswordService.AuthenticatePasswordStatus(username, password, Convert.ToInt32(_globalConfigurationManager.Unionid));
            return Json(new {
                Response = serviceResponse.Data.authenticated ? 0 : 1
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		
		public ActionResult RemoveDuplicateRegistration() {
			_authenticationAccountManager.LogoutUser();
			_sessionAuthenticationManager.RemoveDuplicateRegistration();
			_sessionAuthenticationManager.RemoveDuplicateRegistrationEmail();
			return Redirect(LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID)));
		}
	}
}