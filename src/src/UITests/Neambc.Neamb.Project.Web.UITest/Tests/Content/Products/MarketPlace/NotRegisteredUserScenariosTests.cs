using Neambc.Neamb.Project.Web.UITest.Pages.MarketPlace;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.MarketPlace
{
    [TestFixture]
    public class NotRegisteredUserScenariosTests : NeambTestBaseLarge<MarketPlacePage>
    {
        // -> Click to any store -> Verify sending the correct query params in url
        // -> Login -> Verify getting back the correct query params in url
        // -> Verify Registration Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "EligibleUserForRakuten" })]
        [Test, Category("Content")]
        public void Verify_EligibleUser_FromColdState(string username, string password, string mdsId, string url, string modalId)
        {
            Page.AssertIsLoaded();

            var storeGuid = Page.GetStoreGuidFromFirstCard();
            Page.ClickOnFirstCard()
                .AssertIsExpectedPage(url, "?store=" + storeGuid)
                .GetLoginPage()
                .SignIn<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .AssertIsExpectedPage("", "?result=1&store=" + storeGuid)
                .IsModalVisible(modalId)
                .IsEmailFilledAndValid(modalId);

            // Click a card to test modals showing again by user interaction
            Page.ClickCloseModal(modalId)
                .ClickOnFirstCard()
                .IsModalVisible(modalId)
                .IsEmailFilledAndValid(modalId);

            //Close Modal and Sign Out
            Page.ClickCloseModal(modalId)
                .ClickOnSignOutLink();
        }

        // -> Click to any store -> Verify sending the correct query params in url -> Login -> Verify Not Eligible Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "NotEligibleUserForRakuten" })]
        [Test, Category("Content")]
        public void Verify_NotEligibleUser_FromColdState(string username, string password, string mdsId, string url, string modalId)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("Product disabled in live environment");

            Page.AssertIsLoaded();

            var storeGuid = Page.GetStoreGuidFromFirstCard();
            Page.ClickOnFirstCard()
                .AssertIsExpectedPage(url, "?store=" + storeGuid)
                .GetLoginPage()
                .SignIn<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .AssertIsExpectedPage("", "?result=2")
                .HasNotEligibleBanner()
                .IsModalVisible(modalId);

            // Click a card to test modals showing again by user interaction
            Page.ClickCloseModal(modalId)
                .ClickOnFirstCard()
                .IsModalVisible(modalId);

            //Close Modal and Sign Out
            Page.ClickCloseModal(modalId)
                .ClickOnSignOutLink();
        }

        // Eligible - Test Warm
        // -> Set The Page to Warm state
        // -> Click to any store -> Verify sending the correct query params in url -> Login -> Verify Registration Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "EligibleUserForRakuten" })]
        [Test, Category("Content")]
        public void Verify_EligibleUser_FromWarmState(string username, string password, string mdsId, string url, string modalId)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<MarketPlacePage>(mdsId)
                .AssertIsLoaded();

            var storeGuid = Page.GetStoreGuidFromFirstCard();
            Page.ClickOnFirstCard()
                .AssertIsExpectedPage(url, "?store=" + storeGuid)
                .GetLoginPage()
                .SignIn<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .AssertIsExpectedPage("", "?result=1&store=" + storeGuid)
                .IsModalVisible(modalId)
                .IsEmailFilledAndValid(modalId);

            //Close Modal and Sign Out
            Page.ClickCloseModal(modalId)
                .ClickOnSignOutLink();
        }

        // -> Set The Page to Warm state
        // -> Click to any store -> Verify sending the correct query params in url -> Login -> Verify Not Eligible Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "NotEligibleUserForRakuten" })]
        [Test, Category("Content")]
        public void Verify_NotEligibleUser_FromWarmState(string username, string password, string mdsId, string url, string modalId)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<MarketPlacePage>(mdsId)
                .AssertIsLoaded()
                .HasNotEligibleBanner();

            // Click a card to test modals showing again by user interaction
            Page.ClickOnFirstCard()
                .IsModalVisible(modalId);
        }

        // -> Login -> Click to any store -> Verify sending the correct query params in url -> Verify Registration Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "EligibleUserForRakuten" })]
        [Test, Category("Content")]
        public void Verify_EligibleUser_FromHotState(string username, string password, string mdsId, string url, string modalId)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<MarketPlacePage>(username, password)
                .AssertIsLoaded();

            Page.ClickOnFirstCard()
                .IsModalVisible(modalId)
                .IsEmailFilledAndValid(modalId);

            //Close Modal and Sign Out
            Page.ClickCloseModal(modalId)
                .ClickOnSignOutLink();
        }

        // -> Login -> Click to any store -> Verify sending the correct query params in url -> Verify Not Eligible Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "NotEligibleUserForRakuten" })]
        [Test, Category("Content")]
        public void Verify_NotEligibleUser_FromHotState(string username, string password, string mdsId, string url, string modalId)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("Product disabled in live environment");

            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .HasNotEligibleBanner();

            // Click a card to test modals showing again by user interaction
            Page.ClickOnFirstCard()
                .IsModalVisible(modalId);
        }
    }
}
