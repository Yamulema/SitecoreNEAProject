using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Cache.Model;

namespace Neambc.Neamb.Foundation.Cache.Repositories
{
    public interface ICacheSeiumbConfigRepo
    {
        IRedisCacheSeiumbConfiguration CacheSeiumbConfiguration { get; }
    }
}