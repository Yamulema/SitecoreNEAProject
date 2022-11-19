using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.AlamoRentalCar;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.AlamoRentalCar
	{
    [TestFixture]
    public class AlamoRentalCarTests : NeambTestBaseLarge<AlamoRentalCarPage>
    {
		/// <summary>
		/// MemberEnroll
		/// </summary>
		[TestCaseSource(typeof(AlamoRentalCarPageTestData),
			nameof(AlamoRentalCarPageTestData.TestDataSource),
			new object[] { "LoginInfo" })]
		[Test, Category("Content")]
		public void LoadPartnerPage(string username, string password, string url, string mdsId) {
			Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<AlamoRentalCarPage>(username, password)
				.AssertIsLoaded()
				.ClickOnPrimaryCta()
				.SwitchToNewestTab<RentalCarPage>()
                .IsValidUrl<RentalCarPage>()
                .UrlContainsString<RentalCarPage>(url, "?thirdpartyid=" + mdsId)
                .CloseCurrentTab<RentalCarPage>();
		}
    }
}
