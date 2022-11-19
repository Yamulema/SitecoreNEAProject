using System.Threading;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Seminar
{
    public class SeminarPage : NeambPage
    {
        #region ControlKeys
        private const string LoginWarmColdButton = "LoginButton";
        private const string HotButton = "HotButton";
        private const string PopupButton = "PopupButton";
        #endregion

        #region Constructor
        public SeminarPage(IWebDriver driver, ISettings settings) : base(name: "SeminarPage", driver: driver,
            settings: settings)
        {

        }
        #endregion

        public new SeminarPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }

        

        public new SeminarPage ClickOnCtaButtonHotUser()
        {
            AssertClick(HotButton, timeoutSeconds: 30);
            return this;
        }

        
        public new LoginPage ClickSeminarButtonColdWarmUser()
        {
            AssertClick(LoginWarmColdButton, timeoutSeconds: 30);
            return new LoginPage(this.Driver, this.Settings);
        }

        public new SeminarPage CheckPopupIsDisplayed()
        {
            Thread.Sleep(3000);
            var popup = GetElementFromControlKey(PopupButton);
            AssertIsTrue(popup != null, $"Popup windows is not displayed");
            return new SeminarPage(this.Driver, this.Settings);
        }
    }
}
