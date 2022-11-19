using System;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class PdfMultirowPage : AssertingPageBase
    {
		private IWebDriver driver;
		#region Constructor
		public PdfMultirowPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public PdfMultirowPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public PdfMultirowPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public PdfMultirowPage LinkIsEqual(string url)
        {
			var value = Driver.Url;
			AssertIsTrue(value.Contains(url), $"The url {url} doesn't match with the current url {value}");
			
			return this;
        }
	}
}
