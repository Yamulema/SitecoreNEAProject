using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
	public class SetItemAction<T> : RuleAction<T> where T : RuleContext
	{
		private string _dataSource;
		public string DataSource
		{
			get
			{
				return _dataSource ?? string.Empty;
			}
			set
			{
				Assert.ArgumentNotNull(value, "value");
				_dataSource = value;
			}
		}

		public override void Apply(T ruleContext) { }
	}
}