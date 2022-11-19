using Neambc.Neamb.Project.Web.UITest.Pages.Products.PersonalLoan5k;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.PersonalLoan5k
{
    [TestFixture]
    public class PersonalLoanPageTests : NeambTestBaseLarge<PersonalLoanPage>
    {
        [TestCaseSource(typeof(PersonalLoanPageTestData),
            nameof(PersonalLoanPageTestData.TestDataSource),
            new object[] { "ColdState" })]
        [Test, Category("Content")]
        public void CheckGtmColdState(string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionProductLiteCold(gtmPrimaryAction, gtmSecondaryAction);
        }


        [TestCaseSource(typeof(PersonalLoanPageTestData),
            nameof(PersonalLoanPageTestData.TestDataSource),
            new object[] { "WarmState" })]
        [Test, Category("Content")]
        public void CheckGtmWarmState(string mdsid, string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<PersonalLoanPage>(mdsid)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteWarm(gtmPrimaryAction, gtmSecondaryAction);
        }

        [TestCaseSource(typeof(PersonalLoanPageTestData),
            nameof(PersonalLoanPageTestData.TestDataSource),
            new object[] { "HotState" })]
        [Test, Category("Content")]
        public void CheckGtmHotState(string username, string password, string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<PersonalLoanPage>(username, password)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteHot(gtmPrimaryAction, gtmSecondaryAction);
        }

    }
}
