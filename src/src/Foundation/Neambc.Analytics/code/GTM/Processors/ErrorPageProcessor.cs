using HtmlAgilityPack;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces;
using Neambc.Seiumb.Foundation.Analytics.Helpers;
using Sitecore.Data;
using Sitecore.Pipelines.RenderField;
using System;
using System.Linq;

namespace Neambc.Seiumb.Foundation.Analytics.GTM.Processors
{
    public class ErrorPageProcessor : IErrorPageProcessor
    {
        private readonly IGTMServiceSeiumb _gtmService;

        public ErrorPageProcessor(IGTMServiceSeiumb gtmService)
        {
            _gtmService = gtmService;
        }

        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;

            //if is page type - Error Page
            if (args.Item.Template.ID == new ID("{D613828E-0662-45F9-9A35-D1C6C47271E8}"))
            {
                //Field: - Body
                if (args.FieldName == "{25DF7E83-4554-4B36-98D2-6359D8C7DF59}")
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(input);
                    var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                    if (anchorNodes == null) return input;

                    foreach (var anchorNode in anchorNodes)
                    {
                        if (ProcessorHelper.HasSocialLinks(anchorNode)) continue;
                        if (ProcessorHelper.HasFile(anchorNode)) continue;

                        var anchor = anchorNode;
                        _gtmService.AddOnClickEvent(ref anchor, new NavigationSeiumb()
                        {
                            Event = "navigation",
                            NavType = "embedded link",
                            NavText = ProcessorHelper.GetAnchorText(anchorNode)
                        }, overrideEvents);
                    }
                    return htmlDoc.DocumentNode.OuterHtml;
                }
            }
            return input;
        }
    }
}