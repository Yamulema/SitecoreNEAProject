using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Oshyn.Framework.UITesting.Info;
using Oshyn.OshynWebsite.UITests.PageTypes;
using UI_NEAMB.DTO;
using UI_NEAMB.PagesTypes.DTO;

namespace UI_NEAMB.PagesTypes.CORE {
	public class RegistrationPage : PageBase {
		#region Properties
		public const string ProductUrl = "https://qa.neamb.com/products/nea-accidental-death-and-dismemberment-insurance-plan";
		#endregion
		#region Constructor
		public RegistrationPage(			
			IWebDriver driver,
			ISettings settings
		) : base(driver, settings) {
			DeleteCache();

		}
		#endregion
		
		#region Public Methods
		public void RandomUserRequestsRegistration(UserRegistration user) {
			AssertSetTextBoxValue("firstname_field", user.FirstName);
			AssertSetTextBoxValue("lastname_field", user.LastName);
			AssertSetTextBoxValue("month_field", user.Dob_Month);
			AssertSetTextBoxValue("day_field", user.Dob_Day);
			AssertSetTextBoxValue("year_field", user.Dob_Year);
			AssertSetTextBoxValue("address_field", user.Address);
			AssertSetTextBoxValue("city_field", user.City);
			AssertSetTextBoxValue("state_field", user.State);
			AssertSetTextBoxValue("zip_field", user.Zip);
			AssertSetTextBoxValue("phone_field", user.Phone);
			AssertSetTextBoxValue("email_field", user.Mail);
			AssertSetTextBoxValue("password_field", user.Password);
			AssertSetTextBoxValue("confirmpassword_field", user.Password);
			AssertClick("create_account_button");
			
		}
		public void RandomUserRequestsRegistration2(UserRegistration2 user) {
			AssertSetTextBoxValue("firstname_field", user.FirstName2);
			AssertSetTextBoxValue("lastname_field", user.LastName2);
			AssertSetTextBoxValue("month_field", user.Dob_Month2);
			AssertSetTextBoxValue("day_field", user.Dob_Day2);
			AssertSetTextBoxValue("year_field", user.Dob_Year2);
			AssertSetTextBoxValue("address_field", user.Address2);
			AssertSetTextBoxValue("city_field", user.City2);
			AssertSetTextBoxValue("state_field", user.State2);
			AssertSetTextBoxValue("zip_field", user.Zip2);
			AssertSetTextBoxValue("phone_field", user.Phone2);
			AssertSetTextBoxValue("email_field", user.Mail2);
			AssertSetTextBoxValue("password_field", user.Password2);
			AssertSetTextBoxValue("confirmpassword_field", user.Password2);
			AssertClick("create_account_button");

		}

		public void RepeatMailRegistration(UserRegistration user) {
			AssertSetTextBoxValue("firstname_field", user.FirstName);
			AssertSetTextBoxValue("lastname_field", user.LastName);
			AssertSetTextBoxValue("month_field", user.Dob_Month);
			AssertSetTextBoxValue("day_field", user.Dob_Day);
			AssertSetTextBoxValue("year_field", user.Dob_Year);
			AssertSetTextBoxValue("address_field", user.Address);
			AssertSetTextBoxValue("city_field", user.City);
			AssertSetTextBoxValue("state_field", user.State);
			AssertSetTextBoxValue("zip_field", user.Zip);
			AssertSetTextBoxValue("phone_field", user.Phone);
			AssertSetTextBoxValue("email_field", user.Email);
			AssertSetTextBoxValue("password_field", user.Password);
			AssertSetTextBoxValue("confirmpassword_field", user.Password);
			AssertClick("create_account_button");
			AssertElementExists("repeat_email");
		}
		public void RegistrationProductPage(UserRegistration2 user) {
			Driver.Navigate().GoToUrl(ProductUrl);
			AssertClick("Global.create_sign_in_option");
			RandomUserRequestsRegistration2(user);
			AssertClick("skip_button");
			var URL = Driver.Url;
			Assert.AreEqual(URL, ProductUrl);
			
		}
		#endregion
	}
}
