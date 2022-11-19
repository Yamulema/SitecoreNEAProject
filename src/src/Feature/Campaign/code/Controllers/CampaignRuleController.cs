using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Campaign.Controllers
{
    public class CampaignRuleController<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string Value { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            var campaignCode = HttpContext.Current.Session[ConstantsSeiumb.CampaignCode] != null ? HttpContext.Current.Session[ConstantsSeiumb.CampaignCode].ToString():null ;
            if (!string.IsNullOrEmpty(campaignCode))
            {
                return Compare(campaignCode, Value);
            }
            return false;            
        }
    }
}