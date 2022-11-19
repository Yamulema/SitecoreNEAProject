using System;
using System.Linq;
using Sitecore.Configuration;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using System.Collections.Generic;
using System.Text;

namespace Neambc.Neamb.Feature.GeneralContent.Commands
{
	public class CreateRedirectRules : Command
	{
		public override void Execute(CommandContext context) {

			Sitecore.Diagnostics.Log.Info(String.Format("Starting RedirectPostScript"), this);

			var settingValue = Settings.GetSetting("RedirectSourceFileLocation");

			Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
			Item[] allItemsHome = master.SelectItems("fast:/sitecore/content/NEAMB//*");

			// Get the template to base the new item on
			TemplateItem template = master.GetItem(Templates.RedirectUrl.ID);

			// Get the place in the site tree where the new item must be inserted
			Item parentItem = master.GetItem(Templates.ParentRedirect.ID);

			using (var csv = new CachedCsvReader(new StreamReader(settingValue), true))
			{
				int fieldCount = csv.FieldCount;
				string[] headers = csv.GetFieldHeaders();
				using (new Sitecore.SecurityModel.SecurityDisabler())
				{
					ProcessExcel(allItemsHome, csv, fieldCount, headers, template, parentItem, "General");
				}
			}

			Sitecore.Diagnostics.Log.Info(String.Format("Finishing RedirectPostScript"), this);
		}

		private static void ProcessExcel(Item[] allItemsHome, CachedCsvReader csv, int fieldCount, string[] headers, TemplateItem template, Item parentItem, string type)
		{
			string lineMessage = "";
			string itemName = "";
			Item itemProcessed = null;
			string sourceUrl = "";
			string sourceTitle = "";
			List<string> linesFileError = new List<string>();
			List<string> linesFileProcessed = new List<string>();
			var settingValueFileError = Settings.GetSetting("RedirectErrorFilelocation");
			var settingValueFileProcessed = Settings.GetSetting("RedirectProcessedFilelocation");


			while (csv.ReadNextRecord())
			{
				try
				{

					for (int i = 0; i < fieldCount; i++)
					{
						switch (headers[i].Trim())
						{
							case "Requested URL":
								sourceUrl = CommandHelper.GetFullPathWithoutExtension(csv[i], true);
								break;
							case "Redirect to Item":
								sourceTitle = csv[i];
								break;
						}
					}

					itemProcessed = null;

					//Convert to item name without special characters
					//itemName = Utility.GetItemName(sourceTitle);
					itemName = sourceUrl.Replace(".", "_").Replace("/", "_").Substring(1); ;

					var allItemsMatchName = allItemsHome.Where(item => item.Paths.Path.ToLower().Equals(sourceTitle.ToLower()));

					if (allItemsMatchName.Count() > 1)
					{
						lineMessage = String.Format("More than one item,{0}", itemName);
						linesFileError.Add(lineMessage);

						Sitecore.Diagnostics.Log.Info(lineMessage, "RedirectProcess");
					}
					else
					{
						itemProcessed = allItemsMatchName.FirstOrDefault();
						if (itemProcessed != null)
						{
							Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");

							Item[] allItemsRedirect = master.SelectItems("fast:/sitecore/system/Modules/Redirect Module/Redirects//*");

							var itemFound = allItemsRedirect.FirstOrDefault(item => item.Fields[Templates.RedirectUrl.Fields.RequestedUrl].Value == sourceUrl);
							if (itemFound == null)
							{
								//Create new item
								Item newItem = parentItem.Add(itemName, template);
								newItem.Editing.BeginEdit();
								newItem.Fields[Templates.RedirectUrl.Fields.RedirectItem].Value = itemProcessed.ID.ToString();
								newItem.Fields[Templates.RedirectUrl.Fields.RequestedUrl].Value = sourceUrl;
								newItem.Editing.EndEdit();
								lineMessage = String.Format("Item processed {0}", sourceUrl);
								linesFileProcessed.Add(lineMessage);
							} else {
								lineMessage = String.Format("redirect rule already present:,{0}", sourceUrl);
								linesFileError.Add(lineMessage);

								Sitecore.Diagnostics.Log.Info(lineMessage, "RedirectProcess");
							}
						}
						else
						{
							lineMessage = String.Format("Source item not found with name,{0}", sourceUrl);
							linesFileError.Add(lineMessage);

							Sitecore.Diagnostics.Log.Info(lineMessage, "RedirectProcess");
						}
					}
				}

				catch (Exception ex)
				{
					Sitecore.Diagnostics.Log.Info(String.Format("Error in RedirectPostScript method! {0}", ex.Message), "RedirectProcess");
				}

			}

			System.IO.File.WriteAllLines(settingValueFileError, linesFileError.ToArray());
			System.IO.File.WriteAllLines(settingValueFileProcessed, linesFileProcessed.ToArray());
		}
	}
}