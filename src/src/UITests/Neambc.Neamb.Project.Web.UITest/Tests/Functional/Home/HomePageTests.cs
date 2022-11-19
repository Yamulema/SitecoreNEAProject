using Neambc.Neamb.Project.Web.UITest.Pages.Home;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Home
{
    [TestFixture]
    public class HomePageTests : NeambTestBaseLarge<HomePage>
    {
		/// <summary>
		/// NEAMBMRO-1366
		/// </summary>
		[Test, Category("Functional")]
		public void VerifyUIElementsPresence() {
			Page.AssertIsLoaded();
		}
		/// <summary>
		/// NEAMBMRO-2081
		/// </summary>
		/// <param name="url"></param>
		[TestCaseSource(typeof(HomePageTestData),
			nameof(HomePageTestData.TestDataSource),
			new object[] { "Test_2081" })]
		[Test, Category("Functional")]
		public void VerifyGtmScript(string url,string var1, string var2, string var3, string var4, string var5, string var6) {
			Page.GoToPage<HomePage>(url)
				.ScriptGtm(var1,var2,var3,var4,var5,var6);

		}

	}
}
