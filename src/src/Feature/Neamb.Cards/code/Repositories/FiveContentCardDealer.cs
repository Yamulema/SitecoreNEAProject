using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

namespace Neambc.Neamb.Feature.Cards.Repositories
{
    public class FiveContentCardDealer : RelatedPageCardDealer
    {
        #region Overridden Methods

        protected override T GetPageCard<T>(Item page)
        {
            var result = new FiveContentCard()
            {
                ThumbnailSrc = page.ImageUrl(Templates.PageInfo.Fields.LargeThumbnail),
                ThumbnailAlt = GetImageAltField(page),
                Genre = page.Fields[Templates.Attributes.Fields.Genre]?.Value != null 
                            ? page.Fields[Templates.Attributes.Fields.Genre].Value != "Article"
                                ? page.Fields[Templates.Attributes.Fields.Genre].Value
                                : string.Empty 
                            : string.Empty,
                Title = page.Fields[Templates.PageInfo.Fields.PageTitle].Value,
                Description = page.Fields[Templates.PageInfo.Fields.ShortDescription].Value,
                Cta = LinkManager.GetItemUrl(page),
                Item = page,
				GtmAction = GetGtmAction(page.Fields[Templates.PageInfo.Fields.PageTitle].Value)
			};
            return (T)Convert.ChangeType(result, typeof(T));
        }

		private string GetGtmAction(string title)
		{
			ContentArticle fiveContentCardTracking = new ContentArticle
			{
				Event = "content",
				ContentTitle = title,
				ContentLocation = "category hero banner"
			};
			return _gtmService.GetOnClickEvent(fiveContentCardTracking);
		}

		protected override IEnumerable<Item> GetUserDefinedPages(Item datasource)
        {
            return ((MultilistField)datasource.Fields[Templates.FiveContentItems.Fields.Items]).GetItems();
        }

        protected override IEnumerable<string> GetMatchAttributeTypes(Item datasource)
        {
            //return (((MultilistField)datasource.Fields[Templates.FiveContentItems.Fields.MatchAttribute]).GetItems().Select(x => x.Fields[Templates.CategoryItem.Fields.Value].Value));
            return new List<string>(new string[] { datasource.Fields[Templates.FiveContentItems.Fields.MatchAttribute]?.Value });
        }
        #endregion

        public FiveContentCardDealer(ISearchManager searchManager, IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService) : base(searchManager, globalConfigurationManager, gtmService)
        {
			MaxCardCount = 5;
			TemplateFilters.Add(Templates.PageTypeTemplates.Article);
			TemplateFilters.Add(Templates.PageTypeTemplates.Product);
			TemplateFilters.Add(Templates.PageTypeTemplates.ProductCategory);
			TemplateFilters.Add(Templates.PageTypeTemplates.VideoPage);
		}
        private string GetImageAltField(Item item)
        {
            var imgField = (ImageField)item.Fields["Thumbnail"];
            return imgField.Alt;
        }
    }
}