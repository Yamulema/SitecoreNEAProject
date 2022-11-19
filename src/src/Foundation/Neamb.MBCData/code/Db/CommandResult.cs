using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Db {

	public class CommandResult : ICommandResult {

		#region Fields
		public string Value { get; }
		public bool HasError => ErrorMessage != null;
		public string ErrorMessage { get; set; }
		public string[] ColumnNames { get; }
		public IReadOnlyList<IReadOnlyList<object>> Records { get; }
		#endregion

		#region Constructors
		public CommandResult(List<List<object>> records, string[] columnNames = null) {
			Records = records ?? throw new ArgumentNullException(nameof(records));
			ColumnNames = columnNames;
		}
		public CommandResult(string value, string columnName =null) {
			Value = value;
			if (columnName != null) {
				ColumnNames = new[] { columnName };
			}
		}
		public CommandResult() {
		}
		#endregion
	}
}