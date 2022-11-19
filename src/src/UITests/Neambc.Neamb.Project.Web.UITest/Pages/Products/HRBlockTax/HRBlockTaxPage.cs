using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.HRBlockTax
{
    public class HRBlockTaxPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "HRBlockTaxPage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "HRBlockTaxPage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public HRBlockTaxPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public HRBlockTaxPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public HRBlockTaxPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new HRBlockTaxPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public HRBlockTaxPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }
        public HRBlockTaxPage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return this;
        }
    }
}
