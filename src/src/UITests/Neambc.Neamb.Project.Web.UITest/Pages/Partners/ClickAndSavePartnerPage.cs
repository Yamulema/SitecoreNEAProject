using System;
using System.Web;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class ClickAndSavePartnerPage : AssertingPageBase
    {
        #region Constructor
        public ClickAndSavePartnerPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public ClickAndSavePartnerPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public ClickAndSavePartnerPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public ClickAndSavePartnerPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Header"
            });
            return this;
        }
    }
}
