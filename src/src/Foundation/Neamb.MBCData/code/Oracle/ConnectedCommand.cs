using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Db;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Seiumb.Foundation.Sitecore;
using Oracle.ManagedDataAccess.Client;

namespace Neambc.Neamb.Foundation.MBCData.Oracle {
	/// <summary>
	/// IConnectedCommand specific to Oracle.  Must be used with a Dispose() pattern
	/// </summary>
	[Service(typeof(IConnectedCommand))]
	public class ConnectedCommand : IConnectedCommand {

		#region Fields
		private const string LOGGER_NAME = "OracleProvider";
		private const string DEFAULT_MESSAGE = "SQL failed";
		private readonly OracleConnection _connection;
		private readonly OracleCommand _command;
		private readonly ILog _log;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new connected command. Immediately opens a connection to the database.
		/// </summary>
		/// <param name="connString">Required.  A standard ADO-style connection string</param>
		/// <param name="query">Optional.  The SQL intended to execute</param>
		/// <param name="parameters">Optional.  Any parameters for the SQL</param>
		/// <param name="log">Required.  An implementation of <see cref="ILog"/> to record errors.</param>
		public ConnectedCommand(string connString, string query, OracleParameter[] parameters, ILog log) {
			_log = log ?? throw new ArgumentNullException(nameof(log));
			var cs = connString ?? throw new ArgumentNullException(nameof(connString));
			_connection = new OracleConnection(cs);
			_connection.Open();
			_command = CreateCommand(_connection, query, parameters);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Creates a general SQL-text command object
		/// </summary>
		/// <param name="con"></param>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		private static OracleCommand CreateCommand(OracleConnection con, string query, OracleParameter[] parameters) {
			var ret = con.CreateCommand();
			ret.CommandType = CommandType.Text;
			ret.CommandText = query;
			ret.Parameters.AddRange(parameters);
			ret.Prepare();
			return ret;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Disposes the command and connection resources associated with this object
		/// </summary>
		public void Dispose() {
			_command?.Dispose();
			_connection?.Dispose();
		}
		/// <summary>
		/// Execute this command and return a result.
		/// </summary>
		/// <param name="message">Optional. If not give, <see cref="DEFAULT_MESSAGE"/> will be used</param>
		/// <param name="rowReader">Optional <see cref="RowReader"/> delegate function.  If not given, the default <see cref="ReadRows"/> will be used</param>
		/// <param name="readOneValueOnly">Optional. Defaults to false.  If true, reads a set of records, otherwise read a single value at row 0, column 0</param>
		/// <returns>Always non-null. See <see cref="CommandResult"/> for determining execution success or failure</returns>
		public virtual ICommandResult Execute(string message = DEFAULT_MESSAGE,RowReader rowReader = null,bool readOneValueOnly = false) {
			if (rowReader == null) {
				if (readOneValueOnly) {
					rowReader = ReadValue;
				} else {
					rowReader = ReadRows;
				}
			}
			ICommandResult ret = null;
			_log.Debug("Pre-call: " + message, this);
			var start = System.Diagnostics.Stopwatch.GetTimestamp();
			var result = "Success";
			try {
				using (var reader = _command.ExecuteReader()) {
					ret = rowReader(reader);
				}
			} catch (Exception ex) {
				result = "Failure";
				ret = ret ?? new CommandResult();
				ret.ErrorMessage = ex.Message;
				LogError(ex, message);
			} finally {
				var end = System.Diagnostics.Stopwatch.GetTimestamp();
				_log.Debug($"{result}: {message} completed in {TimeSpan.FromTicks(end-start).TotalMilliseconds}ms");
			}
			return ret;
		}
		#endregion

		#region Protected Methods
		/// <summary>
		/// Logs all details about the command to the log
		/// </summary>
		/// <param name="ex">Required</param>
		/// <param name="message">Optional. If null, then <see cref="DEFAULT_MESSAGE"/> is used.</param>
		protected virtual void LogError(Exception ex, string message) {
			var sb = new StringBuilder();
			foreach (var parameter in _command.Parameters) {
				if (parameter is DbParameter p) {
					sb.Append($"({p.ParameterName}={p.Value})");
				}
			}
			var msg = message ?? DEFAULT_MESSAGE;
			_log.Debug($"Failure: {msg}: Command [{_command.CommandText}] Parameters [{sb}]");
            var e = new NeambDatabaseException($"{msg}", ex);
            _log.Fatal(e.Message, e, LOGGER_NAME);
		}
		/// <summary>
		/// The default data reader
		/// </summary>
		/// <param name="reader"></param>
		protected virtual ICommandResult ReadRows(DbDataReader reader) {
			var rows = new List<List<object>>();
			string[] columnNames = null;
			List<object> row;
			var first = true;
			while (reader.Read() && null != (row = reader.FieldValues())) {
				if (first) {
					columnNames = reader.ColumnNames();
					first = false;
				}
				rows.Add(row);
			}
			return new CommandResult(rows, columnNames);
		}
		protected virtual ICommandResult ReadValue(DbDataReader reader) {
            var result = string.Empty;
            while (reader.Read())
            {
                var valueReturned = reader.GetValue(0);
                if (valueReturned != null)
                {
                    result = valueReturned.ToString();
                }
            }
            return new CommandResult(
                result,
				string.Empty
			);
		}
		#endregion
	}
}