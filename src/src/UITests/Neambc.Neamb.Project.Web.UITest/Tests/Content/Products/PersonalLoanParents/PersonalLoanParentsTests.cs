using Neambc.Neamb.Project.Web.UITest.Pages.Products.PersonalLoanParents;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.PersonalLoanParents
{
    [TestFixture]
    public class PersonalLoanParentsPageTests : NeambTestBaseLarge<PersonalLoanParentsPage>
    {
        [TestCaseSource(typeof(PersonalLoanParentsPageTestData),
            nameof(PersonalLoanParentsPageTestData.TestDataSource),
            new object[] { "ColdState" })]
        [Test, Category("Content")]
        public void CheckGtmColdState(string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionProductLiteCold(gtmPrimaryAction, gtmSecondaryAction);
        }


        [TestCaseSource(typeof(PersonalLoanParentsPageTestData),
            nameof(PersonalLoanParentsPageTestData.TestDataSource),
            new object[] { "WarmState" })]
        [Test, Category("Content")]
        public void CheckGtmWarmState(string mdsid, string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<PersonalLoanParentsPage>(mdsid)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteWarm(gtmPrimaryAction, gtmSecondaryAction);
        }

        [TestCaseSource(typeof(PersonalLoanParentsPageTestData),
            nameof(PersonalLoanParentsPageTestData.TestDataSource),
            new object[] { "HotState" })]
        [Test, Category("Content")]
        public void CheckGtmHotState(string username, string password, string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<PersonalLoanParentsPage>(username, password)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteHot(gtmPrimaryAction, gtmSecondaryAction);
        }

    }
}
