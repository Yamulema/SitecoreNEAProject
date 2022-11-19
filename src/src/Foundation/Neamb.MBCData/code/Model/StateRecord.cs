using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class StateRecord {

		#region Fields
		public string StateCode;
		public string StateName;
		public const string STATE_CODE = "statecode";
		public const string STATE_NAME = "statename";
		private static readonly string[] _defaultColumns = {STATE_CODE, STATE_NAME};
		#endregion

		#region Constructors
		public StateRecord(IReadOnlyList<object> record, IReadOnlyList<string> parameters = null) {
			parameters = parameters ?? _defaultColumns;
			if (record.Count == parameters.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = parameters[i];
					switch (parameter.ToLower()) {
						case STATE_CODE:
							StateCode = field as string;
							break;
						case STATE_NAME:
							StateName = field as string;
							break;
					}
				}
			}
		}
        public StateRecord() {

        }
        #endregion

        #region Public Methods
        public static IEnumerable<StateRecord> DigestCommandResult(ICommandResult result) {
			IEnumerable<StateRecord> ret = null;
			if ((null != result) && !result.HasError && result.Records != null) {
				ret = result.Records
					.Select(x => new StateRecord(x, result.ColumnNames))
					.ToList()
					.AsReadOnly();
			}
			return ret;
		}
		#endregion
	}
}