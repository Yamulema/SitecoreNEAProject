using Neambc.Neamb.Project.Web.UITest.Extensions;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaRetirementProgram;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NeaRetirementProgram
{
    [TestFixture]
    public class NeaRetirementProgramPageTests : NeambTestBaseLarge<NeaRetirementProgramPage>
    {
        /// <summary>
        /// SecurityBenefit
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        [TestCaseSource(typeof(NeaRetirementProgramPageTestData),
            nameof(NeaRetirementProgramPageTestData.TestDataSource),
            new object[] { "SecurityBenefit" })]
        [Test, Category("Content")]
        public void SecurityBenefit(string username, string password)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaRetirementProgramPage>(username, password)
                .AssertIsLoaded()
                .ClickOnSecondaryCta()
                .SwitchToNewestTab<SecurityBenefitPage>()
                .AssertIsLoaded();
        }

        /// <summary>
        /// InvestmentProducts
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        [TestCaseSource(typeof(NeaRetirementProgramPageTestData),
            nameof(NeaRetirementProgramPageTestData.TestDataSource),
            new object[] { "InvestmentProducts" })]
        [Test, Category("Content")]
        public void InvestmentProducts(string username, string password)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaRetirementProgramPage>(username, password)
                .AssertIsLoaded()
                .ClickOnPrimaryCta()
                .SwitchToNewestTab<InvestmentProductsPage>()
                .AssertIsLoaded()
                .CloseCurrentTab<NeaRetirementProgramPage>();
        }

        //MBREQ-1069

        [TestCaseSource(typeof(NeaRetirementProgramPageTestData),
    nameof(NeaRetirementProgramPageTestData.TestDataSource),
    new object[] { "ColdState" })]
        [Test, Category("Content")]
        public void CheckGtmColdState(string gtmPrimaryAction, string gtmSecondaryAction)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionProductLiteCold(gtmPrimaryAction, gtmSecondaryAction);
        }


        [TestCaseSource(typeof(NeaRetirementProgramPageTestData),
            nameof(NeaRetirementProgramPageTestData.TestDataSource),
            new object[] { "WarmState" })]
        [Test, Category("Content")]
        public void CheckGtmWarmState(string mdsid, string gtmPrimaryAction, string gtmSecondaryAction, string gtmStickyPrimaryAction, string gtmStickySecondaryAction)
        {
            Page.AssertIsLoaded()
                .SetAsWarm<NeaRetirementProgramPage>(mdsid)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteWarm(gtmPrimaryAction, gtmSecondaryAction, gtmStickyPrimaryAction, gtmStickySecondaryAction);
        }

        [TestCaseSource(typeof(NeaRetirementProgramPageTestData),
            nameof(NeaRetirementProgramPageTestData.TestDataSource),
            new object[] { "HotState" })]
        [Test, Category("Content")]
        public void CheckGtmHotState(string username, string password, string gtmPrimaryAction, string gtmSecondaryAction, string gtmStickyPrimaryAction, string gtmStickySecondaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaRetirementProgramPage>(username, password)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteHot(gtmPrimaryAction, gtmSecondaryAction, gtmStickyPrimaryAction, gtmStickySecondaryAction);
        }

    }
}
