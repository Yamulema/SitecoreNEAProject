using Neambc.Seiumb.UITests.Pages.NavigationSection;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.NavigationSection
{
    [TestFixture]
    public class GTMNavigationTests :  SeiumbTestBaseLarge<Navigation>
    {

        [TestCaseSource(typeof(GTMNavigationTestData),
            nameof(GTMNavigationTestData.TestDataSource),
            new object[] { "Test_GTM_TopNav" })]
        [Test, Category("GTM")]
        public void ValidateGTMTopNavigation(string gtmAction)
        {
            Page.CheckGtmActionTopNav(gtmAction);
        }

        [TestCaseSource(typeof(GTMNavigationTestData),
            nameof(GTMNavigationTestData.TestDataSource),
            new object[] { "Test_GTM_LeftNav" })]
        [Test, Category("GTM")]
        public void ValidateGTMLeftNavigation(string gtmAction)
        {
            Page.CheckGtmActionLeftNav(gtmAction);
        }

        [TestCaseSource(typeof(GTMNavigationTestData),
            nameof(GTMNavigationTestData.TestDataSource),
            new object[] { "Test_GTM_Footer" })]

        [Test, Category("GTM")]
        public void ValidateGtmFooter(string gtmAction)
        {
            Page.CheckGtmActionFooter(gtmAction);
        }
    }
}
