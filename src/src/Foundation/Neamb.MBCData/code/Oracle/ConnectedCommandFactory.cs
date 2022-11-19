using System;
using System.Configuration;
using System.Data.Common;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Db;
using Neambc.Seiumb.Foundation.Sitecore;
using Oracle.ManagedDataAccess.Client;

namespace Neambc.Neamb.Foundation.MBCData.Oracle {
	/// <summary>
	/// IConnectedCommandFactory specific to Oracle
	/// </summary>
	[Service(typeof(IConnectedCommandFactory))]
	public class ConnectedCommandFactory : IConnectedCommandFactory {

		#region Fields
		protected readonly string _connectionString;
		protected readonly ILog _log;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a factory with the connection string and log that all commadn will use
        /// </summary>
        /// <param name="connString">Required</param>
        /// <param name="log">Required</param>
        public ConnectedCommandFactory(ILog log, string connString = null)
        {
            _connectionString = connString ?? ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <summary>
        /// Creates a new <see cref="ConnectedCommand"/> for executing against an Oracle database.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IConnectedCommand Create(string query, OracleParameter[] parameters) {
			return new ConnectedCommand(_connectionString, query, parameters as OracleParameter[], _log);
		}
		#endregion
	}
}