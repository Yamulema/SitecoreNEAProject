using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class Account
    {
        public string Event { get; set; }
        public string AccountSection { get; set; }
        public string AccountAction { get; set; }
        public string CtaText { get; set; }
    }
}