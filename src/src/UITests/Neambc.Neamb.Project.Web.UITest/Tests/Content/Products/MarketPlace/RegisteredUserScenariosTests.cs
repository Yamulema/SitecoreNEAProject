using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.MarketPlace;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.MarketPlace
{
    [TestFixture]
    public class RegisteredUserScenariosTests : NeambTestBaseLarge<MarketPlacePage>
    {
        // -> Click to any store -> Verify sending the correct query params in url
        // -> Login -> Verify getting back the correct query params in url
        // -> Open tab of store clicked
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "RegisteredRakutenUser" })]
        [Test, Category("Content")]
        public void Verify_RegisteredRakutenUser_FromColdState(string username, string password, string mdsId, string url)
        {
            Page.AssertIsLoaded();

            var storeGuid = Page.GetStoreGuidFromFirstCard();

            Page.ClickOnFirstCard()
                .AssertIsExpectedPage(url, "?store=" + storeGuid)
                .GetLoginPage()
                .SignIn<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .SwitchToNewestTab<MarketPlacePage>()
                .IsStoreLinkValidUrl()
                .CloseCurrentTab<MarketPlacePage>()
                .ClickOnSignOutLink();
        }

        // -> Set The Page to Warm state
        // -> Click to any store -> Verify sending the correct query params in url
        // -> Login -> Verify getting back the correct query params in url
        // -> Open tab of store clicked
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "RegisteredRakutenUser" })]
        [Test, Category("Content")]
        public void Verify_RegisteredRakutenUser_FromWarmState(string username, string password, string mdsId, string url)
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
                .SwitchToNewestTab<MarketPlacePage>()
                .IsStoreLinkValidUrl()
                .CloseCurrentTab<MarketPlacePage>()
                .ClickOnSignOutLink();
        }

        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "RegisteredRakutenUser" })]
        [Test, Category("Content")]
        public void Verify_RegisteredRakutenUser_FromHotState(string username, string password, string mdsId, string url)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .ClickOnFirstCard()
                .SwitchToNewestTab<MarketPlacePage>()
                .IsStoreLinkValidUrl()
                .CloseCurrentTab<MarketPlacePage>()
                .ClickOnSignOutLink();
           
        }
    }
}
