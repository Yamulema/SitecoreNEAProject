using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.HertzCarRental
{
    public class HertzCarRentalPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaWarm = "HertzCarRental_CtaWarm_PrimaryCta";
        private const string SecondaryCtaWarm = "HertzCarRental_CtaWarm_SecondaryCta";
        private const string PrimaryCtaHot = "HertzCarRental_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "HertzCarRental_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public HertzCarRentalPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public HertzCarRentalPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public HertzCarRentalPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new HertzCarRentalPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        

        public new HertzCarRentalPage CheckGtmActionProductLiteWarm(string clickPrimaryFunction, string clickSecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryOnClick = GetElementFromControlKey(SecondaryCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonSecondaryOnClick != null && buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickSecondaryFunction}");

            return this;
        }
        public new HertzCarRentalPage CheckGtmActionProductLiteHot(string clickPrimaryFunction, string clickSecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaHot)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryOnClick = GetElementFromControlKey(SecondaryCtaHot)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonSecondaryOnClick != null && buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickSecondaryFunction}");

            return this;
        }
    }
}
