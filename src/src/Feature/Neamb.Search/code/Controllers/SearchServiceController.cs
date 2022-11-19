using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Search.Interfaces;
using Neambc.Neamb.Feature.Search.Models;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Search.Controllers {
	public class SearchServiceController : BaseController {
		private IContentPageCardManager _contentPageCardManager { get; }
		private ISearchResultManager _searchResultManager { get; }
		private IAnalyticsManager _analyticsManager { get; }

		public SearchServiceController(IContentPageCardManager contentPageCardManager, ISearchResultManager searchResultManager, IAnalyticsManager analyticsManager) {
			Log.Info($"Initializing SearchServiceController", this);
			_contentPageCardManager = contentPageCardManager;
			_searchResultManager = searchResultManager;
			_analyticsManager = analyticsManager;
		}

		[HttpPost]
		
		public ActionResult GetContentPages(Guid pageId, Guid datasourceId, List<string> filters, int skip, int take) {
			Log.Info($"GetContentPages: pageId={pageId} datasourceId={datasourceId}", this);
			var result = _contentPageCardManager.GetContentPages(pageId, datasourceId, filters, skip, take);
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		
		public ActionResult GetSuggestions(string term, int? take) {
			Log.Info($"GetSuggestions: term={term} take={take}", this);
			try {
				var result = _searchResultManager.GetSuggestions(term, take);
				return Json(result, JsonRequestBehavior.AllowGet);
			} catch (Exception e) {
				Log.Error($"Error in GetSuggestions service while searching the term {term}", e, this);
				return Json(new List<SuggestionResult>(), JsonRequestBehavior.AllowGet);
			}
		}
		
		[HttpPost]
		
		public ActionResult Search(string term, int? take, int? skip, bool? resources, bool? offers, bool? solutions) {
			var pageEventItem = PageContext.Current.Item;
            var genre = new List<string>();
            var resourceFilter = Configuration.FilterResource.Split(',');
            var offerFilter = Configuration.FilterOffer.Split(',');
            var solutionFilter = Configuration.FilterSolution.Split(',');
            if (resources.Value == true) {
                genre.AddRange(resourceFilter ?? throw new InvalidOperationException());
            }
            if (offers.Value == true)
            {
                genre.AddRange(offerFilter ?? throw new InvalidOperationException());
            }
            if (solutions.Value == true)
            {
                genre.AddRange(solutionFilter ?? throw new InvalidOperationException());
            }
            Log.Info($"Search: term={term} take={take}", this);
			try {
				var keywords = string.IsNullOrEmpty(term)
					? new string[0]
					: term.Split(' ').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));
				var result = _searchResultManager.GetSearchResultCards(keywords, take, skip, genre);

				foreach (var keyword in keywords) {
					_analyticsManager.TrackSiteSearch(keyword);
				}


				// Changing the signature of the response might break a promise in PL.
				return Json(new { Total = result.Item2, Cards = result.Item1 }, JsonRequestBehavior.AllowGet);
			} catch (Exception e) {
				Log.Error($"Error in Search service while searching the term {term}", e, this);
				return Json(new { Total = 0, Cards = new List<SearchResultCard>() }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}