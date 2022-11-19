using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.OshynWebsite.UITests.PageTypes;

namespace UI_NEAMB.PagesTypes.CORE {
	public class LoginPage : PageBase {
		#region Properties
		public const string Url = "https://www.neamb.com/home.htm?ref=940";
		public const string ProductUrl = "https://qa.neamb.com/products/nea-accidental-death-and-dismemberment-insurance-plan";
		public const string HomeUrl = "https://qa.neamb.com/";
		public const string WarmColdUrl = "https://www.neamb.com/?ref=161";
		#endregion

		#region Constructor
		public LoginPage(			
			IWebDriver driver,
			ISettings settings
		) : base(driver, settings) {
			DeleteCache();

		}
		#endregion

		#region Public Methods
		public void CheckRequiredEmaildValidation(string password) {
			var password_field = AssertElementExists("password_texbox");
			AssertSetTextBoxValue("password_texbox", password);
			AssertClick("signin_button", timeoutSeconds: 30);
			AssertElementExists("error_empty_username");
			password_field.Clear();
			
		}
		public void CheckRequiredPassworddValidation(string mail) {
			var email_field = AssertElementExists("email_textbox");
			AssertSetTextBoxValue("email_textbox", mail);
			AssertClick("signin_button", timeoutSeconds: 30);
			AssertElementExists("error_empty_password");
			CleanFields();
		}
		public void UnregisteredEmailValidation(string username, string password) {
			CleanFields();
			PerformLogin(username, password);
			var ErrorMessage1 = AssertElementExists("error_wronguserpws");
			Assert.AreEqual("We're sorry, but the email address and/or password you entered were not recognized. Please try again.",
				ErrorMessage1.Text);
			CleanFields();
		}
		public void ValidateForgotPasswordPageLoad() {
			AssertClick("forgot_password_link");
			AssertElementExists("forgot_password_header");		
		}
		public void ValidateForgotEmailPageLoad() {
			AssertClick("forgot_email_link");
			AssertElementExists("forgot_email_header");			
		}
		public void ValidateCreateAccountPageLoad() {
			AssertClick("create_account_link_signin");
			AssertElementExists("create_account_header");
		}

		public void ValidateLoginValidCredentials(string username, string password) {
			CleanFields();
			PerformLogin(username, password);
			AssertElementExists("name_success_login");
			LogOut();
		}
		public void ValidateHotLogIn(string username, string password) {
			CleanFields();
			PerformLogin(username, password);
			AssertElementExists("name_success_login");
			AssertClick("Global.sign_in_option");
			AssertElementExists("Global.sign_out_option");
			AssertClick("Global.sign_out_option");

		}
		public void InvalidEmailFormat(string username) {
			CleanFields();
			UsernameValidation(username);
			AssertElementExists("error_email_format");			
		}
		public void WarmIdentificationMDSID() {
			Driver.Navigate().GoToUrl(Url);
			AssertElementExists("name_warm_login");
			AssertClick("Global.sign_in_option");
			AssertClick("warm_sign_in_option");
			var temp = AssertElementExists("email_textbox");
			var extra = string.Compare(temp.GetAttribute("value"), "dup1@oshyn.com");
		}
		public void WarmHotIdentificationMDSID() {
			Driver.Navigate().GoToUrl(Url);
			AssertElementExists("name_warm_login");
			AssertClick("Global.sign_in_option");			
			AssertElementExists("warm_not_you_option");
			AssertElementExists("warm_sign_in_option");
		}
		public void ColdIdentification() {
			Driver.Navigate().GoToUrl(HomeUrl);
			AssertElementExists("Global.sign_in_option");
			AssertElementExists("Global.create_sign_in_option");
			AssertElementExists("default_gray_picture_profile");
		}
		public void WarmColdIdentification() {
			Driver.Navigate().GoToUrl(WarmColdUrl);
			AssertElementExists("name_warm_cold_login");
			AssertClick("Global.sign_in_option");
			AssertElementExists("warm_not_you_option");
			AssertElementExists("warm_cold_create_account");
		}
		public void RememberMeCheckbox(string username, string password) {
			CleanFields();
			PerformLogin(username, password);
			LogOut();
			AssertClick("cold_sign_in_option");
			var temp = AssertElementExists("email_textbox");
			var extra = string.Compare(temp.GetAttribute("value"), "nea.jessica@gmail.com");
			
		}
		public void ReturnedToPageWhereLogin(string username, string password) {
			Driver.Navigate().GoToUrl(ProductUrl);
			AssertClick("cold_sign_in_option");
			CleanFields();
			PerformLogin(username, password);
			var URL = Driver.Url;
			Assert.AreEqual(URL, ProductUrl);
			LogOut();
		}
		public void LogOutValidation(string username, string password) {
			CleanFields();
			PerformLogin(username,password);
			LogOut();
			var URL = Driver.Url;
			Assert.AreEqual(URL, HomeUrl);
		}

		#endregion
	}
}
