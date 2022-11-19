using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Account.Enums
{
    [Flags]
    public enum EmailValidationStatus
    {
        None = 0,
        [Description("error")]
        Error = 1,
        [Description("ok")]
        Ok = 2,
        [Description("10000")]
        AccessDenied = 4,
        [Description("10001")]
        InputDataValidationFail = 8,
        [Description("Y")]
        Valid = 16
    }
}