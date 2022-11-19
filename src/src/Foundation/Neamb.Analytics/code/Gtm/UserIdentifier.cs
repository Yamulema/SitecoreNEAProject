using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class UserIdentifier
    {
        public string UserMdsid { get; set; }
        public string UserPersonaCode { get; set; }
        public string SeaCode { get; set; }
        public string LeaCode { get; set; }
        public string SeaName { get; set; }
        public string LeaName { get; set; }
    }
}