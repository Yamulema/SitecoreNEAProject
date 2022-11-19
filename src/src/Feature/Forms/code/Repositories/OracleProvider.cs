using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Neambc.Seiumb.Feature.Forms.Repositories {
	public static class OracleProvider {
		/// <summary>
		/// Search in the database table SEIU_GTL_RATES
		/// </summary>
		/// <param name="smoker">true/false</param>
		/// <param name="age"></param>
		/// <param name="coverage"></param>
		/// <returns>smoker_rate/nonsmoker_rate</returns>
		public static string ExecuteQueryRatesOracle(bool smoker, string age, string coverage) {
			OracleConnection con = null;
			OracleCommand cmd = null;
			OracleDataReader reader = null;
			string smoker_rate = null;
			try {
				var conection = ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;

				con = new OracleConnection(conection);

				con.Open();
				if (smoker) {
					cmd = new OracleCommand(string.Format("select trim(to_char(smoker_rate,'999.99')) as smoker_rate from SEIU_GTL_RATES where age = {0} AND coverage ={1}", age, coverage), con);
				} else {
					cmd = new OracleCommand(string.Format("select trim(to_char(nonsmoker_rate,'999.99')) as nonsmoker_rate from SEIU_GTL_RATES where age = {0} AND coverage ={1}", age, coverage), con);
				}

				cmd.CommandType = System.Data.CommandType.Text;

				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					smoker_rate = reader.GetString(0);
				}

				con.Close();
			} catch (Exception ex) {
				Sitecore.Diagnostics.Log.Error("Error in QueryRatesOracle", ex, "OracleProvider Forms");
			} finally {
				if (reader != null && !reader.IsClosed) {
					reader.Close();
				}

				if (con != null && con.State == System.Data.ConnectionState.Open) {
					con.Close();
				}

				con.Dispose();
			}
			return smoker_rate;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="locNum"></param>
		/// <returns></returns>
		public static string ExecuteQueryRestrictedLocals(string locNum) {
			OracleConnection con = null;
			OracleCommand cmd = null;
			OracleDataReader reader = null;
			string result = null;
			try {
				var conection = ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;

				con = new OracleConnection(conection);

				con.Open();
				cmd = new OracleCommand(string.Format("select trim(gnrl_cd_val_id) as gnrl_cd_val_id from view_seiu_loc_rstr@mbcdb where trim(gnrl_cd_val_id)='{0}'", locNum), con) {
					CommandType = System.Data.CommandType.Text
				};

				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					result = reader.GetString(0);
				}

				con.Close();
			} catch (Exception ex) {
				Sitecore.Diagnostics.Log.Error("Error in ExecuteQueryRestrictedLocals", ex, "OracleProvider Forms");
			} finally {
				if (reader != null && !reader.IsClosed) {
					reader.Close();
				}

				if (con != null && con.State == System.Data.ConnectionState.Open) {
					con.Close();
				}

				con.Dispose();
			}
			return result;
		}
	}
}