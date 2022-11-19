using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Foundation.Authentication.Enums
{
    public enum EligibilityStatus
    {
        None = 0,
        MdsidNotFound = 1,
        NotSeiuMember = 2,
        NotDefined = 3,
        RestrictedMember = 4,
        Eligible = 5
    }
}