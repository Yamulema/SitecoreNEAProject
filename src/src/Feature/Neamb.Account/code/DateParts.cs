namespace Neambc.Neamb.Feature.Account {
	public class DateParts {
		public string Year;
		public string Month;
		public string Day;
		public DateParts(string year = null, string month = null, string day = null) {
			Year = year;
			Month = month;
			Day = day;
		}
		public string AsBirthDate() {
			return $"{Month}{Day}{Year}";
		}
	}
}