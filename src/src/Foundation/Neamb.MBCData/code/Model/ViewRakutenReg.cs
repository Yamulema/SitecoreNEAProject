using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class ViewRakutenReg
    {
        #region Properties
        public const string MDS_ID = "MDS_ID";
		public const string EMAIL_ADDRESS = "EMAIL_ADDRESS";
        public const string UNION_ID = "UNION_ID";
        public const string STORE_ID = "STORE_ID";
        public const string EB_TOKEN = "EB_TOKEN";
        public const string CELL_CODE = "CELL_CODE";
        public const string FAVORITE_STORE = "FAVORITE_STORE";
        public const string CREATE_DATE = "CREATE_DATE";

        public int MdsId {
			get; set;
		}
		public string EmailAddress {
			get; set;
		}
        public string UnionId
        {
            get; set;
        }
        public string StoreId
        {
            get; set;
        }
        public string EBToken
        {
            get; set;
        }
        public string CellCode
        {
            get; set;
        }
        public string FavoriteStore
        {
            get; set;
        }
        public long CreateDate
        {
            get; set;
        }
        #endregion

        #region Constructor
        public ViewRakutenReg() {

		}
		public ViewRakutenReg(IReadOnlyList<object> record, IReadOnlyList<string> columnNames) {
			if (record.Count == columnNames.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = columnNames[i];
					switch (parameter.ToUpper())  {
						case MDS_ID:
							MdsId = int.TryParse(field.ToString().Trim(), out var indvid) ? indvid : 0;
                            break;
                        case EMAIL_ADDRESS:
                            EmailAddress = field.ToString().Trim();
                            break;
                        case FAVORITE_STORE:
                            FavoriteStore = field.ToString().Trim();
                            break;
                        case EB_TOKEN:
                            EBToken = field.ToString().Trim();
                            break;
                        case STORE_ID:
                            StoreId = field.ToString().Trim(); 
                            break;
                        case UNION_ID:
                            UnionId = field.ToString().Trim();
                            break;
                        case CREATE_DATE:
                            var t = Convert.ToDateTime( field.ToString().Trim() );
                            CreateDate = (Int32)(t.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            break;
                    }
                }
			}
		}
		#endregion

		#region Public Methods
		public static IEnumerable<ViewRakutenReg> DigestCommandResult(ICommandResult result) {
			IEnumerable<ViewRakutenReg> ret = null;
			if ((result != null) && !result.HasError && result.Records != null) {
				ret = result.Records
					.Select(x => new ViewRakutenReg(x, result.ColumnNames))
					.ToList()
					.AsReadOnly();
			}
			return ret;
		}
		#endregion
	}
}