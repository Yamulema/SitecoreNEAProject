using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Membership.Interfaces
{
    public interface ISessionService {
        object Get(string key);
        void Set(string key, object value);
    }
}