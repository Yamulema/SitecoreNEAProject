using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces
{
    public interface IHtmlProcessor
    {
        string GetTextHtml(string html);
        string GetSuppresedText(string inputText);
    }
}