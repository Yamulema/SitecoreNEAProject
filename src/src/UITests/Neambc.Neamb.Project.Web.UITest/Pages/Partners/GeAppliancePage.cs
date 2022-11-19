using System;
using System.Web;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class GeAppliancePage : AssertingPageBase
    {
        #region Constructor
        public GeAppliancePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public GeAppliancePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public GeAppliancePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public GeAppliancePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }
    }
}
