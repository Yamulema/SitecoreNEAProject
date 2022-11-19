using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.MultiProducts
{
    public class TestLINKPage : Base.NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "TestLINKPage_CtaHot_PrimaryCta";
        private const string PrimaryCtaCold = "TestLINKPage_CtaCold_SignIn";
        #endregion
        #region Constructor
        public TestLINKPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public TestLINKPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public TestLINKPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new TestLINKPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public TestLINKPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return new TestLINKPage(this.Driver, this.Settings);
        }

        public TestLINKPage AssertHotState(string urlexpected)
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
