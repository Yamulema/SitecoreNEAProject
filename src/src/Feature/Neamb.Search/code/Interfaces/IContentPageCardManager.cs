using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Search.Models;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Search.Interfaces
{
    public interface IContentPageCardManager
    {
        ContentFilterResult GetContentPages(Guid pageId, Guid datasourceId, List<string> filters, int skip, int take);
    }
}