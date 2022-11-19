using System;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class HRBlockPage : AssertingPageBase
    {
        //private const string FirstName = "EnrollPage_Form_FirstName";
        #region Constructor
        public HRBlockPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public HRBlockPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public HRBlockPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public HRBlockPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
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
