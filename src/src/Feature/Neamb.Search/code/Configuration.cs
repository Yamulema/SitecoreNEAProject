using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Search
{
    public static class Configuration
    {
        public static ID StartSearchId => new ID(Sitecore.Configuration.Settings.GetSetting("StartSearchId"));
        public static int MaxCardCount => int.Parse(Sitecore.Configuration.Settings.GetSetting("MaxCardCount"));
        public static bool CacheEnabled => bool.Parse(Sitecore.Configuration.Settings.GetSetting("CacheEnabled"));
        public static string SearchParmTerm => Sitecore.Configuration.Settings.GetSetting("SearchParmTerm");
        public static string SearchParmTake => Sitecore.Configuration.Settings.GetSetting("SearchParmTake");
        public static string SearchParmFilterResource => Sitecore.Configuration.Settings.GetSetting("SearchParmFilterResource");
        public static string SearchParmFilterOffer => Sitecore.Configuration.Settings.GetSetting("SearchParmFilterOffer");
        public static string SearchParmFilterSolution => Sitecore.Configuration.Settings.GetSetting("SearchParmFilterSolution");
        public static string FilterResource => Sitecore.Configuration.Settings.GetSetting("FilterResource");
        public static string FilterOffer => Sitecore.Configuration.Settings.GetSetting("FilterOffer");
        public static string FilterSolution => Sitecore.Configuration.Settings.GetSetting("FilterSolution");
        public static int DefaultSearchTake => int.Parse(Sitecore.Configuration.Settings.GetSetting("DefaultSearchTake"));
        public static ID SiteSettingsId => new ID(Sitecore.Configuration.Settings.GetSetting("SiteSettingsId"));
        public static int CacheDuration => int.Parse(Sitecore.Configuration.Settings.GetSetting("CacheDuration"));
    }
}