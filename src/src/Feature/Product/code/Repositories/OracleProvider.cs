using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Neambc.Seiumb.Feature.Product.Repositories {
	public static class OracleProvider {
		/// <summary>
		/// Execution in PRODUCT_MAPPING table in Oracle
		/// </summary>
		/// <param name="productCode">Product code data for filter in select statement</param>
		/// <returns>item code</returns>
		public static string ExecuteQueryMaterialOracle(string productCode) {
			OracleConnection con = null;
			OracleCommand cmd = null;
			OracleDataReader reader = null;
			string materialId = null;
			try {
				var conection = ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;

				con = new OracleConnection(conection);

				con.Open();

				cmd = new OracleCommand(string.Format("select * from PRODUCT_MAPPING where PRODUCT_CODE='{0}'", productCode), con) {
					CommandType = System.Data.CommandType.Text
				};

				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					materialId = reader.GetString(0);
				}

				con.Close();
			} catch (Exception ex) {
				Sitecore.Diagnostics.Log.Error("Error in ExecuteQueryMaterialOracle", ex, "Oracle Provider");
			} finally {
				if (reader != null && !reader.IsClosed) {
					reader.Close();
				}

				if (con != null && con.State == System.Data.ConnectionState.Open) {
					con.Close();
				}

				con.Dispose();
			}
			return materialId;
		}

		/// <summary>
		/// Execution of the stored procedure  MDS_UTIL.ORDER_FULFILL@MBCDB 
		/// </summary>
		/// <param name="msdid">User id</param>
		/// <param name="itemcode"></param>
		/// <param name="cellcode"></param>
		/// <param name="value4"></param>
		/// <returns></returns>
		public static int SelectOrderFulfill(int msdid, string itemcode, string cellcode, string value4) {
			OracleConnection con = null;
			OracleCommand cmd = null;
			OracleDataReader reader = null;
			var response = -1;
			try {
				var conection = ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;
				con = new OracleConnection(conection);
				con.Open();
				cmd = new OracleCommand(string.Format("select MDS_UTIL.ORDER_FULFILL@MBCDB({0},'{1}','{2}','{3}') from dual", msdid, itemcode, cellcode, value4), con) {
					CommandType = System.Data.CommandType.Text
				};

				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					response = reader.GetInt16(0);
				}

				con.Close();
			} catch (Exception ex) {
				Sitecore.Diagnostics.Log.Error("Error in ExecuteSelectOrderFulfillOracle", ex, "OracleProvider");
			} finally {
				if (reader != null && !reader.IsClosed) {
					reader.Close();
				}

				if (con != null && con.State == System.Data.ConnectionState.Open) {
					con.Close();
				}

				if (con != null) {
					con.Dispose();
				}
			}
			return response;
		}
	}
}