using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Language.Managers
{
    public interface ICookieManager
    {
        void CreateCookie(string name);
        void CreateCookie(string name, string value, TimeSpan expires);
        HttpCookie GetCookie(string name);
        bool Exists(string name);
    }
}