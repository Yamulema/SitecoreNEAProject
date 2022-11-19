using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Search.Interfaces;
using Neambc.Neamb.Feature.Search.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Fields;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Search.Controllers
{
    public class SearchController : BaseController
    {
        private ISearchResultManager _searchResultManager { get; }
        private readonly IGtmService _gtmService;

        public SearchController(ISearchResultManager searchResultManager, IGtmService gtmService) {
            _searchResultManager = searchResultManager;
            _gtmService = gtmService;
        }
        #region ActionResult Methods
        public ActionResult ContentFilter()
        {
            var model = new ContentFilterDto();
            model.Initialize(RenderingContext.Current.Rendering);

            // Checks if the render has datasource, if not it uses the PageItem instead.
            var datasource = model.Item ?? model.PageItem;

            //Fills the model.
            model.PageId = model.PageItem.ID.Guid.ToString();
            model.DatasourceID = datasource.ID.Guid.ToString();
            model.SeeMoreOnClickEvent = _gtmService.GetOnClickEvent(new Navigation() {
                Event = "navigation",
                NavType = "load more",
                NavText = datasource.Fields[Templates.ContentFilter.Fields.SeeMoreText]?.Value ?? string.Empty
            });

            return View("/Views/Neamb.Search/ContentFilter.cshtml", model);
        }
        public ActionResult Search()
        {
            var model = new SearchDto();
            model.Initialize(RenderingContext.Current.Rendering);

            // Checks if the render has datasource, if not it uses the PageItem instead.
            var datasource = model.Item ?? model.PageItem;

            //Fills the model.
            var keyword = Request.QueryString[Configuration.SearchParmTerm];
            var takeParm = Request.QueryString[Configuration.SearchParmTake];
            var genre = new List<string>();
            var resourceFilter = Configuration.FilterResource.Split(',');
            var offerFilter = Configuration.FilterOffer.Split(',');
            var solutionFilter = Configuration.FilterSolution.Split(',');
            genre.AddRange(resourceFilter);
            genre.AddRange(offerFilter);
            genre.AddRange(solutionFilter);

            var keywords = string.IsNullOrEmpty(keyword)
                ? new string[0]
                : keyword.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));

            var take = int.TryParse(takeParm, out var amount) ? amount : Configuration.DefaultSearchTake;

            var result = _searchResultManager.GetSearchResultCards(keywords, take, null, genre);
            model.SearchResultCards = result.Item1.ToList();
            model.ResultCount = result.Item2;
            var searchItem = (LinkField)(Sitecore.Context.Database.GetItem(Configuration.SiteSettingsId).Fields[Templates.SiteSettings.Fields.SearchPage]);

            if (searchItem?.TargetItem != null)
            {
                var redirectUrl = LinkManager.GetItemUrl(searchItem.TargetItem);
                model.RedirectUrl = redirectUrl;
            }

            model.KeywordParmName = Configuration.SearchParmTerm;
            model.InitialSearchValue = string.Join(" ", keywords);
            model.Take = take + Configuration.DefaultSearchTake;

            var queryString = string.Join("&",
                keywords.Select(x => string.Format("{0}={1}", Configuration.SearchParmTerm, x)));

            model.MoreCta = string.Format("{0}?{1}&{2}={3}", model.RedirectUrl, queryString, Configuration.SearchParmTake, model.Take);
            model.Placeholder = model.Item.Fields[Templates.SearchResults.Fields.SearchBoxPlaceholder].Value;

            return View("/Views/Neamb.Search/Search.cshtml", model);
        }
        #endregion
    }
}