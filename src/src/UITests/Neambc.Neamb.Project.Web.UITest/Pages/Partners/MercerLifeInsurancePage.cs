using System;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class MercerLifeInsurancePage : AssertingPageBase
    {
		private const string Coverage = "MercerLifeInsurancePage_Hot_Card1_Name";
		private const string Certificate = "MercerLifeInsurancePage_Hot_Card1_Certificate";
		#region Constructor
		public MercerLifeInsurancePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public MercerLifeInsurancePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public MercerLifeInsurancePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public MercerLifeInsurancePage AssertLinkHotState(string coverage, string certificate)
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
