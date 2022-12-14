using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Indexing.Enums;
using Neambc.Neamb.Foundation.Indexing.Models;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Indexing.Interfaces
{
    public interface IProductSearchManager
    {
        ProductResult GetContentPages(string productCode);
    }
}