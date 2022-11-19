using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Neambc.Seiumb.Feature.Campaign.Enum;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore;
using Sitecore.DependencyInjection;

namespace Neambc.Seiumb.Feature.Campaign.Personalization
{
    public class UserStateSeiumbRule<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string Value
        {
            get; set;
        }

        protected override bool Execute(T ruleContext)
        {
            var ret = false;
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            var userRepository =
                (IUserRepository)ServiceLocator.ServiceProvider.GetService(typeof(IUserRepository));

            var userStatus = userRepository.GetUserStatus();
            if (string.IsNullOrEmpty(Value))
            {
                Log.Debug("Value is empty", this);
            }
            else
            {
                var selectedItem = Context.Database.GetItem(Value);
                if (selectedItem == null)
                {
                    Log.Debug("Selected item is empty", this);
                }
                else
                {
                    var valueItemReferenced = selectedItem.Name;
                    Log.Debug("Value referenced " + valueItemReferenced, this);
                    switch (userStatus)
                    {
                        case UserStatusCons.WARM_HOT:
                            ret = Compare(UserStatusRuleEnum.Warm_Hot.ToString(), valueItemReferenced);
                            break;
                        case UserStatusCons.WARM_COLD:
                            ret = Compare(UserStatusRuleEnum.Warm_Cold.ToString(), valueItemReferenced);
                            break;
                        case UserStatusCons.HOT:
                            ret = Compare(UserStatusRuleEnum.Hot.ToString(), valueItemReferenced);
                            break;
                        default:
                            ret = Compare(UserStatusRuleEnum.Cold.ToString(), valueItemReferenced);
                            break;
                    }
                }
            }

            return ret;
        }
    }
}