using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class ProductNavigationProcessor : IProductNavigationProcessor
    {
        private readonly IGtmService _gtmService;
        public ProductNavigationProcessor(IGtmService gtmService) {
            _gtmService = gtmService;
        }
        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null) {
            if (string.IsNullOrEmpty(input)) return input;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var node = htmlDoc.DocumentNode.SelectSingleNode($"//ul[contains(@class,'{Configuration.ProductNavigationClass}')]");
            var anchorNodes = node?.SelectNodes("//a");
            if (anchorNodes == null) return input;
            foreach (var anchorNode in anchorNodes)
            {
                var anchor = anchorNode;
                _gtmService.AddOnClickEvent(ref anchor, new Navigation()
                {
                    Event = "navigation",
                    NavType = "product page",
                    NavText = anchorNode.InnerText
                }, overrideEvents);
            }
            return htmlDoc.DocumentNode.OuterHtml;
        }
    }
}