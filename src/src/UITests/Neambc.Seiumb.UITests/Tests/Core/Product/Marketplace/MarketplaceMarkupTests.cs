using Neambc.Seiumb.UITests.Pages.Products.Marketplace;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Product.Marketplace
{
    [TestFixture]
    public class MarketplaceMarkupTests : SeiumbTestBaseLarge<MarketPlacePage>
    {
        /// <summary>
        /// Test if cards container and cards are present in the markup
        /// </summary>
        [Test, Category("Core")]
        public void HasStores()
        {
            Page.AssertIsLoaded()
                .HasStoreCards();
        }

        /// <summary>
        /// Test if store categories are present in the markup
        /// </summary>
        [Test, Category("Core")]
        public void HasCategories()
        {
            Page.AssertIsLoaded()
                .HasStoreCategories();
        }

        /// <summary>
        /// Test if see more button is present in the markup
        /// </summary>
        [Test, Category("Core")]
        public void HasSeeMoreButton()
        {
            Page.AssertIsLoaded()
                .HasSeeMoreButton();
        }

        /// <summary>
        /// Test if filters and sorting are present in the markup when cold user
        /// </summary>
        [Test, Category("Core")]
        public void HasFiltersAndSorting_Cold()
        {
            Page.AssertIsLoaded()
                .HasFiltersAndSorting();
        }

        /// <summary>
        /// Test if filters and sorting are present in the markup when hot user
        /// </summary>
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "RegisteredRakutenUser" })]
        [Test, Category("Core")]
        public void HasFiltersAndSorting_Hot(string username, string password, string mdsId, string modalId)
        {
            Page.AssertIsLoaded()
                .Login<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .HasFiltersAndSorting()
                .HasFavoriteStoresFilter();
        }
    }
}
