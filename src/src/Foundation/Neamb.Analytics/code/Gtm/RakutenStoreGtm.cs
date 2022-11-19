namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class RakutenStoreGtm
    {
        public string Event { get; set; }
        public string CardTitle { get; set; }
        public string CtaText { get; set; }
		public RakutenStoreGtm() {
            Event = "discount offer card";
        }
        public string ClickHref { get; set; }
    }
}