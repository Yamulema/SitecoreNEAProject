using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    public interface IPageSitecoreContext
    {
        Item Current { get; }
    }
}