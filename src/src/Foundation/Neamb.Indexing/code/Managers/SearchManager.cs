using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.Indexing.Models;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Indexing.Enums;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore;
using Sitecore.Diagnostics;
using Neambc.Neamb.Foundation.Configuration.Manager;

namespace Neambc.Neamb.Foundation.Indexing.Managers
{
    [Service(typeof(ISearchManager))]
    public class SearchManager : ISearchManager
    {
        protected ISearchIndex Index;
        private IGlobalConfigurationManager _globalConfigurationManager;

        public SearchManager(IGlobalConfigurationManager globalConfigurationManager)
        {
            _globalConfigurationManager = globalConfigurationManager;
            Index = ContentSearchManager.GetIndex(_globalConfigurationManager.NeambIndex);
        }

        public IEnumerable<SearchResult> GetContentPages(SearchRequest request)
        {
            using (var context = Index.CreateSearchContext())
            {
                try
                {
                    var query = PredicateBuilder.True<SearchResult>();

                    #region And Predicate
                    query = query.And(x => x._IncludeInSearchResults);
                    #endregion

                    // Filter Templates

                    #region Or Predicate
                    if (request.Filters.Any(x => x.Type != FilterType.Template))
                    {
                        var filterPredicate = PredicateBuilder.True<SearchResult>();
                        foreach (var filter in request.Filters.Where(x => x.Type != FilterType.Template))
                        {
                            switch (filter.Type)
                            {
                                case FilterType.Goal:
                                    var goal = filter.Value as ID;                                    
                                    if (ID.IsNullOrEmpty(goal))
                                    {
                                        filterPredicate = filterPredicate.Or(x => x._Goals.Contains(Guid.Empty));
                                        Log.Debug("Goal value is empty, using default.");
                                    }                                        
                                    else
                                        filterPredicate = filterPredicate.Or(x => x._Goals.Contains(goal.Guid));
                                    break;
                                case FilterType.Topic:
                                    var topic = filter.Value as ID;
                                    if (ID.IsNullOrEmpty(topic))
                                    {
                                        filterPredicate = filterPredicate.Or(x => x._Topics.Contains(Guid.Empty));
                                        Log.Debug("Topic value is empty, using default.");
                                    }                                        
                                    else
                                        filterPredicate = filterPredicate.Or(x => x._Topics.Contains(topic.Guid));
                                    break;
                                case FilterType.ProductCategory:
                                    var productCategory = filter.Value as ID;
                                    if (ID.IsNullOrEmpty(productCategory))
                                    {
                                        filterPredicate = filterPredicate.Or(x =>
                                        x._ProductCategory.Contains(Guid.Empty));
                                        Log.Debug("ProductCategory value is empty, using default.");
                                    }                                        
                                    else
                                        filterPredicate = filterPredicate.Or(x =>
                                        x._ProductCategory.Contains(productCategory.Guid));
                                    break;
                                case FilterType.LifeEvent:
                                    var lifeEvent = filter.Value as ID;
                                    if (ID.IsNullOrEmpty(lifeEvent))
                                    {
                                        filterPredicate = filterPredicate.Or(x => x._LifeEvents.Contains(Guid.Empty));
                                        Log.Debug($"LifeEvent value is empty, using default.");
                                    }
                                    else
                                    {
                                        filterPredicate =
                                        filterPredicate.Or(x => x._LifeEvents.Contains(lifeEvent.Guid));
                                        Log.Debug($"LifeEvent value: {lifeEvent.Guid}");
                                    }
                                    break;
                                case FilterType.Seasonality:
                                    var seasonality = filter.Value as ID;
                                    if (ID.IsNullOrEmpty(seasonality))
                                    {
                                        filterPredicate = filterPredicate.Or(x => x._Seasonality.Contains(Guid.Empty));
                                        Log.Debug("Seasonality value is empty, using default.");
                                    }                                        
                                    else
                                        filterPredicate =
                                        filterPredicate.Or(x => x._Seasonality.Contains(seasonality.Guid));
                                    break;
                                case FilterType.PageTitle:
                                    var pageTitle = $"{(string)filter.Value}";
                                    filterPredicate =
                                        filterPredicate.Or(x => x._PageTitle.MatchWildcard($"*{pageTitle}*").Boost(3f));
                                    filterPredicate =
                                        filterPredicate.Or(x => x._PageTitle.MatchWildcard(pageTitle).Boost(600f));
                                    break;
                                case FilterType.ShortDescription:
                                    var shortDescription = $"{(string)filter.Value}";
                                    filterPredicate =
                                        filterPredicate.Or(x => x._ShortDescription.MatchWildcard($"*{shortDescription}*")).Boost(2f);
                                    filterPredicate =
                                    filterPredicate.Or(x => x._ShortDescription.MatchWildcard(shortDescription).Boost(50f));
                                    break;
                                case FilterType.Body:
                                    var body = $"{(string)filter.Value}";
                                    filterPredicate =
                                        filterPredicate.Or(x => x._Body.MatchWildcard($"*{body}*").Boost(1f));
                                    filterPredicate =
                                        filterPredicate.Or(x => x._Body.MatchWildcard(body).Boost(4f));
                                    break;
                            }
                        }

                        query = query.And(filterPredicate.Boost(600f));
                    }
                    #endregion

                    #region Or Predicate
                    var templatePredicate = PredicateBuilder.True<SearchResult>();
                    foreach (var filter in request.Filters)
                    {
                        if (filter.Type == FilterType.Template)
                        {
                            templatePredicate = templatePredicate.Or(x => x.TemplateId == (ID)filter.Value);
                        }
                    }

                    var genrePredicate = PredicateBuilder.True<SearchResult>();
                    foreach (var filter in request.Filters)
                    {
                        if (filter.Type == FilterType.Genre)
                        {
                            genrePredicate = genrePredicate.Or(x => x.Genre == (string)filter.Value);
                        }
                    }

                    query = query.And(templatePredicate.Boost(0f)).And(genrePredicate.Boost(0f));

                    #endregion

                    #region And Predicate
                    
                    if (request.ExcludeIds.Any()) {
                        var excludePredicate = PredicateBuilder.True<SearchResult>();
                        foreach (var excludeId in request.ExcludeIds) {
                            excludePredicate = excludePredicate.And(x => x.ItemId != excludeId);
                        }
                        query = query.And(excludePredicate.Boost(0f));
                    }
                    
                    #endregion

                    //Max number of records for the search results
                    var queryable = context.GetQueryable<SearchResult>().Where(query)
                        .Take(request.Take);

                    //Sorting

                    switch (request.SortBy) {
                        case SortBy.None:
                            break;
                        case SortBy.LastPublishDateDesc:
                            queryable = queryable.OrderByDescending(x => x._LastPublishDate);
                            break;
                        default:
                            break;
                    }

                    //Hits search engine.
                    var result = queryable.GetResults();
                    return result.Hits.Any() ? result.Hits.Select(x => x.Document).ToList() : new List<SearchResult>();
                }
                catch (Exception ex)
                {
                    Log.Fatal("Exception on Search", ex, this);
                    return new List<SearchResult>();
                }
            }
        }

        public IEnumerable<Item> GetContentItems(SearchRequest request)
        {
            return GetContentPages(request).Select(x => x.GetItem());
        }
    }
}