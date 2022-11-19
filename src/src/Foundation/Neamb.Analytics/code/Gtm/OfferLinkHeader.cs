namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class OfferLinkHeader
    {
        public string Event { get; set; }
        public string NavType { get; set; }
        public string NavText { get; set; }
		public OfferLinkHeader() {
            Event = "navigation";
            NavType = "account";
        }
    }
}