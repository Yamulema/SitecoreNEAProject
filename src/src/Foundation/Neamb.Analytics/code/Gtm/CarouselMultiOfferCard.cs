namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class CarouselMultiOfferCard : MultiOfferCard
    {
        public string Position { get; set; }
        public CarouselMultiOfferCard() {
            Event = "multi-offer card";
        }
    }
}