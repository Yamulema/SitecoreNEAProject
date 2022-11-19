using HtmlAgilityPack;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces;
using Neambc.Seiumb.Foundation.Analytics.Helpers;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Pipelines.RenderField;
using System;
using System.Linq;

namespace Neambc.Seiumb.Foundation.Analytics.GTM.Processors
{
    public class RailCardProcessor : IRailCardProcessor
    {
        private readonly IGTMServiceSeiumb _gtmService;

        public RailCardProcessor(IGTMServiceSeiumb gtmService)
        {
            _gtmService = gtmService;
        }

        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;

            //if is page type - Rail Card
            if (args.Item.Template.ID == new ID("{9D32F297-4FF4-4BC5-99F6-E22FF3E8BDAE}"))
            {
                //Field: Body
                if (args.FieldName == "{91E9A811-1D9D-4226-AEBE-6E4C888B0CD1}")
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(input);
                    var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                    if (anchorNodes == null) return input;

                    foreach (var anchorNode in anchorNodes)
                    {
                        if (ProcessorHelper.HasSocialLinks(anchorNode)) continue;

                        var anchor = anchorNode;
                        _gtmService.AddOnClickEvent(ref anchor, new ModuleSeiumb()
                        {
                            Event = "rail module",
                            ModuleTitle = GetTextFromImgField(args),
                            CtaText = anchorNode.InnerText.Trim()
                        }, overrideEvents);
                    }
                    return htmlDoc.DocumentNode.OuterHtml;
                }
            }
            return input;
        }
        private string GetTextFromImgField(RenderFieldArgs args)
        {
            ImageField image = args.Item?.Fields[new ID("{F4A03B66-8C14-4C94-A560-4F74C9E62973}")];
            if (image != null) {
                return !string.IsNullOrEmpty(image.Alt) ? image.Alt : string.Empty;
            }
            return string.Empty;
        }
    }
}