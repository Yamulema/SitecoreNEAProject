using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaIncomeProtectionPlan
{
    public class NeaIncomeProtectionPlanPage : NeambPage
    {
        #region ControlKeys
        private const string SignInButtonCta = "NeaIncomeProtectionPlanPage_CtaCold_SignIn";
        private const string EnrollNowCta = "NeaIncomeProtectionPlanPage_CtaWarm_EnrollNow";
        private const string PrintAnApplicationCta = "NeaIncomeProtectionPlanPage_CtaHot_PrintAnApplication";
       
        /*MBREQ-1069*/
        private const string PrimaryCtaCold = "IncomeProtection_CtaCold_SignIn";
        private const string StickyCtaCold = "IncomeProtection_CtaCold_StickySignIn";
        private const string PrimaryCtaWarm = "IncomeProtection_CtaWarm_PrimaryCta";
        private const string SecondaryCtaWarm = "IncomeProtection_CtaWarm_SecondaryCta";
        private const string StickyPrimaryCtaWarm = "IncomeProtection_CtaWarm_StickyPrimaryCta";
        private const string StickySecondaryCtaWarm = "IncomeProtection_CtaWarm_StickySecondaryCta";
        private const string PrimaryCtaHot = "IncomeProtection_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "IncomeProtection_CtaHot_SecondaryCta";
        private const string StickyPrimaryCtaHot = "IncomeProtection_CtaHot_StickyPrimaryCta";
        private const string StickySecondaryCtaHot = "IncomeProtection_CtaWarm_StickySecondaryCta";
        #endregion
      
        public NeaIncomeProtectionPlanPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaIncomeProtectionPlanPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaIncomeProtectionPlanPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }

        public LoginPage ClickOnSignInCta()
        {
            AssertClick(SignInButtonCta, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public new NeaIncomeProtectionPlanPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }

        public NeaIncomeProtectionPlanPage AssertNotEligibleComponents()
        {
            AssertHasAllControlsForSections(new[] {
                "NotEligible"
            });
            return this;
        }

        public NeaIncomeProtectionPlanPage AssertIsLoadedCold()
        {
            AssertHasAllControlsForSections(new[] {
                "Component",
                "CtaCold"
            });
            return this;
        }

        public NeaIncomeProtectionPlanPage AssertIsLoadedHot()
        {
            AssertHasAllControlsForSections(new[] {
                "ComponentEligible"
            });
            return this;
        }

        public LoginPage ClickOnEnrollNow()
        {
            AssertClick(EnrollNowCta, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickOnPrintAnApplication()
        {
            AssertClick(PrintAnApplicationCta, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
            //return this;
        }

        public NeaIncomeProtectionPlanPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }

        /*MBREQ-1069*/
        public new NeaIncomeProtectionPlanPage CheckGtmActionProductLiteCold(string clickPrimaryFunction, string clickStickyFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaCold)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonOnClick,clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryOnClick = GetElementFromControlKey(StickyCtaCold)?
                .GetAttribute("onclick");

            AssertIsTrue(AssertGTMCode(buttonSecondaryOnClick,clickStickyFunction), $"ClickAction {buttonOnClick} doesn't match {clickStickyFunction}");

            return this;
        }

        public new NeaIncomeProtectionPlanPage CheckGtmActionProductLiteWarm(string clickPrimaryFunction, string clickSecondaryFunction, string clickStickyPrimaryFunction, string clickStickySecondaryFunction)
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
        public new NeaIncomeProtectionPlanPage CheckGtmActionProductLiteHot(string clickPrimaryFunction, string clickSecondaryFunction, string clickStickyPrimaryFunction, string clickStickySecondaryFunction)
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
