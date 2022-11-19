namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
	public class ContentArticle
	{
		public string Event { get; set; }
		public string ContentTitle { get; set; }
		public string ContentLocation { get; set; }
		public ContentArticle()
		{
			Event = "content";
		}
	}
}