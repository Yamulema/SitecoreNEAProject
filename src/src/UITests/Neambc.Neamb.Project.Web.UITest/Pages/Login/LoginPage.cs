using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.ForgotEmail;
using Neambc.Neamb.Project.Web.UITest.Pages.ForgotPassword;
using Neambc.Neamb.Project.Web.UITest.Pages.Home;
using Neambc.Neamb.Project.Web.UITest.Pages.Registration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Login
{
    public class LoginPage : NeambPage
    {
        #region Constants
        private static int Timeout => 30;
        #endregion
        #region Constructor
        public LoginPage(IWebDriver driver, ISettings settings) : base(name: "LoginPage", driver: driver,
            settings: settings)
        { }
        #endregion
        #region ControlKeys
        private const string PasswordTextBox = "LoginPage_Form_PasswordTextbox";
        private const string UsernameTextBox = "LoginPage_Form_EmailTextbox";
        private const string SignInButton = "LoginPage_Form_SignInButton";
        private const string ForgotEmailLink = "LoginPage_Form_ForgotEmailLink";
		private const string ForgotPasswordLink = "LoginPage_Form_ForgotPasswordLink";
        private const string Form = "LoginPage_Form_Form";
        private const string EmailError = "LoginPage_FormErrors_EmailError";
        private const string PasswordError = "LoginPage_FormErrors_PasswordError";
        private const string Error = "LoginPage_FormErrors_Error";
        private const string RememberMe = "LoginPage_Form_RememberMe";
        private const string RegistrationLink = "LoginPage_Registration_Link";
        #endregion

        #region Public Methods
        public T SignIn<T>(string username, string password) where T : class
        {
            CleanFields();
            PerformLoginAction(username, password);
            return Activator.CreateInstance(typeof(T), this.Driver, this.Settings) as T;
        }

        public ForgotEmailPage ClickOnForgotEmailLink()
        {
            AssertClick(ForgotEmailLink, timeoutSeconds: Timeout);
            return new ForgotEmailPage(this.Driver, this.Settings);
        }
		public ForgotPasswordPage ClickOnForgotPasswordLink() {
			AssertClick(ForgotPasswordLink, timeoutSeconds: Timeout);
			return new ForgotPasswordPage(this.Driver, this.Settings);
		}
		public LoginPage ClickOnEmailField()
        {
            AssertClick(UsernameTextBox);
            return this;
        }

        public LoginPage ClickOnPasswordField()
        {
            AssertClick(PasswordTextBox);
            return this;
        }

        public LoginPage ClickOutsideForm()
        {
            AssertClick(Form);
            return this;
        }

        public LoginPage AssertEmailErrorMessage(string message)
        {
            AssertSearchForElement(EmailError, message);
            return this;
        }

        public T ClickOnSignInButton<T>() where T : class
        {
            AssertClick(SignInButton, timeoutSeconds: Timeout);
            return Activator.CreateInstance(typeof(T), this.Driver, this.Settings) as T;
        }
        public LoginPage AssertPasswordErrorMessage(string message)
        {
            AssertSearchForElement(PasswordError, message);
            return this;
        }
        public LoginPage AssertErrorMessage(string message)
        {
            AssertSearchForElement(Error, message);
            return this;
        }
        public new LoginPage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "Header",
                "Form",
                "Registration"
            });
            return this;
        }
        public LoginPage FillEmailField(string email)
        {
            AssertSetTextBoxValue(UsernameTextBox, email);
            return this;
        }

        public LoginPage AssertUsernameFieldValue(string username)
        {
            var textBoxValue = GetElementFromControlKey(UsernameTextBox)?
                .GetAttribute("value");
            AssertIsTrue(string.Equals(username, textBoxValue, StringComparison.InvariantCultureIgnoreCase), $"UsernameFieldValue {textBoxValue} doesn't match {username}");
            return this;
        }
		public LoginPage CheckRememberMe()
        {
            AssertClick(RememberMe);
            return this;
        }

        public RegistrationPage ClickOnCreateAccount()
        {
            AssertClick(RegistrationLink);
            return new RegistrationPage(this.Driver, this.Settings);
        }
        #endregion

        #region Support Asserts
        private void CleanFields()
        {
            AssertElementExists(UsernameTextBox).Clear();
            AssertElementExists(PasswordTextBox).Clear();
        }

        private void PerformLoginAction(string username, string password)
        {
            AssertSetTextBoxValue(UsernameTextBox, username);
            AssertSetTextBoxValue(PasswordTextBox, password);
            AssertClick(SignInButton, timeoutSeconds: Timeout);
        }
        #endregion
    }
}
