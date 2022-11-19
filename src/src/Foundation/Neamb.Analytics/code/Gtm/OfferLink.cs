namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class OfferLink
	{
        public string Event { get; set; }
        public string AccountSection { get; set; }
        public string AccountAction { get; set; }
		public string CtaText { get; set; }
		public string ClickHref { get; set; }
		public OfferLink() {
            Event = "account";
        }
    }
}