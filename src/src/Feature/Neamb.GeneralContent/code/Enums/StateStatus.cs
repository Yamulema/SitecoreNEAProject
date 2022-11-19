using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Enums
{
    [Flags]
    public enum StateStatus
    {
        None = 0,
        Invalid = 1,
        SpecifyZip = 2,
        SpecifyAge = 4
    }
}