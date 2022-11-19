using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaGroupHospitalIncomeInsurancePlan;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaGroupHospitalIncomeInsurancePlan
{
    [TestFixture]
    public class NeaGroupHospitalIncomeInsurancePlanPageTests : NeambTestBaseLarge<NeaGroupHospitalIncomeInsurancePlanPage>
    {
        /// <summary>
        /// NeaGroupHospitalIncomeInsurancePlan
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mdsId"></param>
        [TestCaseSource(typeof(NeaGroupHospitalIncomeInsurancePlanPageTestData),
            nameof(NeaGroupHospitalIncomeInsurancePlanPageTestData.TestDataSource),
            new object[] { "NeaGroupHospitalIncomeInsurancePlan" })]
        [Test, Category("Content")]
        public void ValidateGetAQuoteOrEnroll(string username, string password, string mdsId)
        {
            if (Page.IsQAEnvironment())
                Assert.Ignore("Product not available in QA");

            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaGroupHospitalIncomeInsurancePlanPage>(username, password)
                .AssertIsLoaded()
                .ClickOnPrimaryCta()
                .SwitchToNewestTab<MercerEnrollPage>()
                .AssertIsLoaded()
                .CloseCurrentTab<NeaGroupHospitalIncomeInsurancePlanPage>();
                                   
        }

        [TestCaseSource(typeof(NeaGroupHospitalIncomeInsurancePlanPageTestData),
            nameof(NeaGroupHospitalIncomeInsurancePlanPageTestData.TestDataSource),
            new object[] { "Mercer" })]
        [Test, Category("Content")]
        public void ValidateAccessYourAccount(string username, string password, string coverage, string certificate)
        {
            if (Page.IsQAEnvironment())
                Assert.Ignore("Product not available in QA");

            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaGroupHospitalIncomeInsurancePlanPage>(username, password)
                .AssertIsLoaded()
                .ClickOnSecondaryCta()
                .SwitchToNewestTab<MercerPage>()
                .AssertLinkHotState(coverage, certificate)
                .CloseCurrentTab<NeaGroupHospitalIncomeInsurancePlanPage>();

        }
    }
}
