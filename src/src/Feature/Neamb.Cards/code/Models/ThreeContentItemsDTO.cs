using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Models {
	public class ThreeContentItemsDTO : IRenderingModel {
		public string Header { get; set; }
		public List<Item> ContentItems { get; set; }
		public Item Item { get; private set; }
		public Rendering Rendering { get; private set; }
		public bool NoDisplay { get; set; }

		public void Initialize(Rendering rendering) {
			var page = PageContext.Current.Item;
			Item = rendering.Item;
			Rendering = rendering;
			Header = page.Fields[Templates.ThreeContentItems.Fields.Header].Value;
		}
		public void FillItems(string queryResult) {
			if (queryResult == null || queryResult.Split('|').Length != 3) {
				NoDisplay = true;
				return;
			}
			var items = new List<Item>();
			var queryItems = queryResult.Split('|');
			foreach (var item in queryItems) {
				items.Add(Sitecore.Context.Database.GetItem(new ID(item)));
			}
			ContentItems = items;
		}
	}
}