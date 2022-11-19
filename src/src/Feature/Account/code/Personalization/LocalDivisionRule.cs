using System.Linq;
using Neambc.Seiumb.Feature.Account.Manager;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Managers;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;

namespace Neambc.Seiumb.Feature.Account.Personalization
{
    public class LocalDivisionRule<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string Value
        {
            get; set;
        }

        protected override bool Execute(T ruleContext) {
            bool resultRule = false;
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            var userRepository =
                (IUserRepository)ServiceLocator.ServiceProvider.GetService(typeof(IUserRepository));
            var localDivisionManager =
                (ILocalDivisionManager)ServiceLocator.ServiceProvider.GetService(typeof(ILocalDivisionManager));

            var userStatus = userRepository.GetUserStatus();
            if (!string.IsNullOrEmpty(Value) && (userStatus == UserStatusCons.HOT ||
                userStatus == UserStatusCons.WARM_COLD ||
                userStatus == UserStatusCons.WARM_HOT)) {
                var localGlobalSitecore=localDivisionManager.GetLocalCodesGlobal();
                var selectedDivisionRuleItem = Context.Database.GetItem(Value);

                resultRule=localDivisionManager.ExistLocalCodeUser(localGlobalSitecore, selectedDivisionRuleItem.Name);
            }
            return resultRule;
        }
    }
}