using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers {
	public class ProfileController : BaseController {
		private readonly IAccountRepository _accountRepository;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;
		private readonly IProfileManager _profileManager;
		private readonly ISessionManager _sessionManager;
		private readonly ICacheManager _cacheManager;
		private readonly string _cacheKeyGroup = "AvatarImage";
		private readonly IAmazonS3Repository _amazonS3Repository;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly ICookieManager _cookieManager;
		private readonly IAccountManager _accountManager;

        public ProfileController(IAccountRepository accountRepository, ISessionAuthenticationManager sessionAuthenticationManager, IAuthenticationAccountManager authenticationAccountManager, IProfileManager profileManager, ISessionManager sessionManager, ICacheManager cacheManager, IAmazonS3Repository amazonS3Repository, IGlobalConfigurationManager globalConfigurationManager, ICookieManager cookieManager,
            IAccountManager accountManager) {
			_accountRepository = accountRepository;
			_sessionAuthenticationManager = sessionAuthenticationManager;
            _authenticationAccountManager = authenticationAccountManager;
			_profileManager = profileManager;
			_sessionManager = sessionManager;
			_cacheManager = cacheManager;
			_amazonS3Repository = amazonS3Repository;
			_globalConfigurationManager = globalConfigurationManager;
			_cookieManager = cookieManager;
            _accountManager = accountManager;
        }

		/// <summary>
		/// Form Profile and Password
		/// </summary>
		/// <returns>View</returns>
		public ActionResult ProfilePassword() {
			var pathProfile = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ProfilePage.ID));
			if (Request != null && Request.UrlReferrer != null && !Request.UrlReferrer.AbsolutePath.Equals(pathProfile))
			{
				var requestPage = Request.UrlReferrer.AbsolutePath;
				_sessionManager.StoreInSession<string>(ConstantsNeamb.RequestedPageProfile,requestPage);
			}

			var newcellParam = Request.QueryString[ConstantsNeamb.CellcodeNewParam];
			var oldcellParam = Request.QueryString[ConstantsNeamb.CellcodeOldParam];
			var msrNameParam = Request.QueryString[ConstantsNeamb.MsrnameParam];
			var profileDtoModel = _profileManager.GetProfileDto(RenderingContext.Current.Rendering.Item, newcellParam, oldcellParam, msrNameParam);
			//Get the avatar url
			if (!string.IsNullOrEmpty(profileDtoModel.Webuserid)) {
				var keyImageAvatar = $"{_cacheKeyGroup}:{profileDtoModel.Webuserid}";
                byte[] retrievedFile;
                if (_cacheManager.ExistInCache(keyImageAvatar)) {
					retrievedFile = _cacheManager.RetrieveFromCache<byte[]>(keyImageAvatar);
				} else {
					var baseRequest = new BaseRequestS3
					{
						BucketName = _globalConfigurationManager.BucketNameAvatarImages,
						Key = profileDtoModel.Webuserid,
						IsEncrypted = true
					};
					//Try to get the avatar image from s3
					retrievedFile =
						_amazonS3Repository.GetObjectS3<byte[]>(baseRequest);
				}

				if (retrievedFile != null && retrievedFile.Length > 0) {
					_cacheManager.StoreInCache<byte[]>(keyImageAvatar, retrievedFile, DateTime.Now.AddDays(3));
					//Convert byte arry to base64string
					var imreBase64Data = Convert.ToBase64String(retrievedFile);
					//Build the image url
					profileDtoModel.ImageAvatar = string.Format("data:image/jpg;base64,{0}", imreBase64Data);
				}
			}

			//Get the url of the edit avatar page
			profileDtoModel.UpdateAvatarLink =
				LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.AvatarPage.ID));
			profileDtoModel.SiteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
			return View("/Views/Neamb.Account/ProfilePassword.cshtml", profileDtoModel);
		}

		/// <summary>
		/// Post Profile and Password form
		/// </summary>
		/// <param name="model">data</param>
		/// <param name="isFormPassword">Flag to know what kind information is goint to be updated</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateFormHandler]
		
		public ActionResult ProfilePassword(ProfileDTO model, string isFormPassword) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			if (accountMembership.Status == StatusEnum.Unknown || accountMembership.Status == StatusEnum.Cold) {
				var pathProfile = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ProfilePage.ID));
				return Redirect(pathProfile);
			}
            //Build Gtm string
            var contextItem = RenderingContext.Current.Rendering.Item;
            var redirect = Request.QueryString[ConstantsNeamb.FromLandingCta];

			if (!string.IsNullOrEmpty(isFormPassword) && isFormPassword.Equals("0")) {
				
				model.BirthDate = model.TransformBirthDate(model);
				model = _profileManager.SaveProfileDto(model,
                    ModelState.IsValid, ViewData, RenderingContext.Current.Rendering.Item);
                model.GtmAction=_profileManager.GetGtmAction(isFormPassword, contextItem);
                return RedirectProfile(model, redirect);
            } else {
				model.IsUpdatingPassword = true;
				model.Initialize(RenderingContext.Current.Rendering);
				model.HasTooltipCity = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.CityTooltip]);
                model.HasTooltipAddress =
                    !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.AddressTooltip]);
                model.HasTooltipBirthDate =
                    !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.BirthDateTooltip]);
                model.HasTooltipEmail = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.EmailTooltip]);
                model.HasTooltipFirstName =
                    !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.FirstNameTooltip]);
                model.HasTooltipLastName =
                    !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.LastNameTooltip]);
                model.HasTooltipPhone = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.PhoneTooltip]);
                model.HasTooltipState = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.StateTooltip]);
                model.HasTooltipZip = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.ZipTooltip]);

                //Fill full name
                model.UserFullName = $"{accountMembership.Profile.FirstName} {accountMembership.Profile.LastName}";

                //Get the information of the user to be displayed in the screen
                if (accountMembership.Status == StatusEnum.Hot) {
                    model.Address = accountMembership.Profile.StreetAddress;
                    model.Month = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth)
                        ? accountMembership.Profile.DateOfBirth.Substring(0, 2)
                        : string.Empty;
                    model.Day = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth)
                        ? accountMembership.Profile.DateOfBirth.Substring(2, 2)
                        : string.Empty;
                    model.Year = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth)
                        ? accountMembership.Profile.DateOfBirth.Substring(4, 4)
                        : string.Empty;
                    model.City = accountMembership.Profile.City;
                    model.Email = accountMembership.Username;
                    model.FirstName = accountMembership.Profile.FirstName;
                    model.LastName = accountMembership.Profile.LastName;
                    model.Phone = accountMembership.Profile.Phone;
                    model.State = accountMembership.Profile.StateCode;
                    model.Zip = accountMembership.Profile.ZipCode;
                    model.EmailPermission = accountMembership.Profile.EmailPermissionIndicator;
                }

                //Check for input validation in registration form
                var customErrors = HasPasswordCustomValidationErrors(model);
                if (customErrors) return RedirectProfile(model, redirect);
                var updatePwdResponse = _accountManager.UpdatePassword(accountMembership.Username,
                    model.CurrentPassword, model.NewPassword, (int)Union.NEA);

                if (updatePwdResponse.Success && (updatePwdResponse.Data?.Updated ?? false)) {
                    _authenticationAccountManager.LogoutUser();
                    model.ProcessedSucessfully = true;
                } else {
                    if (updatePwdResponse.ErrorCodeResponse == UserAccountErrorCodesEnum.UsernamePasswordCombinationNoMatch)
                        model.HasErrorPassword = true;
                    else
                        model.HasGeneralError = true;
                    model.ProcessedSucessfully = false;
                }
                model.GtmAction = _profileManager.GetGtmAction(isFormPassword, contextItem);
                return RedirectProfile(model, redirect);
            }
        }

        private ActionResult RedirectProfile(ProfileDTO model, string redirect) {
			if (model.HasDuplicateAccount) {
				_authenticationAccountManager.LogoutUser();
				_cookieManager.RemoveWarmUser();
               
                _sessionAuthenticationManager.SaveDuplicateRegistration(model.Registrations);  

                _sessionAuthenticationManager.SaveDuplicateRegistrationEmail(model.Email.ToLower());
				_sessionManager.StoreInSession<string>(ConstantsNeamb.MdsidDuplication, model.NewMdsid);
				return Redirect(_accountRepository.GetDuplicateRegistrationPageUrl());
			} else {
				//Get the url of the edit avatar page
				model.UpdateAvatarLink =
					LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.AvatarPage.ID));
				model.SiteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);

                if (string.IsNullOrEmpty(redirect) || !model.ProcessedSucessfully) return View("/Views/Neamb.Account/ProfilePassword.cshtml", model);
                var redirectitem = Sitecore.Context.Database.GetItem(redirect);
                if (redirectitem == null) return View("/Views/Neamb.Account/ProfilePassword.cshtml", model);
                var redirectionUrl = LinkManager.GetItemUrl(redirectitem);
                return Redirect(redirectionUrl);
            }
		}

		/// <summary>
		/// Execute special validation in registration forms
		/// </summary>
		/// <param name="model">Registration form data</param>
		/// <returns>True in case of errors: otherwise false</returns>
		private bool HasPasswordCustomValidationErrors(ProfileDTO model) {
			if (!ValidationFieldHelper.ValidatePassword(model.NewPassword) || !ValidationFieldHelper.ValidatePassword(model.ConfirmPassword))
                model.ErrorsNewPassword.Add(Foundation.Config.Models.ErrorStatusEnum.PasswordRequirement);
            else if (!model.ConfirmPassword.Equals(model.NewPassword)) model.ErrorsNewPassword.Add(Foundation.Config.Models.ErrorStatusEnum.PasswordNotEqual);

            return model.ErrorsNewPassword.Count > 0;
		}

		[HttpPost]
		public ActionResult RedirectProfilePage(string idpage) {
			var pathProfilePage =
				$"{LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.ProfilePageId))}?redirect={idpage}";
			return Redirect(pathProfilePage);
		}
	}
}