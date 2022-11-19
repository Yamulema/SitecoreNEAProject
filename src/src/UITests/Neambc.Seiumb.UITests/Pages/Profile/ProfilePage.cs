using System;
using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.Profile
{
    public class ProfilePage : SeiumbPage
    {
        private static int Timeout => 30;

        #region ControlKeys
        private const string FirstNameInput = "FirstNameInput";
        private const string SubmitButton = "SubmitButton";

        #endregion

        #region Constructor
        public ProfilePage(string pageName, IWebDriver driver, ISettings settings) : base(name: pageName, driver: driver,
            settings: settings)
        {

        }

        public ProfilePage(IWebDriver driver, ISettings settings) : base(driver, settings)
        {
        }
        #endregion

        public ProfilePage UpdateProfile(string firstName)
        {
            AssertElementExists(FirstNameInput).Clear();
            AssertSetTextBoxValue(FirstNameInput, firstName);
            AssertClick(SubmitButton, timeoutSeconds: Timeout);
            return this;
        }
    }
}
