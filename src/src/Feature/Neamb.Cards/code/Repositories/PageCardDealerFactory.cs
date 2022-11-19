using Neambc.Neamb.Feature.Cards.Repositories.Enums;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.Config.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Indexing.Interfaces;

namespace Neambc.Neamb.Feature.Cards.Repositories
{
    [Service(typeof(IPageCardDealerFactory))]
    public class PageCardDealerFactory : IPageCardDealerFactory
    {
        protected ISearchManager _searchManager { get; }
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IGtmService _gtmService;
        public PageCardDealerFactory(ISearchManager searchManager, IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService)
        {
            _searchManager = searchManager;
			_globalConfigurationManager = globalConfigurationManager;
            _gtmService = gtmService;
		}
        public IPageCardDealer GetCardDealer(PageCardDealerType Type)
        {
            switch (Type)
            {
                case PageCardDealerType.RelatedContent:
                    return new RelatedContentCardDealer(_searchManager, _globalConfigurationManager, _gtmService);
                case PageCardDealerType.ProductCardCarousel:
                    return new ProductCardCarouselDealer(_searchManager, _globalConfigurationManager, _gtmService);
                case PageCardDealerType.TwoColumnCarousel:
                    return new TwoColumnStackableCardDealer(_searchManager, _globalConfigurationManager, _gtmService);
                case PageCardDealerType.MultiRowProductCards:
                    return new MultiRowProductCardsDealer(_searchManager, _globalConfigurationManager, _gtmService);
                case PageCardDealerType.FiveContentCard:
                    return  new FiveContentCardDealer(_searchManager, _globalConfigurationManager, _gtmService);
                default:
                    throw new Exception(string.Format("There is no concrete class for the requested PageCardDealerType: {0}", Type));
            }
        }
    }
}