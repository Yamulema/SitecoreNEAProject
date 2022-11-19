using System.Collections.Generic;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Neambc.Neamb.Feature.GeneralContent.Personalization {
	/// <summary>
	/// This criteria must allow content authors to compare against the value on the "New member" attribute on an identified user profile
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class NewMemberRule<T> : StringOperatorCondition<T> where T : RuleContext {
		private readonly List<string> Indicators = new List<string> {"1","2","3","4","5"};

		public string Value {
			get; set;
		}

		protected override bool Execute(T ruleContext) {
			var ret = false;
			Assert.ArgumentNotNull(ruleContext, "ruleContext");
			var sessionAuthenticationManager =
				(ISessionAuthenticationManager)ServiceLocator.ServiceProvider.GetService(typeof(ISessionAuthenticationManager));

			var accountMembership = sessionAuthenticationManager.GetAccountMembership();
			if ((accountMembership.Status == StatusEnum.Hot) || 
			    (accountMembership.Status == StatusEnum.WarmHot) || 
			    (accountMembership.Status == StatusEnum.WarmCold)) {

				if (string.IsNullOrEmpty(Value)) {
					Log.Debug("Value is empty", this);
				} else {
					var selectedItem = Context.Database.GetItem(Value);
					if (selectedItem == null) {
						Log.Debug("Selected item is empty", this);
					} else {
						var valueItemReferenced = selectedItem["Value"];
						Log.Debug("Value referenced " + valueItemReferenced, this);
						switch (valueItemReferenced) {
							case "New Member": {
								ret = Indicators.Contains(accountMembership.Profile.Newmembersegmentindicator);
								break;
							}
							case "Not New Member": {
								ret = !Indicators.Contains(accountMembership.Profile.Newmembersegmentindicator);
								break;
							}
						}
					}
				}
			}
			return ret;
		}
	}
}