using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Partners;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaGroupHospitalIncomeInsurancePlan
{
    public class NeaGroupHospitalIncomeInsurancePlanPage : NeambPage
    {
        #region ControlKeys
        private const string PrimaryCtaHot = "NeaGroupHospitalIncomeInsurancePlanPage_CtaHot_PrimaryCta";
        private const string SecondaryCtaHot = "NeaGroupHospitalIncomeInsurancePlanPage_CtaHot_SecondaryCta";
        #endregion
        #region Constructor
        public NeaGroupHospitalIncomeInsurancePlanPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaGroupHospitalIncomeInsurancePlanPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaGroupHospitalIncomeInsurancePlanPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }
        #endregion
        public new NeaGroupHospitalIncomeInsurancePlanPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Component"
            });
            return this;
        }
        public MercerEnrollPage ClickOnPrimaryCta()
        {
            AssertClick(PrimaryCtaHot, timeoutSeconds: 30);
            return new MercerEnrollPage(this.Driver, this.Settings);
        }

        public MercerPage ClickOnSecondaryCta()
        {
            AssertClick(SecondaryCtaHot, timeoutSeconds: 30);
            return new MercerPage(this.Driver, this.Settings);
        }
    }
}
