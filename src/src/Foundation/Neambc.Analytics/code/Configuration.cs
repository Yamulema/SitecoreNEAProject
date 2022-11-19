using System.Collections.Generic;

namespace Neambc.Seiumb.Foundation.Analytics
{
    public static class Configuration
    {
        public static string DataLayerFunction => Sitecore.Configuration.Settings.GetSetting("DataLayerFunction");
    }
}