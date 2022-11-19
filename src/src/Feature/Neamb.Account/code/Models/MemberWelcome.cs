using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Feature.GeneralContent.Enums;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class MemberWelcome : IRenderingModel
    {
        public Item Item { get; set; }
        public WelcomeStatus Status { get; set; }
        public string Imsid { get; set; }
        public string Mdsid { get; set; }
        public string SearchMessage { get; set; }
        public WelcomeErrorStatus ErrorStatus { get; set; }
        public VideoSourceType VideoType { get; set; }
        public SocialShareModel SocialShare { get; set; }
        public string NotYouLinkUrl { get; set; }
	    public string SupportEmail { get; set; }

		public MemberWelcome()
        {
        }

        public MemberWelcome(Item datasource)
        {
            Item = datasource;
        }
        public void Initialize(Rendering rendering)
        {
            Item = rendering.Item;
        }
    }
}