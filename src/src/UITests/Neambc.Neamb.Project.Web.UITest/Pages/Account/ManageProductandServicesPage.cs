using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Account
{
    public class ManageProductandServicesPage : NeambPageBase
    {

        public ManageProductandServicesPage(
            IWebDriver driver,
            ISettings settings) : base(
                "ManageProServPage",
                "/account/manage-products-and-services", driver, settings)
        {

        }
    }
}
