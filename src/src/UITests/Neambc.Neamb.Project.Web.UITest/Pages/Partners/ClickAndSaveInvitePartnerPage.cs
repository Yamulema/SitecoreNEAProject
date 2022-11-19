using System;
using System.Web;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class ClickAndSaveInvitePartnerPage : AssertingPageBase
    {
        #region Constructor
        public ClickAndSaveInvitePartnerPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public ClickAndSaveInvitePartnerPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public ClickAndSaveInvitePartnerPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public ClickAndSaveInvitePartnerPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Header"
            });
            return this;
        }
    }
}
