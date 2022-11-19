using System;
using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.NavigationSection
{
    public class Navigation : SeiumbPage
    {
        #region ControlKeys
        private const string TopMenuLink = "TopMenuLink";
        private const string LeftNavMenuLink = "LeftNavMenuLink";
        private const string FooterMenuLink = "FooterMenuLink";
        #endregion

        #region Constructor
        public Navigation(IWebDriver driver, ISettings settings) : base(name: "NavigationSection", driver: driver,
            settings: settings)
        {

        }
        #endregion

        public Navigation CheckGtmActionTopNav(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(TopMenuLink)?
                .GetAttribute("onclick");
            AssertIsTrue(string.Equals(clickFunction, buttonOnClick, StringComparison.InvariantCultureIgnoreCase), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");
            return this;
        }

        public Navigation CheckGtmActionLeftNav(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(LeftNavMenuLink)?
                .GetAttribute("onclick");
            AssertIsTrue(string.Equals(clickFunction, buttonOnClick, StringComparison.InvariantCultureIgnoreCase), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");
            return this;
        }

        public Navigation CheckGtmActionFooter(string clickFunction)
        {
            var buttonOnClick = GetElementFromControlKey(FooterMenuLink)?
                .GetAttribute("onclick");
            AssertIsTrue(string.Equals(clickFunction, buttonOnClick, StringComparison.InvariantCultureIgnoreCase), $"ClickAction {buttonOnClick} doesn't match {clickFunction}");
            return this;
        }

        
    }
}
