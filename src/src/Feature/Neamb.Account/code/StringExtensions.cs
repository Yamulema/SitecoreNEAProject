namespace Neambc.Neamb.Feature.Account {
	public static class StringExtensions {
		public static string AsFormattedPhoneNumber(this string str) {
			return string.IsNullOrEmpty(str) 
                ? string.Empty 
                : str.Replace(" ", string.Empty);
		}
	}
}