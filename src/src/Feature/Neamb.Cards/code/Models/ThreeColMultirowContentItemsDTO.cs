using System.Collections.Generic;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Models {
	public class ThreeColMultirowContentItemsDTO : IRenderingModel {

		#region Fields
		private const int MinNumberItems = 3;
		#endregion

		#region Properties
		public Rendering Rendering { get; private set; }
		public Item Item { get; private set; }
		public string Header { get; set; }
		public string BackgroundColorClass { get; set; }
		public List<Item> ContentItems { get; set; }
		public bool NoDisplay { get; set; }
		#endregion

		#region Private Methods
		private string GetClass(string value) {
			var colorClass = ConstantsNeamb.WhiteBackgroundColorClass;
			if (value == ConstantsNeamb.GrayBackgroundColor) {
				colorClass = ConstantsNeamb.GrayBackgroundColorClass;
			}
			return colorClass;
		}

		private List<Item> FillItems(MultilistField itemsIds) {
			var items = new List<Item>();
			if (itemsIds != null) {
				foreach (var item in itemsIds.GetItems()) {
					items.Add(item);
				}
			}
			return items;
		}
		#endregion

		#region Public Methods
		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			Header = Item.Fields[Templates.ThreeColMultirowContentItems.Fields.Header].Value;
			BackgroundColorClass = GetClass(Item.Fields[Templates.ThreeColMultirowContentItems.Fields.BackgroundColor].Value);
			MultilistField itemsIds = Item.Fields[Templates.ThreeColMultirowContentItems.Fields.Items];
			ContentItems = FillItems(itemsIds);
			NoDisplay = (ContentItems.Count < MinNumberItems);
		}
		#endregion

	}
}