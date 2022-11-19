using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.NeaRetireeHealthPage
{
    public class NEARetireeHealthProgramRatesPage : NeambPageBase
    {
        public NEARetireeHealthProgramRatesPage(
            IWebDriver driver,
            ISettings settings) : base(
                "HealthProgramRates",
                "/Landing Pages/NEA Retiree Health Program Rate Quote",
                driver, settings)
        {

        }
    }
}
