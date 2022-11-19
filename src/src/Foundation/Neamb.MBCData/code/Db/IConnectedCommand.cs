using System;
using System.Data.Common;

namespace Neambc.Neamb.Foundation.MBCData.Db {

	public delegate ICommandResult RowReader(DbDataReader reader);

	public interface IConnectedCommand : IDisposable {
		ICommandResult Execute(
			string messageError = null,
			RowReader readRow = null,
			bool readOneValueOnly = false
		);
	}
}