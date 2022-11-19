using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.MBCData
{
    public static class Configuration {
        public static readonly ID NewslettersId;
        public static readonly ID NewsletterCtaId;
        public static readonly string DefaultCloudFrontDistributionId;
        public static readonly bool EnableDataExtensionRequest;
        static Configuration()
        {
            NewslettersId = new ID(Sitecore.Configuration.Settings.GetSetting("NewslettersId"));
            NewsletterCtaId = new ID(Sitecore.Configuration.Settings.GetSetting("NewsletterCtaId"));
            DefaultCloudFrontDistributionId = Sitecore.Configuration.Settings.GetSetting("DefaultCloudFrontDistributionId");
            EnableDataExtensionRequest = bool.TryParse(Sitecore.Configuration.Settings.GetSetting("EnableDataExtensionRequest"), out var result) && result;
        }
    }
}