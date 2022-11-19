using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaLTC
{
    public class NeaLTCPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaCold = "NEALTC_CtaCold_SignIn";
        private const string StickyCtaCold = "NEALTC_CtaCold_StickySignIn";
        private const string PrimaryCtaWarm = "NEALTC_CtaWarm_PrimaryCta";
        private const string SecondaryCtaWarm = "NEALTC_CtaWarm_SecondaryCta";
        private const string StickyPrimaryCtaWarm = "NEALTC_CtaWarm_StickyPrimaryCta";
        private const string StickySecondaryCtaWarm = "NEALTC_CtaWarm_StickySecondaryCta";
        private const string PrimaryCtaHot = "NEALTC_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "NEALTC_CtaHot_SecondaryCta";
        private const string StickyPrimaryCtaHot = "NEALTC_CtaHot_StickyPrimaryCta";
        private const string StickySecondaryCtaHot = "NEALTC_CtaHot_StickySecondaryCta";
        #endregion
        #region Constructor
        public NeaLTCPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaLTCPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaLTCPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
       public new NeaLTCPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"                
            });
            return this;
        }
       

        public new NeaLTCPage CheckGtmActionProductLiteCold(string clickPrimaryFunction, string clickStickyFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaCold)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonOnClick, clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            
            return this;
        }

        public new NeaLTCPage CheckGtmActionProductLiteWarm(string clickPrimaryFunction, string clickSecondaryFunction, string clickStickyPrimaryFunction, string clickStickySecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaWarm)?
                .GetAttribute("onclick");
            
            AssertIsTrue(AssertGTMCode(buttonOnClick, clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");


            var buttonSecondaryClick = GetElementFromControlKey(SecondaryCtaWarm)?
               .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonSecondaryClick,clickSecondaryFunction), $"ClickAction {buttonSecondaryClick} doesn't match {clickSecondaryFunction}");

            var buttonStickyOnClick = GetElementFromControlKey(StickyPrimaryCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonStickyOnClick,clickStickyPrimaryFunction), $"ClickAction {buttonStickyOnClick} doesn't match {clickStickyPrimaryFunction}");

            var buttonStickySecondaryOnClick = GetElementFromControlKey(StickySecondaryCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonStickySecondaryOnClick,clickStickySecondaryFunction), $"ClickAction {buttonStickySecondaryOnClick} doesn't match {clickStickySecondaryFunction}");

            return this;
        }
        public new NeaLTCPage CheckGtmActionProductLiteHot(string clickPrimaryFunction, string clickSecondaryFunction, string clickStickyPrimaryFunction, string clickStickySecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaHot)?
               .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonOnClick,clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryClick = GetElementFromControlKey(SecondaryCtaHot)?
               .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonSecondaryClick,clickSecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickSecondaryFunction}");

            var buttonStickyOnClick = GetElementFromControlKey(StickyPrimaryCtaHot)?
               .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonStickyOnClick,clickStickyPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickStickyPrimaryFunction}");

            var buttonStickySecondaryOnClick = GetElementFromControlKey(StickySecondaryCtaHot)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonStickySecondaryOnClick,clickStickySecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickStickySecondaryFunction}");

            return this;
        }

    }
}
