using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.AlamoRentalCar;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Products.MultiProducts
{
    [TestFixture]
    public class TestLINKPageTestDateTests : NeambTestBaseLarge<AlamoRentalCarPage>
    {
        /// <summary>
        /// AutoBuying
        /// </summary>
        [TestCaseSource(typeof(TestLINKPageTestData),
            nameof(TestLINKPageTestData.TestDataSource),
            new object[] { "MakeReservation" })]
        [Test, Category("Functional")]
        public void Auto(string username, string password)
        {
            if (Page.IsLiveEnvironment())
                Assert.Ignore("Product disabled in live environment");
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<AlamoRentalCarPage>(username, password)
                .AssertIsLoaded()
                .ClickOnPrimaryCta()
                .SwitchToNewestTab<RentalCarPage>()
                .AssertIsLoaded()
                .CloseCurrentTab<RentalCarPage>();
        }

    }
}
