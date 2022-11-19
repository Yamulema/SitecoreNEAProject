using System;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class MercerPage : AssertingPageBase
    {
		private const string Coverage = "MercerPage_Hot_Card1_Name";
		private const string Certificate = "MercerPage_Hot_Card1_Certificate";
		#region Constructor
		public MercerPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public MercerPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public MercerPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public MercerPage AssertLinkHotState(string coverage, string certificate)
        {
			var data = new string[] { coverage, certificate };
			var textBoxValue = new string[] { Coverage, Certificate };
			for (var i = 0; i < data.Length; i++) {
				var value = GetElementFromControlKey(textBoxValue[i])?.Text.ToString();
				AssertIsTrue(string.Equals(data[i], value, StringComparison.InvariantCultureIgnoreCase), $"FirstName textBox {value} doesn't match {data[i]}");
			}

			AssertHasAllControlsForSections(new[] {
                "Hot"
            });
            return this;
        }
    }
}
