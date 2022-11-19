using System;

namespace Neambc.Seiumb.Feature.Rakuten.Models
{
    public class SeiumbExclusiveOfferCard
    {
        public string Headline { get; set; }
        public string BackgroundColor { get; set; }
        public SeiumbExclusiveOffer Offer { get; set; }
    }

    public class SeiumbExclusiveOffer
    {
        public Guid Id { get; set; }
        public bool MembersOnly { get; set; }
        public string BaseReward { get; set; }
        public string TotalReward { get; set; }
        public string Type { get; set; }
        public string Tier { get; set; }
        public string ShoppingUrl { get; set; }
        public string Name { get; set; }
        public string Banner { get; set; }
        public string SmallLogo { get; set; }
        public string Thumbnail { get; set; }
        public string Icon11230 { get; set; }
        public string Icon22460 { get; set; }
        public string Icon33690 { get; set; }
        public string LogoEmail { get; set; }
        public string LogoMobile { get; set; }
        public string LogoMobile2x { get; set; }
        public string LogoMobile3x { get; set; }
        public string FeedSquareLogo { get; set; }
        public string BtnClass { get; set; }
        public string CardText { get; set; }
    }
}