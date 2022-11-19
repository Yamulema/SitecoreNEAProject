using System;
using System.Collections.Generic;
using System.Web;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.CancelResetToken;
using Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken;
using Neambc.Neamb.Foundation.MBCData.Services.DeleteUser;
using Neambc.Neamb.Foundation.MBCData.Services.ForgotUserName;
using Neambc.Neamb.Foundation.MBCData.Services.ValidateResetToken;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.WebServices;
using Neambc.Seiumb.Foundation.WebServices.Managers;
using Sitecore;
using Sitecore.Globalization;
using Sitecore.Links;
using Convert = System.Convert;

namespace Neambc.Seiumb.Feature.Forms.Repositories {
	[Service(typeof(IFormsRepository))]
	public class FormsRepository : IFormsRepository {
		private readonly IBase64Service _base64Service;
        private readonly IAccountServiceProxy _neambServiceManager;
		private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly ICreateResetTokenService _createResetTokenService;
        private readonly IAccountManager _accountManager;
        private readonly IForgotUserNameService _forgotUserNameService;
        private readonly ICancelResetTokenService _cancelResetTokenService;
        private readonly IValidateResetTokenService _validateResetTokenService;
    
        public FormsRepository(IBase64Service base64Service, IAccountServiceProxy neambServiceManager, IWebServicesConfiguration webServicesConfiguration,  ICreateResetTokenService createResetTokenService, IAccountManager accountManager, IForgotUserNameService forgotUserNameService, ICancelResetTokenService cancelResetTokenService, IValidateResetTokenService validateResetTokenService)
		{
			_base64Service = base64Service;
            _neambServiceManager = neambServiceManager;
			_webServicesConfiguration = webServicesConfiguration;
            _createResetTokenService = createResetTokenService;
            _accountManager = accountManager;
            _forgotUserNameService = forgotUserNameService;
            _cancelResetTokenService = cancelResetTokenService;
            _validateResetTokenService = validateResetTokenService;
        }

		public void ValidateRetrieveUserName(RetrieveUserNameModel retrieveUserNameModel) {
			
            var response = _forgotUserNameService.ForgotUserNameStatus(retrieveUserNameModel.FirstName, retrieveUserNameModel.LastName, retrieveUserNameModel.ZipCode, retrieveUserNameModel.DateOfBirth.Replace("/", ""), Convert.ToInt32(_webServicesConfiguration.UnionId));
            retrieveUserNameModel.UserNameRetrieved = response.Data.username;
		}

		public void ChangeUsername(UsernameFormModel userNameFormModel, string mdsid, string firstName, string lastName,
			string cellCode, string msrName, string campaignCode, bool isnew) {
			ExactTargetServiceManager.SendExactTargetChangeUsername(mdsid, userNameFormModel.NewUsername, firstName, lastName, cellCode, userNameFormModel.CurrentUsername, msrName, campaignCode, Sitecore.Configuration.Settings.GetSetting("ExacttargetChangeUsernameCustomerDefinition"), isnew);
		}

		public void DuplicateRegistrationExactTarget(string mdsid, string selectedUsername, string firstName, string lastName,
			string cellCode, string campaignCD, IEnumerable<string> removedEmails) {
			var emails = string.Join(",", removedEmails);
			ExactTargetServiceManager.SendExactTargetDuplicateRegistration(mdsid, selectedUsername, firstName, lastName, cellCode, campaignCD, emails, Sitecore.Configuration.Settings.GetSetting("ExacttargetDeleteRegistrationCustomerDefinition"));
		}

