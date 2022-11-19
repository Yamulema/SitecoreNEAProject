using System;
using System.Threading;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Sweepstake
{
    public class SweepstakesPageNoSendEmail : NeambPage
    {
        #region ControlKeys
        private const string HotButton = "HotButtonNoSendEmail";
        private const string PopupButton = "PopupButtonNoSendEmail";
        #endregion

        #region Constructor
        public SweepstakesPageNoSendEmail(IWebDriver driver, ISettings settings) : base(name: "SweepstakesPageNoSendEmail", driver: driver,
            settings: settings)
        {

        }
        #endregion

        public new SweepstakesPageNoSendEmail AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }

        public new SweepstakesPageNoSendEmail ClickSweepstakeButton()
        {
            AssertClick(HotButton, timeoutSeconds: 30);
            Thread.Sleep(3000);
            var popup = GetElementFromControlKey(PopupButton);
            AssertIsTrue(popup != null, $"Popup windows is not displayed");
            return new SweepstakesPageNoSendEmail(this.Driver, this.Settings);
        }
        
    }
}
