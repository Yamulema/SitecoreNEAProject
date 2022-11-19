using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaAutoBuying 
{
    public class NeaAutoBuyingPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaAutoBuyingPage_CtaHot_PrimaryCta";
        #endregion
        #region Constructor
        public NeaAutoBuyingPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaAutoBuyingPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaAutoBuyingPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new NeaAutoBuyingPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public NeaAutoBuyingPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }
    }
}
