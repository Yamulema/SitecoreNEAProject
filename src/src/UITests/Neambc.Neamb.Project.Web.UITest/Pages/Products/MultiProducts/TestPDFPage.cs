using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.MultiProducts
{
    public class TestPDFPage : Base.NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "TestPDFPage_CtaHot_PrimaryCta";
        private const string PrimaryCtaCold = "TestPDFPage_CtaCold_SignIn";
        #endregion
        #region Constructor
        public TestPDFPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public TestPDFPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public TestPDFPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new TestPDFPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public TestPDFPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return new TestPDFPage(this.Driver, this.Settings);
        }

        public TestPDFPage AssertHotState(string urlexpected)
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
