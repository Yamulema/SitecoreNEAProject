using System;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.NeaRetireeHealthPage;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.ContactUs {
    public class ContactUsPage : NeambPage {

        #region ControlKeys
        private const string ToogleEnglish = "EnglishButton";
        private const string ToogleSpanish = "SpanishButton";
        private const string ToogleScrenTitle = "ScreenTitleEnglish";
        private const string ToogleScrenTitleSpanish = "ScreenTitleSpanish";
        #endregion

        public ContactUsPage(
            IWebDriver driver,
            ISettings settings) : base(
                "ContactUsPage",
                "/contact-us", driver, settings) {
        }
        public new ContactUsPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }

        public void ValidateContactUsFields() {
            AssertIsLoaded();
            AssertElementExists("ContactUs_Title");
            AssertElementExists("ContactUs_f_subtitle");
            AssertElementExists("ContactUs_f_FirstName");
            AssertElementExists("ContactUs_f_LastName");
            AssertElementExists("ContactUs_f_Email");
            AssertElementExists("ContactUs_f_State");
            AssertElementExists("ContactUs_f_Topic");
            AssertElementExists("ContactUs_f_Message");
            AssertElementExists("ContactUs_f_Save");
        }

        public NEARetireeHealthProgramRatesPage PerformLogin() {
            AssertSetTextBoxValue("login", "user");
            AssertClick("buttonSubmit");
            
            return new NEARetireeHealthProgramRatesPage(Driver, Settings);
        }

        public void PerformLoginGeneric() {
            AssertSetTextBoxValue("login", "user");
            AssertClick("buttonSubmit");
        }

        public ContactUsPage ClickOnEnglishToogle()
        {
            AssertClick(ToogleEnglish, timeoutSeconds: 30);
            return this;
        }
        public ContactUsPage VerifyEnglishToogle(string title)
        {
            var textBoxValue = GetElementFromControlKey(ToogleScrenTitle, 30)?.Text?.Trim();
            AssertIsTrue(string.Equals(title, textBoxValue, StringComparison.InvariantCultureIgnoreCase), $"Contactus title {textBoxValue} doesn't match {title}");
            return this;
        }
        public ContactUsPage ClickOnSpanishToogle()
        {
            AssertClick(ToogleSpanish, timeoutSeconds: 30);
            return this;
        }
        public ContactUsPage VerifySpanishToogle(string title)
        {
            var textBoxValue = GetElementFromControlKey(ToogleScrenTitleSpanish, 30)?.Text?.Trim();
            AssertIsTrue(textBoxValue.Contains(title), $"Contact title {textBoxValue} doesn't match {title}");
            return this;
        }
    }
}
