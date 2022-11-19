using System.Threading;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Registration {
    public class RegistrationPage : NeambPage
    {
        #region ControlKeys

        private const string FirstName = "Registration_Form_FirstName";
        private const string LastName = "Registration_Form_LastName";
        private const string BirthdateMonth = "Registration_Form_BirthDate_Month";
        private const string BirthdateMonth1 = "Registration_Form_BirthDate_Month1";
        private const string BirthdateDay = "Registration_Form_BirthDate_Day";
        private const string BirthdateYear = "Registration_Form_BirthDate_Year";
        private const string Address = "Registration_Form_Address";
        private const string City = "Registration_Form_City";
        private const string State = "Registration_Form_State";
        private const string Zip = "Registration_Form_Zip";
        private const string Phone = "Registration_Form_Phone";
        private const string EmailField = "Registration_Form_Email";
        private const string Password = "Registration_Form_Password";
        private const string ConfirmPassword = "Registration_Form_ConfirmPassword";
        private const string SubmitButton = "Registration_Form_Submit";
        private const string SubmitButton2 = "Registration_Form_Submit2";
        private const string SubmitButtonDiv = "Registration_Form_Submit_Div";
        private const string PassswordDuplication1 = "Duplicate_Password_Field";
        private const string SubmitEmailSelected = "SubmitEmailSelected";
        private const string ApplyChanges = "ApplyChanges";
        private const string ContinueButton = "ContinueButton";
        private const string FirstEmailToCheck = "FirstEmailToCheck";
        private static int Timeout => 60;

        #endregion

        public RegistrationPage(
            IWebDriver driver,
            ISettings settings) : base(driver, settings)
        {
        }

        public new RegistrationPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[]
            {
                "Welcome",
                "WizardStep1"
            });
            return this;
        }

        public new RegistrationPage CheckErrorEmailField(string email)
        {
            AssertSetTextBoxValue(EmailField, email);

            var errorEmailValidation = GetElementFromControlKey(EmailField)?
                .GetAttribute("data-msg-remote");

            AssertIsTrue(!string.IsNullOrEmpty(errorEmailValidation), $"Email validation doesn't show'");

            return this;
        }

        public new RegistrationPage SubmitStep1(string firstName, string lastName,
            string monthBirthdate, string dayBirthdate, string yearBirthdate, string address, string city, string state,
            string zip, string phone, string email, string password)
        {
            AssertSetTextBoxValue(FirstName, firstName);
            AssertSetTextBoxValue(LastName, lastName);
            //AssertSetTextBoxValue(Address, address);
            //AssertSetTextBoxValue(City, city);
            //AssertSetTextBoxValue(State, state);
            //AssertSetTextBoxValue(Zip, zip);
            //AssertSetTextBoxValue(Phone, phone);
            AssertSetTextBoxValue(EmailField, email);
            AssertSetTextBoxValue(Password, password);
            AssertSetTextBoxValue(ConfirmPassword, password);
            AssertClick(SubmitButton, timeoutSeconds: Timeout);
            return this;
        }

        public new RegistrationPage SubmitStep2(string firstName, string lastName,
            string monthBirthdate, string dayBirthdate, string yearBirthdate, string address, string city, string state,
            string zip, string phone, string email, string password)
        {
            AssertSetTextBoxValue(Address, address);
            AssertSetTextBoxValue(City, city);
            AssertSetTextBoxValue(State, state);
            AssertSetTextBoxValue(Zip, zip);
            
            var element =GetElementFromControlKey(BirthdateMonth1);
            
            element.Click();
            var jsExec = (IJavaScriptExecutor) this.Driver;
            //jsExec.ExecuteScript("window.scrollBy(0,120)");
            var mydata = (string)jsExec.ExecuteScript("arguments[0].click()", element);
            AssertSetTextBoxValue(BirthdateMonth, monthBirthdate);
            //Thread.Sleep(10000);
            //AssertSetTextBoxValue(BirthdateMonth, monthBirthdate);
            AssertSetTextBoxValue(BirthdateDay, dayBirthdate);
            AssertSetTextBoxValue(BirthdateYear, yearBirthdate);
            //Thread.Sleep(10000);
            //var element2 = GetElementFromControlKey(SubmitButtonDiv);

            //element2.Click();
            AssertClick(SubmitButton2, timeoutSeconds: Timeout);
            return this;
        }

        public new RegistrationPage ProcessDuplicateRegistrationWizard1(string password)
        {
            AssertHasAllControlsForSections(new[]
            {
                "DuplicateRegistrationWizard1",
            });
            AssertClick(FirstEmailToCheck, timeoutSeconds: Timeout);

            AssertSetTextBoxValue(PassswordDuplication1, password);
            AssertClick(SubmitEmailSelected, timeoutSeconds: Timeout);
           
            return this;
        }
        public new RegistrationPage ProcessDuplicateRegistrationWizard2()
        {
            AssertHasAllControlsForSections(new[]
            {
                "DuplicateRegistrationWizard2",
            });
            Thread.Sleep(5000);
            AssertClick(ApplyChanges, timeoutSeconds: Timeout);
            return this;
        }

        public new RegistrationPage ProcessDuplicateRegistrationWizard3()
        {
            AssertHasAllControlsForSections(new[]
            {
                "DuplicateRegistrationWizard3",
            });
            Thread.Sleep(8000);
            AssertClick(ContinueButton, timeoutSeconds: Timeout);

            return this;
        }
    }
}
