using System.Globalization;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;


namespace Neambc.Neamb.Feature.GeneralContent.Personalization {
	/// <summary>
	/// This criteria must allow content authors to compare against the value on the "New member" attribute on an identified user profile
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class QueryStringConditionRule<T> : StringOperatorCondition<T> where T : RuleContext {
		/// <summary>
		/// Gets or sets query string name.
		/// </summary>
		public string QueryStringName {
			get; set;
		}

		/// <summary>
		/// Gets or sets query string name.
		/// </summary>
		public string QueryStringValue {
			get; set;
		}

		/// <summary>
		/// Main execute method.
		/// </summary>
		/// <param name="ruleContext">Rule context.</param>
		/// <returns>True or false.</returns>
		protected override bool Execute(T ruleContext) {
			var returnValue = false;
			var foundCaseInsensitiveMatch = false;
			var foundContains = false;
			var foundStartsWith = false;
			var foundEndsWith = false;

			Assert.ArgumentNotNull(ruleContext, "ruleContext");

			var myQueryStringName = QueryStringName ?? string.Empty;
			var myQueryStringValue = QueryStringValue ?? string.Empty;

			if (!string.IsNullOrWhiteSpace(myQueryStringName)) {
				if (HttpContext.Current != null) {
					// Populated with QueryString coming into current Page
					var incomingQueryStringValue = HttpContext.Current.Request.QueryString[myQueryStringName] ?? string.Empty;

					if (incomingQueryStringValue == myQueryStringValue) {
						// Indicates that QueryString coming into Page is equal to QueryString selected by Content Author

						return true;
					}

					if (incomingQueryStringValue.ToLower(CultureInfo.InvariantCulture) == myQueryStringValue.ToLower(CultureInfo.InvariantCulture)) {
						// Indicates that QueryString coming into Page has case-insensitive match to QueryString selected by Content Author
						foundCaseInsensitiveMatch = true;

						// Check other "Found" variables that are not inherently true
						if (incomingQueryStringValue.Contains(myQueryStringValue)) {
							foundContains = true;
						}

						if (incomingQueryStringValue.StartsWith(myQueryStringValue, System.StringComparison.InvariantCultureIgnoreCase)) {
							foundStartsWith = true;
						}

						if (incomingQueryStringValue.EndsWith(myQueryStringValue, System.StringComparison.InvariantCultureIgnoreCase)) {
							foundEndsWith = true;
						}
					} else if (incomingQueryStringValue.Contains(myQueryStringValue)) {
						// Indicates that QueryString coming into Page contains QueryString selected by Content Author
						foundContains = true;

						// Check other "Found" variables that are not inherently true
						if (incomingQueryStringValue.StartsWith(myQueryStringValue, System.StringComparison.InvariantCultureIgnoreCase)) {
							foundStartsWith = true;
						}

						if (incomingQueryStringValue.EndsWith(myQueryStringValue, System.StringComparison.InvariantCultureIgnoreCase)) {
							foundEndsWith = true;
						}
					}
				}
			}

			switch (GetOperator()) {
				case StringConditionOperator.NotEqual:
					returnValue = true;
					break;
				case StringConditionOperator.CaseInsensitivelyEquals:
					returnValue = foundCaseInsensitiveMatch;
					break;
				case StringConditionOperator.NotCaseInsensitivelyEquals:
					returnValue = !foundCaseInsensitiveMatch;
					break;
				case StringConditionOperator.Contains:
					returnValue = foundContains;
					break;
				case StringConditionOperator.StartsWith:
					returnValue = foundStartsWith;
					break;
				case StringConditionOperator.EndsWith:
					returnValue = foundEndsWith;
					break;
			}

			return returnValue;
		}
	}
}