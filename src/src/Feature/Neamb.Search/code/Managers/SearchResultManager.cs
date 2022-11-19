using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Search.Enums;
using Neambc.Neamb.Feature.Search.Interfaces;
using Neambc.Neamb.Feature.Search.Models;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Indexing.Enums;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.Indexing.Models;
using Sitecore.Data.Fields;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Search.Managers
{
    [Service(typeof(ISearchResultManager))]
    public class SearchResultManager : ISearchResultManager
    {
        protected ISearchManager _searchManager { get; }

        public SearchResultManager(ISearchManager searchManager)
        {
            _searchManager = searchManager;
        }
        public IEnumerable<SuggestionResult> GetSuggestions(string term, int? take) {
            var keywords = string.IsNullOrEmpty(term)
                ? new string[0]
                : term.Split(' ').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));

            if (!keywords.Any())
            {
                return new List<SuggestionResult>();
            }

            var filters = new List<SearchFilter>()
            {
                new SearchFilter() {
                    Type = FilterType.Template,
                    Value = Templates.PageTypeTemplates.Product
                },
                new SearchFilter() {
                    Type = FilterType.Template,
                    Value = Templates.PageTypeTemplates.MarketplacePage
                },
                new SearchFilter() {
                    Type = FilterType.PageTitle,
                    Value = string.IsNullOrEmpty(term) ? string.Empty : term
                }
            };
            filters.AddRange(keywords.Select(x => 
                new SearchFilter()
                {
                    Type = FilterType.PageTitle,
                    Value = string.IsNullOrEmpty(x) ? string.Empty : x
                }));

            // Do the search.
            var contentPageResults = _searchManager.GetContentPages(new SearchRequest()
            {
                Filters = filters
            });

            return take == null
                ? contentPageResults.Select(ToSuggestionResult)
                : contentPageResults.Select(ToSuggestionResult).Take(take.Value);
        }

        public Tuple<IEnumerable<SearchResultCard>, int> GetSearchResultCards(IEnumerable<string> termsInput, int? take, int? skip, List<string> genre) {
            List<string> terms = termsInput.ToList();
            if (!terms.Any())
            {
                return new Tuple<IEnumerable<SearchResultCard>, int>(new List<SearchResultCard>(), 0);
            }

            var filters = new List<SearchFilter>();

            filters.AddRange(terms.Select(x => new SearchFilter()
            {
                Type = FilterType.PageTitle,
                Value = string.IsNullOrEmpty(x) ? string.Empty : x
            }));
            filters.AddRange(terms.Select(x => new SearchFilter()
            {
                Type = FilterType.ShortDescription,
                Value = string.IsNullOrEmpty(x) ? string.Empty : x
            }));
            filters.AddRange(terms.Select(x => new SearchFilter()
            {
                Type = FilterType.Body,
                Value = string.IsNullOrEmpty(x) ? string.Empty : x
            }));
            filters.AddRange(genre.Select(x => new SearchFilter()
            {
                Type = FilterType.Genre,
                Value = string.IsNullOrEmpty(x) ? string.Empty : x
            }));
            filters.AddRange(new List<SearchFilter>() {
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.Home},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.Goal},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.ProductCategory},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.Product},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.LandingPage},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.About},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.VideoPage},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.Article},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.GenericPageContentPage},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.Newsletter},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.Sweepstakes},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.ContactUs},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.GuidePage},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.CalculatorPage},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.ContestSubmissionPage},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.ConstestVotePage},
                new SearchFilter(){Type = FilterType.Template, Value = Templates.PageTypeTemplates.MarketplacePage}
            });
            var searchResult = _searchManager.GetContentPages(new SearchRequest()
            {
                Filters = filters
            });
            //searchResult = FilterGenre(searchResult, genre);

            return new Tuple<IEnumerable<SearchResultCard>, int>(
                searchResult.Skip(skip ?? 0).Take(take ?? Configuration.DefaultSearchTake).Select(ToSearchResultCard), searchResult.Count());
        }

        private static SuggestionResult ToSuggestionResult(SearchResult contentPageResult)
        {
            var keywords = string.IsNullOrEmpty(contentPageResult._PageTitle)
                ? new string[0]
                : contentPageResult._PageTitle.Split(' ').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));

            var queryString = string.Join("&",
                keywords.Select(x => string.Format("{0}={1}", Configuration.SearchParmTerm, x)));

            var searchItem = (LinkField)(Sitecore.Context.Database.GetItem(Configuration.SiteSettingsId).Fields[Templates.SiteSettings.Fields.SearchPage]);
            var searchUrl = string.Empty;
            if (searchItem?.TargetItem != null)
            {
                searchUrl = LinkManager.GetItemUrl(searchItem.TargetItem);
            }

            var url = string.Format("{0}?{1}", searchUrl, queryString);
            return new SuggestionResult()
            {
                Title = contentPageResult._PageTitle,
                Url = url
            };
        }

        private static SearchResultCard ToSearchResultCard(SearchResult contentPageResult)
        {
            return new SearchResultCard()
            {
                Title = contentPageResult._PageTitle.Replace("®", "<sup>®</sup>"),
                Description = contentPageResult._ShortDescriptionHtml,
                Cta = contentPageResult.FriendlyUrl,
                Genre = contentPageResult.Genre,
                ThumbnailSrc = $"{contentPageResult._SmallThumbnailUrl}",
                Style = GetSearchResultCardStyle(contentPageResult)
            };
        }

        private static PageResultStyle GetSearchResultCardStyle(SearchResult contentPageResult)
        {
            if (!string.IsNullOrEmpty(contentPageResult.Genre))
            {
                if (contentPageResult.Genre.ToLower() == "benefit")
                {
                    return PageResultStyle.Margin;
                }
            }
            if (contentPageResult.TemplateId == Templates.PageTypeTemplates.Product || contentPageResult.TemplateId == Templates.PageTypeTemplates.MarketplacePage)
            {
                return PageResultStyle.Margin;
            }
            else
            {
                return string.IsNullOrEmpty(contentPageResult._SmallThumbnailUrl)
                    ? PageResultStyle.Simple
                    : PageResultStyle.Thumbnail;
            }
        }
    }
}