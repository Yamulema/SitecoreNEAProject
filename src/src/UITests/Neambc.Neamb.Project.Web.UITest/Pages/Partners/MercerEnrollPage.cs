using System;
using System.Web;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class MercerEnrollPage : AssertingPageBase
    {
        #region Constructor
        public MercerEnrollPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public MercerEnrollPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public MercerEnrollPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public MercerEnrollPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }
    }
}
