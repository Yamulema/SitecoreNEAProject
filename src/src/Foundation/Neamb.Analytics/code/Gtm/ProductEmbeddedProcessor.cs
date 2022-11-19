using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Sitecore.Data;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class ProductEmbeddedProcessor : IProductEmbeddedProcessor
    {
        private readonly IGtmService _gtmService;
        public ProductEmbeddedProcessor(IGtmService gtmService)
        {
            _gtmService = gtmService;
        }
        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (args.Item.Template.ID == new ID("{D1889EB8-BE95-4E99-B8E9-3A0AEB8F4800}"))
            {

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(input);
                var node = htmlDoc.GetElementbyId("how-it-works");
                if (node == null)
                    return input;
                var anchorNodes = node?.SelectNodes("//a");
                if (anchorNodes == null) return input;
                foreach (var anchorNode in anchorNodes)
                {
                    var anchor = anchorNode;
                    _gtmService.AddOnClickEvent(ref anchor, new ProductCtaBase()
                    {
                        Event = "product cta link",
                        ProductName = args.Item?.Fields[new ID("{F71F7747-F88D-499B-AC69-D3A6DC9B0A88}")]?.Value,
                        CtaText = anchorNode.InnerText
                    }, overrideEvents);
                }
                return htmlDoc.DocumentNode.OuterHtml;

            }
            return input;
        }
    }
}