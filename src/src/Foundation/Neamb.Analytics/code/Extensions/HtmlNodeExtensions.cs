using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Neambc.Neamb.Foundation.Analytics.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static bool HasClass(this HtmlNode node, IEnumerable<string> classes) {
            return HasAnyClass(node, classes) ||
                node.Descendants().Any(x => HasAnyClass(x, classes));
        }

        private static bool HasAnyClass(HtmlNode node, IEnumerable<string> classes)
        {
            return classes.Any(x => {
                return node.Attributes["class"] != null
                    ? node.Attributes["class"]
                        .Value.Split(new[] {
                                " "
                            },
                            StringSplitOptions.RemoveEmptyEntries)
                        .Any(y => y.Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    : false;
            });
        }
    }
}