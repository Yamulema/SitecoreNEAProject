using Sitecore.Diagnostics;
using System;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Search.Interfaces;

namespace Neambc.Seiumb.Feature.Search.Controllers
{
    public class SeiumbStoreSearchController : BaseController
    {
        private ISeiumbStoreSearchResultManager _searchResultManager { get; }

        public SeiumbStoreSearchController(ISeiumbStoreSearchResultManager searchResultManager)
        {
            Log.Info($"Initializing SeiumbStoreSearchController", this);
            _searchResultManager = searchResultManager;
        }

        [HttpPost]
        public ActionResult Search(bool popularOffers, bool favoritesOnly, string[] categories, string storeName,
            int sort, int take, int skip)
        {
            Log.Info($"Seiumb Store Search: categories={categories} storeName={storeName} take={take} skip={skip}", this);
            try
            {
                //disable popular offers
                popularOffers = false;
                var result = _searchResultManager.GetStores(popularOffers, favoritesOnly, categories, storeName,
                    sort, take, skip);
                
                return Json(new { Stores = result, Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Log.Error($"Error in Seiumb Store Search service", e, this);
                return Json(new { Stores = new { }, Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FavoriteStore(string store, bool behaviour)
        {
            Log.Info($"Seiumb Favorite Stores: storeID={store} behaviour={behaviour}", this);
            try
            {
                _searchResultManager.FavoriteStores(store, behaviour);
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Log.Error($"Error in Seiumb Favorite Store service", e, this);
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}