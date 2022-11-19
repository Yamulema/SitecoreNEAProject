using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.GeneralContent
{
    public static class Configuration
    {
        public static ID SiteSettingsId => new ID(Sitecore.Configuration.Settings.GetSetting("SiteSettingsId"));
        public static string ContactUsUserEmailCustomerKey => Sitecore.Configuration.Settings.GetSetting("ContactUsUserEmailCustomerKey");
        public static string ContactUsUserEmailCellCode => Sitecore.Configuration.Settings.GetSetting("ContactUsUserEmailCellCode");
        public static string ContactUsUserEmailCampaignCd => Sitecore.Configuration.Settings.GetSetting("ContactUsUserEmailCampaignCd");
        public static string ContactUsAdminEmailCellCode => Sitecore.Configuration.Settings.GetSetting("ContactUsAdminEmailCellCode");
        public static string ContactUsAdminEmailAddress => Sitecore.Configuration.Settings.GetSetting("ContactUsAdminEmailAddress");
        public static string ProductCodesSnippet2 => Sitecore.Configuration.Settings.GetSetting("ProductCodesSnippet2");
        public static string ProductCodesSnippet3 => Sitecore.Configuration.Settings.GetSetting("ProductCodesSnippet3");
        public static string PlatformVideo => Sitecore.Configuration.Settings.GetSetting("PlatformVideo");
        public static string LoginPagePath => Sitecore.Configuration.Settings.GetSetting("LoginPagePath");
        public static string[] PlanOptions => Sitecore.Configuration.Settings.GetSetting("PlanOptions").Split(',').Select(x => x.Trim()).ToArray();
        public static int[] AgeOptions => Sitecore.Configuration.Settings.GetSetting("AgeOptions").Split(',').Select(x => int.TryParse(x.Trim(), out var ageOptions) ? ageOptions : 0).ToArray();
        public static int RateQuotationCacheDuration => int.Parse(Sitecore.Configuration.Settings.GetSetting("RateQuotationCacheDuration"));
        public static string NoDataMessage => Sitecore.Configuration.Settings.GetSetting("NoDataMessage");
        public static string InvalidAgeMessage => Sitecore.Configuration.Settings.GetSetting("InvalidAgeMessage");
        public static string PlanInfoPath => Sitecore.Configuration.Settings.GetSetting("PlanInfoPath");
        public static int[] ValidAgeOptions => Sitecore.Configuration.Settings.GetSetting("ValidAgeOptions").Split(',').Select(x => int.TryParse(x.Trim(), out var ageOptions) ? ageOptions : 0).ToArray();
        public static string MdsidToken => Sitecore.Configuration.Settings.GetSetting("MdsidToken");
        public static string InvalidStateMessage => Sitecore.Configuration.Settings.GetSetting("InvalidStateMessage");
        public static string FloridaStateCode => Sitecore.Configuration.Settings.GetSetting("FloridaStateCode");
        public static string MissouriStateCode => Sitecore.Configuration.Settings.GetSetting("MissouriStateCode");
        public static string FirstNameToken => Sitecore.Configuration.Settings.GetSetting("FirstNameToken");
        public static string LastNameToken => Sitecore.Configuration.Settings.GetSetting("LastNameToken");
	    public static string CaptchaKey => Sitecore.Configuration.Settings.GetSetting("CaptchaKey");
        public static string ReturnUrlArg => Sitecore.Configuration.Settings.GetSetting("ReturnUrlArg");
        public static bool RemoveGaEvents => bool.TryParse(Sitecore.Configuration.Settings.GetSetting("RemoveGaEvents"), out var result) && result;
        public static string[] RemoveGaItems => Sitecore.Configuration.Settings.GetSetting("RemoveGaItems").Split('|');
        public static string PublishingUser => Sitecore.Configuration.Settings.GetSetting("PublishingUser");
        public static string SubscribeResult => Sitecore.Configuration.Settings.GetSetting("SubscribeResult");
        public static string HostForVisitedPage => Sitecore.Configuration.Settings.GetSetting("HostForVisitedPage");
    }
}