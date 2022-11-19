using System;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Neambc.Neamb.Feature.GeneralContent.Personalization {
	/// <summary>
	/// This criteria must allow content authors to compare the previous page requested to verify with a specific template id
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class OriginPageTemplateRule<T> : StringOperatorCondition<T> where T : RuleContext {

		public string Value {
			get; set;
		}

		protected override bool Execute(T ruleContext) {
			Assert.ArgumentNotNull(ruleContext, "ruleContext");

			var sessionManager =
				(ISessionManager) ServiceLocator.ServiceProvider.GetService(typeof(ISessionManager));

			var itemIdRequestRegistration = sessionManager.RetrieveFromSession<Guid>(ConstantsNeamb.ItemIdRequestRegistration);
			if (itemIdRequestRegistration != Guid.Empty) {
				var itemRequestReg = Sitecore.Context.Database.GetItem(new ID(itemIdRequestRegistration));
				var selectedTemplateItem = Context.Database.GetItem(Value);
				if (itemRequestReg.TemplateID == selectedTemplateItem.ID) {
					return true;
				}
			}
			return false;
		}
	}
}