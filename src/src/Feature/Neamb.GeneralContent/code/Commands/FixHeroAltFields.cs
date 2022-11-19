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
	public class FixHeroAltFields : Command
	{
		public override void Execute(CommandContext context)
		{
			Sitecore.Diagnostics.Log.Info(String.Format("Starting AltTagPostScript"), this);


			Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
			Item[] allItemsHome = master.SelectItems("fast:/sitecore/content/NEAMB//*");

			var ariclesItems = GetArticlesWithIncorrectAlt(allItemsHome);

			ProcessItems(ariclesItems);

			Sitecore.Diagnostics.Log.Info(String.Format("Finishing AltTagPostScript"), this);
		}

		private Item[] GetArticlesWithIncorrectAlt(Item[] allItemsHome)
		{
			List<string> itemslist = new List<string>();
			List<Item> articleList = new List<Item>();



			var allArticleItems = allItemsHome.Where(item => item.TemplateID.ToString() == "{15547760-E485-446C-B7C0-98A660FD577E}");

			articleList = allArticleItems.Where(item => item.Fields["Hero Image"].Value.Contains("alt=\"842x474 placeholder\"")).ToList();

			foreach (var article in articleList)
			{

				var lineMessage = String.Format("Article found with wrong Alt,{0},{1}", article.Paths.Path, article.ID);
				itemslist.Add(lineMessage);
			}

			return articleList.ToArray();

		}

		private static void ProcessItems( Item[] allArticleItems)
		{
			string lineMessage = "";
			string itemPath;
			string sourceAlt;
			List<string> linesFileProcessed = new List<string>();
			var settingValueFileProcessed = Settings.GetSetting("AltHeroImageProcessedFilelocation");

			try
			{
				foreach (var article in allArticleItems)
				{
					itemPath = article.Paths.Path;
					sourceAlt = article.Fields["Hero Image"].Value;
					Database master = Sitecore.Configuration.Factory.GetDatabase("master");
					var item = master.GetItem(article.ID);
					item.Editing.BeginEdit();
					item["Hero Image"] = sourceAlt.Replace("842x474 placeholder", "");
					item.Editing.EndEdit();
					item.Editing.AcceptChanges();
					lineMessage = String.Format("Alt changed for item with path: {0}", itemPath);
					linesFileProcessed.Add(lineMessage);
				}
			} catch (Exception e) {
				Sitecore.Diagnostics.Log.Info(String.Format("Error in Method: {0}", e.Message), "Command");
			}
			System.IO.File.WriteAllLines(settingValueFileProcessed, linesFileProcessed.ToArray());
		}
	}
}