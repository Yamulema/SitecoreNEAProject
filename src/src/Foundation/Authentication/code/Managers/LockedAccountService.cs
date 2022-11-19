using System;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.CreateResetToken;
using Neambc.Neamb.Foundation.MBCData.Model.RestResponse;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Registration.Interfaces;
using Neambc.Seiumb.Foundation.WebServices;
using Neambc.Seiumb.Foundation.WebServices.Managers;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Web;
using Convert = System.Convert;

namespace Neambc.Seiumb.Foundation.Authentication.Managers
{
    [Service(typeof(ILockedAccountService))]
    public class LockedAccountService : ILockedAccountService
    {
        private readonly IBase64Service _base64Service;
        private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly ICreateResetTokenService _createResetTokenService;

        public LockedAccountService(IBase64Service base64Service, IWebServicesConfiguration webServicesConfiguration, ICreateResetTokenService createResetTokenService)
        {
            _base64Service = base64Service;
			_webServicesConfiguration = webServicesConfiguration;
            _createResetTokenService = createResetTokenService;
        }

        public LoginErrors HandleLockedAccount(string username, out bool isUsernameValid, bool isFromUserLocked = false)
        {
            var loginPage = Context.Database.GetItem(Configuration.LoginPageId);
            return HandleLockedAccount(username, loginPage.Fields[Templates.LoginForm.Fields.CancelLink], loginPage.Fields[Templates.LoginForm.Fields.ResetLink], out isUsernameValid,
                isFromUserLocked);
        }

        public LoginErrors HandleLockedAccount(string username, LinkField cancelLink, LinkField resetLink, out bool isUsernameValid, bool isFromUserLocked = false)
        {
            try
            {
                var response = _createResetTokenService.CreateResetToken(username, Convert.ToInt32(_webServicesConfiguration.UnionId));
                if (response.Error!=null && response.Error.Code == (int)RestRestResponseErrorEnum.UsernameNoFound)
                {
                    isUsernameValid = false;
                    return LoginErrors.USERNAME_DOES_NOT_EXIST;
                }
                if (response.Success && response.Data!=null && response.Data.NewToken)
                {
                    isUsernameValid = true;
                    SendExactTargetForgetPassword(
                        username,
                        cancelLink,
                        resetLink,
                        response,
                        isFromUserLocked);
                    return isFromUserLocked
                        ? LoginErrors.ACCOUNT_LOCKED_SENT_MAIL
                        : LoginErrors.NONE;
                }
                else
                {
                    isUsernameValid = true;
                    return LoginErrors.ACCOUNT_LOCKED_NOT_SENT_MAIL;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
                isUsernameValid = false;
                return LoginErrors.ERROR;
            }
        }
        private void SendExactTargetForgetPassword(
            string username,
            LinkField cancelLink,
            LinkField resetLink,
			CreateResetTokenResponse requestResetTokenObject,
            bool isFromUserLocked
        ) {
            if (requestResetTokenObject.Success && requestResetTokenObject.Data!=null && requestResetTokenObject.Data.NewToken) {
                var site = Context.Site;
                //todo pending sending the email

                var token = requestResetTokenObject.Data.ResetToken;
                var cookieKey = site.GetCookieKey("lang");
                var languageSelected = WebUtil.GetCookieValue(cookieKey);
                if (string.IsNullOrEmpty(languageSelected)) {
                    languageSelected = "en";
                }
                var cellcodeOne = string.Empty;
                if (isFromUserLocked) {
                    cellcodeOne = Settings.GetSetting("ExacttargetResetPasswordCellcodeUserLocked");
                } else {
                    cellcodeOne = languageSelected.Equals("en")
                        ? Settings.GetSetting("ExacttargetResetPasswordCellcodeEn")
                        : Settings.GetSetting("ExacttargetResetPasswordCellcodeEs");
                }

                var campaignCd = Settings.GetSetting("ExacttargetResetPasswordCampaignCd");
                var options = new ItemUrlBuilderOptions
                {
                    AlwaysIncludeServerUrl = true,  //not include the server
                    Language = Language.Current,
                    LanguageEmbedding = LanguageEmbedding.Always
                };

                var pathCancel = cancelLink != null && cancelLink.TargetItem != null ? LinkManager.GetItemUrl(cancelLink.TargetItem, options) : string.Empty;
                var pathReset = resetLink != null && resetLink.TargetItem != null ? LinkManager.GetItemUrl(resetLink.TargetItem, options) : string.Empty;

                var encodedUserName = _base64Service.Encode(username);

                var resetUrl = string.Format("{0}?id={1}&s={2}", pathReset, encodedUserName, HttpUtility.UrlEncode(token));
                Log.Info("RESET URL " + resetUrl, this);

                var cancelUrl = string.Format("{0}?id={1}", pathCancel, encodedUserName);
                Log.Info("CANCEL URL " + cancelUrl, this);
                var customerDefinition = Settings.GetSetting("ExacttargetResetPasswordCustomerDefinition");
                var firstName = requestResetTokenObject.Data.FirstName;
                //wsoutputcode1: call service and send to same email
                ExactTargetServiceManager.SendExactTargetForgetPassword(firstName,
                    resetUrl,
                    cancelUrl,
                    cellcodeOne,
                    campaignCd,
                    username,
                    customerDefinition);
            }
        }
    }
}