using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Foundation.Analytics.Helpers
{
    public static class ProcessorHelper
    {
        public static bool HasFile(HtmlNode anchorNode)
        {
            var href = anchorNode.Attributes["href"]?.Value;
            if (!string.IsNullOrEmpty(href))
            {
                string ext = System.IO.Path.GetExtension(href);
                return ext.Equals(".ashx") ? true : false;
            }
            return false;
        }

        public static bool HasSocialLinks(HtmlNode anchorNode)
        {
            var childrenSvg = anchorNode.SelectNodes(".//svg")?.Count();
            return (childrenSvg != null && childrenSvg > 0) ? true : false;
        }


        public static string GetAnchorText(HtmlNode anchorNode)
        {
            var childrenImg = anchorNode.Elements("img").Count();

            if (childrenImg > 0)
            {
                //select first image if there is more than one
                var firstImg = anchorNode.Elements("img").First();
                var altText = !string.IsNullOrEmpty(firstImg.Attributes["alt"]?.Value) ?
                    firstImg.Attributes["alt"]?.Value : "";
                return "img-" + altText;
            }
            return anchorNode.InnerText.Trim();
        }

    }
}