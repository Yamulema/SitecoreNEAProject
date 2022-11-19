using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Configuration.Enums
{
    [Flags]
    public enum ModelErrorType
    {
        None = 0,
        Required = 1,
        RegularExpression = 2,
        [Description("MaxLength")]
        MaxLength = 4
    }
}