		[Obsolete("Use HandleLockedAccount from LockedAccountService.")]
		public bool ValidateRetrievePassword(string username, Sitecore.Data.Fields.LinkField cancelLink, Sitecore.Data.Fields.LinkField resetLink, bool isFromUserLocked = false) {
			var result = false;
			var site = Context.Site;
            var response = _createResetTokenService.CreateResetToken(username, Convert.ToInt32(_webServicesConfiguration.UnionId));
            if (response.Success && response.Data != null)
            {
				result = true;
				//todo pending sending the email
				var token = response.Data.ResetToken;

				var cookieKey = site.GetCookieKey("lang");
				var languageSelected = Sitecore.Web.WebUtil.GetCookieValue(cookieKey);
				if (string.IsNullOrEmpty(languageSelected)) {
					languageSelected = "en";
				}
				var cellcodeOne = string.Empty;
				if (isFromUserLocked) {
					cellcodeOne = Sitecore.Configuration.Settings.GetSetting("ExacttargetResetPasswordCellcodeUserLocked");
				} else {
					cellcodeOne = languageSelected.Equals("en") ? Sitecore.Configuration.Settings.GetSetting("ExacttargetResetPasswordCellcodeEn") : Sitecore.Configuration.Settings.GetSetting("ExacttargetResetPasswordCellcodeEs");
				}

				var campaignCd = Sitecore.Configuration.Settings.GetSetting("ExacttargetResetPasswordCampaignCd");

				var options = LinkManager.GetDefaultUrlOptions();
				options.AlwaysIncludeServerUrl = true;
				options.Language = Language.Current;
				options.LanguageEmbedding = LanguageEmbedding.Always;

				var pathCancel = cancelLink != null && cancelLink.TargetItem != null ? LinkManager.GetItemUrl(cancelLink.TargetItem, options) : string.Empty;
				var pathReset = resetLink != null && resetLink.TargetItem != null ? LinkManager.GetItemUrl(resetLink.TargetItem, options) : string.Empty;

				var encodedUserName = _base64Service.Encode(username);

				var resetUrl = string.Format("{0}?id={1}&s={2}", pathReset, encodedUserName, HttpUtility.UrlEncode(token));
				Sitecore.Diagnostics.Log.Info("RESET URL " + resetUrl, this);

				var cancelUrl = string.Format("{0}?id={1}", pathCancel, encodedUserName);
				Sitecore.Diagnostics.Log.Info("CANCEL URL " + cancelUrl, this);
				var customerDefinition = Sitecore.Configuration.Settings.GetSetting("ExacttargetResetPasswordCustomerDefinition");
				var firstName = response.Data.FirstName;
				//wsoutputcode1: call service and send to same email
				ExactTargetServiceManager.SendExactTargetForgetPassword(firstName, resetUrl, cancelUrl, cellcodeOne, campaignCd, username, customerDefinition);

			}
			return result;
		}

		public void ValidateResetToken(string userName, string token, ResetPasswordModel resetPasswordModel) {
            if (resetPasswordModel == null || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("ValidateResetToken invalid input parameters", "userName,token,resetPasswordModel");
            }
           
            var response = _validateResetTokenService.ValidateResetToken(userName,token, Convert.ToInt32(_webServicesConfiguration.UnionId));
			if (response) {
				resetPasswordModel.IsUsernameValidToken = true;
			} else {
				resetPasswordModel.IsUsernameValidToken = false;
			}
		}

		public void ResetPassword(string userName, string newPassword, string confirmPassword, ResetPasswordModel resetPasswordModel) {
            var resetPwdResponse= _accountManager.ResetPassword(userName, newPassword, confirmPassword, (int)Union.SEIU);
            resetPasswordModel.IsPasswordReset = resetPwdResponse;
        }

		public string GetDataCalculator(bool smoker, string age, string coverage) {
			return OracleProvider.ExecuteQueryRatesOracle(smoker, age, coverage);
		}

		public void CancelResetToken(string userName, PasswordDisavowModel passwordDisavowModel) {
            if (passwordDisavowModel== null)
            {
                throw new ArgumentException("Model password disavow Model is null", "passwordDisavowModel");
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("User name is null", "userName");
            }
            var response = _cancelResetTokenService.CancelResetToken(userName, Convert.ToInt32(_webServicesConfiguration.UnionId));

			if (response) {
				passwordDisavowModel.IsCanceled = true;
			} else {
				passwordDisavowModel.IsCanceled = false;
			}
		}
		public void UnsubscribeList(int listid, string mdsid, UnsubscribeModel unsubscribeModel) {
			unsubscribeModel.IsSucess = ExactTargetServiceManager.UnsubscribeListMail(mdsid, listid);
		}

        public bool UpdateUserStatus(string username) {
            return _accountManager.UpdateUserStatus(username, (int)UserStatus.Default, (int) Union.SEIU);
        }

    }
}