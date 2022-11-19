using System;
using System.Threading;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Sweepstake
{
    public class SweepstakesPage : NeambPage
    {
        #region ControlKeys
        private const string LoginWarmColdButton = "LoginButton";
        private const string HotButton = "HotButton";
        private const string PopupButton = "PopupButton";
        #endregion

        #region Constructor
        public SweepstakesPage(IWebDriver driver, ISettings settings) : base(name: "SweepstakesPage", driver: driver,
            settings: settings)
        {

        }
        #endregion

        public new SweepstakesPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }

        public new SweepstakesPage CheckGtmActionCold(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(LoginWarmColdButton)?
                .GetAttribute("onclick");
            //AssertIsTrue(string.Equals(clickFunction, buttonOnClick, StringComparison.InvariantCultureIgnoreCase), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");
            AssertIsTrue(buttonOnClick.StartsWith(clickFunction), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");

            return this;
        }
        public new SweepstakesPage CheckGtmActionHot(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(HotButton)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick.StartsWith(clickFunction), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");

            return this;
        }

        public new SweepstakesPage ClickOnCtaButton()
        {
            AssertClick(HotButton, timeoutSeconds: 30);
            return this;
        }

        /// <summary>
        /// Check the gtm action 
        /// </summary>
        /// <param name="clickFunction"></param>
        /// <returns></returns>
        public new SweepstakesPage CheckGtmActionPopup(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PopupButton)?
                .GetAttribute("onclick");

            AssertIsTrue(string.Equals(clickFunction, buttonOnClick, StringComparison.InvariantCultureIgnoreCase), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");

            return this;
        }

        public new SweepstakesPage ClickSweepstakeButton()
        {
            AssertClick(HotButton, timeoutSeconds: 30);
            Thread.Sleep(3000);
            var popup = GetElementFromControlKey(PopupButton);
            AssertIsTrue(popup != null, $"Popup windows is not displayed");
            return new SweepstakesPage(this.Driver, this.Settings);
        }

        public new LoginPage ClickSweepstakeButtonColdWarmUser()
        {
            AssertClick(LoginWarmColdButton, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public new SweepstakesPage CheckPopupIsDisplayed()
        {
            Thread.Sleep(3000);
            var popup = GetElementFromControlKey(PopupButton);
            AssertIsTrue(popup != null, $"Popup windows is not displayed");
            return new SweepstakesPage(this.Driver, this.Settings);
        }
    }
}
