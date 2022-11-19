using Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaLTC;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Content.Products.NEALongTermCare
{
    [TestFixture]
    public class NEALTCPageTests : NeambTestBaseLarge<NeaLTCPage>
    {

        [TestCaseSource(typeof(NeaLTCPageTestData),
            nameof(NeaLTCPageTestData.TestDataSource),
            new object[] { "ColdState" })]
        [Test, Category("Content")]
        public void CheckGtmColdState(string gtmColdCTA, string gtmStickyColdCTA)
        {
            Page.AssertIsLoaded()
                .CheckGtmActionProductLiteCold(gtmColdCTA,  gtmStickyColdCTA);
        }


        [TestCaseSource(typeof(NeaLTCPageTestData),
            nameof(NeaLTCPageTestData.TestDataSource),
            new object[] { "WarmState" })]
        [Test, Category("Content")]
        public void CheckGtmWarmState(string mdsid, string gtmPrimaryAction, string gtmSecondaryAction, string gtmStickyPrimaryAction, string gtmStickySecondaryAction)
        {
            Page.SetAsWarm<NeaLTCPage>(mdsid)
                .CheckGtmActionProductLiteWarm(gtmPrimaryAction, gtmSecondaryAction, gtmStickyPrimaryAction, gtmStickySecondaryAction);
        }

        [TestCaseSource(typeof(NeaLTCPageTestData),
            nameof(NeaLTCPageTestData.TestDataSource),
            new object[] { "HotState" })]
        [Test, Category("Content")]
        public void CheckGtmHotState(string username, string password, string gtmPrimaryAction, string gtmSecondaryAction, string gtmStickyPrimaryAction, string gtmStickySecondaryAction)
        {
            Page.AssertIsLoaded()
                .ClickOnSignInLink()
                .SignIn<NeaLTCPage>(username, password)
                .AssertIsLoaded()
                .CheckGtmActionProductLiteHot(gtmPrimaryAction, gtmSecondaryAction, gtmStickyPrimaryAction, gtmStickySecondaryAction);
        }

    }
}
