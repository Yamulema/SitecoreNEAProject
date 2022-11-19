using System;
using System.Web;
using Sitecore.Links.UrlBuilders;
using Sitecore.StringExtensions;

namespace Neamb.Project.Common.Helpers {
	public static class SitecoreHtmlHelper {
		/// <summary>
		/// Retrieving canonical URL
		/// </summary>
		/// <returns>Link render in meta data</returns>
		public static System.Web.HtmlString GetCanonicalUrl() {

			var urlOptions = new ItemUrlBuilderOptions
			{
				AlwaysIncludeServerUrl = true,

			};
			
			string canonicalUrl = Sitecore.Links.LinkManager.GetItemUrl(Sitecore.Context.Item, urlOptions);

			return new System.Web.HtmlString(canonicalUrl);
		}
	}
}