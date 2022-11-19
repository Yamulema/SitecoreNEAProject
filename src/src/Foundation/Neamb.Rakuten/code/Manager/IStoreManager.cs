using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Rakuten.Model;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    public interface IStoreManager {
        string GetShoppingLink(string storeId);
        bool CheckEligibilityUser();
        string GetGtmFunction(GtmRakutenType gtmRakutenType, string storeId, string ctaText, bool getFullGtmEvent = true);
        string GetShoppingLinkSeiumb(string storeId);
    }
}