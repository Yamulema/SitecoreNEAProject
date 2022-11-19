using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.DependencyInjection;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System.Collections.Generic;

namespace Neambc.Seiumb.Feature.Account.Personalization
{
    public class IsRakutenMemberConditionSeiumb<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public virtual ISeiumbProfileManager SeiumbProfileManager => (ISeiumbProfileManager)ServiceLocator.ServiceProvider.GetService(typeof(ISeiumbProfileManager));

        protected override bool Execute(T ruleContext) {
            var profile = SeiumbProfileManager.GetProfile();
            return new List<string>(){ UserStatusCons.HOT , UserStatusCons.HOT }.Contains(profile.Status) && SeiumbProfileManager.IsRakutenMember();
        }
    }
}