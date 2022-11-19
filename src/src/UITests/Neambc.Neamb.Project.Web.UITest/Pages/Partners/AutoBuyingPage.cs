using System;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class AutoBuyingPage : AssertingPageBase
    {
        //private const string FirstName = "EnrollPage_Form_FirstName";
        #region Constructor
        public AutoBuyingPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public AutoBuyingPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public AutoBuyingPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public AutoBuyingPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }

        public AutoBuyingPage LinkIsEqual(string url)
        {
            var value = Driver.Url;
            AssertIsTrue(value.Contains(url), $"The url {url} doesn't match with the current url {value}");

            return this;
        }
                
		//public AutoBuyingPage AssertHotState(string firstName) {
		//	var textBoxValue = GetElementFromControlKey(FirstName)?
		//		.GetAttribute("value");
		//	AssertIsTrue(string.Equals(firstName, textBoxValue, StringComparison.InvariantCultureIgnoreCase), $"FirstName textBox {textBoxValue} doesn't match {firstName}");
		//	return this;
		//}
	}
}
