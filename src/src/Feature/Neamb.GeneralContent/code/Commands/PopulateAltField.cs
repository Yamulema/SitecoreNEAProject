using System;
using System.Linq;
using Sitecore.Configuration;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using System.Collections.Generic;
using System.Text;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.GeneralContent.Commands
{
	public class PopulateAltField : Command
	{
		public override void Execute(CommandContext context)
		{
			Sitecore.Diagnostics.Log.Info(String.Format("Starting AltTagPostScript"), this);

			var settingValue = Settings.GetSetting("AltImageSourceFileLocation");

			Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
			Item[] allItemsHome = master.SelectItems("fast:/sitecore/media library//*");

			using (var csv = new CachedCsvReader(new StreamReader(settingValue), true))
			{
				int fieldCount = csv.FieldCount;
				string[] headers = csv.GetFieldHeaders();
				using (new Sitecore.SecurityModel.SecurityDisabler())
				{
					ProcessExcel(allItemsHome, csv, fieldCount, headers, "General");
				}
			}

			Sitecore.Diagnostics.Log.Info(String.Format("Finishing AltTagPostScript"), this);
		}

		private static void ProcessExcel(
			Item[] allItemsHome,
			CachedCsvReader csv,
			int fieldCount,
			string[] headers,
			string type
		)
		{
			Item itemProcessed = null;
			string lineMessage = "";
			string sourceUrl = "";
			string sourceAlt = "";
			List<string> linesFileError = new List<string>();
			List<string> linesFileProcessed = new List<string>();
			var settingValueFileError = Settings.GetSetting("AltImageErrorFilelocation");
			var settingValueFileProcessed = Settings.GetSetting("AltImageProcessedFilelocation");


			while (csv.ReadNextRecord())
			{
				try
				{
					for (int i = 0; i < fieldCount; i++)
					{
						switch (headers[i].Trim())
						{
							case "Image URL":
								sourceUrl = csv[i];
								break;
							case "Alt Text":
								sourceAlt = csv[i];
								break;
						}
					}

					var uri = new Uri(sourceUrl);
					var itemPath = uri.AbsoluteUri.Replace(uri.Query, string.Empty).Replace("https://www.neamb.com/-/media", "/sitecore/media library");
					itemPath = CommandHelper.GetFullPathWithoutExtension(itemPath, false);


					var allItemsMatchName = allItemsHome.Where(item => item.Paths.Path.ToLower().Equals(itemPath.ToLower()));

					if (allItemsMatchName.Count() > 1)
					{
						lineMessage = String.Format("More than one item,{0}", sourceUrl);
						linesFileError.Add(lineMessage);

						Sitecore.Diagnostics.Log.Info(lineMessage, "PopulateAltProcess");
					}
					else
					{
						itemProcessed = allItemsMatchName.FirstOrDefault();
						if (itemProcessed != null)
						{
							Database master = Sitecore.Configuration.Factory.GetDatabase("master");
							var item = master.GetItem(itemProcessed.ID);
							item.Editing.BeginEdit();
							item["alt"] = sourceAlt;
							item.Editing.EndEdit();
							item.Editing.AcceptChanges();
							lineMessage = String.Format("Alt changed for item with url,{0}", sourceUrl);
							linesFileProcessed.Add(lineMessage);

							Sitecore.Diagnostics.Log.Info(lineMessage, "PopulateAltProcess");
						}
						else
						{
							lineMessage = String.Format("Source item not found with name,{0}", sourceUrl);
							linesFileError.Add(lineMessage);

							Sitecore.Diagnostics.Log.Info(lineMessage, "PopulateAltProcess");
						}
					}
				}
				catch (Exception ex)
				{
					Sitecore.Diagnostics.Log.Info(String.Format("Error in PopulateAltField method! {0}", ex.Message), "PopulateAltProcess");
				}
			}

			System.IO.File.WriteAllLines(settingValueFileError, linesFileError.ToArray());
			System.IO.File.WriteAllLines(settingValueFileProcessed, linesFileProcessed.ToArray());
		}
	}
}