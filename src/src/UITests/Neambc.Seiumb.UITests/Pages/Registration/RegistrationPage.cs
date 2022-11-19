using System.Threading;
using Neambc.Seiumb.UITests.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Seiumb.UITests.Pages.Registration {
    public class RegistrationPage : SeiumbPage
    {
        #region ControlKeys

        private const string FirstName = "Registration_Form_FirstName";
        private const string LastName = "Registration_Form_LastName";
        private const string Birthdate = "Registration_Form_BirthDate";
        private const string Address = "Registration_Form_Address";
        private const string City = "Registration_Form_City";
        private const string State = "Registration_Form_State";
        private const string Zip = "Registration_Form_Zip";
        private const string Phone = "Registration_Form_Phone";
        private const string EmailField = "Registration_Form_Email";
        private const string Password = "Registration_Form_Password";
        private const string ConfirmPassword = "Registration_Form_ConfirmPassword";
        private const string SubmitButton = "Registration_Form_Submit";
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
                "Form"
            });
            return this;
        }

        public new RegistrationPage SubmitDuplicateRegistration(string firstName, string lastName,
            string birthdate, string address, string city, string state,
            string zip, string phone, string email, string password)
        {
            AssertSetTextBoxValue(FirstName, firstName);
            AssertSetTextBoxValue(LastName, lastName);
            AssertSetTextBoxValue(Birthdate, birthdate);
            AssertSetTextBoxValue(Address, address);
            AssertSetTextBoxValue(City, city);
            AssertSetTextBoxValue(State, state);
            AssertSetTextBoxValue(Zip, zip);
            AssertSetTextBoxValue(Phone, phone);
            AssertSetTextBoxValue(EmailField, email);
            AssertSetTextBoxValue(Password, password);
            AssertSetTextBoxValue(ConfirmPassword, password);
            AssertClick(SubmitButton, timeoutSeconds: Timeout);
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
            Thread.Sleep(3000);
            AssertClick(ApplyChanges, timeoutSeconds: Timeout);
            return this;
        }

        public new RegistrationPage ProcessDuplicateRegistrationWizard3()
        {
            AssertHasAllControlsForSections(new[]
            {
                "DuplicateRegistrationWizard3",
            });
            Thread.Sleep(5000);
            AssertClick(ContinueButton, timeoutSeconds: Timeout);
            return this;
        }
    }
}
