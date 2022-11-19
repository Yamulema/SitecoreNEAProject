using Neambc.Seiumb.UITests.Pages.Home;
using Neambc.Seiumb.UITests.Pages.Products.GroupTermLifeInsurance;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Product
{
    [TestFixture]
    public class ProductGroupTermLifeInsuranceTest : SeiumbTestBaseLarge<GroupTermLifeInsurancePage>
    {
        /// <summary>
        /// Test gtm tracking in cold user
        /// </summary>
        [TestCaseSource(typeof(ProductGroupTermLifeInsuranceData),
            nameof(ProductGroupTermLifeInsuranceData.TestDataSource),
            new object[] { "Test_Gtm_Cold_User" })]

        [Test, Category("Core")]
        public void VerifyGtmTrackingValueCold(string registerText, string loginText, string loginMobileText)
        {
            Page.CheckGtmTrackingCold(registerText, loginText, loginMobileText);
        }
        /// <summary>
        /// Test token in warm user
        /// </summary>
        [TestCaseSource(typeof(ProductGroupTermLifeInsuranceData),
            nameof(ProductGroupTermLifeInsuranceData.TestDataSource),
            new object[] { "Test_Gtm_Warm_User" })]

        [Test, Category("Core")]
        public void VerifyGtmValueWarm(string mdsId, string primaryAction, string secondaryAction)
        {
            Page.SetAsWarm<GroupTermLifeInsurancePage>(mdsId)
                .CheckGtmTrackingWarm(primaryAction, secondaryAction);
        }
        /// <summary>
        /// Test token in hot user
        /// </summary>
        [TestCaseSource(typeof(ProductGroupTermLifeInsuranceData),
            nameof(ProductGroupTermLifeInsuranceData.TestDataSource),
            new object[] { "Test_Gtm_Hot_User" })]

        [Test, Category("Core")]
        public void VerifyGtmTrackingValueHot(string username, string password,string primaryAction, string secondaryAction)
        {
            Page.Login<GroupTermLifeInsurancePage>(username, password)
                .CheckGtmTrackingHot(primaryAction, secondaryAction);
        }

        [TestCaseSource(typeof(ProductGroupTermLifeInsuranceData),
            nameof(ProductGroupTermLifeInsuranceData.TestDataSource),
            new object[] { "Test_GTM_EmbeddedLink" })]
        [Test, Category("Core")]
        public void ValidateGTMEmbeddedLink(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            Page.CheckGtmActionEmbeddedLink(gtmDataLayer, gtmEvent, gtmModule, gtmText);
        }

        [TestCaseSource(typeof(ProductGroupTermLifeInsuranceData),
            nameof(ProductGroupTermLifeInsuranceData.TestDataSource),
            new object[] { "Test_GTM_DownloadsLink" })]
        [Test, Category("Core")]
        public void ValidateGTMDownloadLink(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            Page.CheckGtmActionDownloadLink(gtmDataLayer, gtmEvent, gtmModule, gtmText);
        }

        [TestCaseSource(typeof(ProductGroupTermLifeInsuranceData),
            nameof(ProductGroupTermLifeInsuranceData.TestDataSource),
            new object[] { "Test_GTM_CTALink" })]
        [Test, Category("Core")]
        public void ValidateGTMCTAAction(string username, string password,
            string url, string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            Page.Login<HomePage>(username, password)
                .GoToPage<GroupTermLifeInsurancePage>(url)
                .CheckGtmActionCTA(gtmDataLayer, gtmEvent, gtmModule, gtmText);
        }
    }
}
