namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
	public class ContactUs
	{
		public string Event { get; set; }
		public string AccountSection { get; set; }
		public string AccountAction { get; set; }
		public string CtaText { get; set; }
		public ContactUs()
		{
			Event = "account";
			AccountSection = "contact us";
		}
	}
}