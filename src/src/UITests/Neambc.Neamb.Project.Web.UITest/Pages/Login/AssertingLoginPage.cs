using NUnit.Framework;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Login {
    public class AssertingLoginPage : AssertingPageBase {

        #region Constructor
        public AssertingLoginPage(IWebDriver driver, ISettings settings) : base(name: "LoginPage", driver: driver,
            settings: settings) { }
        #endregion

        #region ControlKeys
        private const string PasswordTextBox = "login_l_passwordtexbox";
        private const string UsernameTextBox = "login_l_emailtextbox";
        private const string SignInButton = "login_l_signinbutton";
        private const string UsernameErrorMessage = "login_e_EmptyUserName";
        private const string EmptyPasswordErrorMessage = "login_e_EmptyPassword";
        private const string GeneralErrorMessage = "login_w_wronguserpws";
        private const string ForgotPasswordLink = "login_l_ForgotPasswordLink";
        private const string ForgotPasswordPageTitle = "login_fp_Title";
        private const string RegistrationLink = "login_r_registration_link";
        private const string RegistrationPageTitle = "login_r_registration_title";
        #endregion

        #region Standard Values for Actions
        private const int Timeout = 30; // This needs to be check for the default values in the settings file
        #endregion

        #region Expected Values
        private const string ExpectedUnknownUsernameOrPasswordMessage =
            "We're sorry, but the email address and/or password you entered were not recognized. Please try again.";
        private const string ExpectedMissingUsernameOnLoginMessage = "Please enter your email.";
        private const string ExpectedMissingPasswordOnLoginMessage = "Please enter your password.";
        private const string ExpectedInvalidFormatUsernameOnLoginMessage =
            "Invalid email address. Please check that you have entered your email address correctly and try again.";
        private const string ExpectedForgotPasswordLinkText = "Forgot Password";
        private const string ExpectedForgotPasswordPageTitle = "Reset Password";
        private const string ExpectedRegistrationLinkText = "Create an Account";
        private const string ExpectedRegistrationPageTitle = "Create an Account with Member Benefits";
        #endregion

        #region Navigation
        public void CheckLoginPageHasAllControls() {
            /*
             TODO: Review what areas need control validation
             TODO: Review if areas should have xPath with validation (contains) or such validations should be made in code  
             */
            AssertHasAllControlsForSections(new[] {
                "Header"
            });
        }
        #endregion

        #region Field Validation
        public void CheckUsernameRequiredValidation(string password) {
            AssertSetTextBoxValue(PasswordTextBox, password);
            AssertClick(SignInButton, timeoutSeconds: Timeout);
            var errorMessageWebElement = AssertElementExists(UsernameErrorMessage);
            Assert.AreEqual(ExpectedMissingUsernameOnLoginMessage, errorMessageWebElement.Text);
            Assert.IsTrue(errorMessageWebElement.Displayed);
        }
        public void CheckPasswordRequiredValidation(string username) {
            AssertSetTextBoxValue(UsernameTextBox, username);
            AssertClick(SignInButton, timeoutSeconds: Timeout);
            var errorMessageWebElement = AssertElementExists(EmptyPasswordErrorMessage);
            Assert.AreEqual(ExpectedMissingPasswordOnLoginMessage, errorMessageWebElement.Text);
            Assert.IsTrue(errorMessageWebElement.Displayed);
        }
        public void CheckUsernameFormatValidation(string username) {
            AssertSetTextBoxValue(UsernameTextBox, username);
            AssertClick(SignInButton, timeoutSeconds: Timeout);
            var errorMessageWebElement = AssertElementExists(UsernameErrorMessage);
            Assert.AreEqual(ExpectedInvalidFormatUsernameOnLoginMessage, errorMessageWebElement.Text);
            Assert.IsTrue(errorMessageWebElement.Displayed);
        }
        public void UnknownUsernameValidation(string username, string password) {
            PerformLoginAction(username, password);
            var actualUnknownUsernameMessage = AssertElementExists(GeneralErrorMessage);
            Assert.AreEqual(ExpectedUnknownUsernameOrPasswordMessage, actualUnknownUsernameMessage.Text);
            Assert.IsTrue(actualUnknownUsernameMessage.Displayed);
        }
        #endregion

        #region Link Validation
        public void CheckForgotPasswordLink() {
            var actualLink = AssertElementExists(ForgotPasswordLink);
            Assert.AreEqual(ExpectedForgotPasswordLinkText, actualLink.Text);
            AssertClick(ForgotPasswordLink);
            var actualTitle = AssertElementExists(ForgotPasswordPageTitle);
            Assert.AreEqual(ExpectedForgotPasswordPageTitle, actualTitle.Text);
        }

        public void CheckRegistrationLink() {
            var actualLink = AssertElementExists(RegistrationLink);
            Assert.AreEqual(ExpectedRegistrationLinkText, actualLink.Text);
            AssertClick(RegistrationLink);
            var actualTitle = AssertElementExists(RegistrationPageTitle);
            Assert.AreEqual(ExpectedRegistrationPageTitle, actualTitle.Text);
        }
        #endregion

        #region Support Asserts
        public void CleanFields() {
            AssertElementExists(UsernameTextBox).Clear();
            AssertElementExists(PasswordTextBox).Clear();
        }

        public void PerformLoginAction(string username, string password) {
            AssertSetTextBoxValue(UsernameTextBox, username);
            AssertSetTextBoxValue(PasswordTextBox, password);
            AssertClick(SignInButton);
        }
        #endregion

        #region Private Support Methods
        private void ResetToStartPage() {
            GoTo(UrlAbsolutePath);
        }
        #endregion
    }
}
