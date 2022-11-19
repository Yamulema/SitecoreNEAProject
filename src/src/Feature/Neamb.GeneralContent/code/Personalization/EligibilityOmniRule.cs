using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Feature.GeneralContent.Personalization {
	public class EligibilityOmniRule<T> : StringOperatorCondition<T> where T : RuleContext {
		public string Value { get; set; }

		protected override bool Execute(T ruleContext) {
            if (string.IsNullOrEmpty(Value))
            {
                Log.Debug("Value is empty", this);
            } else {
                var selectedItem = Context.Database.GetItem(Value);
                if (selectedItem == null) {
                    Log.Debug("Selected item is empty", this);
                } else {
                    var valueItemReferenced = selectedItem["Value"];
                    Log.Debug("Value referenced " + valueItemReferenced, this);
                    var eligibilityOmni =
                        (IEligibilityOmni) ServiceLocator.ServiceProvider.GetService(typeof(IEligibilityOmni));
                    var sessionAuthenticationManager =
                        (ISessionAuthenticationManager) ServiceLocator.ServiceProvider.GetService(typeof(ISessionAuthenticationManager));

                    var accountMembership = sessionAuthenticationManager.GetAccountMembership();
                    if ((accountMembership.Status == StatusEnum.Hot) ||
                        (accountMembership.Status == StatusEnum.WarmHot) ||
                        (accountMembership.Status == StatusEnum.WarmCold)) {
                        var result = eligibilityOmni.CheckEligibility(accountMembership.Mdsid, valueItemReferenced);
                        return result != null;
                    } else {
                        return false;
                    }
                }
            }
            return false;
        }
	}
}