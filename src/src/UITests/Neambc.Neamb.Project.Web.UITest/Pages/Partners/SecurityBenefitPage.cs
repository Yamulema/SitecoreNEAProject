using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class SecurityBenefitPage : AssertingPageBase
    {
        #region Constructor
        public SecurityBenefitPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public SecurityBenefitPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public SecurityBenefitPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public SecurityBenefitPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Header"
            });
            return this;
        }
    }
}
