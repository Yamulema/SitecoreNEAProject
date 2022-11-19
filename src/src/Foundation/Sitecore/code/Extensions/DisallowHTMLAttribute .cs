using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Foundation.Sitecore.Extensions
{
    public class DisallowHTMLAttribute : RegularExpressionAttribute
    {
        public DisallowHTMLAttribute()
            : base(@"^[a-zA-Z0-9,_./ '\-]*$")
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return "EH";
        }
    }
}