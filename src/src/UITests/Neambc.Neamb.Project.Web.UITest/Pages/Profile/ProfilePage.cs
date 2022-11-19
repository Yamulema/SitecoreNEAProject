using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Profile {
	public class ProfilePage : NeambPage
    {
        #region ControlKeys
        private const string EmailField = "Profile_f_Email";
        #endregion

        public ProfilePage(
            IWebDriver driver,
            ISettings settings) : base(driver, settings)
        {
        }

        public new ProfilePage AssertIsLoaded()
        {
            AssertHasAllControlsForSections(new[] {
                "ProfileForm"
            });
            return this;
        }

        public void LoginProfile(string username, string password)
        {
            AssertSetTextBoxValue("LoginPage.login_l_emailtextbox", username);
            AssertSetTextBoxValue("LoginPage.login_l_passwordtexbox", password);
            AssertElementExists("LoginPage.login_l_signinbutton");
            TryClick("LoginPage.login_l_signinbutton");
        }
        public new ProfilePage CheckErrorEmailField(string email)
        {
            AssertSetTextBoxValue(EmailField, email);

            var errorEmailValidation = GetElementFromControlKey(EmailField)?
                .GetAttribute("data-msg-validatemail");

            AssertIsTrue(!string.IsNullOrEmpty(errorEmailValidation), $"Email validation doesn't show'");

            return this;
        }
    }
}
