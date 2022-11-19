using Neambc.Seiumb.UITests.Pages.Products.Marketplace;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Product.Marketplace
{
    [TestFixture]
    public class NotRegisteredUserScenariosTests : SeiumbTestBaseLarge<MarketPlacePage>
    {
        // -> Click to any store -> Verify Login Modal showing
        // -> Login -> Verify getting back the correct query params in url
        // -> Verify Registration Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "EligibleUserForRakuten" })]
        [Test, Category("Core")]
        public void Verify_EligibleUser_FromColdState(string username, string password, string mdsId, string modalId, string modalRegistration, string modalScreen)
        {
            Page.AssertIsLoaded();
            var storeGuid = Page.GetStoreGuidFromFirstCard();

            Page.ClickOnFirstCard()
                .IsModalVisible(modalId)
                .PerformLoginAction(username, password)
                .AssertIsLoaded()
                .AssertIsExpectedPage("", "?result=1&store=" + storeGuid)
                .IsModalVisible(modalRegistration)
                .IsModalScreenVisible(modalRegistration, modalScreen)
                .IsEmailFilledAndValid(modalRegistration);

            // Click a card to test modals showing again by user interaction
            Page.ClickCloseModal(modalRegistration)
                .ClickOnFirstCard()
                .IsModalVisible(modalRegistration)
                .IsModalScreenVisible(modalRegistration, modalScreen)
                .IsEmailFilledAndValid(modalRegistration);

            //Close Modal and Sign Out
            Page.ClickCloseModal(modalRegistration)
                .ClickOnSignOutLink();
        }

        // -> Click to any store -> Verify Login Modal showing ->
        // -> Login -> Verify getting back the correct query params in url -> Verify Not Eligible Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "NotEligibleUserForRakuten" })]
        [Test, Category("Core")]
        public void Verify_NotEligibleUser_FromColdState(string username, string password, string mdsId, string modalId, string modalNotEligible)
        {
            Page.AssertIsLoaded()
                .ClickOnFirstCard()
                .IsModalVisible(modalId)
                .PerformLoginAction(username, password)
                .AssertIsLoaded()
                .AssertIsExpectedPage("", "?result=2")
                .HasNotEligibleBanner()
                .IsModalVisible(modalNotEligible);

            // Click a card to test modals showing again by user interaction
            Page.ClickCloseModal(modalNotEligible)
                .ClickOnFirstCard()
                .IsModalVisible(modalNotEligible);

            //Close Modal and Sign Out
            Page.ClickCloseModal(modalNotEligible)
                .ClickOnSignOutLink();
        }

        // -> Set The Page to Warm state -> Click to any store -> Verify Login Modal showing
        // -> Login -> Verify getting back the correct query params in url
        // -> Verify Registration Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "EligibleUserForRakuten" })]
        [Test, Category("Core")]
        public void Verify_EligibleUser_FromWarmState(string username, string password, string mdsId, string modalId, string modalRegistration, string modalScreen)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<MarketPlacePage>(mdsId)
                .AssertIsLoaded();

            var storeGuid = Page.GetStoreGuidFromFirstCard();
            Page.ClickOnFirstCard()
                .IsModalVisible(modalId)
                .PerformLoginAction(username, password)
                .AssertIsLoaded()
                .AssertIsExpectedPage("", "?result=1&store=" + storeGuid)
                .IsModalVisible(modalRegistration)
                .IsModalScreenVisible(modalRegistration, modalScreen)
                .IsEmailFilledAndValid(modalRegistration);

            //Close Modal and Sign Out
            Page.ClickCloseModal(modalRegistration)
                .ClickOnSignOutLink();
        }

        // -> Set The Page to Warm state -> Click to any store -> Verify Login Modal showing
        // -> Login -> Verify getting back the correct query params in url
        // -> Verify Not Eligible Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "NotEligibleUserForRakuten" })]
        [Test, Category("Core")]
        public void Verify_NotEligibleUser_FromWarmState(string username, string password, string mdsId, string modalId, string modalNotEligible)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<MarketPlacePage>(mdsId)
                .AssertIsLoaded()
                .HasNotEligibleBanner()
                .ClickOnFirstCard()
                .IsModalVisible(modalNotEligible);
        }

        // -> Login -> Click to any store -> Verify Registration Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "EligibleUserForRakuten" })]
        [Test, Category("Core")]
        public void Verify_EligibleUser_FromHotState(string username, string password, string mdsId, string modalId, string modalRegistration, string modalScreen)
        {
            Page.AssertIsLoaded()
                .Login<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .ClickOnFirstCard()
                .IsModalVisible(modalRegistration)
                .IsModalScreenVisible(modalRegistration, modalScreen)
                .IsEmailFilledAndValid(modalRegistration);

            //Close Modal and Sign Out
            Page.ClickCloseModal(modalRegistration)
                .ClickOnSignOutLink();
        }

        // -> Login -> Click to any store -> Verify Not Eligible Modal Showing
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "NotEligibleUserForRakuten" })]
        [Test, Category("Core")]
        public void Verify_NotEligibleUser_FromHotState(string username, string password, string mdsId, string modalId, string modalNotEligible)
        {
            Page.AssertIsLoaded()
                .Login<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .HasNotEligibleBanner()
                .ClickOnFirstCard()
                .IsModalVisible(modalNotEligible);
        }
    }
}
