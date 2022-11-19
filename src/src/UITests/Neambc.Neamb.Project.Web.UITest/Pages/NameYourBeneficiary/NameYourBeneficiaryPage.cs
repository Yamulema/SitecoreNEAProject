using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Complimentary_Life;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.NameYourBeneficiary
{
    public class NameYourBeneficiaryPage : NeambPage
    {
        #region ControlKeys

        private const string NameYourBeneficiaryButton = "NameYourBeneficiaryButton";

        #endregion

        #region Constructor

        public NameYourBeneficiaryPage(string pageName, string urlPath, IWebDriver driver, ISettings settings) : base(pageName,
            urlPath, driver, settings)
        {
        }

        public NameYourBeneficiaryPage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }

        public NameYourBeneficiaryPage(string name, IWebDriver driver, ISettings settings) : base(name, driver, settings)
        {
        }

        #endregion

        
        public ComplimentaryLifePage ClickNameYourBeneficiaryButtonHot()
        {
            AssertClick(NameYourBeneficiaryButton, timeoutSeconds: 30);
            return new ComplimentaryLifePage(this.Driver, this.Settings);
        }

    }
}
