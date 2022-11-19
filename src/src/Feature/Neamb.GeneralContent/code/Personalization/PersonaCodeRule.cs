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
	/// This criteria must allow content authors to compare against the value on the "Membership Category Code/Persona"attribute on an identified user profile
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PersonaCodeRule<T> : StringOperatorCondition<T> where T : RuleContext {
		public string Value {
			get; set;
		}

		protected override bool Execute(T ruleContext) {
			Assert.ArgumentNotNull(ruleContext, "ruleContext");
			var sessionAuthenticationManager =
				(ISessionAuthenticationManager)ServiceLocator.ServiceProvider.GetService(typeof(ISessionAuthenticationManager));

			var accountMembership = sessionAuthenticationManager.GetAccountMembership();
			if (accountMembership.Status == StatusEnum.Hot || accountMembership.Status == StatusEnum.WarmHot ||
				accountMembership.Status == StatusEnum.WarmCold) {
				if (string.IsNullOrEmpty(Value)) {
					Sitecore.Diagnostics.Log.Debug("Value is empty", this);
					return false;
				}

				var selectedItem = Context.Database.GetItem(Value);
				if (selectedItem == null) {
					Sitecore.Diagnostics.Log.Debug("Selected item is empty", this);

					return false;
				}

				var valueItemReferenced = selectedItem.Name;
				Sitecore.Diagnostics.Log.Debug("Value referenced " + valueItemReferenced, this);
				if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Chelsea.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "S");
				} else if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Jessica.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "J");
				} else if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Amy.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "A");
				} else if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Karen.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "T");
				} else if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Paul.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "P");
				} else if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Owen.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "H");
				} else if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Rose.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "N");
				} else if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Betty.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "R");
				} else if (valueItemReferenced.Equals(PersonaCodeRuleEnum.Unclassified.ToString())) {
					return Compare(accountMembership.Profile.MembershipCategoryCode, "U");
				} else {
					return false;
				}
			} else {
				return false;
			}
		}
	}
}