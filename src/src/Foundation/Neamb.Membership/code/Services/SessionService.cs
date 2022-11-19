using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Membership.Interfaces;

namespace Neambc.Neamb.Foundation.Membership.Services
{
    [Service(typeof(ISessionService))]
    public class SessionService : ISessionService
    {
        public object Get(string key)
        {
            return HttpContext.Current.Session[key];
        }
        public void Set(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }
}