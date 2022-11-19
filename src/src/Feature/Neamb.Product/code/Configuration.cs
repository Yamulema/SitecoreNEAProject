using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Product
{
    public static class Configuration
    {
        //public static string ComplimentaryLifeProductCode => Sitecore.Configuration.Settings.GetSetting("ComplimentaryLifeProductCode");
        //public static string IntroLifeProductCode => Sitecore.Configuration.Settings.GetSetting("IntroLifeProductCode");
        public static ID SiteSettingsId => new ID(Sitecore.Configuration.Settings.GetSetting("SiteSettingsId"));
    }
}