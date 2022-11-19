using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.PersonalLoan5k
{
    public class PersonalLoanPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaCold = "PersonalLoan_CtaCold_SignIn";
        private const string StickyCtaCold = "PersonalLoan_CtaCold_StickySignIn";
        private const string PrimaryCtaWarm = "PersonalLoan_CtaWarm_PrimaryCta";
        private const string StickyCtaWarm = "PersonalLoan_CtaWarm_StickyCta";
        private const string PrimaryCtaHot = "PersonalLoan_CtaHot_PrimaryCta";
        private const string StickyCtaHot = "PersonalLoan_CtaHot_StickyCta";
        #endregion
        #region Constructor
        public PersonalLoanPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public PersonalLoanPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public PersonalLoanPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new PersonalLoanPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        
        public new PersonalLoanPage CheckGtmActionProductLiteCold(string clickPrimaryFunction, string clickSecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaCold)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryOnClick = GetElementFromControlKey(StickyCtaCold)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonSecondaryOnClick != null && buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickSecondaryFunction}");

            return this;
        }

        public new PersonalLoanPage CheckGtmActionProductLiteWarm(string clickPrimaryFunction, string clickSecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryOnClick = GetElementFromControlKey(StickyCtaWarm)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonSecondaryOnClick != null && buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickSecondaryFunction}");

            return this;
        }
        public new PersonalLoanPage CheckGtmActionProductLiteHot(string clickPrimaryFunction, string clickSecondaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PrimaryCtaHot)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            var buttonSecondaryOnClick = GetElementFromControlKey(StickyCtaHot)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonSecondaryOnClick != null && buttonSecondaryOnClick.Contains(clickSecondaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickSecondaryFunction}");

            return this;
        }
    }
}
