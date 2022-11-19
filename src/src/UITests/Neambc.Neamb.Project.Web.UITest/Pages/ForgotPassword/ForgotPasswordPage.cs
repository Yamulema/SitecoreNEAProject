using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.ForgotPassword
	{
    public class ForgotPasswordPage : NeambPage
    {
        #region Constructor
        public ForgotPasswordPage(IWebDriver driver, ISettings settings) : base(name: "ForgotPasswordPage", driver: driver,
            settings: settings)
        {

        }
        #endregion

        public new ForgotPasswordPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Header",
                "Form"
            });
            return this;
        }
    }
}
