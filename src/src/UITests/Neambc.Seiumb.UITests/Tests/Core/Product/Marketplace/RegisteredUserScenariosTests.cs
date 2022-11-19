using Neambc.Seiumb.UITests.Pages.Products.Marketplace;
using Neambc.Seiumb.UITests.Extensions;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Product.Marketplace
{
    [TestFixture]
    public class RegisteredUserScenariosTests : SeiumbTestBaseLarge<MarketPlacePage>
    {
        // -> Click to any store -> Verify Login Modal showing
        // -> Open tab of store clicked -> Validate url of store
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "RegisteredRakutenUser" })]
        [Test, Category("Core")]
        public void Verify_RegisteredRakutenUser_FromColdState(string username, string password, string mdsId, string modalId)
        {
            Page.AssertIsLoaded()
                .ClickOnFirstCard()
                .IsModalVisible(modalId)
                .PerformLoginAction(username, password)
                .AssertIsLoaded()
                .SwitchToNewestTab<MarketPlacePage>()
                .IsStoreLinkValidUrl()
                .CloseCurrentTab<MarketPlacePage>()
                .ClickOnSignOutLink();
        }

        // -> Set The Page to Warm state -> Click to any store -> Verify Login Modal showing
        // -> Open tab of store clicked -> Validate url of store
        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "RegisteredRakutenUser" })]
        [Test, Category("Core")]
        public void Verify_RegisteredRakutenUser_FromWarmState(string username, string password, string mdsId, string modalId)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<MarketPlacePage>(mdsId)
                .AssertIsLoaded()
                .ClickOnFirstCard()
                .IsModalVisible(modalId)
                .PerformLoginAction(username, password)
                .AssertIsLoaded()
                .SwitchToNewestTab<MarketPlacePage>()
                .IsStoreLinkValidUrl()
                .CloseCurrentTab<MarketPlacePage>()
                .ClickOnSignOutLink();
        }

        [TestCaseSource(typeof(MarketPlacePageData),
            nameof(MarketPlacePageData.TestDataSource),
            new object[] { "RegisteredRakutenUser" })]
        [Test, Category("Core")]
        public void Verify_RegisteredRakutenUser_FromHotState(string username, string password, string mdsId, string modalId)
        {
            Page.Login<MarketPlacePage>(username, password)
                .AssertIsLoaded()
                .ClickOnFirstCard()
                .SwitchToNewestTab<MarketPlacePage>()
                .IsStoreLinkValidUrl()
                .CloseCurrentTab<MarketPlacePage>()
                .ClickOnSignOutLink();
        }
    }
}
