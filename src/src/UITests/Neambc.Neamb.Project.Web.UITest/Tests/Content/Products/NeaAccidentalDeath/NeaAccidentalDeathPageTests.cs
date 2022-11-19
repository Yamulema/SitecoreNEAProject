using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaAccidentalDeath;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaAccidentalDeath
{
    [TestFixture]
    public class NeaAccidentalDeathPageTests : NeambTestBaseLarge<NeaAccidentalDeathPage>
    {
        /// <summary>
        /// MemberEnroll
        /// </summary>
        [TestCaseSource(typeof(NeaAccidentalDeathPageTestData),
            nameof(NeaAccidentalDeathPageTestData.TestDataSource),
            new object[] { "MemberEnroll" })]
        [Test, Category("Content")]
        public void MemberEnroll(string username, string password, string firstName, string lastName, string dob)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaAccidentalDeathPage>(username, password)
                .AssertIsLoaded()
                .ClickOnPrimaryCta();
            var x = Page.SwitchToNewestTab<MemberEnrollPage>()
            .AssertFormIsLoaded(firstName, lastName, dob)
            .CloseCurrentTab<NeaAccidentalDeathPage>();
        }


        /// <summary>
        /// Mercer
        /// </summary>
        [TestCaseSource(typeof(NeaAccidentalDeathPageTestData),
            nameof(NeaAccidentalDeathPageTestData.TestDataSource),
            new object[] { "Mercer" })]
        [Test, Category("Content")]
        public void Mercer(string username, string password, string coverage, string certificate)
        {
            var x = Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaAccidentalDeathPage>(username, password)
                .AssertIsLoaded()
                .ClickOnSecondaryCta()
                .SwitchToNewestTab<MercerPage>();
            x.AssertLinkHotState(coverage, certificate)
            .CloseCurrentTab<NeaAccidentalDeathPage>();
        }


    }
}
