using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaAutoBuying;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaAutoBuying 
	{
    [TestFixture]
    public class NeaAutoBuyingPageTests : NeambTestBaseLarge<NeaAutoBuyingPage>
    {
        /// <summary>
        /// AutoBuying
        /// </summary>
        [TestCaseSource(typeof(NeaAutoBuyingPageTestData),
            nameof(NeaAutoBuyingPageTestData.TestDataSource),
            new object[] { "Auto" })]
        [Test, Category("Content")]
        public void Auto(string username, string password)
        {
            if (Page.IsQAEnvironment())
                Assert.Ignore("Product not available in QA");

            Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<NeaAutoBuyingPage>(username, password)
				.AssertIsLoaded()
				.ClickOnPrimaryCta()
				.SwitchToNewestTab<AutoBuyingPage>()
                .AssertIsLoaded()
                .CloseCurrentTab<NeaAutoBuyingPage>();
        }
		
    }
}
