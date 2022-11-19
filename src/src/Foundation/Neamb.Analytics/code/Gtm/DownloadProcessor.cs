using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class DownloadProcessor : IDownloadProcessor
    {
        private readonly IGtmService _gtmService;
        public DownloadProcessor(IGtmService gtmService)
        {
            _gtmService = gtmService;
        }
        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
            if (anchorNodes == null) return input;
            foreach (var anchorNode in anchorNodes) {
                var file = GetPdfFile(anchorNode);
                if (file == null) {
                    continue;
                }
                var anchor = anchorNode;
                _gtmService.AddOnClickEvent(ref anchor, new Download()
                {
                    Event = "downloads",
                    FileName = file.Name,
                    CtaText = anchor?.InnerText?.Trim()
                }, overrideEvents);
            }
            return htmlDoc.DocumentNode.OuterHtml;
        }
        private File GetPdfFile(HtmlNode anchorNode) {
            if (string.IsNullOrEmpty(anchorNode.Attributes["href"]?.Value)) {
                return null;
            }
            var href = anchorNode.Attributes["href"].Value;

            //Matches pdf
            var pattern = @"(.+\/)*(.+)\.(pdf|txt)";
            var match = Regex.Match(href, pattern, RegexOptions.IgnoreCase);

            if (match.Success) {
                return new File() {
                    Name = match.Groups[2]?.Value,
                    Extension = match.Groups[3]?.Value
                };
            } else {
                return null;
            }
        }
    }
}