using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaAccidentalDeath
	{
    public class NeaAccidentalDeathPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaAccidentalDeathPage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "NeaAccidentalDeathPage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public NeaAccidentalDeathPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaAccidentalDeathPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaAccidentalDeathPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new NeaAccidentalDeathPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public NeaAccidentalDeathPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }
        public NeaAccidentalDeathPage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return this;
        }
    }
}
