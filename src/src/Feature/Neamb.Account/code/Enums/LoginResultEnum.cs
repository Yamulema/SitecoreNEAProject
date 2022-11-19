using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Account.Enums
{
    public enum LoginResultEnum
    {
        None = 0,
        Ok=1,
        ErrorLogin=2,
        ErrorRakutenMember=3,
        ErrorEligibilityRakuten=4
    }
}