using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Db {
	public interface ICommandResult {
		string Value {
			get;
		}
		bool HasError {
			get;
		}
		string ErrorMessage {
			get; set;
		}
		IReadOnlyList<IReadOnlyList<object>> Records {
			get;
		}
		string[] ColumnNames {
			get; 
		}
	}
}