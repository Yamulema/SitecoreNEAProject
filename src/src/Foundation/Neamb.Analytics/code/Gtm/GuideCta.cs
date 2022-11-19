namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class GuideCta
    {
        public string Event { get; set; }
        public string FileName { get; set; }
        public string CtaText { get; set; }
		public string ClickHref { get; set; }
		public GuideCta()
        {
            Event = "downloads";
        }
    }
}
