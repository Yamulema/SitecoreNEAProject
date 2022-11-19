using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Neambc.Neamb.Foundation.Analytics.Extensions;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class SocialProcessor : ISocialProcessor
    {
        private readonly IGtmService _gtmService;
        public SocialProcessor(IGtmService gtmService)
        {
            _gtmService = gtmService;
        }
        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var node = htmlDoc.DocumentNode.SelectSingleNode($"//div[contains(@class,'{Configuration.SocialConnectClass}')]");
            var anchorNodes = node?.SelectNodes(".//a");
            if (anchorNodes == null) return input;
            foreach (var anchorNode in anchorNodes)
            {
                var anchor = anchorNode;
                _gtmService.AddOnClickEvent(ref anchor, new Social()
                {
                    Event = "social",
                    SocialAction = "connect",
                    SocialPlatform = GetSocialPlatform(anchor)
                }, overrideEvents);
            }
            return htmlDoc.DocumentNode.OuterHtml;
        }
        private string GetSocialPlatform(HtmlNode anchor)
        {
            if (anchor.HasClass(Configuration.FacebookClasses))
                return "facebook";

            if (anchor.HasClass(Configuration.LinkedinClasses))
                return "linkedin";

            if (anchor.HasClass(Configuration.TwitterClasses))
                return "twitter";

            return string.Empty;
        }
        
    }
}