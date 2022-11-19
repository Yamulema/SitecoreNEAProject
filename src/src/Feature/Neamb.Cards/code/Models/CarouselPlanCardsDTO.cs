using System;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Models {
	public class CarouselPlanCardsDTO : IRenderingModel {

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
			Cards = ((MultilistField)Item.Fields[Templates.CarrouselPlanCards.Fields.Cards]).GetItems();
			HideComponent = (Cards.Length < minNumberOfCards);
		}
		#endregion
	}
}