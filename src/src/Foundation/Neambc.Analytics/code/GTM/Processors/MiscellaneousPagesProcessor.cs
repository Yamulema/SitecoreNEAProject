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
    public class MiscellaneousPagesProcessor : IMiscellaneousPagesProcessor
    {
        private readonly IGTMServiceSeiumb _gtmService;

        public MiscellaneousPagesProcessor(IGTMServiceSeiumb gtmService)
        {
            _gtmService = gtmService;
        }

        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;

            //if is page type - Two Column Miscellaneous / One Column Miscellaneous
            if (args.Item.Template.ID == new ID("{EF07CBF6-E44B-45CE-B502-9A8592266BA1}") ||
                args.Item.Template.ID == new ID("{2D96EC72-CD15-45E1-BFFB-06AF9804BB68}"))
            {
                //Field: - Body
                if (args.FieldName == "{FC7C8C2A-1003-4734-BE98-35024110C2F5}")
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