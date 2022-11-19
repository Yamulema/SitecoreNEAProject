using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class ViewSeminarReg
    {
        #region Properties
        public const string SEMINAR_ID = "SEMINAR_ID";
		public const string INDV_ID = "INDV_ID";

        public string SeminarId {
			get; set;
		}
		public string IndvId {
			get; set;
		}
		
		#endregion

        #region Constructor
        public ViewSeminarReg() {

		}
		public ViewSeminarReg(IReadOnlyList<object> record, IReadOnlyList<string> columnNames) {
			if (record.Count == columnNames.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = columnNames[i];
					switch (parameter.ToUpper()) {
						case SEMINAR_ID:
                            SeminarId = field.ToString().Trim();
                            break;
						case INDV_ID:
							IndvId = field.ToString().Trim();
                            break;
					}
                }
			}
		}
		#endregion

		#region Public Methods
		public static IEnumerable<ViewSeminarReg> DigestCommandResult(ICommandResult result) {
			IEnumerable<ViewSeminarReg> ret = null;
			if ((result != null) && !result.HasError && result.Records != null) {
				ret = result.Records
					.Select(x => new ViewSeminarReg(x, result.ColumnNames))
					.ToList()
					.AsReadOnly();
			}
			return ret;
		}
		#endregion
	}
}