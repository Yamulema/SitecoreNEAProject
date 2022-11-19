using System;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class RentalCarPage : AssertingPageBase
    {
        //private const string FirstName = "EnrollPage_Form_FirstName";
        #region Constructor
        public RentalCarPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public RentalCarPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public RentalCarPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public RentalCarPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }
	
	}
}
