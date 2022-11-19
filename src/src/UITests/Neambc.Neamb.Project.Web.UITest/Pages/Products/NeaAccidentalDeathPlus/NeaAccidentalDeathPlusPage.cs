using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaAccidentalDeathPlus
	{
    public class NeaAccidentalDeathPlusPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaAccidentalDeathPlusPage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "NeaAccidentalDeathPlusPage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public NeaAccidentalDeathPlusPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaAccidentalDeathPlusPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaAccidentalDeathPlusPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
		#endregion
		
		
        public new NeaAccidentalDeathPlusPage AssertIsLoaded()
        {
			
			AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public NeaAccidentalDeathPlusPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }
        public NeaAccidentalDeathPlusPage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return this;
        }
    }
}
