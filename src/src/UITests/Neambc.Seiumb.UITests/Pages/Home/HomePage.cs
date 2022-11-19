using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using System;
using System.Threading;

namespace Neambc.Seiumb.UITests.Pages.Home
{
    public class HomePage : SeiumbPage
    {
        #region ControlKeys
        private const string FirstCard = "FirstCard";
        private const string EnglishLanguageToggle = "EnglishLanguageToggle";
        private const string SpanishLanguageToggle = "SpanishLanguageToggle";
        private const string ArrowFirstCarousel = "ArrowFirstCarousel";

        #endregion

        private const string ContactUsLink = "ContactUsLink";
        private const string ContactUsIconMail = "ContactUsIconMail";
        private const string ContactUsIconPhone = "ContactUsIconPhone";
        private const string ChatIcon = "ChatIcon";

        public HomePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public HomePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public HomePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        public new HomePage AssertIsLoaded()
        {
            //TODO: Call AssertHasAllControlsForSections.
            return this;
        }

        public HomePage CheckGtmActionCard(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               FirstCard, "CheckGtmActionCard");
            return this;
        }

        public HomePage CheckGtmActionLanguageEnglish(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
                EnglishLanguageToggle, "CheckGtmActionLanguageEnglish");
            return this;
        }

        public HomePage CheckGtmActionLanguageSpanish(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
                SpanishLanguageToggle, "CheckGtmActionLanguageSpanish");
            return this;
        }

        public HomePage CheckGtmActionContactUs(string gtmDataLayer, string gtmEvent,
            string gtmModule, string gtmText)
        {
            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               ContactUsLink, "CheckGtmActionContactUs");

            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               ContactUsIconMail, "CheckGtmActionContactUs");

            CheckGtmAction(gtmDataLayer, gtmEvent, gtmModule, gtmText,
               ContactUsIconPhone, "CheckGtmActionContactUs");
            return this;
        }

        private HomePage CheckGtmAction(string gtmDataLayer, string gtmEvent,
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

        public HomePage VerifyChatIcon()
        {
            Thread.Sleep(3000);
            By elementToCheck = By.XPath(ChatIcon);
            AssertIsTrue(!Exists(elementToCheck), "Chat Icon is present in Home Page");
            return this;
        }

        public bool Exists(By by)
        {
            return Driver.FindElements(by).Count != 0;
        }
    }
}
