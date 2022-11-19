using System;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;

namespace Neambc.Neamb.Project.Web.UITest.Pages {
    [Obsolete("Use NeambPage")]
	public class NeambPageBase : AssertingPageBase {

		#region Properties
		public const string JessicaEmail = "nea.jessica@gmail.com";
		public const string JessicaPassword = "secret12";
		#endregion

		#region Constructors
		protected NeambPageBase(
			string pageName,
			string urlPath,
			IWebDriver driver,
			ISettings settings)
			: base(pageName, urlPath, driver, settings) {
		}
		protected NeambPageBase(
			IWebDriver driver,
			ISettings settings)
			: base(driver, settings) {
		}
		protected NeambPageBase(
			string name,
			IWebDriver driver,
			ISettings settings)
			: base(name, driver, settings) {
		}
		#endregion

		#region Public Methods
		public virtual void Login(string username, string password, string signinKey = null, bool force = false) {
			var signInElement = GetElementFromControlKey("LoginPage.signin", 0);
			if (force && (signInElement == null)) {
				// user is logged in but we wish to re-login
				Driver.Manage().Cookies.DeleteAllCookies();
				GoTo();
				IsLoaded();
				signInElement = GetElementFromControlKey("LoginPage.signin", 0);
			}
			if (force || (signInElement != null)) {
                AssertClick(signinKey ?? "Products.products_signin_button");
                AssertIsLoaded();
				AssertSetTextBoxValue("LoginPage.login_l_emailtextbox", username);
				AssertSetTextBoxValue("LoginPage.login_l_passwordtexbox", password);
                AssertElementExists("LoginPage.login_l_signinbutton", string.Empty,10);
				TryClick("LoginPage.login_l_signinbutton");
			}
		}
		public virtual void Logout() {
			Driver.Manage().Cookies.DeleteAllCookies();
			GoTo();
			IsLoaded();
		}
		#endregion
	}
}
