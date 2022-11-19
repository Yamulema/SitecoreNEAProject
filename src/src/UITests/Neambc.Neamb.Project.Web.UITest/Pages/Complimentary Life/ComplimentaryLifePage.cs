using Neambc.Neamb.Project.Web.UITest.Pages.Base;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;

namespace Neambc.Neamb.Project.Web.UITest.Pages.Complimentary_Life {
	public class ComplimentaryLifePage : NeambPage
    {

		public ComplimentaryLifePage(
			IWebDriver driver,
			ISettings settings) : 
			base(driver, settings) {
		}

        public virtual void LoginCompLife(string username, string password)
        {
           // var signInElement = GetElementFromControlKey("ComplimentaryLifePage.CompLife_h_SignIn", 0);
           //TryClick(signInElement);
             //AssertClick(signinKey ?? "ProfilePage.Profile_h_SignIn");
                AssertSetTextBoxValue("LoginPage.LoginPage_Form_EmailTextbox", username);
                AssertSetTextBoxValue("LoginPage.LoginPage_Form_PasswordTextbox", password);
                TryClick("LoginPage.LoginPage_Form_SignInButton");
          
        }

        public ComplimentaryLifePage VerifyUrlHasParameter(string parameterName)
        {
            var value = Driver.Url;
            AssertIsTrue(value.Contains(parameterName), $"The url doesn't have the parameter {parameterName}");

            return this;
        }
    }
}
