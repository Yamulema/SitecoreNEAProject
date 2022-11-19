using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.DependencyInjection;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Neambc.Neamb.Feature.GeneralContent.Personalization
{
    public class IsRakutenMemberCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            var sessionAuthenticationManager =
                (ISessionAuthenticationManager)ServiceLocator.ServiceProvider.GetService(typeof(ISessionAuthenticationManager));

            var accountMembership = sessionAuthenticationManager.GetAccountMembership();
            if (accountMembership.Status == StatusEnum.Hot || accountMembership.Status == StatusEnum.WarmHot)
                return accountMembership.Profile.IsRakutenMember;

            return false;
        }
    }
}