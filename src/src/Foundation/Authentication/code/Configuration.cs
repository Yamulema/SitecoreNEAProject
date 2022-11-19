using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;

namespace Neambc.Seiumb.Foundation.Authentication
{
    public static class Configuration
    {
        public static int CachedProfileTimeOut => int.TryParse(Settings.GetSetting("CachedProfileTimeOut"), out var cachedProfileExpiration) ? cachedProfileExpiration : 0;
        public static ID LoginPageId => new ID(Settings.GetSetting("LoginPageId"));
    }
}