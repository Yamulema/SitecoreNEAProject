using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.Sitecore.Utility;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers {
	public class ZipCodeVerificationController : BaseController {

		private const string ZIPCODE_VERIFY_VIEW = "/Views/Neamb.Account/ZipCodeVerification.cshtml";
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly ISessionManager _sessionManager;
		private readonly IAuthenticationAccountManager _authenticationAccountManager;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly ICookieManager _cookieManager;

		public ZipCodeVerificationController(IAccountRepository accountRepository, ISessionManager sessionManager,
			IAuthenticationAccountManager authenticationAccountManager, IGlobalConfigurationManager globalConfigurationManager, ICookieManager cookieManager, ISessionAuthenticationManager sessionAuthenticationManager) {
			_sessionManager = sessionManager;
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_authenticationAccountManager = authenticationAccountManager;
			_globalConfigurationManager = globalConfigurationManager;
			_cookieManager = cookieManager;
		}

		public ActionResult ZipCodeVerification() {
			_sessionAuthenticationManager.RemoveZipCodeValidationSuccess();
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            if (accountMembership.Status == StatusEnum.WarmCold) {
				//Remove the attempt counter from session each this screen is loaded
				_sessionAuthenticationManager.RemoveAttemptZipCodeValidation();
				var model = new ZipCodeVerificationDTO();
				model.Initialize(RenderingContext.Current.Rendering);
				model.FullName = string.Format("{0} {1}", accountMembership.Profile.FirstName, accountMembership.Profile.LastName);
				model.ButtonText = RenderingContext.Current.Rendering.Item[Templates.ZipCodeVerificationForm.Fields.Next];
				model.HasTooltip = !string.IsNullOrEmpty(RenderingContext.Current.Rendering.Item[Templates.ZipCodeVerificationForm.Fields.Tooltip]);

                string sitecoreIdItem="";
                //Change for returning to the previous page after registration
                //----------------------------------------------------------
                if (Request != null && Request.UrlReferrer != null)
                {
                    var requestPage = Request.UrlReferrer;
                    var fullPath = RedirectionHelper.GetFullPath(requestPage);
					var itemToRedirect = Sitecore.Context.Database.GetItem(fullPath);
					if (itemToRedirect != null)
					{
						sitecoreIdItem = itemToRedirect.ID.ToString();						
					}
				}
                else
                {
					sitecoreIdItem = Templates.HomePage.ID.ToString();
                }
				_sessionManager.StoreInSession("PageToRedirect", sitecoreIdItem);
				//----------------------------------------------------------

                return View(ZIPCODE_VERIFY_VIEW, model);

			} else {
				var pathRegistration = LinkManager.GetItemUrl(Context.Database.GetItem(Templates.RegistrationPage.ID));
				return Redirect(pathRegistration);
			}
		}

		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult ZipCodeVerification(string zipCode, string ckbOptin) {
			ZipCodeVerificationDTO model = new ZipCodeVerificationDTO();
			model.ZipCode = zipCode;
			model.Initialize(RenderingContext.Current.Rendering);
			model.ButtonText = RenderingContext.Current.Rendering.Item[Templates.ZipCodeVerificationForm.Fields.Next];
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			model.FullName = string.Format("{0} {1}", accountMembership.Profile.FirstName, accountMembership.Profile.LastName);
            model.HasTooltip = !string.IsNullOrEmpty(RenderingContext.Current.Rendering.Item[Templates.ZipCodeVerificationForm.Fields.Tooltip]);

            if (ModelState.IsValid) {
				//Get the account membership user logged from session and verify status WarmCold
				if (accountMembership.Status == Foundation.Membership.Model.StatusEnum.WarmCold) {
					//If the zipcode of the user logged matches with the entered in the screen redirect to the registration page with the information of the user logged
					if (accountMembership.Profile.ZipCode.Equals(model.ZipCode)) {
						_sessionAuthenticationManager.SaveZipCodeValidationSuccess();
                        return Redirect(GetRegistrationPageUrl());

					} else {
						//Get the attempt counter from session
						var attemptNumber = _sessionAuthenticationManager.GetAttemptZipCodeValidation();
						attemptNumber++;
						if (attemptNumber >= _globalConfigurationManager.AttemptZipCodeValidation) {
							//If the attempt number is greater than 3 redirect logout the user and redirect to the registration page
							_authenticationAccountManager.LogoutUser(false);
							_cookieManager.RemoveWarmUser();
                            return Redirect(GetRegistrationPageUrl());
                        } else {
							//If the attempt number is less than 3 redirect to the same screen
							_sessionAuthenticationManager.SaveAttemptZipCodeValidation(attemptNumber);
							model.Attempts = attemptNumber;
							model.ErrorsZipCode.Add(Foundation.Config.Models.ErrorStatusEnum.MatchNotFound);
							return View(ZIPCODE_VERIFY_VIEW, model);
						}
					}
				} else {
					model.ErrorsZipCode.Add(Foundation.Config.Models.ErrorStatusEnum.MatchNotFound);
					return View(ZIPCODE_VERIFY_VIEW, model);
				}
			} else {
				model.ErrorsZipCode =
					ValidationFieldHelper.SetErrorsField(ViewData.ModelState[nameof(model.ZipCode)], true, true, true);
				return View(ZIPCODE_VERIFY_VIEW, model);
			}
		}
        
		/// <summary>
		/// Get the url from the Registration general link in Zip Code Verification page
		/// </summary>
		/// <returns>Registration url</returns>
		private string GetRegistrationPageUrl() {
			LinkField linkRegistration =
				RenderingContext.Current.Rendering.Item.Fields[Templates.ZipCodeVerificationForm.Fields.Registration];
			if (linkRegistration != null && linkRegistration.TargetItem != null) {
				return LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(linkRegistration.TargetItem.ID));
			} else {
				return LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID));
			}
		}
	}
}