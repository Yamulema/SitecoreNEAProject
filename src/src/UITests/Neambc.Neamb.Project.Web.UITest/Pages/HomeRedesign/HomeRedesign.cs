using System.Threading;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.HomeRedesign
{
    public class HomeRedesign : NeambPage
    {
        #region ControlKeys
        private const string HeroBannerTitle = "HeroBanner_Title";
        private const string ButtonSSO = "ButtonSSO";
        private const string ButtonSSOCold = "ButtonSSOCold";
        private const string ButtonLinkTokens = "ButtonLinkTokens";
        private const string ButtonLinkTokensCold = "ButtonLinkTokensCold";
        private const string ButtonDatapassHotWarm = "ButtonDatapassHotWarm";
        private const string ButtonDatapassCold = "ButtonDatapassCold";
        private const string ButtonNextCarousel = "ButtonNextCarousel";
        private const string ButtonEfulfillment = "ButtonEfulfillment";
        private const string ButtonEfulfillmentCold = "ButtonEfulfillmentCold";
        #endregion
        #region Constructor

        public HomeRedesign(IWebDriver driver, ISettings settings) : base(name: "HomeRedesignPage", driver: driver,
            settings: settings)
        {
        }
        #endregion
        #region Public

        public new HomeRedesign AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "HeroBanner"
            });
            return this;
        }
		public new HomeRedesign VerifyTitleText(string text) {
            var tdRow0 = GetElementFromControlKey(HeroBannerTitle)?.Text;
            AssertIsTrue(tdRow0 != null && tdRow0.Equals(text));
            return this;
		}

        public HomeRedesign ClickCarouselSSOCardButton()
        {
            AssertClick(ButtonSSO, timeoutSeconds: 30);
            return this;
        }
        public new HomeRedesign CheckGtmActionSSOHot(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(ButtonSSO)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonOnClick,clickFunction), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");

            return this;
        }

        public LoginPage ClickCarouselSSOCardButtonForLoginWarmHot()
        {
            AssertClick(ButtonSSO, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickCarouselSSOCardButtonForLoginCold()
        {
            AssertClick(ButtonSSOCold, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public HomeRedesign ClickCarouselLinkTokensCardButton()
        {
            AssertClick(ButtonLinkTokens, timeoutSeconds: 30);
            return this;
        }
        public new HomeRedesign CheckGtmActionLinkTokensHot(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(ButtonLinkTokens)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick.StartsWith(clickFunction), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");

            return this;
        }

        public LoginPage ClickCarouselLinkTokensCardButtonForLoginWarmHot()
        {
            AssertClick(ButtonLinkTokens, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickCarouselLinkTokensCardButtonForLoginCold()
        {
            AssertClick(ButtonLinkTokensCold, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }
        public HomeRedesign ClickCarouselDatapassCardButton()
        {
            AssertClick(ButtonDatapassHotWarm, timeoutSeconds: 30);
            return this;
        }
        public new HomeRedesign CheckGtmActionDatapassHot(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(ButtonDatapassHotWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick.StartsWith(clickFunction), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");

            return this;
        }

        public LoginPage ClickCarouselDatapassCardButtonForLoginWarmHot()
        {
            AssertClick(ButtonDatapassHotWarm, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickCarouselDatapassCardButtonForLoginCold()
        {
            AssertClick(ButtonDatapassCold, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }
        public HomeRedesign ClickButtonNextCarousel()
        {
            AssertClick(ButtonNextCarousel, timeoutSeconds: 30);
            return this;
        }
        public HomeRedesign ClickCarouselEfulfillmentCardButton()
        {
            AssertClick(ButtonEfulfillment, timeoutSeconds: 30);
            return this;
        }
        public new HomeRedesign CheckGtmActionEfulfillmentHot(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(ButtonEfulfillment)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick.StartsWith(clickFunction), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");

            return this;
        }
        public LoginPage ClickCarouselEfulfillmentCardButtonForLoginWarmHot()
        {
            AssertClick(ButtonEfulfillment, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }
        public LoginPage ClickCarouselEfulfillmentCardButtonForLoginCold()
        {
            AssertClick(ButtonEfulfillmentCold, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public HomeRedesign WaitTime()
        {
            Thread.Sleep(3000);
            return this;
        }
        #endregion
    }
}
