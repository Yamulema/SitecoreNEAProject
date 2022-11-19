using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Indexing.Enums;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.Indexing.Models;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Cards.Repositories {
	public abstract class RelatedPageCardDealer : PageCardDealer {
		// Sets the maximum amount of cards that the Carousel is allowed to render.
		protected int MaxCardCount { get; set; }

		/// <summary>
		/// Gets all the cards defined by content and related ones.
		/// </summary>
		/// <param name="page"></param>
		/// <param name="datasource"></param>
		/// <returns></returns>
		public override IEnumerable<T> GetPageCards<T>(Item page, Item datasource) {
			var result = new List<T>();

			if (datasource == null) {
				return result;
			}

			var pages = new List<Item>();
			Datasource = datasource;

			pages.AddRange(MaxCardCount != -1
				? GetUserDefinedPages(datasource).Take(MaxCardCount).Where(x => x != null)
				: GetUserDefinedPages(datasource).Where(x => x != null));

			var pageCount = pages.Count;

			if (pageCount == 0) {
				pages.AddRange(GetRelatedPages(page, datasource, pages));
			}

			// Converts page items into cards.
			result.AddRange(pages.Select(GetPageCard<T>));

            return result;
		}
		/// <summary>
		/// Gets the type of matching attribute.
		/// </summary>
		/// <param name="datasource"></param>
		/// <returns></returns>
		protected abstract IEnumerable<string> GetMatchAttributeTypes(Item datasource);

		/// <summary>
		/// Gets the sitecore Items that match C-014 criteria.
		/// </summary>
		/// <param name="page"></param>
		/// <param name="datasource"></param>
		/// <param name="excludedPages"></param>
		/// <returns></returns>
		protected virtual IEnumerable<Item> GetRelatedPages(Item page, Item datasource, List<Item> excludedPages) {
			var filters = new List<SearchFilter>();
			filters.AddRange(TemplateFilters.Select(x => new SearchFilter(){ Type = FilterType.Template, Value = x}));

			var size = MaxCardCount - excludedPages.Count;
			var matchAttributeTypes = GetMatchAttributeTypes(datasource);

			foreach (var matchAttributeType in matchAttributeTypes) {
				var matchFieldId = new ID(Guid.Empty);
				switch (matchAttributeType) {
					case "Goal": {
						matchFieldId = Templates.GoalsAttribute.Fields.Goals;
						filters.Add(new  SearchFilter(){ Type = FilterType.Goal, Value = ((MultilistField)page.Fields[matchFieldId]).GetItems().FirstOrDefault()?.ID});
						break;
					}
					case "Topic": {
						matchFieldId = Templates.Attributes.Fields.Topics;
                        filters.Add(new SearchFilter() { Type = FilterType.Topic, Value = ((MultilistField)page.Fields[matchFieldId]).GetItems().FirstOrDefault()?.ID });
                            break;
					}
					case "Product Category": {
						matchFieldId = Templates.ProductCategoriesAttribute.Fields.ProductCategories;
                        filters.Add(new SearchFilter() { Type = FilterType.ProductCategory, Value = ((MultilistField)page.Fields[matchFieldId]).GetItems().FirstOrDefault()?.ID });
						break;
					}
					case "Life Event": {
						matchFieldId = Templates.Attributes.Fields.LifeEvents;
                        filters.Add(new SearchFilter() { Type = FilterType.LifeEvent, Value = ((MultilistField)page.Fields[matchFieldId]).GetItems().FirstOrDefault()?.ID });
                            break;
					}
					case "Seasonality": {
						matchFieldId = Templates.Attributes.Fields.Seasonality;
                        filters.Add(new SearchFilter() { Type = FilterType.Seasonality, Value = ((MultilistField)page.Fields[matchFieldId]).GetItems().FirstOrDefault()?.ID });
                            break;
					}
					default:
						break;
				}
			}
			// Gets all the matching pages excluding the current page.
            var excludeIds = new List<ID>();
            excludeIds.Add(page.ID);
            excludeIds.AddRange(excludedPages.Select(x=>x.ID));
            
            return MaxCardCount != -1 ?
				GetContentFromIndex(new SearchRequest() {
                        Filters = filters,
                        ExcludeIds = excludeIds,
                        SortBy = SortBy.LastPublishDateDesc,
                        Take = size
                    })
					.Where(x => x != null)
					.Distinct()
				: GetContentFromIndex(new SearchRequest()
                    {
                        Filters = filters,
                        ExcludeIds = excludeIds,
                        SortBy = SortBy.LastPublishDateDesc
                    })
					.Where(x => x != null)
                    .Distinct();
		}

		private IEnumerable<Item> GetContentFromIndex(SearchRequest searchRequest) {
			return !searchRequest.Filters.Any() ? new List<Item>() : _searchManager.GetContentItems(searchRequest);
		}

		protected RelatedPageCardDealer(ISearchManager searchManager, IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService) : base(searchManager, globalConfigurationManager, gtmService) {
			MaxCardCount = _globalConfigurationManager.MaxCardCount;
		}
	}
}