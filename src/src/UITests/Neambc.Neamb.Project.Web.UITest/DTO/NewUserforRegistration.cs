using System;
using System.IO;
using System.Xml.Serialization;

namespace Neambc.Neamb.Project.Web.UITest.DTO {
	[Serializable]
	public class NewUserforRegistration {

		#region Fields
		private static string AssemblyPath {
			get {
				var assemblyPath = typeof(NewUserforRegistration).Assembly.Location;
				return Path.GetFullPath(Path.GetDirectoryName(assemblyPath) ?? string.Empty);
			}
		}
		#endregion

		#region Properties
		public string FirstName;
		public string LastName;
		public string DateMonth;
		public string DateDay;
		public string DateYear;
		public string Address;
		public string City;
		public string State;
		public string ZipCode;
		public string Phone;
		public string Email;
		public string Password;
		public string ConfirmPassword;
		#endregion

		#region Public Methods
		public static NewUserforRegistration LoadFromFile(string filename) {
			NewUserforRegistration ret = null;
			using (var reader = new FileStream(Path.Combine(AssemblyPath, filename), FileMode.Open)) {
				ret = new XmlSerializer(typeof(NewUserforRegistration)).Deserialize(reader) as NewUserforRegistration;
			}
			return ret;
		}
		public void SaveToFile(string filename) {
			using (var writer = new StreamWriter(Path.Combine(AssemblyPath, filename))) {
				new XmlSerializer(GetType()).Serialize(writer, this);
				writer.Close();
			}
		}
		#endregion
	}
}


