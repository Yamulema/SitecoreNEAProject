using System.Collections.Generic;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class SweepstakesBaseDTO
    {
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public Item PageItem { get; set; }
		public SocialShareModel SocialShare { get; set; }
		public bool ShowContactInfo { get; set; }
		public bool ContactInfoText { get; set; }
		public List<string> ListPartners { get; set; }
		public List<Item> ListPartnersItems { get; set; }
		public string CtaText { get; set; }
		public bool ShowAcknowledgement { get; set; }
		public string LoginText { get; set; }
        public string LoginUrl { get; set; }
		public bool HasTermsAndConditions { get; set; }
		public string Cellcode { get; set; }
		public string Campaigncode { get; set; }
		public bool IsProcessedSuccessfully { get; set; }
		public string Video { get; set; }
		public bool IsPositionRight { get; set; }
		public bool IsAuthenticated { get; set; }
		public bool HasErrors { get; set; }
		public Reminder Reminder { get; set; }
        public string ComponentId { get; set; }
        public string ComponentIdAuthentication { get; set; }
        public string GtmAction { get; set; }
        public string ActionClickAuthentication { get; set; }
        public bool HasResultAuthentication { get; set; }

        public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			//ShowSocialMedia = false;
			SocialShare = new SocialShareModel();
			ShowContactInfo = false;
			ListPartners = new List<string>();
			ListPartnersItems = new List<Item>();
			HasTermsAndConditions = false;
			IsProcessedSuccessfully = false;
			HasErrors = false;
            HasResultAuthentication = false;

        }
    }
}