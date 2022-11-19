using HtmlAgilityPack;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces;
using Neambc.Seiumb.Foundation.Analytics.Helpers;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Pipelines.RenderField;
using System.Linq;
using System.Text.RegularExpressions;

namespace Neambc.Seiumb.Foundation.Analytics.GTM.Processors
{
    public class HomePageProcessor : IHomePageProcessor
    {
        private readonly IGTMServiceSeiumb _gtmService;

        public HomePageProcessor(IGTMServiceSeiumb gtmService)
        {
            _gtmService = gtmService;
        }

        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;

            //if is page type - Card Item
            if (args.Item.Template.ID == new ID("{81463630-D9FD-4AE6-9B28-67039341F16C}"))
            {
                //Field: Body
                if (args.FieldName == "{D0853B60-C18B-4FAC-9FF8-808326C049E2}")
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(input);
                    var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                    var actionBtn = htmlDoc.DocumentNode.SelectSingleNode($"//a[contains(@class,'panelBtn')]");
                    var title = htmlDoc.DocumentNode.SelectSingleNode("//h3");
                    if (anchorNodes == null) return input;

                    foreach (var anchorNode in anchorNodes)
                    {
                        if (ProcessorHelper.HasSocialLinks(anchorNode)) continue;

                        var anchor = anchorNode;
                        _gtmService.AddOnClickEvent(ref anchor, new ModuleSeiumb()
                        {
                            Event = "large module",
                            ModuleTitle = GetTitle(args, actionBtn),
                            CtaText = title != null ?  title.InnerText.Trim() : string.Empty
                        }, overrideEvents);
                    }
                    return htmlDoc.DocumentNode.OuterHtml;
                }
            }
            return input;
        }

        private string GetTitle(RenderFieldArgs args, HtmlNode actionBtn)
        {
            var title = args.Item?.Fields[new ID("{63056AC1-9C1A-4191-9844-505C806CCA22}")]?.Value;
            var clickText = actionBtn != null ? actionBtn.InnerText.Trim() : string.Empty;
            return title + " | " + clickText;
        }
    }
}