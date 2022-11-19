using HtmlAgilityPack;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces;
using Neambc.Seiumb.Foundation.Analytics.Helpers;
using Sitecore.Data;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Seiumb.Foundation.Analytics.GTM.Processors
{
    public class ProductDetailsProcessor : IProductDetailsProcessor
    {
        private readonly IGTMServiceSeiumb _gtmService;

        public ProductDetailsProcessor(IGTMServiceSeiumb gtmService)
        {
            _gtmService = gtmService;
        }

        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;

            //if is page type - Product Detail
            if (args.Item.Template.ID == new ID("{E20C8AEE-8D86-4564-BB55-208152C4D7EB}"))
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(input);

                //Field: Product Description
                if (args.FieldName == "{965B2932-7D14-4605-AFBE-6D1033958BF0}")
                {                    
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

                //Field: Promo Description
                if (args.FieldName == "{5E340330-767F-415D-9614-18F1C98DDFDD}")
                {                  
                    var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                    if (anchorNodes == null) return input;

                    foreach (var anchorNode in anchorNodes)
                    {
                        if (ProcessorHelper.HasSocialLinks(anchorNode)) continue;
                        if (ProcessorHelper.HasFile(anchorNode)) continue;

                        var anchor = anchorNode;
                        _gtmService.AddOnClickEvent(ref anchor, new ProductCTASeiumb()
                        {
                            Event = "product cta link",
                            ProductName = args.Item?.Fields[new ID("{66E1662D-6485-4D32-8FCB-ADD4770B8F0C}")]?.Value,
                            CtaText = ProcessorHelper.GetAnchorText(anchorNode)
                        }, overrideEvents);
                    }
                    return htmlDoc.DocumentNode.OuterHtml;
                }

                //Field: Body
                if (args.FieldName == "{FC7C8C2A-1003-4734-BE98-35024110C2F5}")
                {
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