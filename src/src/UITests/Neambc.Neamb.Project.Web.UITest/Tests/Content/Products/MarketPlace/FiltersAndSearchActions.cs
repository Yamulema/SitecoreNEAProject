using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.MarketPlace;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.MarketPlace
{
    [TestFixture]
    public class FiltersAndSearchActions : NeambTestBaseLarge<MarketPlacePage>
    {
        /// <summary>
        /// Test See More Functionality - Loading of more cards
        /// </summary>
        [Test, Category("Content")]
        public void ClickSeeMoreButton()
        {
            Page.AssertIsLoaded()
               .HasSeeMoreButton();

            var cardsBeforeBtnCLick = Page.GetStoresCountBySelector(".cards .card");
            Page.ClickSeeMoreButton();
            var cardsAfterBtnCLick = Page.GetStoresCountBySelector(".cards .card");

            Assert.IsTrue(cardsAfterBtnCLick > cardsBeforeBtnCLick);
        }

        /// <summary>
        /// Test Search of Store by User Input in Textbox
        /// </summary>
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "SearchStore" })]
        [Test, Category("Core")]
        public void SearchByUserInput(string value)
        {
            Page.AssertIsLoaded()
                //.UncheckPopularOfferFilters()
                .PerformSearchAction(value)
                .CheckLoaders();
            Assert.IsTrue(Page.HasSuggestionBoxVisible(), "Store suggestions not visible");
            
            var cardsBeforeSearch = Page.GetStoresCountBySelector(".cards .card");

            Page.ClickFirstSearchResult();
            Assert.IsTrue(Page.GetStoresCountBySelector(".cards .card") == 1, "Could not click the store searched");

            // Clear Search
            Page.ClickClearSearchResult();
            Assert.IsTrue(cardsBeforeSearch == Page.GetStoresCountBySelector(".cards .card"), "Clear Search not refreshing cards");
        }

        /// <summary>
        /// Test Sorting by Cashback with dropdown
        /// </summary>
        [Test, Category("Core")]
        public void SortingByCashback()
        {
            Page.AssertIsLoaded()
                .PerformSortingByValue("2")
                .PerformSortingByValue("1");

            var valuesSorted = Page.GetCashBackValueFromCards();
            var expectedSortedValues = valuesSorted.OrderByDescending(x => x);
            Assert.IsTrue(valuesSorted.SequenceEqual(expectedSortedValues));
        }

        /// <summary>
        /// Test Resetting of all filters
        /// </summary>
        [Test, Category("Core")]
        public void ResetAllFilters()
        {
            Page.AssertIsLoaded();
        }
    }
}
