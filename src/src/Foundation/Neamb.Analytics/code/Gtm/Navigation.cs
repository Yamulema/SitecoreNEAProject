using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class Navigation
    {
        public string Event { get; set; }
        public string NavType { get; set; }
        public string NavText { get; set; }
        public Navigation() {
            Event = "navigation";
        }
    }
}