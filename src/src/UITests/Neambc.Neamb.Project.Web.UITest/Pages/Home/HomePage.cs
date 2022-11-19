using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Controls;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.NUnit;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Home
{
    public class HomePage : NeambPage
    {
        #region ControlKeys
        #endregion
        #region Constructor

        public HomePage(IWebDriver driver, ISettings settings) : base(name: "HomePage", driver: driver,
            settings: settings)
        {
        }
        #endregion
        #region Public

        public new HomePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                //"Headline",
                "Cards"
            });
            return this;
        }
		public new HomePage ScriptGtm(string var1, string var2, string var3, string var4, string var5, string var6) {
			var gtm = AssertElementExists("HomePage_GTM_Script");
			string[] codes = { "userMdsid", "userPersonaCode ", "userSeaCode ", "userLeaCode ", "userSeaName ", "userLeaName " };
			string[] values = { var1, var2, var3, var4, var5, var6 };
			for (var i = 0; i < codes.Length; i++) {
				var htmlCode = (string)((IJavaScriptExecutor)Driver).ExecuteScript("return " + codes[i] + ";", gtm);
				string.Equals(htmlCode, values[i]);
			}
			return this;
		}
		#endregion
	}
}
