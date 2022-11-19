using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Cache.Model
{
    public interface IRedisCacheConnectionConfiguration
    {
        string PooledConnection { get; set; }
    }
}