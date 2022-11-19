using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Navigation
{
    public struct Templates
    {
        public struct NavigationRoot
        {
            public static readonly ID ID = new ID("{20BD3AE1-E650-4D73-A4E7-928588E635F0}");
        }

        public struct Navigable
        {
            public static readonly ID ID = new ID("{40E15E44-1FC9-4A5D-9C09-2EAB6F214A80}");

            public struct Fields
            {
                public static readonly ID ShortTitle = new ID("{A137093C-DE37-4C2D-A519-2A5D29B3147B}");
                public static readonly ID ShowInNavigationMenu = new ID("{33D3AD01-DDD6-411E-AED4-DA1E42FCCA24}");
            }
        }       
    }
}