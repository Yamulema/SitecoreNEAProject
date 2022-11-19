using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.TestProducts.SingleSignOn;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Products.MultiProducts
{
    [TestFixture]
    public class TestSSOPageTestDataTests : NeambTestBaseLarge<SingleSignOn>
    {
        /// <summary>
        /// AutoBuying
        /// </summary>
        [TestCaseSource(typeof(TestSSOPageTestData),
            nameof(TestSSOPageTestData.TestDataSource),
            new object[] { "Auto" })]
        [Test, Category("Functional")]
        public void Auto(string username, string password, string url)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("Product disabled in live environment");

            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<SingleSignOn>(username, password)
                .AssertIsLoaded()
                .ClickOnPrimaryCta()
                .SwitchToNewestTab<AutoBuyingPage>()
                .LinkIsEqual(url)
                .CloseCurrentTab<SingleSignOn>();
        }

    }
}
