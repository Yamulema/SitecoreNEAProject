using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaVacations;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.OmniChannel;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Products.OmniChannel
{
    [TestFixture]
    public class ProductTestPageTests : NeambTestBaseLarge<ProductTestPage>
    {
        /// <summary>
        /// Test hot state when the user is eligible for Omni
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(ProductTestPageData),
            nameof(ProductTestPageData.TestDataSource),
            new object[] { "OmniHot" })]
        [Test, Category("Functional")]
        public void TestHotState(string username, string password, string url)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<ProductTestPage>(username, password)
                .AssertIsLoaded()
                .ClickOnPrimaryCta()
                .SwitchToNewestTab<ProductTestPage>()
                .AssertHotState(url)
                .CloseCurrentTab<ProductTestPage>()
                .ClickOnSignOutLink();
        }

        /// <summary>
        /// Test warm state when the user is eligible for Omni
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mdsId"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(ProductTestPageData),
            nameof(ProductTestPageData.TestDataSource),
            new object[] { "OmniWarm" })]
        [Test, Category("Functional")]
        public void TestWarmState(string username, string password, string mdsId, string url)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.SetAsWarm<ProductTestPage>(mdsId)
                .ClickPrimaryButtonRedirectLogin()
                .SignIn<ProductTestPage>(username, password)
                .AssertIsLoaded()
                .SwitchToNewestTab<ProductTestPage>()
                .AssertHotState(url)
                .CloseCurrentTab<ProductTestPage>()
                .ClickOnSignOutLink();
        }

        /// <summary>
        /// Test warm state when the user is eligible for Omni
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mdsId"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(ProductTestPageData),
            nameof(ProductTestPageData.TestDataSource),
            new object[] { "OmniCold" })]
        [Test, Category("Functional")]
        public void TestColdState(string username, string password, string mdsId, string url)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.ClickLoginButtonRedirect()
                .SignIn<ProductTestPage>(username, password)
                .AssertIsLoaded()
                .SwitchToNewestTab<ProductTestPage>()
                .AssertHotState(url)
                .CloseCurrentTab<ProductTestPage>()
                .ClickOnSignOutLink();
        }
    }
}
