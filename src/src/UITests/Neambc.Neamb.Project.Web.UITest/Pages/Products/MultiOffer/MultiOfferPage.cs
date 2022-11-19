using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.MultiOffer
{
    public class MultiOfferPage : NeambPage
    {
        #region ControlKeys
        private const string MultiOfferCtaWarm = "MultiOffer_CtaWarm";
        private const string MultiOfferCtaHot = "MultiOffer_CtaHot";
        private const string MultiOfferSSOHot = "MultiOffer_SSOHot";
        private const string MultiOfferPDFHot = "MultiOffer_PDFHot";
        private const string MultiOfferLinkHot = "MultiOffer_LinkHot";

        #endregion
        #region Constructor
        public MultiOfferPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public MultiOfferPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public MultiOfferPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public MultiOfferPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "section-normal bg-gray c-013 t-006"
            });
            return this;
        }
        
        public new MultiOfferPage MultiOffer_Warm(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(MultiOfferCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            return this;
        }
        public new MultiOfferPage MultiOffer_CtaHot(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(MultiOfferCtaHot)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            return this;
        }
        public new MultiOfferPage MultiOffer_SSO(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(MultiOfferSSOHot)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            return this;
        }
        public new MultiOfferPage MultiOffer_PDF(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(MultiOfferPDFHot)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            return this;
        }
        public new MultiOfferPage MultiOffer_Link(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(MultiOfferLinkHot)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            return this;
        }

    }
}
