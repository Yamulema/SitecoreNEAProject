using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Seiumb.Foundation.Indexing.PageTypes;
using Sitecore;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Sitecore.DependencyInjection;

namespace Neambc.Seiumb.Foundation.Indexing.Repository
{
    public class GlobalSearchRepository : BaseRepository
    {
        private static GlobalSearchRepository instance;        

        public GlobalSearchRepository(string indexName) : base(indexName)
        {
        }

        public static GlobalSearchRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    var globalConfigurationManager =(IGlobalConfigurationManager)ServiceLocator.ServiceProvider.GetService(typeof(IGlobalConfigurationManager));

                    instance = new GlobalSearchRepository(globalConfigurationManager.SeiumbIndex);
                }
                return instance;
            }
        }

        /// <summary>
        /// Get the result to be displayed in the search page using solr
        /// </summary>
        /// <param name="searchText">term entered by the user in the searhc</param>
        /// <returns></returns>
        public IEnumerable<SearchResult> GetSearchResult(string searchText)
        {
            using (var context = Index.CreateSearchContext())
            {
                try
                {
                    var query = PredicateBuilder.True<SearchResult>();

                    //Filter the correct templates for searching
                    query = query.And(x => x.TemplateId == Templates.ProductDetail 
                    || x.TemplateId == Templates.ProductList
                    || x.TemplateId == Templates.ContactUs
                    || x.TemplateId == Templates.Home
                    || x.TemplateId == Templates.OneColumnMiscellaneous
                    || x.TemplateId == Templates.RegistrationPage
                    || x.TemplateId == Templates.TwoColumnMiscellaneous
                    || x.TemplateId == Templates.TwoColumnList
                    || x.TemplateId == Templates.LoginHelp
                    || x.TemplateId == Templates.RetrieveUsername
                    || x.TemplateId == Templates.RetrievePassword
                    || x.TemplateId == Templates.MyAccountPage
                    || x.TemplateId == Templates.LandingPage
                    || x.TemplateId == Templates.Marketplace
                    );

                    query = query.And(x => x._IncludeInSearchResults);
                    var site = Context.Site;
                    var languageSelected = "en";

                    if (site != null)
                    {
                        var cookieKey = site.GetCookieKey("lang");
                        languageSelected = Sitecore.Web.WebUtil.GetCookieValue(cookieKey);
                        if (string.IsNullOrEmpty(languageSelected))
                        {
                            languageSelected = "en";
                        }
                    }

                    query = query.And(x => x.Language.Equals(languageSelected));

                    //Build the query with the Meta title and Meta description 
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        var words = searchText.Split(' ');
                        var expression = PredicateBuilder.False<SearchResult>();
                        if (words.Count() > 1)
                        {
                            foreach (var itemWord in words)
                            {
                                //var expressionInner = PredicateBuilder.False<SearchResult>();
                                expression = expression.Or(x => x._Metatitle.Contains("*" + itemWord + "*").Boost(3f))
                                    .Or(x => x._MetaDescription.Contains("*" + itemWord + "*").Boost(2f));
                                
                            }
                            query = query.And(expression);
                        }
                        else
                        {
                            expression = expression.Or(x => x._Metatitle.Contains("*" + searchText + "*").Boost(3f))
                                .Or(x => x._MetaDescription.Contains("*" + searchText + "*").Boost(2f));

                            query = query.And(expression);
                        }
                        
                    }

                    //Max number of records for the search results
                    var maxNumberResults = Sitecore.Configuration.Settings.GetSetting("MaxNumerSearchResult");

                    var queryable = context.GetQueryable<SearchResult>().Where(query).Take(System.Convert.ToInt32(maxNumberResults));
                    var result = queryable.GetResults();
                    if (result.Hits.Any())
                    {
                        return result.Hits.Select(x => x.Document).ToList();
                    }
                    return new List<SearchResult>();
                }
                catch (Exception e)
                {
                    Sitecore.Diagnostics.Log.Warn(e.Message, e, this);
                    return new List<SearchResult>();
                }
            }
        }
    }
}