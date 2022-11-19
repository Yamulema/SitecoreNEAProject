using System;
using System.Web;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class IcePage : AssertingPageBase
    {
        #region Constructor
        public IcePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public IcePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public IcePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public IcePage AssertHotState(string mdsId)
        {
            var uri = new Uri(this.Driver.Url);
            var thirdPartyId = HttpUtility.ParseQueryString(uri.Query).Get("thirdPartyId");
            AssertHasAllControlsForSections(new[] {
                "Header"
            });
            AssertIsTrue(string.Equals(mdsId, thirdPartyId, StringComparison.InvariantCultureIgnoreCase));
            return this;
        }
    }
}
