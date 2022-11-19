using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    public interface ICustomLocker {
        KeyLocker Lock();
    }
}