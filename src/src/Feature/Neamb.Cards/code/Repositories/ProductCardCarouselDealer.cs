using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Sitecore.Data.Fields;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Cards.Repositories
{
    public class ProductCardCarouselDealer : RelatedPageCardDealer
    {
        #region Overridden Methods
        protected override T GetPageCard<T>(Item page)
        {
            var ctaText = "Learn More"; 

            var result = new ProductCardCarousel()
            {
                Title = page.Fields[Templates.PageInfo.Fields.PageTitle].Value,
                Description = page.Fields[Templates.PageInfo.Fields.ShortDescription].Value,
                Cta = LinkManager.GetItemUrl(page),
                TermsAndConditionsCta = string.Format("{0}#{1}", LinkManager.GetItemUrl(page), _globalConfigurationManager.TermsAndConditionsControlId),
                OnClickEvent = _gtmService.GetOnClickEvent(new Card()
                {
                    Event = "call card",
                    CardTitle = page?.Fields[Templates.PageInfo.Fields.PageTitle]?.Value,
                    CtaText = ctaText
                })
            };
            return (T)Convert.ChangeType(result, typeof(T));
        }
        protected override IEnumerable<Item> GetUserDefinedPages(Item datasource)
        {
            return ((MultilistField)datasource.Fields[Templates.ProductCardsCarousel.Fields.Items]).GetItems();
        }

        protected override IEnumerable<string> GetMatchAttributeTypes(Item datasource)
        {
            return new List<string>(new string[] { datasource.Fields[Templates.ProductCardsCarousel.Fields.MatchAttribute]?.Value });
        }
        #endregion

        public ProductCardCarouselDealer(ISearchManager searchManager, IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService) : base(searchManager, globalConfigurationManager, gtmService)
        {
			TemplateFilters.Add(Templates.PageTypeTemplates.Product);
            TemplateFilters.Add(Templates.PageTypeTemplates.MarketPlace);
        }
	}
}