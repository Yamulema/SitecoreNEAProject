using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaGroupTermLifeInsurance
{
    public class NeaGroupTermLifeInsurancePage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaGroupTermLifeInsurancePage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "NeaGroupTermLifeInsurancePage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public NeaGroupTermLifeInsurancePage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaGroupTermLifeInsurancePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaGroupTermLifeInsurancePage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new NeaGroupTermLifeInsurancePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public NeaGroupTermLifeInsurancePage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }
        public NeaGroupTermLifeInsurancePage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return this;
        }
    }
}
