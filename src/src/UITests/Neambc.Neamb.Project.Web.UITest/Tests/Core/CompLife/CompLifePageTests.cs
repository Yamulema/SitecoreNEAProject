using Neambc.Neamb.Project.Web.UITest.Pages.CompLife;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Core.CompLife
{
    [TestFixture]
    public class CompLifeTests : NeambTestBaseLarge<CompLifePage>
    {
        [TestCaseSource(typeof(CompLifePageTestData),
                  nameof(CompLifePageTestData.TestDataSource),
                  new object[] { "Test_GTM" })]
        [Test, Category("Content")]

        public void CompLife_Submit_GTM(string username, string password, string gtmPrimaryAction, string housing)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<CompLifePage>(username, password)
                .AssertIsLoaded().Del_Beneficiary()
                .CompLife_Submit_GTM(gtmPrimaryAction, housing);
        }

        [TestCaseSource(typeof(CompLifePageTestData),
                 nameof(CompLifePageTestData.TestDataSource),
                 new object[] { "Profile" })]
        [Test, Category("Content")]
        public void CompLife_Update_Profile(string username, string password, string FirstName, string LastName, string Phone, string Housing)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<CompLifePage>(username, password)
                .Edit_Profile(FirstName, LastName, Phone)
                .CompLife_Submit(Housing);
        }
        [TestCaseSource(typeof(CompLifePageTestData),
                 nameof(CompLifePageTestData.TestDataSource),
                 new object[] { "Test_Add" })]
        [Test, Category("Content")]
        public void CompLife_Add_Bene(string username, string password, string FirstName, string LastName, string relationship, string payout, string otherentityname, string payout2, string housing)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<CompLifePage>(username, password)
                .CompLife_Add_Bene(FirstName, LastName, relationship, payout)
                .CompLife_Add_Bene_Other(otherentityname, payout2)
                .CompLife_Submit(housing);
        }

        [TestCaseSource(typeof(CompLifePageTestData),
                  nameof(CompLifePageTestData.TestDataSource),
                  new object[] { "Test_Del" })]
        [Test, Category("Content")]
        public void CompLife_Del_Bene(string username, string password, string housing)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<CompLifePage>(username, password)
                .Del_Beneficiary()
                .CompLife_Submit(housing);
        }


    }
}
