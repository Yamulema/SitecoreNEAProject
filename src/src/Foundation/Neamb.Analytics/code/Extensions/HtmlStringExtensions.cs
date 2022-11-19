using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Configuration.Pipelines;

namespace Neambc.Neamb.Foundation.Analytics.Extensions
{
    public static class HtmlStringExtensions
    {
        public static HtmlString Process(this HtmlString htmlString, IStringProcessor stringProcessor, bool overrideEvents = false) {
            return new HtmlString(stringProcessor.Process(htmlString.ToHtmlString()));
        }
    }
}