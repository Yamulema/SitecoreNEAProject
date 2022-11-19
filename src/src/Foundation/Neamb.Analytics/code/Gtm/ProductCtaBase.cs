namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class ProductCtaBase
    {
        public string Event { get; set; }
        public string ProductName { get; set; }
        public string CtaText { get; set; }
		public ProductCtaBase() {
            Event = "product cta";
        }
        public string ClickHref { get; set; }
    }
}