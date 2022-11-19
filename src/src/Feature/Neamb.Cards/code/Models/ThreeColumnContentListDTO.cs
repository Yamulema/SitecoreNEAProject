using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Models {
	public class ThreeColumnContentListDTO : IRenderingModel {

		#region Fields
		private const int minNumberOfCards = 1;
		#endregion

		#region Properties
		public Rendering Rendering { get; private set; }
		public Item Item { get; private set; }
		public Item[] Cards { get; set; }
		public Boolean HideComponent { get; set; }
		#endregion

		#region Public Methods
		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			Cards = Item.GetChildren().Select(x => x).ToArray();
			HideComponent = (Cards.Length < minNumberOfCards);
		}
		#endregion
	}
}