using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaRetirementProgram
{
    public class NeaRetirementProgramPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaRetirementProgramPage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "NeaRetirementProgramPage_CtaHot_SecondaryCta";
        //MBREQ-1069
        private const string PrimaryCtaCold = "Retirement_CtaCold_SignIn";
        private const string StickyCtaCold = "Retirement_CtaCold_StickySignIn";
        private const string PrimaryCtaWarm = "Retirement_CtaWarm_PrimaryCta";
        private const string SecondaryCtaWarm = "Retirement_CtaWarm_SecondaryCta";
        private const string StickyPrimaryCtaWarm = "Retirement_CtaWarm_StickyPrimaryCta";
        private const string StickySecondaryCtaWarm = "Retirement_CtaWarm_StickySecondaryCta";
        private const string PrimaryCtaHotGTM = "Retirement_CtaHot_PrimaryCta";
        private const string SecondaryCtaHotGTM = "Retirement_CtaHot_SecondaryCta";
        private const string StickyPrimaryCtaHot = "Retirement_CtaHot_StickyPrimaryCta";
        private const string StickySecondaryCtaHot = "Retirement_CtaWarm_StickySecondaryCta";
        #endregion
        #region Constructor
        public NeaRetirementProgramPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaRetirementProgramPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaRetirementProgramPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new NeaRetirementProgramPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public NeaRetirementProgramPage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return this;
        }
        public NeaRetirementProgramPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }

        public new NeaRetirementProgramPage CheckGtmActionProductLiteCold(string clickPrimaryFunction, string clickStickyFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaCold)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonOnClick,clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryOnClick = GetElementFromControlKey(StickyCtaCold)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonSecondaryOnClick,clickStickyFunction), $"ClickAction {buttonOnClick} doesn't match {clickStickyFunction}");

            return this;
        }

        public new NeaRetirementProgramPage CheckGtmActionProductLiteWarm(string clickPrimaryFunction, string clickSecondaryFunction, string clickStickyPrimaryFunction, string clickStickySecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonOnClick,clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryClick = GetElementFromControlKey(SecondaryCtaWarm)?
               .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonSecondaryClick,clickSecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickSecondaryFunction}");

            var buttonStickyOnClick = GetElementFromControlKey(StickyPrimaryCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonStickyOnClick,clickStickyPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickStickyPrimaryFunction}");

            var buttonStickySecondaryOnClick = GetElementFromControlKey(StickySecondaryCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonStickySecondaryOnClick,clickStickySecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickStickySecondaryFunction}");

            return this;
        }
        public new NeaRetirementProgramPage CheckGtmActionProductLiteHot(string clickPrimaryFunction, string clickSecondaryFunction, string clickStickyPrimaryFunction, string clickStickySecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaHotGTM)?
               .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonOnClick,clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryClick = GetElementFromControlKey(SecondaryCtaHotGTM)?
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
