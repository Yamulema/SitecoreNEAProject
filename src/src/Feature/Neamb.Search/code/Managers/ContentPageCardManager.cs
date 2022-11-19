using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Search.Interfaces;
using Neambc.Neamb.Feature.Search.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Indexing.Enums;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.Indexing.Models;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Search.Managers
{
    [Service(typeof(IContentPageCardManager))]
	public class ContentPageCardManager : IContentPageCardManager {
		protected ISearchManager _SearchManager { get; }
		protected ICacheManager _CacheManager { get; }
		protected List<Tuple<FilterType, ID>> FieldFilters = new List<Tuple<FilterType, ID>>();
		protected List<ID> TemplateFilters = new List<ID>();
		protected string CacheKeyGroup => "ContentFilter";
		protected IGtmService _GtmService { get; }
		public ContentPageCardManager(ISearchManager searchManager, ICacheManager cacheManager, IGtmService gtmService) {
			_SearchManager = searchManager;
			_CacheManager = cacheManager;
			_GtmService = gtmService;
			_GtmService = gtmService;
			Initialize();
		}

		private void Initialize() {
			TemplateFilters.Add(Templates.PageTypeTemplates.Article);
			TemplateFilters.Add(Templates.PageTypeTemplates.VideoPage);
			TemplateFilters.Add(Templates.PageTypeTemplates.CalculatorPage);
			FieldFilters.Add(new Tuple<FilterType, ID>(FilterType.Goal, Templates.ContentFilter.Fields.AddGoal));
			FieldFilters.Add(new Tuple<FilterType, ID>(FilterType.LifeEvent, Templates.ContentFilter.Fields.AddLifeEvent));
			FieldFilters.Add(new Tuple<FilterType, ID>(FilterType.ProductCategory, Templates.ContentFilter.Fields.AddProductCategory));
			FieldFilters.Add(new Tuple<FilterType, ID>(FilterType.Seasonality, Templates.ContentFilter.Fields.AddSeasonality));
			FieldFilters.Add(new Tuple<FilterType, ID>(FilterType.Topic, Templates.ContentFilter.Fields.AddTopic));
		}
		public ContentFilterResult GetContentPages(Guid pageId, Guid datasourceId, List<string> filters, int skip, int take) {
			var page = Sitecore.Context.Database.GetItem(new ID(pageId));
			var datasource = Sitecore.Context.Database.GetItem(new ID(datasourceId));
			var allCategories = new List<string>();
			var result = new ContentFilterResult() {
				PageCards = GetFilterContentPageCards(page, datasource, filters, skip, take, ref allCategories, out var total).ToList(),
				AllCategories = allCategories,
				Total = total
			};
			return result;
		}
		private IEnumerable<ContentPageCard> GetFilterContentPageCards(Item page, Item datasource, List<string> filters, int skip, int take, ref List<string> allCategories, out int total) {
			IEnumerable<ContentPageCard> result;

			var isEditor = Sitecore.Context.PageMode.IsExperienceEditor || Sitecore.Context.PageMode.IsPreview;

			var key = string.Format("{0}:{1}_{2}", CacheKeyGroup, page.ID.Guid.ToString(), datasource.ID.Guid.ToString());
			Log.Info(string.Format("Checking key {0} in Cache", key), this);
			if (Configuration.CacheEnabled && _CacheManager.ExistInCache(key) && !isEditor) {
				Log.Info(string.Format("Fetching ContentPageCards with key {0} from Cache", key), this);
				result = _CacheManager.RetrieveFromCache<List<ContentPageCard>>(key);
				Log.Info(string.Format("Found {0} ContentPageCards with key {1} from Cache", result.Count(), key), this);
			} else {
				Log.Info(string.Format("Key {0} not found in Cache", key), this);
				Log.Info("Looking for not found in Cache", this);
				result = GetPageCards<ContentPageCard>(datasource);
				if (Configuration.CacheEnabled && !isEditor) {
					Log.Info(string.Format("Fetching ContentPageCards with key {0} from Cache", key), this);
					_CacheManager.StoreInCache(key, result.ToList(), DateTime.Now.AddDays(Configuration.CacheDuration));
				}
			}

			allCategories = result.SelectMany(x => x.Categories).Distinct().ToList();

			if (filters != null && filters.Count > 0 && filters.All(x => !string.IsNullOrEmpty(x))) {
				total = (from item in result
						 from cat in item.Categories
						 from filter in filters
						 where cat.Equals(filter)
						 select item)
					.GroupBy(x => x.Cta)
					.Select(x => x.First()).Count();

				return (from item in result
						from cat in item.Categories
						from filter in filters
						where cat.Equals(filter)
						select item)
						.GroupBy(x => x.Cta)
						.Select(x => x.First())
						.Skip(skip)
						.Take(take);
			} else {
				total = result.Count();
				return result.Skip(skip).Take(take);
			}
		}

		private T GetPageCard<T>(Item page) {
			var result = new ContentPageCard() {
				ThumbnailSrc = page.ImageUrl(Templates.PageInfo.Fields.Thumbnail),
                ThumbnailAlt = GetImageAltField(page),
                Genre = page.Fields[Templates.Article.Fields.Genre]?.Value != null
							? page.Fields[Templates.Article.Fields.Genre].Value != "Article"
								? page.Fields[Templates.Article.Fields.Genre].Value
								: string.Empty
							: string.Empty,
				Title = page.Fields[Templates.PageInfo.Fields.PageTitle].Value,
				Description = page.Fields[Templates.PageInfo.Fields.ShortDescription].Value,
				Cta = LinkManager.GetItemUrl(page),
				GtmAction = _GtmService.GetGtmEvent(new Foundation.Analytics.Gtm.ContentArticle()
				{
					Event = "content",
					ContentTitle = page[Templates.PageInfo.Fields.PageTitle],
					ContentLocation = "spotlight"
				})
			};

			return (T)Convert.ChangeType(result, typeof(T));
		}

		private IEnumerable<T> GetPageCards<T>(Item datasource) {
			var result = new List<T>();
			if (datasource == null) {
				return result;
			}

			var filteredPages = new List<Tuple<Item, List<string>>>();
			filteredPages.AddRange(GetUserDefinedPages(datasource));

			// If the editor doesn't define specific items use current page attributes for search.
			if (filteredPages.Count == 0) {
				filteredPages.AddRange(GetFilterPages(datasource));
			}

			result.AddRange(filteredPages.Select(x => GetCategorizedPageCard<T>(x.Item1, x.Item2)));
			return result;
		}

		private IEnumerable<Tuple<Item, List<string>>> GetUserDefinedPages(Item datasource) {
			var result = new List<Tuple<Item, List<string>>>();

			// Gets a categorized list of the selected match attribute items of the current datasource.
			var matchingAtributes = GetMatchAttributes(datasource);
			var userDefinedItems = ((MultilistField)datasource.Fields[Templates.ContentFilter.Fields.Items])?.GetItems();

			if (userDefinedItems == null) {
				Log.Warn($"ContentFilter items field is null for datasource with id:{datasource.ID}", this);
				return result;
			}

			foreach (var userDefinedItem in userDefinedItems) {
				var categories = new List<string>();
				foreach (var matchingAtribute in matchingAtributes) {
					// MatchingAtribute.Item2 is not needed on this scope. The filter type is what it matters.
					switch (matchingAtribute.Item1) {
						case FilterType.Goal:
							categories.AddRange(
								((MultilistField)userDefinedItem.Fields[Templates.GoalsAttribute.Fields.Goals])
								.GetItems()
								.Select(x => x.Fields[Templates.PageInfo.Fields.PageTitle]?.Value));
							break;
						case FilterType.Topic:
							categories.AddRange(
								((MultilistField)userDefinedItem.Fields[Templates.Attributes.Fields.Topics])
								.GetItems()
								.Select(x => x.Fields[Templates.CategoryItem.Fields.Value]?.Value));
							break;
						case FilterType.ProductCategory:
							categories.AddRange(
								((MultilistField)userDefinedItem.Fields[Templates.ProductCategoriesAttribute.Fields.ProductCategories])
								.GetItems()
								.Select(x => x.Fields[Templates.PageInfo.Fields.PageTitle]?.Value));
							break;
						case FilterType.LifeEvent:
							categories.AddRange(
								((MultilistField)userDefinedItem.Fields[Templates.Attributes.Fields.LifeEvents])
								.GetItems()
								.Select(x => x.Fields[Templates.CategoryItem.Fields.Value]?.Value));
							break;
						case FilterType.Seasonality:
							categories.AddRange(
								((MultilistField)userDefinedItem.Fields[Templates.Attributes.Fields.Seasonality])
								.GetItems()
								.Select(x => x.Fields[Templates.CategoryItem.Fields.Value]?.Value));
							break;
					}
				}
				result.Add(new Tuple<Item, List<string>>(userDefinedItem, categories.Where(x => !string.IsNullOrEmpty(x)).ToList()));
			}
			return result;
		}

		protected T GetCategorizedPageCard<T>(Item page, IEnumerable<string> categories) {
			var result = GetPageCard<ContentPageCard>(page);
			// Removes duplicated categories.
			result.Categories = categories.GroupBy(x => x).Select(x => x.First());
			return (T)Convert.ChangeType(result, typeof(T));
		}

		protected IEnumerable<Tuple<FilterType, Item>> GetMatchAttributes(Item datasource) {
			var result = new List<Tuple<FilterType, Item>>();
			var matchAttributes = ((MultilistField)datasource.Fields[Templates.ContentFilter.Fields.MatchAttribute])?.GetItems()?.Select(x => x.ID);

			if (matchAttributes == null) {
				Log.Warn($"MatchAttribute is null for datasource with id:{datasource.ID}", this);
				return result;
			}

			foreach (var matchAttribute in matchAttributes) {
				if (matchAttribute == Items.Categories.Attributes.Goal) {
					result.Add(new Tuple<FilterType, Item>(FilterType.Goal, ((MultilistField)datasource.Fields[Templates.GoalsAttribute.Fields.Goals]).GetItems().FirstOrDefault()));
				}

				if (matchAttribute == Items.Categories.Attributes.Topic) {
					result.Add(new Tuple<FilterType, Item>(FilterType.Topic, ((MultilistField)datasource.Fields[Templates.Attributes.Fields.Topics]).GetItems().FirstOrDefault()));
				}

				if (matchAttribute == Items.Categories.Attributes.ProductCategory) {
					result.Add(new Tuple<FilterType, Item>(FilterType.ProductCategory, ((MultilistField)datasource.Fields[Templates.ProductCategoriesAttribute.Fields.ProductCategories]).GetItems().FirstOrDefault()));
				}

				if (matchAttribute == Items.Categories.Attributes.LifeEvent) {
					result.Add(new Tuple<FilterType, Item>(FilterType.LifeEvent, ((MultilistField)datasource.Fields[Templates.Attributes.Fields.LifeEvents]).GetItems().FirstOrDefault()));
				}

				if (matchAttribute == Items.Categories.Attributes.Seasonality) {
					result.Add(new Tuple<FilterType, Item>(FilterType.Seasonality, ((MultilistField)datasource.Fields[Templates.Attributes.Fields.Seasonality]).GetItems().FirstOrDefault()));
				}
			}

			return result;
		}

		/// <summary>
		/// Filters and categorizes sitecore items using Sitecore ContentSearch API. This result should be CACHED since its an expensive operation.
		/// </summary>
		/// <param name="datasource"></param>
		/// <returns></returns>
		protected IEnumerable<Tuple<Item, List<string>>> GetFilterPages(Item datasource) {
			var result = new List<Tuple<Item, List<string>>>();

			// Gets the matching items of the current datasource MatchAttribute field. 
			var matchAttributes = GetMatchAttributes(datasource);

			// Gets all the added matching attributes from the current datasource.
			matchAttributes = matchAttributes.Concat(GetAddedTermAttributes(datasource));

			// Get the matching items of the current datasource.
			result.AddRange(GetCategorizedSearchResults(matchAttributes));

            // Apply Global Mask
            result.RemoveAll(IsExcluded);

			// Removes duplicates.
			return result.GroupBy(x => x.Item1.ID)
						 .Select(x => x.First())
						 .OrderByDescending(x => x.Item1[Templates.StatisticsCustom.Fields.LastPublishDate]);
		}
        private bool IsExcluded(Tuple<Item, List<string>> tuple) {
            var configuration = Sitecore.Context.Database.GetItem(Configuration.SiteSettingsId);
            if (configuration == null) {
                return false;
            }
            var includedSeasons = ((MultilistField) configuration.Fields[Templates.SiteSettings.Fields.Seasonality]).GetItems();
            var itemSeasons = ((MultilistField)tuple.Item1.Fields[Templates.Attributes.Fields.Seasonality]).GetItems();
            var result = itemSeasons.All(x => includedSeasons.Any(y => y.ID == x.ID));
            return !result;
        }

        private IEnumerable<Tuple<FilterType, Item>> GetAddedTermAttributes(Item datasource) {
			// Gets all the Matching fields that have at least one selected item.
			var matchAttributeTypes = FieldFilters
				.Where(x => ((MultilistField)datasource.Fields[x.Item2])
					.GetItems().Any());

			return matchAttributeTypes
				.SelectMany(x => {
					return (((MultilistField)datasource.Fields[x.Item2]).GetItems()).Select(y =>
						new Tuple<FilterType, Item>(x.Item1, y));
				});
		}

		private IEnumerable<Tuple<Item, List<string>>> GetCategorizedSearchResults(IEnumerable<Tuple<FilterType, Item>> matchAttributes) {
			var result = new List<Tuple<Item, List<string>>>();

			// Adds field filters.
			var filters = matchAttributes.Where(x => x != null)
				.Where(x => x.Item2 != null)
				.Select(x => new SearchFilter(){Type = x.Item1, Value = x.Item2.ID}).ToList();

			if (!filters.Any()) {
				return result;
			}

			{
				// Adds template filters. 
				filters.AddRange(TemplateFilters.Select(x => new SearchFilter(){ Type = FilterType.Template, Value = x}));

				// Do the Search!
				var searchResults = _SearchManager.GetContentPages(new SearchRequest() {
                    Filters = filters
                });

				// Categorizes all the matched results.
				foreach (var contentPageResult in searchResults.Where(x => x != null)) {
					var categories = new List<string>();

					foreach (var matchAttribute in matchAttributes.Where(x => x != null).Where(x => x.Item2 != null)) {
						switch (matchAttribute.Item1) {
							case FilterType.Goal:
                                if (contentPageResult._Goals != null)
                                {
                                    if (contentPageResult._Goals.Contains(matchAttribute.Item2.ID.Guid))
                                    {
                                        categories.Add(
                                            matchAttribute.Item2.Fields[Templates.CategoryItem.Fields.Value]?.Value);
                                    }
                                }
                                break;
							case FilterType.Topic:
								if (contentPageResult._Topics != null) {
									if (contentPageResult._Topics.Contains(matchAttribute.Item2.ID.Guid)) {
										categories.Add(
											matchAttribute.Item2.Fields[Templates.CategoryItem.Fields.Value]?.Value);
									}
								}
								break;
							case FilterType.ProductCategory:
								if (contentPageResult._ProductCategory != null) {
									if (contentPageResult._ProductCategory.Contains(matchAttribute.Item2.ID.Guid)) {
										categories.Add(
											matchAttribute.Item2.Fields[Templates.PageInfo.Fields.PageTitle]?.Value);
									}
								}
								break;
							case FilterType.LifeEvent:
								if (contentPageResult._LifeEvents != null) {
									if (contentPageResult._LifeEvents.Contains(matchAttribute.Item2.ID.Guid)) {
										categories.Add(matchAttribute.Item2.Fields[Templates.CategoryItem.Fields.Value]
											?.Value);
									}
								}
								break;
							case FilterType.Seasonality:
								if (contentPageResult._Seasonality != null) {
									if (contentPageResult._Seasonality.Contains(matchAttribute.Item2.ID.Guid)) {
										categories.Add(matchAttribute.Item2
											.Fields[Templates.CategoryItem.Fields.Value]?.Value);
									}
								}
								break;
							default:
								Log.Warn(string.Format("No handle for matching filter type of {0}", matchAttribute.Item1), this);
								break;
						}
					}
					result.Add(new Tuple<Item, List<string>>(contentPageResult.GetItem(), categories));
				}
			}

			return result;
		}
        private string GetImageAltField(Item item)
        {
            var imgField = (ImageField)item.Fields["Thumbnail"];
            return imgField.Alt;
        }
    }
}
