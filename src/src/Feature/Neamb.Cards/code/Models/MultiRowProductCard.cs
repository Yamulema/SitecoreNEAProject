using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class MultiRowProductCard : PageCard
    {
        public bool IsComingSoon { get; set; }
        public Tuple<string, string> TermsAndConditionsCta { get; set; }
        public bool HasTermsAndConditions { get; set; }
    }
}