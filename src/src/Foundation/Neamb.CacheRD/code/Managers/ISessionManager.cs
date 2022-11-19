using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Cache.Managers
{
    public interface ISessionManager
    {
	    bool Remove(string key);
	    T RetrieveFromSession<T>(string key);
	    void StoreInSession<T>(string key, T element);
    }
}