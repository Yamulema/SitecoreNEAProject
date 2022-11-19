using System.Collections.Generic;
using System.Data.Common;

namespace Neambc.Neamb.Foundation.MBCData.Db {
	public static class DbDataReaderExtensions {
		public static List<object> FieldValues(this DbDataReader reader) {
			var ret = (reader.FieldCount > 0) ? new List<object>(reader.FieldCount) : null;
			for (var i = 0; i < reader.FieldCount; i++) {
				// ReSharper disable once PossibleNullReferenceException
				ret.Add(reader.GetFieldValue<object>(i));
			}
			return ret;
		}
		public static string[] ColumnNames(this DbDataReader reader) {
			string[] ret = null;
			if (reader.HasRows) {
				ret = new string[reader.FieldCount];
				for (var i = 0; i < reader.FieldCount; i++) {
					ret[i] = reader.GetName(i);
				}
			}
			return ret;
		}
	}
}