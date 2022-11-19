using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.ForgotEmail
{
    public class ForgotEmailPage : NeambPage
    {
        #region Constructor
        public ForgotEmailPage(IWebDriver driver, ISettings settings) : base(name: "ForgotEmailPage", driver: driver,
            settings: settings)
        {

        }
        #endregion

        public new ForgotEmailPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Header",
                "Form"
            });
            return this;
        }
    }
}
