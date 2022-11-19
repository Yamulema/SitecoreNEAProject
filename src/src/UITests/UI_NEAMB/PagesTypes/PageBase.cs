using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.Framework.UITesting.Page;
namespace Oshyn.OshynWebsite.UITests.PageTypes {

	public class PageBase : AssertingPageBase {

		#region Constructors
		protected PageBase(string pageName, string urlPath, IWebDriver driver, ISettings settings)
			: base(pageName, urlPath, driver, settings) {
		}
		protected PageBase(IWebDriver driver, ISettings settings)
			: base(driver, settings) {
		}
		#endregion

		#region Public Methods
		public void DeleteCache() {
			Driver.Manage().Cookies.DeleteAllCookies();
		}
		#endregion

		#region Public Methods
		public void PerformLogin(string username, string password) {
			AssertSetTextBoxValue("email_textbox", username);
			AssertSetTextBoxValue("password_texbox", password);
			AssertClick("signin_button");
		}
		public void UsernameValidation(string username) {
			AssertSetTextBoxValue("email_textbox", username);
			AssertClick("signin_button");
		}
		public void CleanFields() {
			var clr_username = AssertElementExists("email_textbox");
			var clr_password = AssertElementExists("password_texbox");
			clr_username.Clear();
			clr_password.Clear();
		}
		public void LogOut() {
			AssertClick("Global.sign_in_option");
			AssertClick("Global.sign_out_option");
		}
		#endregion


	}
}