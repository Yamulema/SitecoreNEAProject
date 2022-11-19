using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Project.Web
{
    public static class Configuration
    {
        public static ID SiteSettingsId => new ID(Sitecore.Configuration.Settings.GetSetting("SiteSettingsId"));
        public static ID NeambItemId => new ID(Sitecore.Configuration.Settings.GetSetting("NeambItemId"));
        public static ID SeiumbItemId => new ID(Sitecore.Configuration.Settings.GetSetting("SeiumbItemId"));
        public static ID SitecoreItemId => new ID(Sitecore.Configuration.Settings.GetSetting("SitecoreItemId"));
        public static string NeambCssPath => Sitecore.Configuration.Settings.GetSetting("NeambCssPath");
        public static string SeiumbCssPath => Sitecore.Configuration.Settings.GetSetting("SeiumbCssPath");
        public static string SitecoreDatabase => Sitecore.Configuration.Settings.GetSetting("SitecoreDatabase", "master");
        public static string ProfileEditingPageTemplateId => Sitecore.Configuration.Settings.GetSetting("ProfileEditingPageTemplateId");
        public static string SearchParmTerm => Sitecore.Configuration.Settings.GetSetting("SearchParmTerm");
        public static string NeambToolsfilePath => Sitecore.Configuration.Settings.GetSetting("NeambToolsfilePath");
        public static string SeiumbToolsfilePath => Sitecore.Configuration.Settings.GetSetting("SeiumbToolsfilePath");
        public static ID NeambSnippetsRootId => new ID(Sitecore.Configuration.Settings.GetSetting("NeambSnippetsRootId"));
        public static ID SeiumbSnippetsRootId => new ID(Sitecore.Configuration.Settings.GetSetting("SeiumbSnippetsRootId"));
        public static string SitecoreCoreDatabase => Sitecore.Configuration.Settings.GetSetting("SitecoreCoreDatabase", "core");
        public static ID MemberVerificationPageId => new ID("{F916AE47-4165-49F1-86CA-3501B71B8214}");
        public static string SiteReleaseVersion => Sitecore.Configuration.Settings.GetSetting("Project.Release.Number", string.Empty);
        public static string MediaLinkServerUrl => Sitecore.Configuration.Settings.GetSetting("Media.MediaLinkServerUrl", string.Empty);
        public static bool AlwaysIncludeServerUrl => bool.TryParse(Sitecore.Configuration.Settings.GetSetting("Media.AlwaysIncludeServerUrl"), out var result) && result;
        public static bool EnableFlushCacheService => bool.TryParse(Sitecore.Configuration.Settings.GetSetting("EnableFlushCacheService"), out var result) && result;
        public static bool EnableCdnInvalidation => bool.TryParse(Sitecore.Configuration.Settings.GetSetting("EnableCdnInvalidation"), out var result) && result;
    }
}