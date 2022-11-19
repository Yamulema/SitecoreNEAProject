using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Neambc.Seiumb.Feature.Reminder.Repositories {
	public class ReminderRepository {

		private static ReminderRepository instance;

		private ReminderRepository() { }

		public static ReminderRepository Instance {
			get {
				if (instance == null) {
					instance = new ReminderRepository();
				}
				return instance;
			}
		}
		public string GetAllReminderlogById(string reminderId, string mdsIndvId) {
			OracleConnection con = null;
			OracleCommand cmd = null;
			OracleDataReader reader = null;
			string reminder = null;
			try {
				var conection = ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;

				con = new OracleConnection(conection);

				con.Open();

				cmd = new OracleCommand(string.Format("select count (*) as TOTAL from reminderlog where reminderid='{0}' and mds_indv_id='{1}'", reminderId, mdsIndvId), con) {
					CommandType = System.Data.CommandType.Text
				};


				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					reminder = reader.GetDecimal(0).ToString();
				}

				con.Close();
			} catch (Exception ex) {
				Sitecore.Diagnostics.Log.Error("Error in GetAllReminderlogById", ex, "Reminder DatabaseProvider");
			} finally {
				if (reader != null && !reader.IsClosed) {
					reader.Close();
				}

				if (con != null && con.State == System.Data.ConnectionState.Open) {
					con.Close();
				}

				con.Dispose();
			}
			return reminder;
		}

		public string InsertReminder(string reminderId, string mdsIndvId) {
			OracleConnection con = null;
			OracleCommand cmd = null;
			OracleDataReader reader = null;
			string reminder = null;
			try {
				var conection = ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;

				con = new OracleConnection(conection);

				con.Open();

				cmd = new OracleCommand(string.Format("insert into reminderlog (reminderid, mds_indv_id, datetime)values('{0}', '{1}', sysdate)", reminderId, mdsIndvId), con) {
					CommandType = System.Data.CommandType.Text
				};


				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					reminder = reader.GetDecimal(0).ToString();
				}

				con.Close();
			} catch (Exception ex) {
				Sitecore.Diagnostics.Log.Error("Error in InsertReminder", ex, "Reminder DatabaseProvider");
			} finally {
				if (reader != null && !reader.IsClosed) {
					reader.Close();
				}

				if (con != null && con.State == System.Data.ConnectionState.Open) {
					con.Close();
				}

				con.Dispose();
			}
			return reminder;
		}
	}
}