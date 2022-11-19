using Moq;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Feature.Cards.Repositories.Enums;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using NUnit.Framework;

namespace Neambc.Neamb.Feature.Cards.UnitTest.RelatedContentCardDealer {
	[TestFixture]
	public class GetPageCardsShould {
        #region Fields
        protected Mock<ISearchManager> _searchManager;
        protected Mock<IGlobalConfigurationManager> _globalConfigurationManager;
        protected Mock<IGtmService> _gtmService;
        protected IPageCardDealer _sut;
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
			_sut = new Repositories.RelatedContentCardDealer(_searchManager.Object, _globalConfigurationManager.Object, _gtmService.Object);
        }

        [Test]
		public void Return_EmptyRelatedContentCardsCollection_When_DatasourceIsNull() {
			//Arrange

			//Act
			var result = _sut.GetPageCards<RelatedContentCard>(null, null);

			//Assert
			Assert.IsEmpty(result);
		}
		[Test]
		public void Return_EmptyRelatedContentCardsCollection_When_PageIsNull() {
			//Arrange

			//Act
			var result = _sut.GetPageCards<RelatedContentCard>(null, null);

			//Assert
			Assert.IsEmpty(result);
		}
	}
}
