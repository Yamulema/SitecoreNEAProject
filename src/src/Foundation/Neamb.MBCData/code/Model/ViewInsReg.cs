using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class ViewInsReg {

		#region Properties
		public const string INDIVIDUAL_ID = "indv_id";
		public const string WAGE_EARNER_CODE = "wage_earn_cd";
		public const string DTAB_MARITAL_STATUS_CODE = "dtab_mrtl_stat_cd";
		public const string FAMILY_INCOME_RANGE_CODE = "fmly_incm_rnge_cd";
		public const string HOUSING_STATUS_CODE = "hous_stat_cd";
		public const string SPOUSE_EMPTY_CODE = "spse_empt_cd";
		public const string DEPENDENCY_CHILD_COUNT = "depn_chld_cnt";
		public const string DTAB_VOCL_LEVEL_CODE = "dtab_vocl_lvl_cd";

		public int Indv_id {
			get; set;
		}
		public string Wage_earn_cd {
			get; set;
		}
		public string Dtab_mrtl_stat_cd {
			get; set;
		}
		public string Fmly_incm_rnge_cd {
			get; set;
		}
		public string Hous_stat_cd {
			get; set;
		}
		public string Spse_empt_cd {
			get; set;
		}
		public int Depn_chld_cnt {
			get; set;
		}
		public string Dtab_vocl_lvl_cd {
			get; set;
		}
		#endregion

		#region Constructor
		public ViewInsReg() {

		}
		public ViewInsReg(IReadOnlyList<object> record, IReadOnlyList<string> columnNames) {
			if (record.Count == columnNames.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = columnNames[i];
					switch (parameter.ToLower()) {
						case INDIVIDUAL_ID:
							Indv_id = int.TryParse(field.ToString().Trim(), out var individualId) ? individualId : 0;
							break;
						case WAGE_EARNER_CODE:
							Wage_earn_cd = (field as string)?.Trim();
							break;
						case DTAB_MARITAL_STATUS_CODE:
							Dtab_mrtl_stat_cd = (field as string)?.Trim();
							break;
						case FAMILY_INCOME_RANGE_CODE:
							Fmly_incm_rnge_cd = (field as string)?.Trim();
							break;
						case HOUSING_STATUS_CODE:
							Hous_stat_cd = (field as string)?.Trim();
							break;
						case SPOUSE_EMPTY_CODE:
							Spse_empt_cd = (field as string)?.Trim();
							break;
						case DEPENDENCY_CHILD_COUNT:
							Depn_chld_cnt = int.TryParse(field.ToString().Trim(), out var dependentChildrenCount)
								? (dependentChildrenCount < 0 ? 0 : dependentChildrenCount)
								: 0;
							break;
						case DTAB_VOCL_LEVEL_CODE:
							Dtab_vocl_lvl_cd = (field as string)?.Trim();
							break;
					}
				}
			}
		}
		#endregion

		#region Public Methods
		public static IEnumerable<ViewInsReg> DigestCommandResult(ICommandResult result) {
			IEnumerable<ViewInsReg> ret = null;
			if ((result != null) && !result.HasError && result.Records != null) {
				ret = result.Records
					.Select(x => new ViewInsReg(x, result.ColumnNames))
					.ToList()
					.AsReadOnly();
			}
			return ret;
		}
		#endregion
	}
}