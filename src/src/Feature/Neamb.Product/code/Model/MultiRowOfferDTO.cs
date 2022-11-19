using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class MultiRowOfferDTO: IRenderingModel
	{
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }

		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			
		}
	}
}