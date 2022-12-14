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
    public class LandingPageProcessor : ILandingPageProcessor
    {
        private readonly IGTMServiceSeiumb _gtmService;

        public LandingPageProcessor(IGTMServiceSeiumb gtmService)
        {
            _gtmService = gtmService;
        }

        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;

            //if is page type - Landing Page
            if (args.Item.Template.ID == new ID("{B136D132-50E9-43D2-ACC8-83576D5EB51B}"))
            {
                //Field: - Default Content
                if (args.FieldName == "{7EAEF1C7-C4D4-4C7F-B53E-EAEAEE47DD3E}")
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