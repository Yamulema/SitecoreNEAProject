using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Db;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Seiumb.Foundation.Sitecore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	[Service(typeof(IOracleDatabase))]
	public class OracleDatabase : IOracleDatabase {

		#region Fields
		public const int MAX_TEXT_LEN = 1000;
		protected readonly string _connString;
		protected readonly ILog _log;
		protected readonly IConnectedCommandFactory _cmdFactory;

		// compute-once values
		private readonly string[] _specialRateStates = { "FL", "MO" };
		private readonly Regex _regexOptionCheck = new Regex("^[A-Z]$", RegexOptions.Compiled);
		
		#endregion

		#region Constructor
		public OracleDatabase(IConnectedCommandFactory cmdFactory, ILog log, string connectionString = null) {
			_cmdFactory = cmdFactory ?? throw new ArgumentNullException(nameof(cmdFactory));
			_log = log ?? throw new ArgumentNullException(nameof(log));
			_connString = connectionString ?? ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;
		}
		#endregion

		#region Protected Methods

        protected virtual ICommandResult ExecuteCommand(string query, OracleParameter[] parameters, string message = null) {
            try {
                using (var cmd = _cmdFactory.Create(query, parameters)) {
                    var ret = cmd.Execute(message, null, true);
                    if (ret != null && ret.HasError) {
                        _log.Debug($"Execution query {query} in Oracle has error");
                    }
                    return ret;
                }
            } catch (Exception e) {
                var ex = new NeambDatabaseException($"Error in ExecuteCommand for query :{query}", e);
                _log.Fatal(ex.Message, ex, this);
                throw ex;
            }
        }
        protected virtual ICommandResult ExecuteQuery(string query, OracleParameter[] parameters, string message = null) {
			try {
                using (var cmd = _cmdFactory.Create(query, parameters))
                {
                    var ret = cmd.Execute(message);
                    return ret;
                }
            } catch (Exception e) {
                var ex = new NeambDatabaseException($"Error in ExecuteQuery for query :{query}", e);
                _log.Fatal(ex.Message, ex, this);
                throw ex;
            }
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Execution of the stored procedure  MDS_UTIL.ORDER_FULFILL@MBCDB 
		/// </summary>
		/// <param name="msdId">User id</param>
		/// <param name="itemCode"></param>
		/// <param name="cellCode"></param>
		/// <param name="value4"></param>
		/// <returns></returns>
		public bool SelectOrderFulfill(int msdId, string itemCode, string cellCode, string value4) {
			const string query =
				"select MDS_UTIL.ORDER_FULFILL@MBCDB(:msdid,:itemcode,:cellcode,:value4) from dual";
			var parameters = new[] {
				new OracleParameter(":msdid", OracleDbType.Int32) {Value = msdId},
				new OracleParameter(":itemcode", OracleDbType.Varchar2) {Value = itemCode},
				new OracleParameter(":cellcode", OracleDbType.Varchar2) {Value = cellCode},
				new OracleParameter(":value4", OracleDbType.Varchar2) {Value = value4}
			};
			var messageError = $"order_fulfill stored procedure {msdId} {itemCode} {cellCode} ";
			return !ExecuteCommand(query, parameters, messageError).HasError;
		}

		public string SelectItemCodeForProductCode(string productCode) {
			const string query = "select ITEM_CODE from PRODUCT_MAPPING where PRODUCT_CODE = :productCode";
			var parameters = new[] {
				new OracleParameter(":productCode", OracleDbType.Varchar2) {Value = productCode}
			};
			var messageError = $"PRODUCT_MAPPING query, productCode: {productCode}";
			var oracleResult = ExecuteCommand(query, parameters, messageError);
			return oracleResult.Value;
		}

		public bool InsertReminder(string reminderId, string mdsIndividualId) {
			const string query =
				"insert into reminder_log " +
				"(reminder_id, mds_indv_id, date_time) " +
				"values(:reminderId, :mdsIndividualId, sysdate)";
			var parameters = new[] {
				new OracleParameter(":reminderId", OracleDbType.Varchar2) {Value = reminderId},
				new OracleParameter(":mdsIndividualId", OracleDbType.Varchar2) {Value = mdsIndividualId}
			};
			var messageError = $"Error in InsertReminder {reminderId} {mdsIndividualId}";
			return !ExecuteCommand(query, parameters, messageError).HasError;
		}

		public int ReminderLogCount(string reminderId, string mdsIndividualId) {
			const string query =
				"select count (*) as TOTAL " +
				"from reminder_log " +
				"where reminder_id = :reminderId " +
				"and mds_indv_id = :mdsIndividualId";
			var parameters = new[] {
				new OracleParameter(":reminderId", OracleDbType.Varchar2) {Value = reminderId},
				new OracleParameter(":mdsIndividualId", OracleDbType.Varchar2) {Value = mdsIndividualId}
			};
			var messageError = $"ReminderLogCount {reminderId} {mdsIndividualId}";
			var oracleResult = ExecuteCommand(query, parameters, messageError);
			int.TryParse(oracleResult.Value ?? "0", out var ret);
			return ret;
		}

		public bool EnrollInSweepstakes(string msdId, string sweepstakesId) {
			var ret = false;
			if ((msdId != null) && (sweepstakesId != null)) {
				const string query = "CALL MBC.INSERT_INDV_SWEEPS@MBCDB(:msdid,:sweeptakesid)";
				var parameters = new[] {
					new OracleParameter(":msdid", OracleDbType.Varchar2) {Value = msdId},
					new OracleParameter(":sweeptakesid", OracleDbType.Varchar2) {Value = sweepstakesId}
				};
				var messageError = $"MBC.INSERT_INDV_SWEEPS failed {msdId} {sweepstakesId}";
				ret = !ExecuteCommand(query, parameters, messageError).HasError;
			}
			return ret;
		}

		public IReadOnlyList<string> SelectLifeInsuranceRates(
			string coverage, string term, string age, string memberSpouse, string smoker
		) {
			IReadOnlyList<string> ret = null;
			const string query =
				"select rate from LPGT_RATES " +
				"WHERE face = :coverage " +
				"AND term = :term " +
				"AND age = :age " +
				"AND memberspouse = :memberspouse " +
				"AND underwrite = :smoker " +
				"ORDER BY RATE";
			var parameters = new[] {
				new OracleParameter(":coverage", OracleDbType.Varchar2) { Value = coverage },
				new OracleParameter(":term", OracleDbType.Varchar2) { Value = term },
				new OracleParameter(":age", OracleDbType.Varchar2) { Value = age },
				new OracleParameter(":memberspouse", OracleDbType.Varchar2) { Value = memberSpouse },
				new OracleParameter(":smoker", OracleDbType.Varchar2) { Value = smoker }
			};
			var messageError = $"LPGT_RATES failed {coverage},{term},{age},{memberSpouse},{smoker}";
			var oracleResult = ExecuteQuery(query, parameters, messageError);
			if (!oracleResult.HasError && (oracleResult.Records != null)) {
				ret = new List<string>(oracleResult.Records
					.Select(record => record.First().ToString()))
					.AsReadOnly();
			}
			return ret;
		}

		public string SelectLpgtRates(string coverage, string age) {
			string ret = null;
			if ((null != coverage) && (null != age)) {
				var query = $"SELECT rate " +
				            "FROM LI_RATES " +
				            "WHERE coverage = :coverage " +
				            "AND age = :age " +
				            "AND ROWNUM = 1 " +
				            "ORDER BY rate";
				var parameters = new[] {
					new OracleParameter(":coverage", OracleDbType.Varchar2) {Value = coverage},
					new OracleParameter(":age", OracleDbType.Varchar2) {Value = age}
				};
				var messageError = $"SelectLpgtRates {coverage},{age}";
				ret = ExecuteCommand(query, parameters, messageError).Value;
			}
			return ret;
		}

		public string SelectMdsId(string individualId) {
			string ret = null;
			if (null != individualId) {
				var query = $"SELECT indv_id " +
							"FROM view_indv@mbcdb " +
							"WHERE user_indv_id = :imsid " +
							"AND ROWNUM = 1";
				var parameters = new[] {
					new OracleParameter(":imsid", OracleDbType.Varchar2) {Value = individualId}
				};
				var messageError = $"GetMdsId individualId:{individualId}";
				var oracleResult = ExecuteCommand(query, parameters, messageError);
				if (!oracleResult.HasError) {
					ret = oracleResult.Value;
				}
			}
			return ret;
		}

		public string SelectImsId(string msdId) {
			string ret = null;
			if (null != msdId) {
				var query = $"select user_indv_id " +
							"from view_indv@mbcdb " +
							"where indv_id = :mdsid";
				var parameters = new[] {
					new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = msdId}
				};
				var messageError = $"SelectImsId mdsid:{msdId}";
				var oracleResult = ExecuteCommand(query, parameters, messageError);
				if (!oracleResult.HasError) {
					ret = oracleResult.Value;
				}
			}
			return ret;
		}


		public IEnumerable<string> SelectRates(string state, string zip, int[] ages, string option) {
			if (!_regexOptionCheck.IsMatch(option)) {
				throw new ArgumentException(
					$"option parameter [{option}] does not match injection security check [A-Z]");
			}
			IEnumerable<string> ret = null;
			if (state != null) {
				const string formatString = "MIN(CASE WHEN min_age='{0}' THEN TO_CHAR(rate,'$9,999.99') END) as r{1}";
				string ageFilter;
				if (_specialRateStates.Contains(state)
					&& (ages.Length == 1)
					&& (ages[0] > 66) && (ages[0] < 70)) {

					ageFilter = string.Format(formatString, "66", ages[0]);
				} else {
					ageFilter = string.Join(",", ages.Select(x => string.Format(formatString, x, x)));
				}
				var zipFilter = string.Empty;
				var parameters = new List<OracleParameter> {
					new OracleParameter(":state1", OracleDbType.Varchar2) {Value = state}
				};
				if (!string.IsNullOrEmpty(zip)) {
					zipFilter = "AND area = (" +
								"SELECT area FROM mcs_zip " +
								"WHERE statecode = :state2 " +
								"AND zipcode = :zip)";
					parameters.Add(new OracleParameter(":state2", OracleDbType.Varchar2) { Value = state });
					parameters.Add(new OracleParameter(":zip", OracleDbType.Varchar2) { Value = zip });
				}
				var sqlQuery = $"SELECT {ageFilter} FROM (SELECT rate_{option}/100 as rate, " +
							   "min_age FROM mcs_rates WHERE min_age <= 80 " +
							   $"AND statecode = :state1 {zipFilter})";
				const string messageError = "GetRates";
				var oracleResult = ExecuteQuery(sqlQuery, parameters.ToArray(), messageError);

				if (!oracleResult.HasError && (oracleResult.Records != null)) {
					ret = oracleResult.Records.FirstOrDefault()
						?.Select(x => x as string);
				}
			}
			return ret;
		}

		public int SelectZipCodeCount(string state) {
			var ret = 0;
			var query = "SELECT COUNT(ZIPCODE) AS STATEOUT FROM mcs_zip WHERE statecode = :state";
			var parameters = new[] {
				new OracleParameter(":state", OracleDbType.Varchar2) {Value = state}
			};
			var messageError = $"Error in GetZipCodeCount for state:{state}";
			var oracleResult = ExecuteCommand(query, parameters, messageError);
			if (!oracleResult.HasError) {
				ret = int.TryParse(oracleResult.Value, out var zipCodeCount) ? zipCodeCount : 0;
			}
			return ret;
		}

		public IReadOnlyList<ViewBeneficiary> SelectBeneficiaries(string mdsIndividualId) {
			const string sql = "SELECT " +
					  "NVL(bnry_nm, ' ') AS " + ViewBeneficiary.BENEFICIARY_ENTITY_NAME + ", " +
					  "NVL(bnry_frst_nm, ' ') AS " + ViewBeneficiary.BENEFICIARY_FIRST_NAME + ", " +
					  "NVL(bnry_last_nm,' ') AS " + ViewBeneficiary.BENEFICIARY_LAST_NAME + ", " +
					  "NVL(bnry_mid_nm,' ') AS " + ViewBeneficiary.BENEFICIARY_MIDDLE_NAME + ", " +
					  "bnry_typ AS " + ViewBeneficiary.BENEFICIARY_TYPE + ", " +
					  "NVL(bnry_email_addr_txt, ' ') AS " + ViewBeneficiary.BENEFICIARY_EMAIL + ", " +
					  "bnry_desg_pct*100 AS " + ViewBeneficiary.BENEFICIARY_DESIGNATED_PTS + ", " +
					  "bnry_desg_cd AS " + ViewBeneficiary.BENEFICIARY_DESIGNATED_CODE + " " +
					  "FROM view_bene@mbcdb " +
					  "WHERE dtab_rgr_id=( " +
					  "select dtab_rgr_id from view_ins_reg@mbcdb where indv_id = :mdsIndividualId " +
					  ")";
			var parameters = new[] {
				new OracleParameter(":mdsIndividualId", OracleDbType.Varchar2) { Value = mdsIndividualId }
			};
			var message = $"GetViewBeneficiaries: {mdsIndividualId}";
			var result = ExecuteQuery(sql, parameters, message);
			return ViewBeneficiary.DigestCommandResult(result)
				?.ToList()
				.AsReadOnly();
		}

		public bool InsertComplimentaryLife(ComplimentaryLifeDb complimentaryLifeDb) {
			var cldb = complimentaryLifeDb ?? throw new ArgumentNullException(nameof(complimentaryLifeDb));
			var message = $"ExecuteInsertComplimentaryLife for mdsid:{cldb.IndvID}";
			var vString = JsonConvert.SerializeObject(cldb);
			_log.Info($"Attempting to fetch ExecuteInsertComplimentaryLife for IndvID:{cldb.IndvID}.", this);
			_log.Debug($"{message} vString:{vString}", this);
			const string query = "SELECT comp_life_utl.comp_life_beneficiary@mbcdb (:vString) from dual";
			var parameters = new[] {
				new OracleParameter(":vString", OracleDbType.Varchar2) { Value = vString }
			};
			var ret = false;
			var oracleResult = ExecuteCommand(query, parameters, message);
			if (int.TryParse(oracleResult.Value, out var resultCode) && resultCode == 0) {
				ret = true;
			}
			return ret;
		}

		public string SelectLastUpdateDate(string mdsId) {
			string ret = null;
			if (null != mdsId) {
				const string query =
					@"SELECT TO_CHAR(dtab_rgr_sign_dt,'MM/DD/YYYY') as DTAB_RGR_SIGN_DT " +
					"FROM view_indv@mbcdb " +
					"WHERE indv_id = :mdsid";
				var parameters = new[] {
					new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsId}
				};
				var message = $"GetLastUpdateDate for mdsid:{mdsId}";
				var oracleResult = ExecuteCommand(query, parameters, message);
				if (!oracleResult.HasError) {
					ret = oracleResult.Value;
				}
			}
			return ret;
		}

		/// <summary>
		/// Operation insert invite family member
		/// </summary>
		/// <param name="msdId">User id</param>
		/// <param name="firstName">First name</param>
		/// <param name="lastName">Last name</param>
		/// <param name="email">email</param>
		/// <param name="dateOfBirth">Date of birth</param>
		/// <param name="relationshipCode">Relationship</param>
		/// <returns></returns>
		public bool InsertFamilyMember(string msdId, string firstName, string lastName, string email, string dateOfBirth, string relationshipCode) {
			var ret = false;
			if (msdId != null) {
				const string query =
					"SELECT MBC.FAMILY_UTIL.ASSOCIATE@MBCDB( " +
					":msdid,:fname,:lname,:email,'',TO_DATE(:datebirth,'mmddyyyy'),:relationshipcode,'') " +
					"AS RETURNCODE FROM DUAL";
				var parameters = new[] {
					new OracleParameter(":msdid", OracleDbType.Varchar2) {Value = msdId},
					new OracleParameter(":fname", OracleDbType.Varchar2) {Value = firstName},
					new OracleParameter(":lname", OracleDbType.Varchar2) {Value = lastName},
					new OracleParameter(":email", OracleDbType.Varchar2) {Value = email},
					new OracleParameter(":datebirth", OracleDbType.Varchar2) {Value = dateOfBirth},
					new OracleParameter(":relationshipcode", OracleDbType.Varchar2) {Value = relationshipCode}
				};
				var messageError = $"MBC.FAMILY_UTIL.ASSOCIATE for mdsid:{msdId}" +
				                   $"Data to be inserted familymember mdsid:{msdId}, fname:{firstName}, " +
				                   $"lname:{lastName}, email:{email},datebirth:{dateOfBirth}, relationshipcode:{relationshipCode} ";
				ret = !ExecuteCommand(query, parameters, messageError).HasError;
			}
			return ret;
		}

		/// <summary>
		/// Get the List of family members
		/// </summary>
		/// <param name="mdsId">User id</param>
		/// <returns></returns>
		public IEnumerable<string> SelectFamilyMembersList(string mdsId) {
			IEnumerable<string> ret = null;
			if (null != mdsId) {
				const string query =
					@"SELECT (TRIM(ASSC_INDV_ID) ||'|'|| TRIM(INDV_ASSN_TYP_CD)) AS IDPIPEDCODE " +
					"FROM VIEW_ASSC_INDV@mbcdb " +
					"WHERE INDV_ID = :mdsid " +
					"ORDER BY ASSC_INDV_ID DESC";
				var parameters = new[] {
					new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsId}
				};
				var message = $"GetListFamilyMembers for mdsid:{mdsId}";
				var result = ExecuteQuery(query, parameters, message);
				if (!result.HasError && result.Records != null) {
					ret = result.Records
						.Select(x => x.FirstOrDefault()?.ToString())
						.Where(x => x != null);
				}
			}
			return ret;
		}

		/// <summary>
		/// Get detail of the family member
		/// </summary>
		/// <param name="familyMemberId">Family member id</param>
		/// <returns>null if none found</returns>
		public ViewIndividual SelectFamilyMemberInfo(string familyMemberId) {
			ViewIndividual ret = null;
			if (familyMemberId != null) {
				var resultFields = new[] {
					ViewIndividual.INDIVIDUAL_ID,
					ViewIndividual.INDIVIDUAL_FIRST_NAME,
					ViewIndividual.INDIVIDUAL_LAST_NAME,
					ViewIndividual.INDIVIDUAL_EMAIL_ADDRESS
				};
				const string sqlQuery = "SELECT TRIM(INDV_ID) AS " + ViewIndividual.INDIVIDUAL_ID + ", " +
				                        "INITCAP(TRIM(INDV_FRST_NM)) AS " + ViewIndividual.INDIVIDUAL_FIRST_NAME +
				                        ", " +
				                        "INITCAP(TRIM(INDV_LAST_NM)) AS " + ViewIndividual.INDIVIDUAL_LAST_NAME + ", " +
				                        "EMAIL_ADDR_TXT AS " + ViewIndividual.INDIVIDUAL_EMAIL_ADDRESS + " " +
				                        "FROM VIEW_INDV@mbcdb " +
				                        "WHERE INDV_ID = :familymemberid " +
				                        "AND ROWNUM = 1";
				var parameters = new[] {
					new OracleParameter(":familymemberid", OracleDbType.Varchar2) {Value = familyMemberId}
				};
				var message = $"GetFamilyMemberInfo {familyMemberId}";
				var oracleResult = ExecuteQuery(sqlQuery, parameters, message);

				if (!oracleResult.HasError && (oracleResult.Records?.Any() == true)) {
					ret = new ViewIndividual(oracleResult.Records.First(), resultFields);
				}
			}
			return ret;
		}

		/// <summary>
		/// Delete the family member if it exists
		/// </summary>
		/// <param name="msdId">User id</param>
		/// <param name="memberid">Member id</param>
		/// <returns>true if delete did not cause an error</returns>
		public bool DeleteFamilyMember(string msdId, string memberid) {
			var ret = false;
			if ((null != msdId) && (null != memberid)) {

				const string query = "SELECT MBC.FAMILY_UTIL.ASSOCIATE@MBCDB(:msdid,:memberid) AS RETURNCODE FROM DUAL";
				var parameters = new[] {
					new OracleParameter(":msdid", OracleDbType.Varchar2) {Value = msdId},
					new OracleParameter(":memberid", OracleDbType.Varchar2) {Value = memberid},
				};
				var messageError =
					$"delete family member, FAMILY_UTIL.ASSOCIATE stored procedure failed {msdId} {memberid}";
				ret = !ExecuteCommand(query, parameters, messageError).HasError;
			}
			return ret;
		}

		/// <summary>
		/// Check if spouse or domestic partner is already added in a member
		/// </summary>
		/// <param name="msdId">User id</param>
		/// <returns></returns>
		public bool IsSpouseDomesticPartnerAssociated(string msdId) {
			var ret = false;
			if (msdId != null) {
				const string query = @"SELECT COUNT(ASSC_INDV_ID) AS ASSC_INDV_ID " +
				                     "FROM VIEW_ASSC_INDV@mbcdb " +
				                     "WHERE INDV_ID = :mdsid " +
				                     "AND INDV_ASSN_TYP_CD IN ('01','35')";
				var parameters = new[] {
					new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = msdId}
				};
				var messageError = $"CheckSpouseDomesticPartner for mdsid:{msdId}";
				var oracleResult = ExecuteCommand(query, parameters, messageError);
				ret = int.TryParse(oracleResult.Value, out var value) && (value > 0);
			}
			return ret;
		}

		/// <summary>
		/// Verify that a given item code exists in the table PRODUCT_MAPPING
		/// </summary>
		/// <param name="itemCode">Required</param>
		/// <returns>True only if the PRODUCT_MAPPING.ITEM_CODE record exists</returns>
		public bool SelectItemCodeExists(string itemCode) {
			var ic = itemCode ?? throw new ArgumentNullException(nameof(itemCode));
			var query = $"SELECT ITEM_CODE FROM PRODUCT_MAPPING WHERE ITEM_CODE = :itemCode";
			var parameters = new [] {
				new OracleParameter(":itemCode", OracleDbType.Varchar2) {Value = itemCode}
			};
			var messageError = $"SelectItemCodeExists for itemCode: {itemCode}";
			var oracleResult = ExecuteCommand(query, parameters, messageError);
			return oracleResult.Value == itemCode;

		}

		public virtual IEnumerable<ViewUnredeemedRewards> SelectUnredeemedRewards(string mdsid) {
			var resultFields = new List<string> {
				ViewUnredeemedRewards.INDV_ID,
				ViewUnredeemedRewards.REWARDS_NM,
				ViewUnredeemedRewards.USER_REWARDS_DESC,
				ViewUnredeemedRewards.AWARDED_VAL
			};
			var sqlQuery = $"select {string.Join(",",resultFields)}, " +
						   $"TO_CHAR({ViewUnredeemedRewards.DATE_AWARDED}, 'YYYY/MM/DD') AS {ViewUnredeemedRewards.DATE_AWARDED}" +
						   " FROM view_unredeemed_rewards@mbcdb " +
									"where indv_id = :mdsIndividualId " +
									"AND date_awarded IS NOT NULL";
			var parameters = new[] {
				new OracleParameter(":mdsIndividualId", OracleDbType.Varchar2) {Value = mdsid}
			};
			var message = $"GetUnredeemedRewards {mdsid}";
			var result = ExecuteQuery(sqlQuery, parameters, message);
			return ViewUnredeemedRewards.DigestCommandResult(result);
		}

        public virtual decimal SelectUnredeemedRewardsTotal(string mdsId)
        {
            decimal ret = 0;
            if (null != mdsId)
            {
                const string query = "SELECT awarded_val AS awarded_val_total " +
                                     "FROM view_unredeemed_total@mbcdb " +
                                     "WHERE indv_id = :mdsid " +
                                     "AND ROWNUM = 1";
                var parameters = new[] {
                    new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsId}
                };
                var message = $"GetUnredeemedRewardsTotal for mdsid: {mdsId}";
                var result = ExecuteQuery(query, parameters, message);

                // ----- ENSURING RESULT OF ONE ROW IS PRESENT BEFORE PARSING THE VALUE -----
                if (!result.HasError && result.Records.Count == 1)
                {
                    ret = decimal.Parse(result.Records.FirstOrDefault()?.FirstOrDefault().ToString());
                }
            }
            return ret;
        }

        public IReadOnlyList<ViewSeminar> ViewAllSeminar() {
            IReadOnlyList<ViewSeminar> ret = null;
            const string query = "SELECT * FROM view_seminar@mbcdb";
            //const string query = "SELECT * FROM seminar";
            var messageError = $"MBC.view_seminar failed";
            OracleParameter[] parameters = new OracleParameter[0];
            var result = ExecuteQuery(query, parameters, messageError);
            ret = ViewSeminar.DigestCommandResult(result)
                ?.ToList()
                .AsReadOnly();
            return ret;
        }

        public bool InsertInSeminar(int msdId, string leaCode)
        {
            var ret = false;
            if (!string.IsNullOrEmpty(leaCode))
            {
                const string query = "CALL MBC.INSERT_INDV_SEMINAR@MBCDB(:msdid,:leaCode)";
                //const string query = "CALL insertSEMINAR_USER(:msdid,:leaCode)";
                var parameters = new[] {
                    new OracleParameter(":msdid", OracleDbType.Int32) {Value = msdId},
                    new OracleParameter(":leaCode", OracleDbType.Varchar2) {Value = leaCode}
                };
                var messageError = $"MBC.INSERT_INDV_SEMINAR failed {msdId} {leaCode}";
                ret = !ExecuteCommand(query, parameters, messageError).HasError;
            }
            return ret;
        }
        public IReadOnlyList<ViewSeminarReg> ViewAllSeminarReg()
        {
            IReadOnlyList<ViewSeminarReg> ret = null;
            const string query = "SELECT * FROM view_seminar_reg@mbcdb";
            //const string query = "SELECT * FROM SEMINAR_USER";
            var messageError = $"MBC.view_seminar_reg failed";
            OracleParameter[] parameters = new OracleParameter[0];
            var result = ExecuteQuery(query, parameters, messageError);
            ret = ViewSeminarReg.DigestCommandResult(result)
                ?.ToList()
                .AsReadOnly();
            return ret;
        }

        public IList<ViewOmni> ExecuteViewOmni(string mdsid, string productCode)
        {
            IList<ViewOmni> ret = null;
            string dateNow = DateTime.Now.ToString("yyyy/MM/dd");

            //const string query = "SELECT * FROM view_seminar_reg@mbcdb";
            //var query = $"SELECT * " +
            //    //"FROM view_indv@mbcdb " +
            //    "FROM VIEW_OMNI " +
            //    "WHERE indv_id = :imsid " +
            //    "AND user_item_id = :productCode " +
            //    "AND web_eff_dt <= TO_DATE(:today, 'yyyy/mm/dd') " +
            //    "AND web_end_dt >= TO_DATE(:today, 'yyyy/mm/dd')";

            var query = $"SELECT * " +
                "FROM view_omni@mbcdb " +
                //"FROM VIEW_OMNI " +
                "WHERE indv_id = :imsid " +
                "AND user_item_id = :productCode " +
                "AND web_eff_dt <= TO_DATE(:today, 'yyyy/mm/dd') " +
                "AND web_end_dt >= TO_DATE(:today, 'yyyy/mm/dd')";

            var parameters = new[] {
                new OracleParameter(":imsid", OracleDbType.Varchar2) {Value = mdsid},
                new OracleParameter(":productCode", OracleDbType.Varchar2) {Value = productCode},
                new OracleParameter(":today", OracleDbType.Varchar2) {Value = dateNow},
            };

            var messageError = $"MBC.view_omni failed";
            //OracleParameter[] parameters = new OracleParameter[0];
            var result = ExecuteQuery(query, parameters, messageError);
            ret = ViewOmni.DigestCommandResult(result)
                ?.ToList();
            return ret;
        }

        public bool DeleteAllTestUsers()
        {
            var parameters = new OracleParameter[0];
			var message = $"ExecuteRemoveTestUsers";
            const string query = "CALL DEL_TEST_WEB_USERS()";
            var ret = !ExecuteCommand(query, parameters, message).HasError;
            return ret;
        }

        public bool Rakuten_Registration(string mdsId, string emailAddress, string regId, string ebToken, 
                             string unionId, string cellCode)
        {
            string getCellCode = !string.IsNullOrEmpty(cellCode) ? cellCode.ToString().Replace("'", "") : string.Empty;
            if (getCellCode.Length > 8) getCellCode = getCellCode.Substring(0, 8);
            const string query = "insert into rakuten_registrations " +
                  "(mds_indv_id, login_user_id, create_date, union_id, rakuten_id, rakuten_ebtoken,  cell_cd)"+
                  "values(:mdsId, :email, to_date( to_char(sysdate,'mm/dd/yyyy') ,'mm/dd/yyyy'), :unionId, :regId, :token, :getCellCode)";
            var parameters = new[] { 
                new OracleParameter(":mdsId", OracleDbType.Varchar2) {Value = mdsId},
                new OracleParameter(":email", OracleDbType.Varchar2) {Value = emailAddress },
                new OracleParameter(":unionId", OracleDbType.Varchar2) {Value = unionId},
                new OracleParameter(":regId", OracleDbType.Varchar2) {Value = regId },
                new OracleParameter(":token", OracleDbType.Varchar2) {Value = ebToken},
                new OracleParameter(":cellCode", OracleDbType.Varchar2) {Value = getCellCode}
            };
            var messageError = $"Error in Insert Rakuten Registration {mdsId} {emailAddress}";
            return !ExecuteCommand(query, parameters, messageError).HasError;
        }
        public bool RakutenRegExists(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress)) return false;
            var query = $"SELECT * FROM rakuten_registrations where login_user_id=:email";
            var parameters = new[] {
                new OracleParameter(":email", OracleDbType.Varchar2) {Value = emailAddress }
            };
            var messageError = $"RakutenRegExists : {emailAddress}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            return oracleResult.Value == emailAddress;

        }

        public ViewRakutenReg ViewRakutenRegs(string emailAddress)
        {
            ViewRakutenReg ret = null;
            if (emailAddress != null)
            {
                var resultFields = new[] {
                    ViewRakutenReg.MDS_ID,
                    ViewRakutenReg.EMAIL_ADDRESS,
                    ViewRakutenReg.FAVORITE_STORE,
                    ViewRakutenReg.EB_TOKEN,
                    ViewRakutenReg.STORE_ID,
                    ViewRakutenReg.UNION_ID,
                    ViewRakutenReg.CREATE_DATE
                };
                const string query = "SELECT mds_indv_id, login_user_id, favorite_stores, rakuten_ebtoken, rakuten_id, union_id, create_date FROM rakuten_registrations where login_user_id=:email";
                var messageError = $"select rakuten_registrations query failed";
                var parameters = new[] {
                    new OracleParameter(":email", OracleDbType.Varchar2) {Value = emailAddress }
                };
                var oracleResult = ExecuteQuery(query, parameters, messageError);
                if (!oracleResult.HasError && (oracleResult.Records?.Any() == true))
                {
                    ret = new ViewRakutenReg(oracleResult.Records.First(), resultFields);
                }
            }
			return ret;
        }

        public bool Update_Favorite_Stores(string mdsId, string emailAddress, string favoriteStores)
        {

			string query = "update rakuten_registrations set mds_indv_id=: mdsId, favorite_stores=";
			while( favoriteStores.Length > MAX_TEXT_LEN )
			{
				query += "to_clob( '" + favoriteStores.Substring(0, MAX_TEXT_LEN) + "') || ";
				favoriteStores = favoriteStores.Substring(MAX_TEXT_LEN);
            }
			query += "to_clob('" + favoriteStores + "') where login_user_Id= :email";
            var parameters = new[] {
                new OracleParameter(":mdsId", OracleDbType.Varchar2) {Value = mdsId},
                new OracleParameter(":email", OracleDbType.Varchar2) {Value = emailAddress }
              };
            var messageError = $"Error in update Rakuten favorite store {emailAddress}";
            var result = ExecuteCommand(query, parameters, messageError);
			return !result.HasError;
        }

        public string Search_Favorite_Stores(string emailAddress)
        {
            const string query = "select favorite_stores from  rakuten_registrations where login_user_Id= :email)";
            var parameters = new[] {
                new OracleParameter(":email", OracleDbType.Varchar2) {Value = emailAddress },
                };
            var messageError = $"Error in search Rakuten favorite store {emailAddress}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            return oracleResult.Value;
        }

		public string PermisionUpdate(string jsonObject)
		{
			const string query = "select mds_util.permission_update@mbcdb(:json) from dual";
			var parameters = new[] {
				new OracleParameter(":json", OracleDbType.Varchar2) {Value = jsonObject },
				};
			var messageError = $"Error in permission update process {jsonObject}";
			var oracleResult = ExecuteCommand(query, parameters, messageError);
			return oracleResult.Value;
		}
		#endregion
	}
}