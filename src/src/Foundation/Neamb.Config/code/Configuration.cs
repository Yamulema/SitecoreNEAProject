using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Configuration
{
    public static class Configuration
    {
        public static string ReminderKey => Sitecore.Configuration.Settings.GetSetting("ReminderKey");
        public static string ProfileKey => Sitecore.Configuration.Settings.GetSetting("ProfileKey");
        public static string SubscriptionKey => Sitecore.Configuration.Settings.GetSetting("SubscriptionKey");
        public static string ComplifeKey => Sitecore.Configuration.Settings.GetSetting("ComplifeKey");
        public static ID ProfilePasswordId => new ID(Sitecore.Configuration.Settings.GetSetting("ProfilePasswordId"));
        public static ID SettingSubscriptionId => new ID(Sitecore.Configuration.Settings.GetSetting("SettingSubscriptionId"));
        public static ID CompLifeId => new ID(Sitecore.Configuration.Settings.GetSetting("CompLifeId"));
        public static bool MigrateVisitedPages => bool.TryParse(Sitecore.Configuration.Settings.GetSetting("MigrateVisitedPages"), out var result) && result;
    }
}