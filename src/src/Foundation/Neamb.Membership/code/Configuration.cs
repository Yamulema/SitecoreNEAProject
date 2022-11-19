using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Membership
{
    public static class Configuration
    {
        public static string IcePointsTemplate => Sitecore.Configuration.Settings.GetSetting("IcePointsTemplate");
        public static string ReturnUrlArg => Sitecore.Configuration.Settings.GetSetting("ReturnUrlArg");
    }
}