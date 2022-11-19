using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Neambc.Neamb.Foundation.Configuration.Pipelines;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class FooterProcessor : IFooterProcessor
    {
        private readonly IGtmService _gtmService;
        public FooterProcessor(IGtmService gtmService)
        {
            _gtmService = gtmService;
        }
        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var node = htmlDoc.DocumentNode.SelectSingleNode($"//div[contains(@class,'{Configuration.FooterClass}')]");
            var anchorNodes = node?.SelectNodes("//a");
            if (anchorNodes == null) return input;
            foreach (var anchorNode in anchorNodes) {
                var anchor = anchorNode;
                _gtmService.AddOnClickEvent(ref anchor, new Navigation()
                {
                    Event = "navigation",
                    NavType = "footer",
                    NavText = GetNavText(anchorNode)
                }, overrideEvents);
            }
            return htmlDoc.DocumentNode.OuterHtml;
        }
        private string GetNavText(HtmlNode anchor) {
            var result = GetNavTextFromAnchor(anchor);
            return string.IsNullOrEmpty(result)
                ? GetNavTextFromImg(anchor)
                : result;

        }
        private string GetNavTextFromImg(HtmlNode anchor) {
            var result = anchor?.SelectSingleNode("//img")?.Attributes["alt"]?.Value;
            return result ?? string.Empty;
        }
        private string GetNavTextFromAnchor(HtmlNode anchor)
        {
            var result = anchor?.InnerText?.Trim();
            if (string.IsNullOrEmpty(result)) {
                result = anchor?.Attributes["title"]?.Value;
            }
            return result ?? string.Empty;
        }
    }
}