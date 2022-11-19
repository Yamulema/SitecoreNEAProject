using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class ViewOmni
    {
        #region Properties
        public const string WEB_APP_URL = "WEB_APP_URL";
        public const string WEB_SOCT_URL = "WEB_SOCT_URL";
        public const string WEB_END_DT = "WEB_END_DT";

        public string WebAppUrl {
			get; set;
		}
        public string WebSoctUrl
        {
            get; set;
        }
        public string WebEndDt
        {
            get; set;
        }
        #endregion

        #region Constructor
        public ViewOmni() {

		}
		public ViewOmni(IReadOnlyList<object> record, IReadOnlyList<string> columnNames) {
			if (record.Count == columnNames.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = columnNames[i];
					switch (parameter.ToUpper()) {
						case WEB_APP_URL:
                            WebAppUrl = field.ToString().Trim();
                            break;
                        case WEB_SOCT_URL:
                            WebSoctUrl = field.ToString().Trim();
                            break;
                        case WEB_END_DT:
                            DateTime fieldDate = (DateTime) field;
                            var day = fieldDate.Day;
                            var month = fieldDate.Month;
                            WebEndDt = $"{month}/{day}/{fieldDate.Year}";
                            break;
                    }
                }
			}
		}
		#endregion

		#region Public Methods
		public static IEnumerable<ViewOmni> DigestCommandResult(ICommandResult result) {
			IEnumerable<ViewOmni> ret = null;
			if ((result != null) && !result.HasError && result.Records != null && result.Records.Count>0) {
				ret = result.Records
					.Select(x => new ViewOmni(x, result.ColumnNames))
					.ToList()
					.AsReadOnly();
			}
			return ret;
		}
		#endregion
	}
}