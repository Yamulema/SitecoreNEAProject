using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
	[Service(typeof(IRegistrationRedirection))]
	public class RegistrationRedirection: IRegistrationRedirection
	{
		public Item GetItemRedirection() {
			//The Configuration item
			var configuration = Sitecore.Context.Database.GetItem(Templates.RegistrationPage.ID);
			//The field name on the Configuration item that contains the rules
			var fieldName = ConstantsNeamb.SuccessRule;
			RuleContext ruleContext = new RuleContext();
			//Use Sitecore.Rules.RuleFactory to extract the rules from the field.
			IEnumerable<Rule<RuleContext>> rules = RuleFactory.GetRules<RuleContext>(new[] {
						configuration
					},
					fieldName)
				.Rules;

			//Evaluate a rule and if something fails, go to the next rule.
			foreach (Rule<RuleContext> rule in rules) {
				bool ruleOk = rule.Evaluate(ruleContext);
				if (!ruleOk)
					continue;

				//I'm only executing the first action. Which is always "set data source to Item".
				RuleAction<RuleContext> actionItem = rule.Actions.FirstOrDefault();
				if (actionItem == null)
					continue;

				//Convert the RuleAction to a custom implementation that has a DataSource property.
				//The DataSource property will be automatically populated by the rules engine.
				//Note that this means that the Action defined in Sitecore must have the parameter "DataSource".
				string itemId = ((SetItemAction<RuleContext>) actionItem).DataSource;
				ID sitecoreItemId;
				if (!ID.TryParse(itemId, out sitecoreItemId))
					continue;

				//Get the item and return it
				Item datasourceItem = Sitecore.Context.Database.GetItem(sitecoreItemId);
				return datasourceItem;
			}
			return null;
		}
	}
}