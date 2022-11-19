using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using UI_NEAMB.PagesTypes.CORE;

namespace UI_NEAMB.Tests.CORE {
	[TestFixture]
	public class LoginTests : TestBaseLarge<LoginPage> {
		#region Tests
		[Test, Category("Navigation")]
		public void LoginHasAllControls() {
			Page.AssertHasAllControlsForSections(new[] {
				"Header",
				"LoginForm",
				"AccountSection"
			});
		}

		[Test, Category("Invalid Email")]
		public void InvalidLoginEmptyEmailField() {
			Page.CheckRequiredEmaildValidation("Secret12");
		}

		[Test, Category("Invalid Password")]
		public void InvalidLoginEmptyPasswordField() {
			Page.CheckRequiredPassworddValidation("nea.jessica@gmail.com");
		}
		[Test, Category("Invalid Login")]
		public void ValidateUnregisteredEmail() {
			Page.UnregisteredEmailValidation("nea.jesi@gmail.com", "sec12");
		}

		[Test, Category("Link to Forgot Password Page")]
		public void ForgotPasswordLink() {
			Page.ValidateForgotPasswordPageLoad();
		}
		[Test, Category("Link to Forgot Password Page")]
		public void CreateAccountLink() {
			Page.ValidateCreateAccountPageLoad();
		}

		[Test, Category("Link to Forgot Email Page")]
		public void ForgoEmailLink() {
			Page.ValidateForgotEmailPageLoad();
		}
		[Test, Category("Login")]
		public void ValidateLogin() {
			Page.ValidateLoginValidCredentials("nea.jessica@gmail.com", "secret12");
		}
		[Test, Category("Login")]
		public void ValidateHotLogin() {
			Page.ValidateHotLogIn("nea.jessica@gmail.com", "secret12");
		}
		[Test, Category("Login")]
		public void ValidateInvalidEmailFormat() {			
			var invalid_mails = new List<string>() { "anyemail","any@test","anyemail@test.c" };
			for (var i = 0; i < invalid_mails.Count; i++) {
				Page.InvalidEmailFormat(invalid_mails[i]);
			}	
		}
		[Test, Category("Login")]
		public void ValidateWarmUserFromMDS() {
			Page.WarmIdentificationMDSID();
		}
		[Test, Category("Login")]
		public void ValidateWarmHotStatusAccurancy() {
			Page.WarmHotIdentificationMDSID();
		}
		[Test, Category("Login")]
		public void ValidateRememberMeCheckbox() {
			Page.RememberMeCheckbox("nea.jessica@gmail.com", "secret12");			
		}
		[Test, Category("Login")]
		public void ValidateLoginReturnedPage() {
			Page.ReturnedToPageWhereLogin("nea.jessica@gmail.com", "secret12");
		}
		[Test, Category("Login")]
		public void ValidateColdStatusAccurancy() {
			Page.ColdIdentification();
		}
		[Test, Category("Login")]
		public void ValidateWarmColdStatusAccurancy() {
			Page.WarmColdIdentification();
		}
		[Test, Category("Login")]
		public void LogOutValidation() {
			Page.LogOutValidation("nea.jessica@gmail.com", "secret12");
		}

		#endregion
	}
}
