using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaGuaranteedIssueLifeInsurancePlanPage
{
    public class NeaGuaranteedIssueLifeInsurancePlanPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaGuaranteedIssueLifeInsurancePlanPage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "NeaGuaranteedIssueLifeInsurancePlanPage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public NeaGuaranteedIssueLifeInsurancePlanPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaGuaranteedIssueLifeInsurancePlanPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaGuaranteedIssueLifeInsurancePlanPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion

        public new NeaGuaranteedIssueLifeInsurancePlanPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public NeaGuaranteedIssueLifeInsurancePlanPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return this;
        }
        public NeaGuaranteedIssueLifeInsurancePlanPage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return this;
        }
    }
}
