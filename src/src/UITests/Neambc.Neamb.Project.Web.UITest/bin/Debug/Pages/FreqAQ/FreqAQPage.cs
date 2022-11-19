using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.FreqAQ
{
    public class FreqAQPage : NeambPage
    {
        #region ControlKeys
        private const string Login = "Topic_login";
        private const string Account = "Topic_account";
        private const string Reset = "Topic_reset";
        #endregion
        #region Constructor
        public FreqAQPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public FreqAQPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public FreqAQPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new FreqAQPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "FQA"
            });
            return this;
        }

        public new FreqAQPage CheckGtmFQALogin(string LoginLink)
        {
            var buttonOnClick = GetElementFromControlKey(Login)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(LoginLink), $"ClickAction {buttonOnClick} doesn't match {LoginLink}");

            return this;
        }

        public new FreqAQPage CheckGtmFAQReset(string ResetPwdLink)
        {
            var buttonOnClick = GetElementFromControlKey(Reset)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(ResetPwdLink), $"ClickAction {buttonOnClick} doesn't match {ResetPwdLink}");

            return this;
        }

        public new FreqAQPage CheckGtmFQAAccount(string AccountLink)
        {
            var buttonOnClick = GetElementFromControlKey(Account)?
                .GetAttribute("onclick");

            AssertIsTrue(buttonOnClick != null && buttonOnClick.Contains(AccountLink), $"ClickAction {buttonOnClick} doesn't match {AccountLink}");

            return this;
        }

    }
}

