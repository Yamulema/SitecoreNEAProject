using Neambc.Neamb.Project.Web.UITest.Pages.PromoTout;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.PromoTout
{
    [TestFixture]
    public class PromoToutTests : NeambTestBaseLarge<PromoToutSection>
    {
        [TestCaseSource(typeof(PromoToutTestData),
            nameof(PromoToutTestData.TestDataSource),
            new object[] { "Test_Eligible" })]
        [Test, Category("Core")]
        public void VerifyPromoToutUIEligible(string mdsId)
        {
            Page.SetAsWarm<PromoToutSection>(mdsId)
                .AssertPromoTout();
        }

        [TestCaseSource(typeof(PromoToutTestData),
            nameof(PromoToutTestData.TestDataSource),
            new object[] { "Test_Not_Eligible" })]
        [Test, Category("Core")]
        public void VerifyPromoToutUINotEligible(string mdsId)
        {
            Page.SetAsWarm<PromoToutSection>(mdsId)
                .AssertNotEligiblePromoTout();
        }
        /// <summary>
        /// MBREQ-1002
        /// </summary>
        [TestCaseSource(typeof(PromoToutTestData),
            nameof(PromoToutTestData.TestDataSource),
            new object[] { "HotState" })]
        [Test, Category("Core")]
        public void CheckGtmHotState(string username, string password,string gtmPrimaryAction)
        {
			Page.AssertIsLoaded()
				.ClickOnSignInLink()
				.SignIn<PromoToutSection>(username, password)
                .CheckGtmActionProductLiteHot(gtmPrimaryAction);
        }

        /// <summary>
        /// MBREQ-1002
        /// </summary>
        [TestCaseSource(typeof(PromoToutTestData),
            nameof(PromoToutTestData.TestDataSource),
            new object[] { "WarmState" })]
        [Test, Category("Core")]
        public void CheckGtmWarmState(string mdsid, string gtmPrimaryAction)
        {
			Page.AssertIsLoaded()
				.SetAsWarm<PromoToutSection>(mdsid)
                .CheckGtmActionProductLiteWarm(gtmPrimaryAction);
        }
    }
}
