using System;
using HtmlAgilityPack;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class ContentCarouselProcessor : IContentCarouselProcessor
	{
        private readonly IGtmService _gtmService;
        public ContentCarouselProcessor(IGtmService gtmService) {
            _gtmService = gtmService;
        }
        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null) {
            if (string.IsNullOrEmpty(input)) return input;
            var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(input);
            var nodeRoot = htmlDoc.DocumentNode.SelectSingleNode($"//section[contains(@class,'{Configuration.ContentCarouselSectionClass}')]");
			if (nodeRoot != null) {
				//Get the classes from the config value
				var cards = Configuration.ContentCarouselCardClass;
				foreach (var cardItemClass in cards) {
					var nodes= nodeRoot.SelectNodes($".//div[contains(@class,'{cardItemClass}')]");
					if (nodes != null) {
						foreach (var node in nodes) {
								ProcessHtml(overrideEvents, node);
						}
					}
				}
			}
			return htmlDoc.DocumentNode.OuterHtml;
        }
		private void ProcessHtml(bool overrideEvents, HtmlNode node) {
			string contentTitle = "";
			//Get the first h1, h2, or h3
			var nodeTitleH1 = node?.SelectSingleNode(".//h1");
			contentTitle = nodeTitleH1?.InnerHtml;
			if (String.IsNullOrEmpty(contentTitle))
			{
				var nodeTitleH2 = node?.SelectSingleNode(".//h2");
				contentTitle = nodeTitleH2?.InnerHtml;
			}
			if (String.IsNullOrEmpty(contentTitle))
			{
				var nodeTitleH3 = node?.SelectSingleNode(".//h3");
				contentTitle = nodeTitleH3?.InnerHtml;
			}
			if (!String.IsNullOrEmpty(contentTitle)) {
				var anchorNodes = node?.SelectNodes(".//a");
				if (anchorNodes != null) {
					foreach (var anchorNode in anchorNodes) {
						var anchor = anchorNode;
						_gtmService.AddOnClickEvent(ref anchor,
							new ContentCarousel() {
								Event = "content",
								ContentTitle = contentTitle,
								ContentLocation = "carousel"
							},
							overrideEvents);
					}
				}
			}
		}
	}
}