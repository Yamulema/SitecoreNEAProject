namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class MultiOfferCard
	{
        public string Event { get; set; }
        public string CardTitle { get; set; }
        public string CtaText { get; set; }
		public string ClickHref { get; set; }
		public MultiOfferCard() {
            Event = "multi-offer card";
        }
    }
}