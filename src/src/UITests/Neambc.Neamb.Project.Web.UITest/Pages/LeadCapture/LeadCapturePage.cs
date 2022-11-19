using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using System;

namespace Neambc.Neamb.Project.Web.UITest.Pages.LeadCapture
{
    public class LeadCapturePage : NeambPage
    {
        #region Constructor
        public LeadCapturePage(IWebDriver driver, ISettings settings) : base(name: "LeadCapturePage", driver: driver,
            settings: settings)
        {

        }
        #endregion

        #region ControlKeys
        private const string Form = "LeadCapturePage_Form";
        private const string FirstNameInput = "LeadCapturePage_Form_FirstName";
        private const string FirstNameError = "LeadCapturePage_Form_FirstName_Error";       
        private const string LastNameInput = "LeadCapturePage_Form_LastName";
        private const string LastNameError = "LeadCapturePage_Form_LastName_Error";
        private const string EmailInput = "LeadCapturePage_Form_Email";
        private const string EmailError = "LeadCapturePage_Form_Email_Error";
        private const string AddressInput = "LeadCapturePage_Form_Address";
        private const string AddressError = "LeadCapturePage_Form_Address_Error";
        private const string CityInput = "LeadCapturePage_Form_City";
        private const string CityError = "LeadCapturePage_Form_City_Error";
        private const string StateSelect = "LeadCapturePage_Form_State";
        private const string StateError = "LeadCapturePage_Form_State_Error";
        private const string ZipCodeInput = "LeadCapturePage_Form_ZipCode";
        private const string ZipCodeError = "LeadCapturePage_Form_ZipCode_Error";
        private const string PhoneInput = "LeadCapturePage_Form_Phone";
        private const string PhoneError = "LeadCapturePage_Form_Phone_Error";
        private const string RequestForRadio = "LeadCapturePage_Form_RequestFor";
        private const string SubmitButton = "LeadCapturePage_Form_Submit";
        #endregion

        public new LeadCapturePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Form"
            });
            return this;
        }

        public LeadCapturePage ClickOnFirstNameField()
        {
            AssertClick(FirstNameInput);
            return this;
        }

        public LeadCapturePage ClickSubmitForm()
        {
            AssertClick(SubmitButton);
            return this;
        }

        public LeadCapturePage FillFirstNameField(string text)
        {
            AssertSetTextBoxValue(FirstNameInput, text);
            return this;
        }

        public LeadCapturePage FillLastNameField(string text)
        {
            AssertSetTextBoxValue(LastNameInput, text);
            return this;
        }

        public LeadCapturePage FillEmailField(string text)
        {
            AssertSetTextBoxValue(EmailInput, text);
            return this;
        }

        public LeadCapturePage FillAddressField(string text)
        {
            AssertSetTextBoxValue(AddressInput, text);
            return this;
        }

        public LeadCapturePage FillStateField(string text)
        {
            AssertSetComboBoxValueByValue(StateSelect, text);
            return this;
        }

        public LeadCapturePage FillCityField(string text)
        {
            AssertSetTextBoxValue(CityInput, text);
            return this;
        }

        public LeadCapturePage FillZipCodeField(string text)
        {
            AssertSetTextBoxValue(ZipCodeInput, text);
            return this;
        }

        public LeadCapturePage FillPhoneField(string text)
        {
            AssertSetTextBoxValue(PhoneInput, text);
            return this;
        }

        public LeadCapturePage AssertFirstNameErrors(string message)
        {
            var error = GetElementFromControlKey(FirstNameError)?
                .Text.ToString();
            AssertIsTrue(string.Equals(error, message, StringComparison.InvariantCultureIgnoreCase),
                $"FirstName validation failing'");

            return this;
        }

        public LeadCapturePage AssertLastNameErrors(string message)
        {
            var error = GetElementFromControlKey(LastNameError)?
                .Text.ToString();
            AssertIsTrue(string.Equals(error, message, StringComparison.InvariantCultureIgnoreCase),
                $"LastName validation failing'");

            return this;
        }

        public LeadCapturePage AssertEmailErrors(string message)
        {           
            var error = GetElementFromControlKey(EmailError)?
                .Text.ToString();
            AssertIsTrue(string.Equals(error, message, StringComparison.InvariantCultureIgnoreCase), 
                $"Email validation failing'");

            return this;
        }

        public LeadCapturePage AssertAddressErrors(string message)
        {
            var error = GetElementFromControlKey(AddressError)?
                .Text.ToString();
            AssertIsTrue(string.Equals(error, message, StringComparison.InvariantCultureIgnoreCase),
                $"Address validation failing'");

            return this;
        }

        public LeadCapturePage AssertCityErrors(string message)
        {
            var error = GetElementFromControlKey(CityError)?
                .Text.ToString();
            AssertIsTrue(string.Equals(error, message, StringComparison.InvariantCultureIgnoreCase),
                $"City validation failing'");

            return this;
        }

        public LeadCapturePage AssertStateErrors(string message)
        {
            var error = GetElementFromControlKey(StateError)?
                .Text.ToString();
            AssertIsTrue(string.Equals(error, message, StringComparison.InvariantCultureIgnoreCase),
                $"State validation failing'");

            return this;
        }

        public LeadCapturePage AssertZipCodeErrors(string message)
        {
            var error = GetElementFromControlKey(ZipCodeError)?
                .Text.ToString();
            AssertIsTrue(string.Equals(error, message, StringComparison.InvariantCultureIgnoreCase),
                $"ZipCode validation failing'");

            return this;
        }

        public LeadCapturePage AssertPhoneErrors(string message)
        {
            var error = GetElementFromControlKey(PhoneError)?
                .Text.ToString();
            AssertIsTrue(string.Equals(error, message, StringComparison.InvariantCultureIgnoreCase),
                $"Phone validation failing'");

            return this;
        }

        public LeadCapturePage AssertWarmUserData(string firstName, string lastName,
            string email, string address, string city, string state, string zipCode, string phone)
        {
            AssertInputFormValues(FirstNameInput, firstName);
            AssertInputFormValues(LastNameInput, lastName);
            AssertInputFormValues(EmailInput, email);
            AssertInputFormValues(AddressInput, address);
            AssertInputFormValues(CityInput, city);
            AssertInputFormValues(StateSelect, state);
            AssertInputFormValues(ZipCodeInput, zipCode);
            AssertInputFormValues(PhoneInput, phone);
            return this;
        }

        private LeadCapturePage AssertInputFormValues(string controlKey, string value)
        {
            var controKeyValue = GetElementFromControlKey(controlKey)?
                    .GetAttribute("value");
            AssertIsTrue(string.Equals(controKeyValue, value, StringComparison.InvariantCultureIgnoreCase),
                $"{controlKey} {controKeyValue} doesn't match {value}");
            return this;
        }
    }
}
