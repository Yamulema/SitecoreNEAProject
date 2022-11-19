using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaVacations;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaVacations
{
    [TestFixture]
    public class NeaVacationsPageTests : NeambTestBaseLarge<NeaVacationsPage>
    {
        /// <summary>
        /// Ice
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mdsId"></param>
        [TestCaseSource(typeof(NeaVacationsPageTestData),
            nameof(NeaVacationsPageTestData.TestDataSource),
            new object[] { "Ice" })]
        [Test, Category("Content")]
        public void Ice(string username, string password, string mdsId)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaVacationsPage>(username, password)
                .AssertIsLoaded()
                .ClickOnPrimaryCta()
                .SwitchToNewestTab<IcePage>()
                .AssertHotState(mdsId)
                .SwitchToNewestTab<NeaVacationsPage>();
        }
    }
}
