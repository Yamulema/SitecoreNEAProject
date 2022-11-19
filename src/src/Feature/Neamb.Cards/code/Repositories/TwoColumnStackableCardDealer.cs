using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Feature.Cards.Repositories.Enums;
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
	public class TwoColumnStackableCardDealer : PageCardDealer {
		#region Overridden Methods
		public override IEnumerable<T> GetPageCards<T>(Item page, Item datasource) {
			var result = new List<T>();

			if (datasource == null) {
				return result;
			}

			// Converts page items into cards.
			result.AddRange(GetUserDefinedPages(datasource).Select(GetPageCard<T>));

			return result;
		}
		protected override IEnumerable<Item> GetUserDefinedPages(Item datasource) {
			return datasource.GetChildren();
		}
		protected override T GetPageCard<T>(Item page) {
			var result = new CarouselPageCard() {
				ThumbnailSrc = page.ImageUrl(Templates.CarouselItem.Fields.Image),
                ThumbnailAlt = GetImageAltField(page)
			};

			var pullTextItem = ((MultilistField)page.Fields[Templates.CarouselItem.Fields.PullText]).GetItems()
				.FirstOrDefault();
			var destinationItem = ((LinkField)page.Fields[Templates.CarouselItem.Fields.Destination]).TargetItem;

			// Defines the related item source.
			var sourceType = pullTextItem != null ? CarouselSourceType.PullText :
				destinationItem != null ? CarouselSourceType.Destination : CarouselSourceType.None;

			switch (sourceType) {
				case CarouselSourceType.PullText: {
					result.Title = pullTextItem.Fields[Templates.PageInfo.Fields.PageTitle].Value;
					result.Description = pullTextItem.Fields[Templates.PageInfo.Fields.ShortDescription].Value;
					result.Cta = LinkManager.GetItemUrl(pullTextItem);
					break;
				}
				case CarouselSourceType.Destination: {
					result.Title = page.Fields[Templates.CarouselItem.Fields.Headline].Value;
					result.Description = page.Fields[Templates.CarouselItem.Fields.Description].Value;
					result.Cta = LinkManager.GetItemUrl(destinationItem);
					result.Target = ((LinkField)page.Fields[Templates.CarouselItem.Fields.Destination]).Target;
					break;
				}
				case CarouselSourceType.None: {
					result.Title = page.Fields[Templates.CarouselItem.Fields.Headline].Value;
					result.Description = page.Fields[Templates.CarouselItem.Fields.Description].Value;
				}
				break;
				default:
					break;
			}
			return (T)Convert.ChangeType(result, typeof(T));
		}
        #endregion
        private string GetImageAltField(Item item)
        {
            var imgField = (ImageField)item.Fields[Templates.CarouselItem.Fields.Image];
            return imgField.Alt;
        }

        public TwoColumnStackableCardDealer(ISearchManager searchManager, IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService) : base(searchManager, globalConfigurationManager, gtmService) {
		}
	}
}