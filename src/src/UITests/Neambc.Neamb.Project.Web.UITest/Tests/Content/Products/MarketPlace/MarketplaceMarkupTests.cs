using Neambc.Neamb.Project.Web.UITest.Pages.MarketPlace;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.MarketPlace
{
    [TestFixture]
    public class MarketplaceMarkupTests : NeambTestBaseLarge<MarketPlacePage>
    {
        /// <summary>
        /// Test if cards container and cards are present in the markup
        /// </summary>
        [Test, Category("Content")]
        public void HasStores()
        {
            Page.AssertIsLoaded()
                .HasStoreCards();
        }

        /// <summary>
        /// Test if store categories are present in the markup
        /// </summary>
        [Test, Category("Content")]
        public void HasCategories()
        {
            Page.AssertIsLoaded()
                .HasStoreCategories();
        }

        /// <summary>
        /// Test if see more button is present in the markup
        /// </summary>
        [Test, Category("Content")]
        public void HasSeeMoreButton()
        {
            Page.AssertIsLoaded()
                .HasSeeMoreButton();
        }

        /// <summary>
        /// Test if filters and sorting are present in the markup when cold user
        /// </summary>
        [Test, Category("Content")]
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
        [Test, Category("Content")]
        public void HasFiltersAndSorting_Hot(string username, string password, string mdsId, string url)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .HasFiltersAndSorting()
                .HasFavoriteStoresFilter();
        }
    }
}
