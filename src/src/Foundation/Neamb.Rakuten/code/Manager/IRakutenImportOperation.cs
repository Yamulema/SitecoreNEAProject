using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    public interface IRakutenImportOperation {
        void DeleteItemSitecore(Item item);
        void PublishItem(ID parentId);
        void PublishItem(Item rootItem, bool withChildren = true);
    }
}