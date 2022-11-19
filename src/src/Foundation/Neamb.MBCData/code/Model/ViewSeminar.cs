using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class ViewSeminar
    {
        #region Properties
        public const string SEMINAR_ID = "SEMINAR_ID";
		public const string LEA_CODE = "LEA_CODE";
        public const string LEA_NAME = "LEA_NAME";
		public const string SEMINAR_NAME = "SEMINAR_NAME";
		public const string LOCATION1 = "LOCATION1";
		public const string LOCATION2 = "LOCATION2";
		public const string ADDRESS = "ADDRESS";
		public const string CITY = "CITY";
		public const string STATE = "STATE";
        public const string ZIP = "ZIP";
        public const string SEMINAR_DATE = "SEMINAR_DATE";
        public const string SEMINAR_RVP_NAME = "SEMINAR_RVP_NAME";
        public const string LAST_UPDT_DT = "LAST_UPDT_DT";
        public const string LAST_UPDT_USER_ID = "LAST_UPDT_USER_ID";
        public const string SEMINAR_TIME = "SEMINAR_TIME";
        public const string SEA_NAME = "SEA_NAME";
        public const string PRESENTER_NAME = "PRESENTER_NAME";

        public string SeminarId {
			get; set;
		}
		public string LeaCode {
			get; set;
		}
        public string LeaName
        {
            get; set;
        }
        public string SeminarName {
			get; set;
		}
		public string Location1 {
			get; set;
		}
		public string Location2 {
			get; set;
		}
		public string Address {
			get; set;
		}
		public string City {
			get; set;
		}
		public string State {
			get; set;
		}
        public string Zip
        {
            get; set;
        }
        public string SeminarDate
        {
            get; set;
        }
        public string SeminarRvpName
        {
            get; set;
        }
        public string LastUpdtDt
        {
            get; set;
        }
        public string LastUpdtUserId
        {
            get; set;
        }
        public string SeminarTime
        {
            get; set;
        }
        public string SeaName
        {
            get; set;
        }
        public string PresenterName
        {
            get; set;
        }
        #endregion

        #region Constructor
        public ViewSeminar() {

		}
		public ViewSeminar(IReadOnlyList<object> record, IReadOnlyList<string> columnNames) {
			if (record.Count == columnNames.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = columnNames[i];
					switch (parameter.ToUpper()) {
						case SEMINAR_ID:
                            SeminarId = field.ToString().Trim();
                            break;
						case LEA_CODE:
							LeaCode = field.ToString().Trim();
                            break;
                        case LEA_NAME:
                            LeaName = field.ToString().Trim();
                            break;
                        case SEMINAR_NAME:
							SeminarName= field.ToString().Trim();
                            break;
						case LOCATION1:
							Location1 = field.ToString().Trim();
							break;
						case LOCATION2:
							Location2 = field.ToString().Trim();
							break;
						case ADDRESS:
							Address = field.ToString().Trim();
							break;
						case CITY:
							City = field.ToString().Trim();
							break;
                        case STATE:
                            State = field.ToString().Trim();
                            break;
                        case ZIP:
                            Zip = field.ToString().Trim();
                            break;
                        case SEMINAR_DATE:
                            DateTime myDt = DateTime.Parse(field.ToString().Trim());
                            SeminarDate = myDt.ToString("D");
                            break;
                        case SEMINAR_RVP_NAME:
                            SeminarRvpName = field.ToString().Trim();
                            break;
                        case LAST_UPDT_DT:
                            LastUpdtDt = field.ToString().Trim();
                            break;
                        case LAST_UPDT_USER_ID:
                            LastUpdtUserId = field.ToString().Trim();
                            break;
                        case SEMINAR_TIME:
                            SeminarTime = field.ToString().Trim();
                            break;
                        case SEA_NAME:
                            SeaName = field.ToString().Trim();
                            break;
                        case PRESENTER_NAME:
                            PresenterName = field.ToString().Trim();
                            break;

                    }
                }
			}
		}
		#endregion

		#region Public Methods
		public static IEnumerable<ViewSeminar> DigestCommandResult(ICommandResult result) {
			IEnumerable<ViewSeminar> ret = null;
			if ((result != null) && !result.HasError && result.Records != null) {
				ret = result.Records
					.Select(x => new ViewSeminar(x, result.ColumnNames))
					.ToList()
					.AsReadOnly();
			}
			return ret;
		}
		#endregion
	}
}