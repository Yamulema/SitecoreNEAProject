using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Neambc.Neamb.Feature.GeneralContent.Personalization {
	/// <summary>
	/// This criteria must allow content authors to compare against the value on the "User Authentication State" attribute on the user profile.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class UserStateRule<T> : StringOperatorCondition<T> where T : RuleContext {
		public string Value {
			get; set;
		}

		protected override bool Execute(T ruleContext) {
			var ret = false;
			Assert.ArgumentNotNull(ruleContext, "ruleContext");
			var sessionAuthenticationManager =
				(ISessionAuthenticationManager)ServiceLocator.ServiceProvider.GetService(typeof(ISessionAuthenticationManager));

			var accountMembership = sessionAuthenticationManager.GetAccountMembership();
			if (string.IsNullOrEmpty(Value)) {
				Log.Debug("Value is empty", this);
			} else {
				var selectedItem = Context.Database.GetItem(Value);
				if (selectedItem == null) {
					Log.Debug("Selected item is empty", this);
				} else {
					var valueItemReferenced = selectedItem[Templates.CategoryItem.Fields.Value];
                    Log.Debug("Value referenced " + valueItemReferenced, this);
					switch (accountMembership.Status) {
						case StatusEnum.Cold:
						case StatusEnum.Unknown:
							ret = Compare(UserStateRuleEnum.Cold.ToString(), valueItemReferenced);
							break;
						case StatusEnum.WarmHot:
                            ret = Compare(UserStateRuleEnum.Warm_Hot.ToString(), valueItemReferenced);
                            break;
                        case StatusEnum.WarmCold:
                            ret = Compare(UserStateRuleEnum.Warm_Cold.ToString(), valueItemReferenced);
							break;
						case StatusEnum.Hot:
							ret = Compare(UserStateRuleEnum.Hot.ToString(), valueItemReferenced);
							break;
					}
				}
			}

			return ret;
		}
	}
}