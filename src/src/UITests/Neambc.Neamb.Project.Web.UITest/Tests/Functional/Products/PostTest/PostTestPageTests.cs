using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.PostTest;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Products.PostTest
{
    [TestFixture]
    public class PostTestPageTests : NeambTestBaseLarge<PostTestPage>
    {
        /// <summary>
        /// Test hot state when the user clicks on a primary cta button and has some post data
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(PostTestPageData),
            nameof(PostTestPageData.TestDataSource),
            new object[] { "PostTestCtaPageHot" })]
        [Test, Category("Core")]
        public void TestCtaButtonHotState(string username, string password, string url,string td0, string td1)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
			Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<PostTestPage>(username, password)
				.ClickCtaButtonHotState()
				.SwitchToNewestTab<PostTestPage>()
                .AssertHotStateCtaButton(url,td0,td1)
                .CloseCurrentTab<PostTestPage>()
                .ClickOnSignOutLink();
        }
        /// <summary>
        /// Test hot state when the user clicks on a multicard cta button and has some post data
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(PostTestPageData),
            nameof(PostTestPageData.TestDataSource),
            new object[] { "PostTestMultiCardPageHot" })]
        [Test, Category("Core")]
        public void TestMultiCardButtonHotState(string username, string password, string url, string td0)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<PostTestPage>(username, password)
                .ClickMultiCardHotState()
                .SwitchToNewestTab<PostTestPage>()
                .AssertHotStateMultiCardSpecialButton(url, td0)
                .CloseCurrentTab<PostTestPage>()
                .ClickOnSignOutLink();
        }
        /// <summary>
        /// Test hot state when the user clicks on a special offer button and has some post data
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(PostTestPageData),
            nameof(PostTestPageData.TestDataSource),
            new object[] { "PostTestSpecialPageHot" })]
        [Test, Category("Core")]
        public void TestSpecialButtonHotState(string username, string password, string url, string td0)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<PostTestPage>(username, password)
                .ClickSpecialOfferHotState()
                .SwitchToNewestTab<PostTestPage>()
                .AssertHotStateMultiCardSpecialButton(url, td0)
                .CloseCurrentTab<PostTestPage>()
                .ClickOnSignOutLink();
        }

        /// <summary>
        /// Test warm state when the user clicks on a primary cta button and has some post data
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(PostTestPageData),
            nameof(PostTestPageData.TestDataSource),
            new object[] { "PostTestCtaPageWarm" })]
        [Test, Category("Core")]
        public void TestCtaButtonWarmState(string mdsId, string username, string password, string url, string td0, string td1)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
			Page.SetAsWarm<PostTestPage>(mdsId)
				.ClickCtaButtonRedirectLogin()
				.SignIn<PostTestPage>(username, password)
				.SwitchToNewestTab<PostTestPage>()
				.AssertHotStateCtaButton(url, td0, td1)
				.CloseCurrentTab<PostTestPage>()
                .ClickOnSignOutLink();
        }

        /// <summary>
        /// Test warm state when the user clicks on a multi card button and has some post data
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(PostTestPageData),
            nameof(PostTestPageData.TestDataSource),
            new object[] { "PostTestMultiCardPageWarm" })]
        [Test, Category("Core")]
        public void TestMultiCardButtonWarmState(string mdsId, string username, string password, string url, string td0)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
			Page.SetAsWarm<PostTestPage>(mdsId)
				.ClickMultiCardButtonRedirectLogin()
				.SignIn<PostTestPage>(username, password)
                .SwitchToNewestTab<PostTestPage>()
                .AssertHotStateMultiCardSpecialButton(url, td0)
                .CloseCurrentTab<PostTestPage>()
                .ClickOnSignOutLink();
        }

        /// <summary>
        /// Test warm state when the user clicks on a special offer button and has some post data
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(PostTestPageData),
            nameof(PostTestPageData.TestDataSource),
            new object[] { "PostTestSpecialOfferPageWarm" })]
        [Test, Category("Core")]
        public void TestSpecialOfferButtonWarmState(string mdsId, string username, string password, string url, string td0)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[QA do not delete] folder does not exist in live environment");
            Page.SetAsWarm<PostTestPage>(mdsId)
                .ClickSpecialOfferButtonRedirectLogin()
                .SignIn<PostTestPage>(username, password)
                .SwitchToNewestTab<PostTestPage>()
                .AssertHotStateMultiCardSpecialButton(url, td0)
                .CloseCurrentTab<PostTestPage>()
                .ClickOnSignOutLink();
        }

    }
}
