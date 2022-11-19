using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Analytics
{
    public static class Configuration
    {
        public static string DataLayerFunction => Sitecore.Configuration.Settings.GetSetting("DataLayerFunction");
        public static string FooterClass => Sitecore.Configuration.Settings.GetSetting("FooterClass");
        public static string ContactUsClass => Sitecore.Configuration.Settings.GetSetting("ContactUsClass");
        public static string NavigationClass => Sitecore.Configuration.Settings.GetSetting("NavigationClass");
        public static string ProductNavigationClass => Sitecore.Configuration.Settings.GetSetting("ProductNavigationClass");
        public static string SocialConnectClass => Sitecore.Configuration.Settings.GetSetting("SocialConnectClass");
        public static IEnumerable<string> FacebookClasses => Sitecore.Configuration.Settings.GetSetting("FacebookClasses").Split('|');
        public static IEnumerable<string> LinkedinClasses => Sitecore.Configuration.Settings.GetSetting("LinkedinClasses").Split('|');
        public static IEnumerable<string> TwitterClasses => Sitecore.Configuration.Settings.GetSetting("TwitterClasses").Split('|');
        public static string AccountProductsClass => Sitecore.Configuration.Settings.GetSetting("AccountProductsClass");
		public static string ContentCarouselSectionClass => Sitecore.Configuration.Settings.GetSetting("ContentCarouselSectionClass");
		public static IEnumerable<string> ContentCarouselCardClass => Sitecore.Configuration.Settings.GetSetting("ContentCarouselCardClass").Split('|');

	}
}