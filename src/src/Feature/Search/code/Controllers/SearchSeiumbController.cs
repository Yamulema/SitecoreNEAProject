using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Search.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Neambc.Seiumb.Foundation.Indexing.Managers;
using Neambc.Seiumb.Foundation.Indexing.PageTypes;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Search.Controllers {
	public class SearchSeiumbController : BaseController
    {

        private readonly IGTMServiceSeiumb _gtmService;
        public SearchSeiumbController(IGTMServiceSeiumb gtmService) {
            _gtmService = gtmService;
        }

        /// <summary>
        /// Redirect to the search result page
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchResultAction() {
			var searchResultModel = new SearchResultModel { ContextItem = RenderingContext.Current.ContextItem };
			return View("~/Views/Search/Renderings/SearchResults.cshtml", searchResultModel);
		}

		/// <summary>
		/// Redirect to the search page sent in the url the search term entered by the user
		/// </summary>
		/// <returns></returns>
		[HttpPost]
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    try
        //    {
        //        this.ActionInvoker.InvokeAction(this.ControllerContext, "Redirection");
        //       // Redirect("/404");
        //        //new HttpForbiddenResult();
        //        //this.View("Redirection").ExecuteResult(this.ControllerContext);
        //    }
        //    catch (Exception)
        //    {
        //        // here we can catch the view not found error}
        //    }
        //}

        //public ActionResult Redirection() {
        //    return Redirect("/404");
        //    //return new HttpForbiddenResult();
        //}

        public ActionResult Search() {
			var searchText = Request.Params["searchtext"];
			var SearchResultPage = Sitecore.Context.Database.GetItem(Templates.SearchPageItem);
			var redirectUrl = LinkManager.GetItemUrl(SearchResultPage);
			return Redirect($"{redirectUrl}?searchtext={searchText}");
		}

		[HttpPost]
		
		public ActionResult GlobalSearch(string searchText) {
			IEnumerable<SearchResult> result = new List<SearchResult>();
			if (!string.IsNullOrEmpty(searchText)) {
				result = SearchManager.GlobalSearch(searchText);
			}

            var jsonResult = result.Select(x => new GlobalSearchJsonModel {
				title = x._Metatitle,
				content = GetDescription(x._MetaDescription),
				url = GetUrl(x._FullPath),
                onClickEventTitle = _gtmService.GetGtmEvent(new SearchSeiumb()
                {
                    Event = "search",
                    SearchAction = "result click",
                    SearchText = x._Metatitle
                }),
                onClickEventUrl = _gtmService.GetGtmEvent(new SearchSeiumb()
                {
                    Event = "search",
                    SearchAction = "result click",
                    SearchText = GetUrl(x._FullPath)
                })
            });

			return Json(new {
				results = jsonResult
			}, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Get the description truncated to 150 characters
		/// </summary>
		/// <param name="description"></param>
		/// <returns></returns>
		private string GetDescription(string description) {
			if (!string.IsNullOrEmpty(description)) {
				if (description.Length <= 150) {
					return description;
				}
				return $"{description.Substring(0, 150)}...";
			}
			return description;
		}

		/// <summary>
		/// Get the url of the item
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private string GetUrl(string path) {
			var dataItem = Sitecore.Context.Database.GetItem(path);
			if (dataItem != null) {
				var options = new ItemUrlBuilderOptions
				{
					AlwaysIncludeServerUrl = true,

				};

				return LinkManager.GetItemUrl(dataItem, options);
			}
			return string.Empty;
		}
	}
}