using Neambc.Seiumb.UITests.Pages.Home;
using NUnit.Framework;

namespace Neambc.Seiumb.UITests.Tests.Core.Home
{
    [TestFixture]
    public class HomePageTests : SeiumbTestBaseLarge<HomePage>
    {
        /// <summary>
        /// ValidateLogin
        /// </summary>
        [TestCaseSource(typeof(HomePageTestData),
            nameof(HomePageTestData.TestDataSource),
            new object[] { "ValidateLogin" })]
        [Test, Category("Login")]
        public void ValidateLogin(string username, string password)
        {
            Page.AssertIsLoaded()
                .Login<HomePage>(username, password);
        }

        [TestCaseSource(typeof(HomePageTestData),
            nameof(HomePageTestData.TestDataSource),
            new object[] { "Test_GTM_Card" })]
        [Test, Category("GTM")]
        public void ValidateGtmCards(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            Page.CheckGtmActionCard(gtmDataLayer, gtmEvent, gtmModule, gtmText);
        }

        [TestCaseSource(typeof(HomePageTestData),
            nameof(HomePageTestData.TestDataSource),
            new object[] { "Test_GTM_LanguageTranslator" })]
        [Test, Category("GTM")]
        public void ValidateGtmLanguageToogle(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            Page.CheckGtmActionLanguageEnglish(gtmDataLayer, gtmEvent, gtmModule, gtmText);
            Page.CheckGtmActionLanguageSpanish(gtmDataLayer, gtmEvent, gtmModule, gtmText);
        }

        [TestCaseSource(typeof(HomePageTestData),
            nameof(HomePageTestData.TestDataSource),
            new object[] { "Test_GTM_ContactUs" })]
        [Test, Category("GTM")]
        public void ValidateGTMContactUs(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            Page.CheckGtmActionContactUs(gtmDataLayer, gtmEvent, gtmModule, gtmText);
        }
    }
}
