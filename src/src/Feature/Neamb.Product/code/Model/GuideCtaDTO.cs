using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class GuideCtaDTO
	{
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public Item PageItem { get; set; }
		public StatusEnum UserStatus { get; set; }
		public string ActionPrimary { get; set; }
		public string ActionPrimaryDescription { get; set; }
		public string ActionClickPrimary { get; set; }
		public string ActionPrimaryTargetBlank { get; set; }
		public string ComponentId { get; set; }
		public bool DisplayAction { get; set; }
        public string AnonymousText { get; set; }
        public bool HasBrokenLink { get; set; }
		public SocialShareModel SocialShare { get; set; }

		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			UserStatus = StatusEnum.Cold;
			DisplayAction = true;
            HasBrokenLink = false;
        }
	}
}