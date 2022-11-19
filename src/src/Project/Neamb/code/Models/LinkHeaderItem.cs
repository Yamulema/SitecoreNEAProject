using System.Collections.Generic;
using Neambc.Neamb.Feature.Product.Model;

namespace Neambc.Neamb.Project.Web.Models
{
	public class LinkHeaderItem
	{
		public string LinkUrl { get; set; }
		public string LinkName { get; set; }
		public bool HasNotification { get; set; }
		public string ClickAction { get; set; }
        public bool IsOffer { get; set; }
        public List<HeaderOfferItemDto> LinkOffers { get; set; }

    }
}