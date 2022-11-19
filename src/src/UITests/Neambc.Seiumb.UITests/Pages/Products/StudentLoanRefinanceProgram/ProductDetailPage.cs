using System;
using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.Products.SeiuGroupTermLifeInsurance
{
    public class ProductDetailPage : SeiumbPage
    {
        private static int Timeout => 30;

        #region ControlKeys
        private const string EmbeddedLink = "EmbeddedLink";
        private const string DownloadLink = "DownloadLink";
        private const string CTALink = "CTALink";

        #endregion

        #region Constructor
        public ProductDetailPage(string pageName, IWebDriver driver, ISettings settings) : base(name: pageName, driver: driver,
            settings: settings)
        {

        }

        public ProductDetailPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }
        #endregion

        public ProductDetailPage CheckGtmActionEmbeddedLink(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            AssertElementExists(EmbeddedLink);
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               EmbeddedLink, "CheckGtmActionEmbeddedLink");
            return this;
        }

        public ProductDetailPage CheckGtmActionDownloadLink(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            AssertElementExists(DownloadLink);
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               DownloadLink, "CheckGtmActionDownloadLink");
            return this;
        }

        public ProductDetailPage CheckGtmActionCTA(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            AssertElementExists(CTALink);
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               CTALink, "CheckGtmActionCTA");
            return this;
        }

        private ProductDetailPage CheckGtmAction(string gtmDataLayer, string gtmEvent,
           string gtmModule, string gtmText, string controlKey, string section)
        {
            var onClickFunction = GetElementFromControlKey(controlKey)?
                .GetAttribute("onclick");

            var hasDatalayerFunction = onClickFunction.Contains(gtmDataLayer);
            var hasGTMEvent = onClickFunction.Contains(gtmEvent);
            var hasGTMModule = onClickFunction.Contains(gtmModule);
            var hasGTMText = onClickFunction.Contains(gtmText);

            AssertIsTrue(hasDatalayerFunction, $"Error in {section} checking {gtmDataLayer}");
            AssertIsTrue(hasGTMEvent, $"Error in {section} checking {gtmEvent}");
            AssertIsTrue(hasGTMModule, $"Error in {section} checking {gtmModule}");
            AssertIsTrue(hasGTMText, $"Error in {section} checking {gtmText}");
            return this;
        }
    }
}
