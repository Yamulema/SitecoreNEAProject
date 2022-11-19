using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Account.Enums
{
    public enum RegistrationStatus
    {
        None = 0,
        Success=1,
		Error=2,
		Duplicated=3
    }
}