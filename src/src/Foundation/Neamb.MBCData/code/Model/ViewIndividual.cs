using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class ViewIndividual {

		#region Properties
		public const string INDIVIDUAL_ID = "indv_id";
		public const string INDIVIDUAL_FIRST_NAME = "indv_frst_nm";
		public const string INDIVIDUAL_LAST_NAME = "indv_last_nm";
		public const string INDIVIDUAL_EMAIL_ADDRESS = "email_addr_txt";

		public string IndividualId {
			get;
		}
		public string FirstName {
			get;
		}
		public string LastName {
			get;
		}
		public string Email {
			get;
		}
		public ViewIndividual() {

		}
		#endregion

		#region Constructors
		public ViewIndividual(IReadOnlyList<object> record, IReadOnlyList<string> columnNames) {
			if ((null != record) && (null != columnNames) && record.Count == columnNames.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = columnNames[i];
					switch (parameter.ToLower()) {
						case INDIVIDUAL_ID:
							IndividualId = field as string;
							break;
						case INDIVIDUAL_FIRST_NAME:
							FirstName = field as string;
							break;
						case INDIVIDUAL_LAST_NAME:
							LastName = field as string;
							break;
						case INDIVIDUAL_EMAIL_ADDRESS:
							Email = field as string;
							break;
					}
				}
			}
		}
		#endregion
	}
}