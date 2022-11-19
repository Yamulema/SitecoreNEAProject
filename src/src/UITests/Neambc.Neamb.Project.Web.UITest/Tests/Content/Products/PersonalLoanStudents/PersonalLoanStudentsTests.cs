using Neambc.Neamb.Project.Web.UITest.Pages.Products.PersonalLoanStudents;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.PersonalLoanStudents
{
    [TestFixture]
    public class PersonalLoanStudentsPageTests : NeambTestBaseLarge<PersonalLoanStudentsPage>
    {
        [TestCaseSource(typeof(PersonalLoanStudentsPageTestData),
            nameof(PersonalLoanStudentsPageTestData.TestDataSource),
            new object[] { "ColdState" })]
        [Test, Category("Content")]
        public void CheckGtmColdState(string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionProductLiteCold(gtmPrimaryAction, gtmSecondaryAction);
        }


        [TestCaseSource(typeof(PersonalLoanStudentsPageTestData),
            nameof(PersonalLoanStudentsPageTestData.TestDataSource),
            new object[] { "WarmState" })]
        [Test, Category("Content")]
        public void CheckGtmWarmState(string mdsid, string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<PersonalLoanStudentsPage>(mdsid)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteWarm(gtmPrimaryAction, gtmSecondaryAction);
        }

        [TestCaseSource(typeof(PersonalLoanStudentsPageTestData),
            nameof(PersonalLoanStudentsPageTestData.TestDataSource),
            new object[] { "HotState" })]
        [Test, Category("Content")]
        public void CheckGtmHotState(string username, string password, string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<PersonalLoanStudentsPage>(username, password)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteHot(gtmPrimaryAction, gtmSecondaryAction);
        }

    }
}
