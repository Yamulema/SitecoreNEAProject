using Neambc.Neamb.Feature.Search.Interfaces;
using Sitecore.Diagnostics;
using System;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Feature.Search.Controllers
{
    public class StoreServiceController : BaseController
    {
        private IStoreSearchResultManager _searchResultManager { get; }

        public StoreServiceController(IStoreSearchResultManager searchResultManager) {
            Log.Info($"Initializing SearchServiceController", this);
            _searchResultManager = searchResultManager;
        }

        [HttpPost]
        public ActionResult Search(bool popularOffers, bool favoritesOnly, string[] categories, string storeName, 
            int sort, int take, int skip) {
            Log.Info($"Store Search: categories={categories} storeName={storeName} take={take} skip={skip}", this);
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
                Log.Error($"Error in Store Search service", e, this);
                return Json(new { Stores = new {}, Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FavoriteStore(string store, bool behaviour){
            Log.Info($"Favorite Stores: storeID={store} behaviour={behaviour}", this);
            try
            {
                var result = _searchResultManager.FavoriteStores(store, behaviour);
                return Json(new { Stores = result, Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Log.Error($"Error in Favorite Store service", e, this);
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}