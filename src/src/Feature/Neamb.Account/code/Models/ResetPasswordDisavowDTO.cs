using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class ResetPasswordDisavowDTO : IRenderingModel
	{
		public bool ProcessedSucessfully { get; set; }
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		
		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			ProcessedSucessfully = false;
		}
	}
}
