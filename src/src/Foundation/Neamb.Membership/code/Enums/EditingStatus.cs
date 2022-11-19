using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Membership.Enums
{
    [Flags]
    public enum EditingStatus
    {
        None = 0,
        BeneficiariesChanged = 1,
        YourInformationChanged = 4,
        Saved = 8
    }
}