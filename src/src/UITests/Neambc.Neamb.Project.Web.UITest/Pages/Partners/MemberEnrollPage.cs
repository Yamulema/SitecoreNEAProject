using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Partners
{
    public class MemberEnrollPage : AssertingPageBase
    {
        private const string FirstName = "MemberEnroll_Form_FirtName";
        private const string LastName = "MemberEnroll_Form_LastName";
		private const string DOB = "MemberEnroll_Form_DOB";
        #region Constructor
        public MemberEnrollPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public MemberEnrollPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public MemberEnrollPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public MemberEnrollPage AssertFormIsLoaded(string firstName, string lastName, string dob)
        {
            Thread.Sleep(8000);
			var data = new string[] { firstName, lastName, dob };
			var textBoxValue = new string[] { FirstName, LastName, DOB };
            if (data.Length > 0)
			{
				var value = GetElementFromControlKey(textBoxValue[0])?.GetAttribute("value");
			    AssertIsTrue(string.Equals(data[0], value, StringComparison.InvariantCultureIgnoreCase), $"Wrong value '{value}' found at form textBox, expected: '{data[0]}'");
                
			}
			return this;
		}
	}
}
