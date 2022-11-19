using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.HRBlockTax;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.HRBlockTax
{
    [TestFixture]
    public class HRBlockTaxTests : NeambTestBaseLarge<HRBlockTaxPage>
    {
		/// <summary>
		/// H&R Block Tax
		/// </summary>
		[TestCaseSource(typeof(HRBlockTaxPageTestData),
			nameof(HRBlockTaxPageTestData.TestDataSource),
			new object[] { "HRBlock" })]
		[Test, Category("Content")]
		public void MakeReservation(string username, string password) {

            if (Page.IsLiveEnvironment() || Page.IsQAEnvironment())
                Assert.Ignore("Product disabled in live and qa environment");
            Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<HRBlockTaxPage>(username, password)
				.AssertIsLoaded()
				.ClickOnPrimaryCta()
				.SwitchToNewestTab<HRBlockPage>()
				.AssertIsLoaded()
				.CloseCurrentTab<HRBlockPage>();
		}


		/// <summary>
		/// PDF
		/// </summary>
		[TestCaseSource(typeof(HRBlockTaxPageTestData),
			nameof(HRBlockTaxPageTestData.TestDataSource),
			new object[] { "PDF" })]
		[Test, Category("Content")]
		public void VerifyEfullfillment(string username, string password, string url) {
            if (Page.IsLiveEnvironment() || Page.IsQAEnvironment())
                Assert.Ignore("Product disabled in live and qa environment");
            Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<HRBlockTaxPage>(username, password)
				.AssertIsLoaded()
				.ClickOnSecondaryCta()
				.SwitchToNewestTab<PdfMultirowPage>()
				.LinkIsEqual(url)
				.CloseCurrentTab<PdfMultirowPage>();
		}


	}
}
