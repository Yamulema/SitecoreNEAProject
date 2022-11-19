using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class ViewBeneficiary {

		#region Properties
		public const string BENEFICIARY_ENTITY_NAME = "bnry_nm";
		public const string BENEFICIARY_FIRST_NAME = "bnry_frst_nm";
		public const string BENEFICIARY_LAST_NAME = "bnry_last_nm";
		public const string BENEFICIARY_MIDDLE_NAME = "bnry_mid_nm";
		public const string BENEFICIARY_TYPE = "bnry_typ";
		public const string BENEFICIARY_EMAIL = "bnry_email_addr_txt";
		public const string BENEFICIARY_DESIGNATED_PTS = "bnry_desg_pts";
		public const string BENEFICIARY_DESIGNATED_CODE = "bnry_desg_cd";

		public string EntityName {
			get; set;
		}
		public string FirstName {
			get; set;
		}
		public string LastName {
			get; set;
		}
		public string MiddleName {
			get; set;
		}
		public string BeneficiaryType {
			get; set;
		}
		public string EmailAddress {
			get; set;
		}
		public int DesignatedPts {
			get; set;
		}
		public string DesignatedCd {
			get; set;
		}
		#endregion

		#region Constructors
		public ViewBeneficiary() {

		}
		public ViewBeneficiary(IReadOnlyList<object> record, IReadOnlyList<string> parameters) {
			if (record.Count == parameters.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = parameters[i];
					switch (parameter.ToLower()) {
						case BENEFICIARY_ENTITY_NAME:
							EntityName = field as string;
							break;
						case BENEFICIARY_FIRST_NAME:
							FirstName = field as string;
							break;
						case BENEFICIARY_LAST_NAME:
							LastName = field as string;
							break;
						case BENEFICIARY_MIDDLE_NAME:
							MiddleName = field as string;
							break;
						case BENEFICIARY_TYPE:
							BeneficiaryType = field as string;
							break;
						case BENEFICIARY_EMAIL:
							EmailAddress = field as string;
							break;
						case BENEFICIARY_DESIGNATED_PTS:
							DesignatedPts = int.TryParse(field.ToString(), out var pts) ? pts : 0;
							break;
						case BENEFICIARY_DESIGNATED_CODE:
							DesignatedCd = field as string;
							break;
					}
				}
			}
		}
		#endregion

		#region Public Methods
		public static IEnumerable<ViewBeneficiary> DigestCommandResult(ICommandResult result) {
			IEnumerable<ViewBeneficiary> ret = null;
			if ((result != null) && !result.HasError && result.Records != null) {
				ret = result.Records
					.Select(x => new ViewBeneficiary(x, result.ColumnNames))
					.ToList()
					.AsReadOnly();
			}
			return ret;
		}
		#endregion

	}
}