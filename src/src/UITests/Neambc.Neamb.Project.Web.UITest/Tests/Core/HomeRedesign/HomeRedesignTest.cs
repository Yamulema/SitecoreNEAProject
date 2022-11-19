using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.PostTest;
using Neambc.Neamb.Project.Web.UITest.Tests.Core.Login;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.HomeRedesign
{
    [TestFixture]
    public class HomeRedesignTest : NeambTestBaseLarge<Pages.HomeRedesign.HomeRedesign>
    {
        /// <summary>
        /// Verify the text in Hero Banner when it is cold
        /// </summary>
        /// <param name="textCold">Text to verify</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_ColdAction" })]
        [Test, Category("Core")]
        public void VerifyHeroBannerTitleInCold(string textCold)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .VerifyTitleText(textCold);
        }

        /// <summary>
        /// Verify the text in Hero Banner when it is warm
        /// </summary>
        /// <param name="textWarm">Text to verify</param>
        /// <param name="mdsid">Mdsid to change to warm</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_WarmAction" })]
        [Test, Category("Core")]
        public void VerifyHeroBannerTitleInWarm(string textWarm, string mdsid)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .SetAsWarm<Pages.HomeRedesign.HomeRedesign>(mdsid)
                .VerifyTitleText(textWarm);

        }

        /// <summary>
        /// Verify the text in Hero Banner when it is hot
        /// </summary>
        /// <param name="textHot">Text to verify</param>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_HotAction" })]
        [Test, Category("Core")]
        public void VerifyHeroBannerTitleInHot(string textHot, string username, string password)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .VerifyTitleText(textHot)
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify SSO in carousel product card. Hot state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="gtmAction">Gtm action to verify</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardHotAction" })]
        [Test, Category("Core")]
        public void VerifyCardSsoInHot(string username, string password,string gtmAction, string partnerLink)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .CheckGtmActionSSOHot(gtmAction)
                .ClickCarouselSSOCardButton()
                .SwitchToNewestTab<AutoBuyingPage>()
                .LinkIsEqual(partnerLink)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify SSO in carousel product card. Warm state
        /// </summary>
        /// <param name="mdsid">Mdsid to change to warm</param>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardWarmAction" })]
        [Test, Category("Core")]
        public void VerifyCardSsoInWarm(string mdsid, string username, string password, string partnerLink)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .SetAsWarm<Pages.HomeRedesign.HomeRedesign>(mdsid)
                .ClickCarouselSSOCardButtonForLoginWarmHot()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .SwitchToNewestTab<AutoBuyingPage>()
                .LinkIsEqual(partnerLink)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify SSO in carousel product card. Cold state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardColdAction" })]
        [Test, Category("Core")]
        public void VerifyCardSsoInCold(string username, string password, string partnerLink)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickCarouselSSOCardButtonForLoginCold()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .SwitchToNewestTab<AutoBuyingPage>()
                .LinkIsEqual(partnerLink)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify LInk in carousel product card. Hot state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="url">Url to open in link</param>
        /// <param name="td0">Row 1 in postparameter page</param>
        /// <param name="td3">Row 4 in postparameter page</param>
        /// <param name="td4">Row 5 in postparameter page</param>
        /// <param name="gtmFunction">Gtm funtion string to compare</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardHotActionLink" })]
        [Test, Category("Core")]
        public void VerifyCardtLinkTokensInHot(string username, string password,string url, string td0, string td3, string td4,string gtmFunction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .CheckGtmActionLinkTokensHot(gtmFunction)
                .ClickCarouselLinkTokensCardButton()
                .SwitchToNewestTab<PostTestPage>()
                .AssertForCarouselCards(url, td0,td3,td4)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify LInk in carousel product card. Warm state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="url">Url to open in link</param>
        /// <param name="td0">Row 1 in postparameter page</param>
        /// <param name="td3">Row 4 in postparameter page</param>
        /// <param name="td4">Row 5 in postparameter page</param>
        /// <param name="gtmFunction">Gtm funtion string to compare</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardHotActionLink" })]
        [Test, Category("Core")]
        public void VerifyCardLinkTokensInWarm(string username, string password, string url, string td0, string td3, string td4, string gtmFunction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .SetAsWarm<Pages.HomeRedesign.HomeRedesign>(td0)
                .ClickCarouselLinkTokensCardButtonForLoginWarmHot()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .SwitchToNewestTab<PostTestPage>()
                .AssertForCarouselCards(url, td0,td3,td4)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify LInk in carousel product card. Cold state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="url">Url to open in link</param>
        /// <param name="td0">Row 1 in postparameter page</param>
        /// <param name="td3">Row 4 in postparameter page</param>
        /// <param name="td4">Row 5 in postparameter page</param>
        /// <param name="gtmFunction">Gtm funtion string to compare</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardHotActionLink" })]
        [Test, Category("Core")]
        public void VerifyCardLinkTokensInCold(string username, string password, string url, string td0, string td3, string td4, string gtmFunction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickCarouselLinkTokensCardButtonForLoginCold()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .SwitchToNewestTab<PostTestPage>()
                .AssertForCarouselCards(url, td0,td3,td4)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify Datapass in carousel product card. Hot state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="userInfo">User name to be verified in partner page</param>
        /// <param name="gtmAction">Gtm funtion string to be verified</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardHotActionDatapass" })]
        [Test, Category("Core")]
        public void VerifyCardDatapassInHot(string username, string password, string userInfo,string gtmAction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .CheckGtmActionDatapassHot(gtmAction)
                .ClickCarouselDatapassCardButton()
                .SwitchToNewestTab<PlanEnrollmentPage>()
                .AssertHotState(userInfo)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify Datapass in carousel product card. Warm state
        /// </summary>
        /// <param name="mdsid">Mdsid to convert to warm</param>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="userInfo">User name to be verified in partner page</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardWarmActionDatapass" })]
        [Test, Category("Core")]
        public void VerifyCardDatapassInWarm(string mdsid,string username, string password, string userInfo)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .SetAsWarm<Pages.HomeRedesign.HomeRedesign>(mdsid)
                .ClickCarouselDatapassCardButtonForLoginWarmHot()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .SwitchToNewestTab<PlanEnrollmentPage>()
                .AssertHotState(userInfo)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }
        /// <summary>
        /// Verify Datapass in carousel product card. Cold state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="userInfo">User name to be verified in partner page</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardColdActionDatapass" })]
        [Test, Category("Core")]
        public void VerifyCardDatapassInCold(string username, string password, string userInfo)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickCarouselDatapassCardButtonForLoginCold()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .SwitchToNewestTab<PlanEnrollmentPage>()
                .AssertHotState(userInfo)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify Efulfillment in carousel product card. Hot state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="url">Url to open in action</param>
        /// <param name="gtmAction">Gtm action function as string</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardHotActionEfulfillment" })]
        [Test, Category("Core")]
        public void VerifyCardEfulfillmentInHot(string username, string password, string url, string gtmAction)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .ClickButtonNextCarousel()
                .CheckGtmActionEfulfillmentHot(gtmAction)
                .ClickCarouselEfulfillmentCardButton()
                .SwitchToNewestTab<PdfMultirowPage>()
                .LinkIsEqual(url)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime().ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify Efulfillment in carousel product card. Warm state
        /// </summary>
        /// <param name="mdsid">Mdsid to convert to warm</param>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="url">Url to open in action</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardWarmActionEfulfillment" })]
        [Test, Category("Core")]
        public void VerifyCardEfulfillmentInWarm(string mdsid,string username, string password, string url)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .SetAsWarm<Pages.HomeRedesign.HomeRedesign>(mdsid)
                .ClickButtonNextCarousel()
                .ClickCarouselEfulfillmentCardButtonForLoginWarmHot()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
            .SwitchToNewestTab<PdfMultirowPage>()
            .LinkIsEqual(url)
            .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime()
                .ClickOnSignOutLink();
        }

        /// <summary>
        /// Verify Efulfillment in carousel product card. Cold state
        /// </summary>
        /// <param name="username">User name to logged in </param>
        /// <param name="password">Password to logged in</param>
        /// <param name="url">Url to open in action</param>
        [TestCaseSource(typeof(HomeRedesignTestData),
            nameof(HomeRedesignTestData.TestDataSource),
            new object[] { "Test_CarouselCardWarmActionEfulfillmentCold" })]
        [Test, Category("Core")]
        public void VerifyCardEfulfillmentInCold(string username, string password, string url)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("[UI test HomeRedesign] does not exist in live environment");
            Page.AssertIsLoaded()
                .ClickButtonNextCarousel()
                .ClickCarouselEfulfillmentCardButtonForLoginCold()
                .SignIn<Pages.HomeRedesign.HomeRedesign>(username, password)
                .SwitchToNewestTab<PdfMultirowPage>()
                .LinkIsEqual(url)
                .CloseCurrentTab<Pages.HomeRedesign.HomeRedesign>()
                .WaitTime()
                .ClickOnSignOutLink();
        }
    }
}
