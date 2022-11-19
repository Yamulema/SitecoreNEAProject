using System;
using System.IO;
using System.Text;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Shell.Framework.Commands;

namespace Neambc.Neamb.Feature.GeneralContent.Commands
{
	public class UpdateLastPublishDate : Command {
		public override void Execute(CommandContext context) {
			var csv = new StringBuilder();
			Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
			Sitecore.Data.Database web = Sitecore.Data.Database.GetDatabase("web");
			Item[] children = master.SelectItems("fast:/sitecore/content/NEAMB//*");
			var globalConfigurationManager =
				(IGlobalConfigurationManager)ServiceLocator.ServiceProvider.GetService(typeof(IGlobalConfigurationManager));

			string filePath = globalConfigurationManager.PathFileProcessUpdatePublishDate;
			var executeUpdate = globalConfigurationManager.ExecuteProcessUpdatePublishDate;

			foreach (var item in children)
			{
				using (new Sitecore.SecurityModel.SecurityDisabler())
				{
					using (new EditContext(item, false, false))
					{
						var lastPublishDate = item[Templates.StatisticsCustom.Fields.LastPublishDate];
						if (!String.IsNullOrEmpty(lastPublishDate) && !lastPublishDate.Contains("Z"))
						{
							var utcDt = $"{lastPublishDate}Z";
							if (executeUpdate) {
								item[Templates.StatisticsCustom.Fields.LastPublishDate] = utcDt;
							}
							var newLine = $"master,{item.ID},{item.Name},{lastPublishDate},{utcDt}";
							csv.AppendLine(newLine);
						}
					}
				}
				
			}

			//web items
			Item[] childrenWeb = web.SelectItems("fast:/sitecore/content/NEAMB//*");
			foreach (var item in childrenWeb)
			{
				using (new Sitecore.SecurityModel.SecurityDisabler())
				{
					using (new EditContext(item, false, false))
					{
						var lastPublishDate = item[Templates.StatisticsCustom.Fields.LastPublishDate];
						if (!String.IsNullOrEmpty(lastPublishDate) && !lastPublishDate.Contains("Z"))
						{
							var utcDt = $"{lastPublishDate}Z";
							if (executeUpdate) {
								item[Templates.StatisticsCustom.Fields.LastPublishDate] = utcDt;
							}
							var newLine = $"web,{item.ID},{item.Name},{lastPublishDate},{utcDt}";
							csv.AppendLine(newLine);
						}
					}
				}
			}
			File.WriteAllText(filePath, csv.ToString());
		}
	}
}