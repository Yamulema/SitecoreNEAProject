using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.TestProducts.Efulfillment;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Products.MultiProducts
{
    class TestPDFPageTestDataTests : NeambTestBaseLarge<Efulfillment>
    {
        // <summary>
        // Mercer
        // </summary>
        // This test is not available on PROD
        [TestCaseSource(typeof(TestPDFPageTestData),
            nameof(TestPDFPageTestData.TestDataSource),
            new object[] { "PDF" })]
        [Test, Category("Functional")]
        public void VerifyEfullfillment(string username, string password, string url)
        {
            if (Page.IsLiveEnvironment() )
                Assert.Ignore("Product disabled in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<Efulfillment>(username, password)
                .AssertIsLoaded()
                .ClickOnPrimaryCta()
                .SwitchToNewestTab<PdfMultirowPage>()
				.LinkIsEqual(url)
				.CloseCurrentTab<PdfMultirowPage>();
        }


    }
}

