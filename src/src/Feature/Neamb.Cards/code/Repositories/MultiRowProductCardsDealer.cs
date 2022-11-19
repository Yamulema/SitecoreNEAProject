using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Indexing.Enums;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Cards.Repositories
{
    public class MultiRowProductCardsDealer : RelatedPageCardDealer
    {

        protected override IEnumerable<string> GetMatchAttributeTypes(Item datasource)
        {
            return new[] { "Product Category" };
        }

        protected override T GetPageCard<T>(Item page) {
            var ctaText = Datasource?.Fields[Templates.MultiRowProductCards.Fields.CtaText]?.Value;

            var result = new MultiRowProductCard()
            {
                Item = page,
                Cta = LinkManager.GetItemUrl(page),
                IsComingSoon = ((CheckboxField)page.Fields[Templates.ProductCTAs.Fields.ComingSoon]) != null && ((CheckboxField)page.Fields[Templates.ProductCTAs.Fields.ComingSoon]).Checked,
                OnClickEvent = _gtmService.GetOnClickEvent(new Card() {
                    Event = "call card",
                    CardTitle = page?.Fields[Templates.PageInfo.Fields.PageTitle]?.Value,
                    CtaText = ctaText
                })
            };

            if (!string.IsNullOrEmpty(page.Fields[Templates.Product.Fields.ProductCardLink]?.Value))
            {
                var productCardLink = (LinkField)page.Fields[Templates.Product.Fields.ProductCardLink];

                result.TermsAndConditionsCta = new Tuple<string, string>(
                    productCardLink.GetFriendlyUrl(), productCardLink.Text);
                result.HasTermsAndConditions = true;
            }
            else if (!string.IsNullOrEmpty(page.Fields[Templates.Product.Fields.TermsAndConditions]?.Value))
            {
                result.TermsAndConditionsCta = new Tuple<string, string>(
                    $"{LinkManager.GetItemUrl(page)}#{ _globalConfigurationManager.TermsAndConditionsControlId}", Datasource.Fields[Templates.MultiRowProductCards.Fields.TermsAndConditionsText]?.Value ?? string.Empty);
                result.HasTermsAndConditions = true;
            }
            else
            {
                result.TermsAndConditionsCta = new Tuple<string, string>(string.Empty, string.Empty);
                result.HasTermsAndConditions = false;
            }

            //Overrides the logic of showing or hidding Terms and Conditions based on a flag in Product pages.
            result.HasTermsAndConditions = !((CheckboxField)page.Fields[Templates.Product.Fields.HideTermsAndConditionsOnProductCard]).Checked;

            return (T)Convert.ChangeType(result, typeof(T));
        }

        protected override IEnumerable<Item> GetUserDefinedPages(Item datasource)
        {
            return ((MultilistField)datasource.Fields[Templates.MultiRowProductCards.Fields.Items]).GetItems();
        }

        public MultiRowProductCardsDealer(ISearchManager searchManager, IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService) : base(searchManager, globalConfigurationManager, gtmService)
        {
			MaxCardCount = -1; //Unlimited results.
			TemplateFilters.Add(Templates.PageTypeTemplates.Product);
		}
	}
}