using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Sitecore.DependencyInjection;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Neambc.Neamb.Feature.GeneralContent.Personalization {
	public class CompLifeMemberEligibility<T> : StringOperatorCondition<T> where T : RuleContext {
		public string Value { get; set; }

		protected override bool Execute(T ruleContext) {
			var compLifeMemberEligibilityManager =
				(ICompLifeMemberEligibilityManager)ServiceLocator.ServiceProvider.GetService(typeof(ICompLifeMemberEligibilityManager));

			return compLifeMemberEligibilityManager.GetResultEligibility(Value);
		}
	}
}