using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.PromoTout
{
    public class PromoToutSection : NeambPage
    {
        #region Constructor
        public PromoToutSection(
             IWebDriver driver, ISettings settings) : base("PromoToutSection", driver, settings)
        { }
        #endregion

        #region ControlKeys
        private const string PromoTout = "LoginPage_Form_PasswordTextbox";
        private const string PromoTout_PrimaryButton = "PromoTout_PrimaryButton";
        #endregion

        public new PromoToutSection AssertPromoTout()
        {
            AssertHasAllControlsForSections(new[] {
                "PromoToutSection"
            });
            return this;
        }

        public new PromoToutSection AssertNotEligiblePromoTout()
        {
            By elementToCheck = By.XPath(PromoTout);
            AssertIsTrue(!Exists(elementToCheck), "PromoTout is visible for Not Eligible User");
            return this;
        }

        public bool Exists(By by)
        {
            if (Driver.FindElements(by).Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public new PromoToutSection CheckGtmActionProductLiteWarm(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PromoTout_PrimaryButton)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            return this;
        }
        public new PromoToutSection CheckGtmActionProductLiteHot(string clickPrimaryFunction)
        {
            var buttonOnClick = GetElementFromControlKey(PromoTout_PrimaryButton)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(clickPrimaryFunction), $"ClickAction {buttonOnClick} doesn't match {clickPrimaryFunction}");

            return this;
        }
    }
}
