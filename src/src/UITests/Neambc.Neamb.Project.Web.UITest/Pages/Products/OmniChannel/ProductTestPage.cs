using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.OmniChannel
{
    public class ProductTestPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "ProductTestPage_CtaHot_PrimaryCta";
        private const string PrimaryCtaCold = "ProductTestPage_CtaCold_SignIn";
        #endregion
        #region Constructor
        public ProductTestPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public ProductTestPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public ProductTestPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new ProductTestPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public ProductTestPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return new ProductTestPage(this.Driver, this.Settings);
        }

        public ProductTestPage AssertHotState(string urlexpected)
        {
            var urlOpened = this.Driver.Url;
            AssertIsTrue( urlOpened.Contains(urlexpected));
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
