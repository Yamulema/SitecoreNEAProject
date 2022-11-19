using System;

	namespace UI_NEAMB.PagesTypes.DTO {
	[Serializable]
	public class UserRegistration {
		public string FirstName;
		public string LastName;
		public string Dob_Month;
		public string Dob_Day;
		public string Dob_Year;
		public string Address;
		public string City;
		public string State;
		public string Zip;
		public string Phone;
		public string Mail;
		public string Email;
		public string Password;
		public string ConfirmPassword;
	}
}
