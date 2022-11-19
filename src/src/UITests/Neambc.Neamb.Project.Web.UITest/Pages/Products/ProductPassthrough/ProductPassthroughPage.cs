using System;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.ProductPassthrough 
{
    public class ProductPassthroughPage : NeambPage
    {
        #region ControlKeys
        private const string GeAppliancePage_Input_FirstName = "GeAppliancePage_Input_FirstName";
        
        #endregion
        #region Constructor
        public ProductPassthroughPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public ProductPassthroughPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public ProductPassthroughPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new ProductPassthroughPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }

        public ProductPassthroughPage AssertInputFirstNameValues(string value)
        {
            var controKeyValue = GetElementFromControlKey(GeAppliancePage_Input_FirstName)?
                .GetAttribute("value");
            AssertIsTrue(string.Equals(controKeyValue, value, StringComparison.InvariantCultureIgnoreCase),
                $"{GeAppliancePage_Input_FirstName} {controKeyValue} doesn't match {value}");
            return this;
        }

    }
}
