using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace Neambc.Neamb.Foundation.MBCData.Db {
	public interface IConnectedCommandFactory {
		IConnectedCommand Create(string query, OracleParameter[] parameters);
	}

}