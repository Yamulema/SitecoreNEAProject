using System;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class PlanEnrollmentPage : AssertingPageBase
    {
        private const string UserInfo = "PlanEnrollmentPage_HeaderHot_UserInfo";
        #region Constructor
        public PlanEnrollmentPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public PlanEnrollmentPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public PlanEnrollmentPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public PlanEnrollmentPage AssertHotState(string userInfo)
        {
            AssertHasAllControlsForSections(new[] {
                "HeaderHot"
            });
            var textBoxValue = GetElementFromControlKey(UserInfo, 30)?.Text?.Trim();
            AssertIsTrue(string.Equals(userInfo, textBoxValue, StringComparison.InvariantCultureIgnoreCase), $"UserInfo control {textBoxValue} doesn't match {userInfo}");
            return this;
        }
    }
}
