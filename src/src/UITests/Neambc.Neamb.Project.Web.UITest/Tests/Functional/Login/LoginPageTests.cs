using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using Neambc.Neamb.Project.Web.UITest.Pages.Home;
using Neambc.Neamb.Project.Web.UITest.Pages.Login;
using NUnit.Framework;

namespace Neambc.Neamb.Project.Web.UITest.Tests.Functional.Login
{
    [TestFixture]
    public class LoginPageTests : NeambTestBaseLarge<LoginPage>
    {
        /// <summary>
        /// NEAMBMRO-1366
        /// </summary>
        [Test, Category("Functional")]
        public void VerifyUElementsPresence()
        {
            Page.AssertIsLoaded();
        }

        /// <summary>
        /// NEAMBMRO-1919
        /// </summary>
        [Test, Category("Functional")]
        public void ValidateForgotUsernameLink()
        {
			Page.AssertIsLoaded()
				.ClickOnForgotEmailLink()
                .AssertIsLoaded();
        }

        /// <summary>
        /// NEAMBMRO-1927
        /// </summary>
        /// <param name="emailErrorMessage"></param>
        /// <param name="passwordErrorMessage"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1927" })]
        [Test, Category("Functional")]
        public void ValidateEmptyEmailErrorMessage(string emailErrorMessage, string passwordErrorMessage)
        {
            Page.ClickOnEmailField()
                .ClickOutsideForm()
                .AssertEmailErrorMessage(emailErrorMessage)
                .ClickOnSignInButton<LoginPage>()
                .AssertPasswordErrorMessage(passwordErrorMessage);
        }

        /// <summary>
        /// NEAMBMRO-1587
        /// </summary>
        /// <param name="emailErrorMessage"></param>
        /// <param name="passwordErrorMessage"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1587" })]
		//errors
        [Test, Category("Functional")]
        public void ValidateEmptyPasswordErrorMessage(string emailErrorMessage, string passwordErrorMessage)
        {

            Page.ClickOnSignInButton<LoginPage>()
               .AssertPasswordErrorMessage(passwordErrorMessage)
               .AssertEmailErrorMessage(emailErrorMessage);
        }

        /// <summary>
        /// NEAMBMRO-1762
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="errorMessage"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1762" })]
        [Test, Category("Functional")]
        public void ValidateLoginWithUnregisteredEmail(string username, string password, string errorMessage)
        {
            Page.AssertIsLoaded()
                .SignIn<LoginPage>(username, password)
                .AssertErrorMessage(errorMessage);
        }

        /// <summary>
        /// NEAMBMRO-1733
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1733" })]
        [Test, Category("Functional")]
        public void ValidateLoginWithValidCredentials(string username, string password, string url)
        {
            Page.GoToPage<HomePage>(url)
                .AssertIsLoaded()
                .ClickOnSignInLink()
                .AssertIsLoaded()
                .SignIn<HomePage>(username, password)
                .AssertIsLoggedIn();
        }

        /// <summary>
        /// NEAMBMRO-1404
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emailErrorMessage"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1404" })]
        [Test, Category("Functional")]
        public void VerifyInvalidEmailFormatValidation(string email, string emailErrorMessage)
        {
            Page.FillEmailField(email)
                .ClickOnSignInButton<LoginPage>()
                .AssertEmailErrorMessage(emailErrorMessage);
        }

        /// <summary>
        /// NEAMBMRO-1922
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1922" })]
        [Test, Category("Functional")]
        public void ValidateAuthenticationStatusAccuracyHot(string username, string password)
        {
            Page.SignIn<NeambPage>(username, password)
                .AssertIsLoggedIn();
        }

        /// <summary>
        /// NEAMBMRO-1799
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1799" })]
        [Test, Category("Functional")]
        public void VerifyWarmUserIdentificationFromMdsIdOnUrl(string url, string username)
        {
            Page.GoToPage<LoginPage>(url)
                .AssertUsernameFieldValue(username);
        }

        /// <summary>
        /// NEAMBMRO-1571
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="loginUrl"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1571" })]
        [Test, Category("Functional")]
        public void VerifyThatUsernameIsRememberedAcrossSessionsWhenRememberMeCheckboxIsMarked(string username, string password, string loginUrl)
        {
            Page//.CheckRememberMe() // RememberMe is checked by default
                .SignIn<NeambPage>(username, password)
                .AssertIsLoggedIn()
                .ClickOnSignOutLink()
                .GoToPage<LoginPage>(loginUrl)
                .AssertIsLoaded()
                .AssertUsernameFieldValue(username);
        }

        /// <summary>
        /// NEAMBMRO-1916
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1916" })]
        [Test, Category("Functional")]
        public void ValidateThatUserIsReturnedToThePageFromWhereTheLoginWasInitiated(string username, string password, string url)
        {
            Page
                .GoToPage<NeambPage>(url)
                .ClickOnSignInLink()
                .AssertIsLoaded()
                .SignIn<NeambPage>(username, password)
                .AssertIsLoaded()
                .AssertIsLoggedIn()
                .AssertUrl(url)
                .ClickOnSignOutLink()
                .AssertIsLoggedOut();
        }

        /// <summary>
        /// NEAMBMRO-1921
        /// </summary>
        /// <param name="url"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1921" })]
        [Test, Category("Functional")]
        public void ValidateAuthenticationStatusAccuracyWarmHot(string url)
        {
            Page
                .GoToPage<HomePage>(url)
                .AssertIsLoaded()
                .AssertIsWarm();
        }

        /// <summary>
        /// NEAMBMRO-1210
        /// </summary>
        [Test, Category("Functional")]
        public void ValidateAuthenticationStatusAccuracyCold()
        {
            Page
                .GoToPage<HomePage>("/")
                .AssertIsLoaded()
                .AssertIsCold();
        }

        /// <summary>
        /// NEAMBMRO-1273
        /// </summary>
        /// <param name="url"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1273" })]
        [Test, Category("Functional")]
        public void ValidateAuthenticationStatusAccuracyWarmCold(string url)
        {
            Page
                .GoToPage<HomePage>(url)
                .AssertIsLoaded()
                .AssertIsWarmCold();
        }

        /// <summary>
        /// NEAMBMRO-1983
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        [TestCaseSource(typeof(LoginPageTestData),
            nameof(LoginPageTestData.TestDataSource),
            new object[] { "Test_1983" })]
        [Test, Category("Functional")]
        public void ValidateLogOut(string username, string password)
        {
            Page
                .SignIn<NeambPage>(username, password)
                .AssertIsLoggedIn()
                .ClickOnSignOutLink()
                .AssertIsLoggedOut();
        }

        /// <summary>
        /// NEAMBMRO-1719
        /// </summary>
        [Test, Category("Functional")]
        public void ValidateForgotPasswordLink()
        {
            Page
                .ClickOnForgotPasswordLink()
                .AssertIsLoaded();
        }

        /// <summary>
        /// NEAMBMRO-1654
        /// </summary>
        [Test, Category("Functional")]
        public void ValidateCreateAnAccountLink()
        {
            Page
                .ClickOnCreateAccount()
                .AssertIsLoaded();
        }
	}
}
