using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class InvestmentProductsPage : AssertingPageBase
    {
        #region Constructor
        public InvestmentProductsPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public InvestmentProductsPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public InvestmentProductsPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public InvestmentProductsPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Header"
            });
            return this;
        }
    }
}
