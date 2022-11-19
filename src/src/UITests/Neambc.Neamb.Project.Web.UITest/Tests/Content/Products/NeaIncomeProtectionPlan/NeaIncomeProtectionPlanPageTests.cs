using System.Threading;
using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaIncomeProtectionPlan;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaIncomeProtectionPlan
{
    [TestFixture]
    public class NeaIncomeProtectionPlanPageTests : NeambTestBaseLarge<NeaIncomeProtectionPlanPage>
    {
        /// <summary>
        /// NEAMBMRO-1756
        /// </summary>
        [Test, Category("Content")]
        public void ValidateUiElementsPresentedToColdUser()
        {
            Page.AssertIsLoadedCold();
        }

        /// <summary>
        /// NEAMBMRO-1268
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
            nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
            new object[] { "Test_1268" })]
        [Test, Category("Content")]
        public void ValidateUiElementsPresentedToNotEligibleHotUser(string username, string password)
        {
            Page.ClickOnSignInCta()
                .SignIn<NeaIncomeProtectionPlanPage>(username, password)
                .AssertIsLoaded()
                .AssertNotEligibleComponents();
        }

        /// <summary>
        /// NEAMBMRO-1977
        /// </summary>
        /// <param name="mdsId"></param>
        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
            nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
            new object[] { "Test_1977" })]
        [Test, Category("Content")]
        public void ValidateUiElementsPresentedToNotEligibleWarmUser(string mdsId)
        {
            Page.SetAsWarm<NeaIncomeProtectionPlanPage>(mdsId)
                .AssertNotEligibleComponents();
        }

        /// <summary>
        /// NEAMBMRO-1975
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mdsId"></param>
        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
            nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
            new object[] { "Test_1975" })]
        [Test, Category("Content")]
        public void ValidateEligibilityCheckFlowWithEligibleWarmHotUser(string username, string password, string mdsId)
        {
            Page.SetAsWarm<NeaIncomeProtectionPlanPage>(mdsId)
                .AssertIsLoadedHot()
                .ClickOnEnrollNow()
                .SignIn<NeaIncomeProtectionPlanPage>(username, password)
                .AssertIsLoaded();
        }

        /// <summary>
        /// NEAMBMRO-1406
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mdsId"></param>
        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
            nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
            new object[] { "Test_1406" })]
        [Test, Category("Content")]
        public void ValidateEligibilityCheckFlowInitiatedAsEligibleWarmUserCompletedUsingNotEligibleUserCredentials
            (string username, string password, string mdsId)
        {
            Page.SetAsWarm<NeaIncomeProtectionPlanPage>(mdsId)
                .AssertIsLoadedHot()
                .ClickOnEnrollNow()
                .SignIn<NeaIncomeProtectionPlanPage>(username, password)
                .AssertNotEligibleComponents();
        }

        /// <summary>
        /// NEAMBMRO-1262_1
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userInfo"></param>
        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
            nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
            new object[] { "Test_1262_1" })]
        [Test, Category("Content")]
        public void ValidateDatapassProductsFunctionality
            (string username, string password, string userInfo)
        {
            Page.ClickOnSignInCta()
                .SignIn<NeaIncomeProtectionPlanPage>(username, password)
                .AssertIsLoadedHot()
                .ClickOnEnrollNow()
                .SwitchToNewestTab<PlanEnrollmentPage>()
                .AssertHotState(userInfo);
        }

        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
            nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
            new object[] { "Test_1262_2" })]
        [Test, Category("Content")]
        public void ValidatePrintAnApplication
            (string username, string password, string mdsId, string url)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaIncomeProtectionPlanPage>(username, password)
                .AssertIsLoaded()
                .ClickOnPrintAnApplication()
                .SwitchToNewestTab<NeaIncomeProtectionPlanPdf>()
                .UrlContainsString<NeaIncomeProtectionPlanPdf>(url, "")
                .CloseCurrentTab<NeaIncomeProtectionPlanPdf>();

        }
        //MBREQ-1069
        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
           nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
           new object[] { "ColdState" })]
        [Test, Category("Content")]
        public void CheckGtmColdState(string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionProductLiteCold(gtmPrimaryAction, gtmSecondaryAction);
        }


        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
            nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
            new object[] { "WarmState" })]
        [Test, Category("Content")]
        public void CheckGtmWarmState(string mdsid, string gtmPrimaryAction, string gtmSecondaryAction, string gtmStickyPrimaryAction, string gtmStickySecondaryAction)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<NeaIncomeProtectionPlanPage>(mdsid)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteWarm(gtmPrimaryAction, gtmSecondaryAction, gtmStickyPrimaryAction, gtmStickySecondaryAction);
        }

        [TestCaseSource(typeof(NeaIncomeProtectionPlanPageTestData),
            nameof(NeaIncomeProtectionPlanPageTestData.TestDataSource),
            new object[] { "HotState" })]
        [Test, Category("Content")]
        public void CheckGtmHotState(string username, string password, string gtmPrimaryAction, string gtmSecondaryAction, string gtmStickyPrimaryAction, string gtmStickySecondaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaIncomeProtectionPlanPage>(username, password)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteHot(gtmPrimaryAction, gtmSecondaryAction, gtmStickyPrimaryAction, gtmStickySecondaryAction);
        }
    }
}
