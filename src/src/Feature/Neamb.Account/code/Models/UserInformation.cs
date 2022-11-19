namespace Neambc.Neamb.Feature.Account.Models {
	public class UserInformation {
		public string Name {
			get; set;
		}
		public string BirthDate {
			get; set;
		}
		public string Address {
			get; set;
		}
		public string Phone {
			get; set;
		}
		public static UserInformation CreateSample() {
			return new UserInformation() {
				Name = "Jessica",
				BirthDate = "July 6, 1980",
				Phone = "5635559540",
				Address = "207 Orchid Maquoketa, CA 52060"
			};
		}
	}
}