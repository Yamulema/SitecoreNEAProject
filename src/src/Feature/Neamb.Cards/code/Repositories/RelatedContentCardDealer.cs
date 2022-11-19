using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Cards.Repositories {
	public class RelatedContentCardDealer : RelatedPageCardDealer {
		#region Overridden Methods
		protected override T GetPageCard<T>(Item page) {
			var result = new RelatedContentCard() {
				ThumbnailSrc = page.ImageUrl(Templates.PageInfo.Fields.Thumbnail),
                ThumbnailAlt = GetImageAltField(page),
				Genre = page.Template.BaseTemplates.Any(x => x.ID == Templates.Article.ID) ? page.Fields[Templates.Attributes.Fields.Genre].Value ?? string.Empty : string.Empty,
				Title = page.Fields[Templates.PageInfo.Fields.PageTitle].Value,
				Description = page.Fields[Templates.PageInfo.Fields.ShortDescription].Value,
				Cta = LinkManager.GetItemUrl(page),
				GtmAction = _gtmService.GetOnClickEvent(new Foundation.Analytics.Gtm.ContentArticle()
				{
					Event = "content",
					ContentTitle = page[Templates.PageInfo.Fields.PageTitle],
					ContentLocation = "carousel"
				})
			};
			return (T)Convert.ChangeType(result, typeof(T));
		}
		protected override IEnumerable<Item> GetUserDefinedPages(Item datasource) {
			return ((MultilistField)datasource.Fields[Templates.RelatedContent.Fields.Items]).GetItems();
		}
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

			pages.AddRange(MaxCardCount != -1
				? GetUserDefinedPages(datasource).Take(MaxCardCount).Where(x => x != null)
				: GetUserDefinedPages(datasource).Where(x => x != null));

			var pageCount = pages.Count;

			if (pageCount < MaxCardCount) {
				pages.AddRange(GetRelatedPages(page, datasource, pages));
			}

			// Converts page items into cards.
			result.AddRange(pages.Select(GetPageCard<T>));

			return result;
		}
		protected override IEnumerable<string> GetMatchAttributeTypes(Item datasource) {
			return new List<string>(new string[] { datasource.Fields[Templates.RelatedContent.Fields.MatchAttribute]?.Value });
		}

		#endregion

		public RelatedContentCardDealer(ISearchManager searchManager, IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService) : base(searchManager, globalConfigurationManager, gtmService) {
			MaxCardCount = 12;
			TemplateFilters.Add(Templates.PageTypeTemplates.Article);
			TemplateFilters.Add(Templates.PageTypeTemplates.VideoPage);
			TemplateFilters.Add(Templates.PageTypeTemplates.Goal);
		}
        private string GetImageAltField(Item item) {
            var imgField = (ImageField)item.Fields["Thumbnail"];
            return imgField.Alt;
        }
	}
}