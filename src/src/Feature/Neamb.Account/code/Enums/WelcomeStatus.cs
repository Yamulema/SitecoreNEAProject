using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Account.Enums
{
    [Flags]
    public enum WelcomeStatus
    {
        None = 0,
        NewMember = 1,
        ExistingMember = 2
    }
}