using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.Login
{
    [TestFixture]
    public class ProductPageTests : NeambTestBaseLarge<ProductPage>
    {
        [TestCaseSource(typeof(ProductPageTestData),
            nameof(ProductPageTestData.TestDataSource),
            new object[] { "Test_1756" })]
        [Test, Category("NEAMBMRO-1756")]
        public void ValidateUIElementsPresentedToColdUserAnonymous(string url)
        {
            Page.GoToPage<ProductPage>(url)
                .AssertIsLoadedCold();
        }

        [TestCaseSource(typeof(ProductPageTestData),
        nameof(ProductPageTestData.TestDataSource),
        new object[] { "Test_1268" })]
        [Test, Category("NEAMBMRO-1268")]
        public void ValidateUIElementsPresentedToNotEligibleHotUser(string username, string password, string url)
        {
            Page.GoToPage<ProductPage>(url)
                .AssertClickSignInButton()
                .AssertIsLoaded()
                .SignIn<ProductPage>(username, password)
                .AssertNotEligibleComponents();
            //.ClickOnSignOutLink();
        }
        [TestCaseSource(typeof(ProductPageTestData),
            nameof(ProductPageTestData.TestDataSource),
            new object[] { "Test_1977" })]
        [Test, Category("NEAMBMRO-1977")]
        public void ValidateUIElementsPresentedToNotEligibleWarmUser(string url)
        {
            Page.GoToPage<ProductPage>(url)
                .AssertNotEligibleComponents();
        }
        [TestCaseSource(typeof(ProductPageTestData),
            nameof(ProductPageTestData.TestDataSource),
            new object[] { "Test_1978" })]
        [Test, Category("NEAMBMRO-1978")]
        public void ValidateUIElementsPresentedToNotEligibleWarmColdUser(string url)
        {
            Page.GoToPage<ProductPage>(url)
                .AssertNotEligibleComponents();
        }
    }
}
