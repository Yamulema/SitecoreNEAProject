using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using HtmlAgilityPack;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class EmbeddedProcessor : IEmbeddedProcessor
    {
        private readonly IGtmService _gtmService;
        public EmbeddedProcessor(IGtmService gtmService) {
            _gtmService = gtmService;
        }
        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null) {
            if (string.IsNullOrEmpty(input)) return input;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
            if (anchorNodes == null) return input;
            foreach (var anchorNode in anchorNodes)
            {
                var anchor = anchorNode;
                _gtmService.AddOnClickEvent(ref anchor, new Navigation()
                {
                    Event = "navigation",
                    NavType = "embedded link",
                    NavText = anchorNode.InnerText
                },overrideEvents);
            }
            return htmlDoc.DocumentNode.OuterHtml;
        }
    }
}