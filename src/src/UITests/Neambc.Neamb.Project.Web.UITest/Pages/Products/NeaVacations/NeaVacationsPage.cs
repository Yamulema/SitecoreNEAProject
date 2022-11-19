using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaVacations
{
    public class NeaVacationsPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaVacationsPage_CtaHot_PrimaryCta";
        #endregion
        #region Constructor
        public NeaVacationsPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaVacationsPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaVacationsPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new NeaVacationsPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public IcePage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return new IcePage(this.Driver, this.Settings);
        }
    }
}
