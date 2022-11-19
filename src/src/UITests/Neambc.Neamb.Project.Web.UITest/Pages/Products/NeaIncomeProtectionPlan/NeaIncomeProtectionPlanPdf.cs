using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Products.NeaIncomeProtectionPlan
{
    public class NeaIncomeProtectionPlanPdf : NeambPage
    {
        #region ControlKeys
        private const string SignInButtonCta = "NeaIncomeProtectionPlanPdf_CtaCold_SignIn";
        private const string EnrollNowCta = "NeaIncomeProtectionPlanPdf_CtaWarm_EnrollNow";
        private const string PrintAnApplicationCta = "NeaIncomeProtectionPlanPdf_CtaWarm_PrintApplication";
        
        #endregion
        public NeaIncomeProtectionPlanPdf(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName, urlPath, driver, settings)
        {
        }

        public NeaIncomeProtectionPlanPdf(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NeaIncomeProtectionPlanPdf(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }

        
        
        
    }
}
