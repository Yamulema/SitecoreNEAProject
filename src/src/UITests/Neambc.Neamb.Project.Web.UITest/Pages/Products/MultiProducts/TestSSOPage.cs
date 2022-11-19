using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.MultiProducts
{
    public class TestSSOPage : Base.NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "TestSSOPage_CtaHot_PrimaryCta";
        private const string PrimaryCtaCold = "TestSSOPage_CtaCold_SignIn";
        #endregion
        #region Constructor
        public TestSSOPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public TestSSOPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public TestSSOPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new TestSSOPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public TestSSOPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return new TestSSOPage(this.Driver, this.Settings);
        }

        public TestSSOPage AssertHotState(string urlexpected)
        {
            var urlOpened = this.Driver.Url;
            AssertIsTrue(urlOpened.Contains(urlexpected));
            return this;
        }
        public LoginPage ClickPrimaryButtonRedirectLogin()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public LoginPage ClickLoginButtonRedirect()
        {
            AssertClick(PrimaryCtaCold, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }
    }
}
