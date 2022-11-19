using Moq;
using Neambc.Neamb.Feature.Cards.Repositories;
using Neambc.Neamb.Feature.Cards.Repositories.Enums;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using NUnit.Framework;
using Sitecore;

namespace Neambc.Neamb.Feature.Cards.UnitTest.PageCardDealerFactory {
	[TestFixture]
	public class GetCardDealerShould {

        #region Fields
        protected Mock<ISearchManager> _searchManager;
        protected Mock<IGlobalConfigurationManager> _globalConfigurationManager;
        protected Mock<IGtmService> _gtmService;
        protected IPageCardDealerFactory _sut;
        #endregion

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            // set up default mock objects once 
            // tests can still create their own, but 
            // defaults are available, and kept if used
            _searchManager = new Mock<ISearchManager>();
			_globalConfigurationManager = new Mock<IGlobalConfigurationManager>();
            _gtmService = new Mock<IGtmService>();
			_sut = new Repositories.PageCardDealerFactory(_searchManager.Object, _globalConfigurationManager.Object, _gtmService.Object);
        }

        [Test]
		public void Return_RelatedContentCardDealer_When_RelatedContentTypeIsPassed() {
			//Arrange

			//Act
			var result = _sut.GetCardDealer(PageCardDealerType.RelatedContent);

			//Assert
			Assert.IsInstanceOf<Repositories.RelatedContentCardDealer>(result);
		}
		[Test]
		public void Return_ProductCardCarouselDealer_When_ProductCardCarouselTypeIsPassed() {
			//Arrange

			//Act
			var result = _sut.GetCardDealer(PageCardDealerType.ProductCardCarousel);

			//Assert
			Assert.IsInstanceOf<ProductCardCarouselDealer>(result);
		}
		[Test]
		public void Return_TwoColumnStackableCardDealer_When_TwoColumnCarouselTypeIsPassed() {
			//Arrange

			//Act
			var result = _sut.GetCardDealer(PageCardDealerType.TwoColumnCarousel);

			//Assert
			Assert.IsInstanceOf<TwoColumnStackableCardDealer>(result);
		}
		[Test]
		public void Return_MultiRowProductCardsDealer_When_MultiRowProductCardsTypeIsPassed() {
			//Arrange

			//Act
			var result = _sut.GetCardDealer(PageCardDealerType.MultiRowProductCards);

			//Assert
			Assert.IsInstanceOf<MultiRowProductCardsDealer>(result);
		}
		[Test]
		public void Return_FiveContentCardDealer_When_FiveContentCardTypeIsPassed() {
			//Arrange

			//Act
			var result = _sut.GetCardDealer(PageCardDealerType.FiveContentCard);

			//Assert
			Assert.IsInstanceOf<FiveContentCardDealer>(result);
		}
	}
}
