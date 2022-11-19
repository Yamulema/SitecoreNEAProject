using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(IOracleManager))]
    public class OracleManager : IOracleManager
    {
        public OracleManager() { }
        
        private OracleResult ExecuteCommand(string query, OracleParameter[] parameters, string messageError)
        {
            var oracleResult = new OracleResult();
            try
            {
                using (var con =
                    new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddRange(parameters);
                    cmd.Prepare();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var valueReturned = reader.GetValue(0);
                            if (valueReturned != null)
                            {
                                oracleResult.Result = valueReturned.ToString();
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                oracleResult.HasError = true;
                Sitecore.Diagnostics.Log.Error(messageError, ex, "OracleProvider");
            }
            return oracleResult;
        }

        private OracleResult ExecuteQuery(string query, OracleParameter[] parameters, string messageError)
        {
            var oracleResult = new OracleResult();
            try
            {
                using (var con =
                    new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.AddRange(parameters);
                    cmd.Prepare();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new List<object>();
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetFieldValue<object>(i));
                            }

                            if (row.Any())
                            {
                                oracleResult.Records.Add(row);
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                oracleResult.HasError = true;
                Sitecore.Diagnostics.Log.Error(messageError, ex, "OracleProvider");
            }
            return oracleResult;
        }
        /// <summary>
        /// Execution of the stored procedure  MDS_UTIL.ORDER_FULFILL@MBCDB 
        /// </summary>
        /// <param name="msdid">User id</param>
        /// <param name="itemcode"></param>
        /// <param name="cellcode"></param>
        /// <param name="value4"></param>
        /// <returns></returns>
        public bool ExecuteSelectOrderFulfillOracle(int msdid, string itemcode, string cellcode, string value4)
        {
            var query =
                $"select MDS_UTIL.ORDER_FULFILL@MBCDB(:msdid,:itemcode,:cellcode,:value4) from dual";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":msdid", OracleDbType.Int32) {Value = msdid},
                new OracleParameter(":itemcode", OracleDbType.Varchar2) {Value = itemcode},
                new OracleParameter(":cellcode", OracleDbType.Varchar2) {Value = cellcode},
                new OracleParameter(":value4", OracleDbType.Varchar2) {Value = value4}
            };
            var messageError = $"The execution to Oracle database, order_fulfill storedprocedure failed {itemcode}";
            var result = ExecuteCommand(query, parameters, messageError);
            return !result.HasError;
        }

        public string ExecuteQueryMaterialOracle(string productCode)
        {
            var query = $"select ITEM_CODE from PRODUCT_MAPPING where PRODUCT_CODE = :productCode";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":productCode", OracleDbType.Varchar2) {Value = productCode}
            };
            var messageError = $"The execution to Oracle database, order_fulfill storedprocedure failed {productCode}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            return oracleResult.Result;
        }

        public bool InsertReminder(string reminderId, string mdsIndvId)
        {
            var query =
                $"insert into reminder_log (reminder_id, mds_indv_id, date_time)values(:reminderId, :mdsIndvId, sysdate)";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":reminderId", OracleDbType.Varchar2) {Value = reminderId},
                new OracleParameter(":mdsIndvId", OracleDbType.Varchar2) {Value = mdsIndvId}
            };
            var messageError = $"Error in InsertReminder {reminderId} {mdsIndvId}";
            var result = ExecuteCommand(query, parameters, messageError);
            return !result.HasError;
        }

        public string GetAllReminderlogById(string reminderId, string mdsIndvId)
        {
            var query =
                $"select count (*) as TOTAL from reminder_log where reminder_id = :reminderId and mds_indv_id = :mdsIndvId";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":reminderId", OracleDbType.Varchar2) {Value = reminderId},
                new OracleParameter(":mdsIndvId", OracleDbType.Varchar2) {Value = mdsIndvId}
            };
            var messageError = $"Error in GetAllReminderlogById {reminderId} {mdsIndvId}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            return oracleResult.Result;
        }

        public bool ExecuteInsertSweeps(string msdid, string sweeptakesid)
        {
            var query = $"CALL MBC.INSERT_INDV_SWEEPS@MBCDB(:msdid,:sweeptakesid)";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":msdid", OracleDbType.Varchar2) {Value = msdid},
                new OracleParameter(":sweeptakesid", OracleDbType.Varchar2) {Value = sweeptakesid}
            };
            var messageError =
                $"The execution to Oracle database, insert sweeps storedprocedure failed {msdid} {sweeptakesid}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            return oracleResult.HasError;
        }

        public List<string> ExecuteLpgtRates(string coverage, string term, string age, string memberspouse, string smoker)
        {
            var result = new List<string>();

            var query =
                $"select rate from LPGT_RATES where face = :coverage AND term = :term AND age = :age AND memberspouse = :memberspouse AND underwrite = :smoker order by rate";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":coverage", OracleDbType.Varchar2) {Value = coverage},
                new OracleParameter(":term", OracleDbType.Varchar2) {Value = term},
                new OracleParameter(":age", OracleDbType.Varchar2) {Value = age},
                new OracleParameter(":memberspouse", OracleDbType.Varchar2) {Value = memberspouse},
                new OracleParameter(":smoker", OracleDbType.Varchar2) {Value = smoker}
            };
            var messageError =
                $"The execution to Oracle database select LPGT_RATES failed {coverage},{term},{age},{memberspouse},{smoker}";
            var oracleResult = ExecuteQuery(query, parameters, messageError);

            if (!oracleResult.HasError)
            {
                foreach (var record in oracleResult.Records)
                {
                    foreach (var recorditem in record)
                    {
                        result.Add(recorditem.ToString());
                    }
                }
            }
            return result;
        }

        public string ExecuteLpgtRates(string coverage, string age)
        {
            var query = $"select rate from LI_RATES where coverage = :coverage AND age = :age order by rate";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":coverage", OracleDbType.Varchar2) {Value = coverage},
                new OracleParameter(":age", OracleDbType.Varchar2) {Value = age}
            };
            var messageError = $"The execution to Oracle database select LI_RATES failed {coverage},{age}";
            var result = ExecuteCommand(query, parameters, messageError);
            return result.Result;
        }

        public string GetMdsid(string imsid)
        {
            Log.Info($"Attempting to call GetMdsid for imsid:{imsid}.", this);

            var query = $"select indv_id from view_indv@mbcdb where user_indv_id = :imsid";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":imsid", OracleDbType.Varchar2) {Value = imsid}
            };
            var messageError = $"Error in GetMdsid for imsid:{imsid}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            if (!oracleResult.HasError)
            {
                Log.Info($"Successful call to Oracle for GetMdsid using imsid:{imsid}.", this);
                return oracleResult.Result;
            }
            else
            {
                Log.Error($"Failed to fetch GetMdsid for imsid:{imsid}.", this);
                return null;
            }
        }

        public string GetImsid(string mdsid)
        {
            Log.Info($"Attempting to call GetImsid for mdsid:{mdsid}.", this);

            var query = $"select user_indv_id from view_indv@mbcdb where indv_id = :mdsid";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsid}
            };
            var messageError = $"Error in GetImsid for mdsid:{mdsid}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            if (!oracleResult.HasError)
            {
                Log.Info($"Successful call to Oracle for GetImsid using mdsid:{mdsid}.", this);
                return oracleResult.Result;
            }
            else
            {
                Log.Error($"Failed to fetch GetImsid for mdsid:{mdsid}.", this);
                return null;
            }
        }

        public List<string> GetRates(string state, string zip, int[] ages, string option)
        {
            Log.Info($"Attempting to fetch GetRates.", this);
            var result = new List<string>();
            var ageFilter = string.Join(",",
                ages.Select(x => $"min(case when min_age='{x}' then TO_CHAR(rate,'$9,999.99') end) as r{x}")); ;
            if (((string.Equals(state, "FL") || (string.Equals(state, "MO")) && (ages.Length == 1))))
            {
                if ((ages[0] > 66) && (ages[0] < 70))
                {
                    ageFilter = "min(case when min_age='66' then TO_CHAR(rate,'$9,999.99') end) as r" + ages[0].ToString();
                }
            } 
            
            var zipFilter = string.Empty;

            var parameters = new List<OracleParameter>
            {
                new OracleParameter(":state1", OracleDbType.Varchar2) {Value = state}
            };

            if (!string.IsNullOrEmpty(zip))
            {
                zipFilter = $"and area = (select area from mcs_zip where statecode = :state2 and zipcode = :zip)";
                parameters.Add(new OracleParameter(":state2", OracleDbType.Varchar2) { Value = state });
                parameters.Add(new OracleParameter(":zip", OracleDbType.Varchar2) {Value = zip});
            }

            var sqlQuery = $"select {ageFilter} from (select rate_{option}/100 as rate, min_age from mcs_rates where min_age <= 80 and statecode = :state1 {zipFilter})";
            var messageError = $"Error in GetRates.";
            var oracleResult = ExecuteQuery(sqlQuery, parameters.ToArray(), messageError);

            if (!oracleResult.HasError)
            {
                result = oracleResult.Records.FirstOrDefault()?.Select(x => x as string).ToList();
                Log.Info($"Successful to fetch GetRates.", this);
            }
            else
            {
                Log.Error($"Failed to fetch GetRates.", this);
                Log.Error(messageError, this);
            }
            return result;
        }

        public int GetZipCodeCount(string state)
        {
            Log.Info($"Attempting to call GetZipCodeCount for state:{state}.", this);

            var query = $"select count(ZIPCODE) as STATEOUT from mcs_zip where statecode = :state";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":state", OracleDbType.Varchar2) {Value = state}
            };
            var messageError = $"Error in GetZipCodeCount for state:{state}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            if (!oracleResult.HasError)
            {
                Log.Info($"Successful call to Oracle for GetZipCodeCount for state:{state}.", this);
                return int.TryParse(oracleResult.Result, out var zipCodeCount) ? zipCodeCount : 0;
            }
            else
            {
                Log.Error($"Failed to fetch GetZipCodeCount for state:{state}.", this);
                return 0;
            }
        }


        public List<StatesDB> GetStates()
        {
            Log.Info($"Attempting to fetch GetStates.", this);
            var result = new List<StatesDB>();
            var resultFields = new List<string>
            {
                "statecode",
                "statename"
            };
            var fields = string.Join(",", resultFields);

            var sqlQuery = $"SELECT {fields} FROM state ORDER BY statename ASC";
            var parameters = new OracleParameter[]{};
            var messageError = "Error in GetStates";
            var oracleResult = ExecuteQuery(sqlQuery, parameters, messageError);

            if (!oracleResult.HasError)
            {
                result = oracleResult.Records.Select(x => new StatesDB(x, resultFields)).ToList();
                Log.Info($"Successful to fetch GetStates.", this);
            }
            else
            {
                Log.Error($"Failed to fetch GetStates.", this);
                Log.Error(messageError, this);
            }
            return result;
        }

        public List<ViewInsReg> GetViewInsReg(string mdsIndvId)
        {
            Log.Info($"Attempting to fetch GetViewInsReg for mdsid:{mdsIndvId}.", this);
            var result = new List<ViewInsReg>();
            var resultFields = new List<string>
            {
                "wage_earn_cd",
                "dtab_mrtl_stat_cd",
                "fmly_incm_rnge_cd",
                "hous_stat_cd",
                "spse_empt_cd",
                "depn_chld_cnt",
                "dtab_vocl_lvl_cd"
            };
            var fields = string.Join(",", resultFields);

            var sqlQuery = $"SELECT {fields} FROM view_ins_reg@mbcdb WHERE indv_id = :mdsIndvId";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":mdsIndvId", OracleDbType.Varchar2) {Value = mdsIndvId}
            };
            var messageError = String.Format("Error in GetViewInsReg {0}", mdsIndvId);
            var oracleResult = ExecuteQuery(sqlQuery, parameters, messageError);

            if (!oracleResult.HasError)
            {
                result = oracleResult.Records.Select(x => new ViewInsReg(x, resultFields)).ToList();
                Log.Info($"Successful to fetch GetViewInsReg for mdsid:{mdsIndvId}.", this);
            }
            else
            {
                Log.Error($"Failed to fetch GetViewInsReg for mdsid:{mdsIndvId}.", this);
                Log.Error(messageError, this);
            }
            return result;
        }

        public List<ViewBene> GetViewBene(string mdsIndvId)
        {
            Log.Info($"Attempting to fetch GetViewBene for mdsid:{mdsIndvId}.", this);
            var result = new List<ViewBene>();

            var resultFields = new List<string>
            {
                "bnry_nm",
                "bnry_frst_nm",
                "bnry_last_nm",
                "bnry_mid_nm",
                "bnry_typ",
                "bnry_email_addr_txt",
                "bnry_desg_pts",
                "bnry_desg_cd"
            };

            var sqlQuery =
                $"select NVL(bnry_nm, ' ') as BNRY_NM, NVL(bnry_frst_nm, ' ') as BNRY_FRST_NM, NVL(bnry_last_nm,' ') as BNRY_LAST_NM, NVL(bnry_mid_nm,' ') as BNRY_MID_NM, bnry_typ as BNRY_TYP, NVL(bnry_email_addr_txt, ' ') as BNRY_EMAIL_ADDR_TXT, bnry_desg_pct*100 as bnry_desg_pts, bnry_desg_cd from view_bene@mbcdb where dtab_rgr_id=(select dtab_rgr_id from view_ins_reg@mbcdb where indv_id = :mdsIndvId)";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":mdsIndvId", OracleDbType.Varchar2) {Value = mdsIndvId}
            };
            var messageError = $"Error in GetViewBene {mdsIndvId}";
            var oracleResult = ExecuteQuery(sqlQuery, parameters, messageError);

            if (!oracleResult.HasError)
            {
                result = oracleResult.Records.Select(x => new ViewBene(x, resultFields)).ToList();
                Log.Info($"Successful to fetch GetViewBene for mdsid:{mdsIndvId}.", this);
            }
            else
            {
                Log.Error($"Failed to fetch GetViewBene for mdsid:{mdsIndvId}.", this);
                Log.Error(messageError, this);
            }
            return result;
        }

        public bool ExecuteInsertComplimentaryLife(ComplimentaryLifeDb complimentaryLifeDb)
        {
            Log.Info($"Attempting to fetch ExecuteInsertComplimentaryLife for IndvID:{complimentaryLifeDb.IndvID}.", this);
            var vString = JsonConvert.SerializeObject(complimentaryLifeDb);
            Log.Info($"vString:{vString}", this);
            var query = $"SELECT comp_life_utl.comp_life_beneficiary@mbcdb (:vString) from dual";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":vString", OracleDbType.Varchar2) {Value = vString}
            };

            var messageError = $"Error in ExecuteInsertComplimentaryLife for mdsid:{complimentaryLifeDb.IndvID}";

            var oracleResult = ExecuteCommand(query, parameters, messageError);

            if (oracleResult.HasError)
            {
                Log.Error($"Failed to ExecuteInsertComplimentaryLife for mdsid:{complimentaryLifeDb.IndvID}.", this);
                return false;
            }

            if (string.IsNullOrEmpty(oracleResult.Result))
            {
                Log.Error($"Failed to ExecuteInsertComplimentaryLife for mdsid:{complimentaryLifeDb.IndvID}.", this);
                return false;
            }

            if (int.TryParse(oracleResult.Result, out var resultCode) && resultCode == 0)
            {
                Log.Info($"Successful to ExecuteInsertComplimentaryLife for mdsid:{complimentaryLifeDb.IndvID}.", this);
                return true;
            }
            else
            {
                Log.Error($"Failed to ExecuteInsertComplimentaryLife for mdsid:{complimentaryLifeDb.IndvID}.", this);
                return false;
            }
        }

        public string GetLastUpdateDate(string mdsid)
        {
            Log.Info($"Attempting to fetch GetLastUpdateDate for mdsid:{mdsid}.", this);
            var query =
                $@"SELECT TO_CHAR(dtab_rgr_sign_dt,'MM/DD/YYYY') as DTAB_RGR_SIGN_DT FROM view_indv@mbcdb WHERE indv_id = :mdsid";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsid}
            };

            var messageError = $"Error in GetLastUpdateDate for mdsid:{mdsid}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);

            if (!oracleResult.HasError)
            {
                Log.Info($"Successful to fetch GetLastUpdateDate for mdsid:{mdsid}.", this);
                return oracleResult.Result;
            }
            Log.Error($"Failed to fetch GetLastUpdateDate for mdsid:{mdsid}.", this);
            Log.Error(messageError, this);
            return string.Empty;
        }

        public List<int> GetViewChildYr(string mdsid)
        {
            var result = new List<int>();
            Log.Info($"Attempting to fetch GetViewChildYr for mdsid:{mdsid}.", this);
            var sqlQuery = $"SELECT DEPN_BRTH_YR FROM view_child_yr@mbcdb where dtab_rgr_id=(select dtab_rgr_id from view_ins_reg@mbcdb where indv_id = :mdsid)";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsid}
            };

            var messageError = $"Error in GetViewChildYr {mdsid}";
            var oracleResult = ExecuteQuery(sqlQuery, parameters, messageError);

            if (oracleResult.HasError)
            {
                Log.Error($"Failed to fetch GetViewChildYr for mdsid:{mdsid}.", this);
                Log.Error(messageError, this);
                return result;
            }
            Log.Info($"Successful to fetch GetViewChildYr for mdsid:{mdsid}.", this);
            foreach (var record in oracleResult.Records)
            {
                var value = record?.FirstOrDefault()?.ToString();
                result.Add(int.TryParse(value, out var year) ? year : 0);
            }
            return result;
        }

		/// <summary>
		/// Operation insert invite family member
		/// </summary>
		/// <param name="msdid">User id</param>
		/// <param name="fname">First name</param>
		/// <param name="lname">Last name</param>
		/// <param name="email">email</param>
		/// <param name="datebirth">Date of birth</param>
		/// <param name="relationshipcode">Relationship</param>
		/// <returns></returns>
	    public bool ExecuteInserFamilyMember(string msdid, string fname, string lname, string email,string datebirth, string relationshipcode)
	    {
			Log.Info($"Data to be inserted familymember mdsid:{msdid}, fname:{fname}, lname:{lname}, email:{email},datebirth:{datebirth}, relationshipcode:{relationshipcode} ",this);
			var query =
				$"SELECT MBC.FAMILY_UTIL.ASSOCIATE@MBCDB(:msdid,:fname,:lname,:email,'',TO_DATE(:datebirth,'mmddyyyy'),:relationshipcode,'') AS RETURNCODE FROM DUAL";
			//var query =
			//	$"CALL insertVIEW_INDV(:msdid,:fname,:lname,:email,:datebirth,:relationshipcode)";
			var parameters = new OracleParameter[]
		    {
			    new OracleParameter(":msdid", OracleDbType.Varchar2) {Value = msdid},
			    new OracleParameter(":fname", OracleDbType.Varchar2) {Value = fname},
			    new OracleParameter(":lname", OracleDbType.Varchar2) {Value = lname},
			    new OracleParameter(":email", OracleDbType.Varchar2) {Value = email},
			    new OracleParameter(":datebirth", OracleDbType.Varchar2) {Value = datebirth},
			    new OracleParameter(":relationshipcode", OracleDbType.Varchar2) {Value = relationshipcode}
		    };
		    var messageError = $"The execution to Oracle database MBC.FAMILY_UTIL.ASSOCIATE storedprocedure failed {msdid}";
		    var result = ExecuteCommand(query, parameters, messageError);
		    return !result.HasError;
	    }

		/// <summary>
		/// Get the List of family members
		/// </summary>
		/// <param name="mdsid">User id</param>
		/// <returns></returns>
	    public FamilyMemberListResult GetListFamilyMembers(string mdsid)
	    {
		    FamilyMemberListResult familyMemberListResult = new FamilyMemberListResult();
			var result = new List<string>();
			Log.Info($"Attempting to fetch GetListFamilyMembers for mdsid:{mdsid}.", this);
			var query =
				$@"SELECT (TRIM(ASSC_INDV_ID) ||'|'|| TRIM(INDV_ASSN_TYP_CD)) AS IDPIPEDCODE FROM VIEW_ASSC_INDV@mbcdb WHERE INDV_ID = :mdsid ORDER BY ASSC_INDV_ID DESC";
			//var query =
			//	$@"SELECT (TRIM(ASSC_INDV_ID) ||'|'|| TRIM(INDV_ASSN_TYP_CD)) AS IDPIPEDCODE FROM VIEW_ASSC_INDV WHERE INDV_ID = :mdsid ORDER BY ASSC_INDV_ID DESC";
			var parameters = new OracleParameter[]
		    {
			    new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsid}
		    };

		    var messageError = $"Error in GetListFamilyMembers for mdsid:{mdsid}";
			var oracleResult = ExecuteQuery(query, parameters, messageError);
		    familyMemberListResult.HasError = oracleResult.HasError;

			if (oracleResult.HasError)
		    {
			    Log.Error($"Failed to fetch GetListFamilyMembers for mdsid:{mdsid}.", this);
			    Log.Error(messageError, this);
			    return familyMemberListResult;
		    }
		    Log.Info($"Successful to fetch GetListFamilyMembers for mdsid:{mdsid}.", this);
		    foreach (var record in oracleResult.Records)
		    {
			    var value = record?.FirstOrDefault()?.ToString();
			    result.Add(value);
		    }

		    familyMemberListResult.Records = result;

			return familyMemberListResult;
		}

		/// <summary>
		/// Get detail of the family member
		/// </summary>
		/// <param name="familyMemberId">Family member id</param>
		/// <returns></returns>
	    public ViewIndv GetFamilyMemberInfo(string familyMemberId)
	    {
		    Log.Info($"Attempting to fetch GetFamilyMemberInfo for familyMemberId:{familyMemberId}.", this);
		    var result = new ViewIndv();

		    var resultFields = new List<string>
		    {
			    "indv_id",
			    "indv_frst_nm",
			    "indv_last_nm",
				"email_addr_txt"
			};

			var sqlQuery =
				$"SELECT TRIM(INDV_ID) AS indv_id, INITCAP(TRIM(INDV_FRST_NM)) AS indv_frst_nm, INITCAP(TRIM(INDV_LAST_NM)) AS indv_last_nm,EMAIL_ADDR_TXT FROM VIEW_INDV@mbcdb WHERE INDV_ID = :familymemberid";
			//var sqlQuery =
			//	$"SELECT TRIM(INDV_ID) AS INDV_ID, INITCAP(TRIM(INDV_FRST_NM)) AS INDV_FRST_NM, INITCAP(TRIM(INDV_LAST_NM)) AS INDV_LAST_NM,EMAIL AS EMAIL_ADDR_TXT FROM VIEW_INDV WHERE INDV_ID = :familymemberid";
			var parameters = new OracleParameter[]
		    {
			    new OracleParameter(":familymemberid", OracleDbType.Varchar2) {Value = familyMemberId}
		    };
		    var messageError = $"Error in GetFamilyMemberInfo {familyMemberId}";
		    var oracleResult = ExecuteQuery(sqlQuery, parameters, messageError);

		    if (!oracleResult.HasError)
		    {
			    result = oracleResult.Records.Select(x => new ViewIndv(x, resultFields)).FirstOrDefault();
			    Log.Info($"Successful to fetch GetFamilyMemberInfo for familyMemberId:{familyMemberId}.", this);
		    }
		    else
		    {
			    Log.Error($"Failed to fetch GetFamilyMemberInfo for familyMemberId:{familyMemberId}.", this);
			    Log.Error(messageError, this);
		    }
		    return result;
	    }

		/// <summary>
		/// Delete the family member
		/// </summary>
		/// <param name="msdid">User id</param>
		/// <param name="memberid">Member id</param>
		/// <returns></returns>
	    public bool ExecuteDeleteFamilyMember(string msdid, string memberid)
	    {
		    Log.Info($"Attempting to delete family member for msdid:{msdid}, memberid:{memberid}", this);

			var query = $"SELECT MBC.FAMILY_UTIL.ASSOCIATE@MBCDB(:msdid,:memberid) AS RETURNCODE FROM DUAL";
			//var query = $"CALL deleteVIEW_INDV(:msdid,:memberid)";

			var parameters = new OracleParameter[]
		    {
			    new OracleParameter(":msdid", OracleDbType.Varchar2) {Value = msdid},
			    new OracleParameter(":memberid", OracleDbType.Varchar2) {Value = memberid},
		    };
		    var messageError = $"The execution to Oracle database delete family member, FAMILY_UTIL.ASSOCIATE storedprocedure failed {msdid} {memberid}";
		    var result = ExecuteCommand(query, parameters, messageError);
		    return !result.HasError;
	    }

		/// <summary>
		/// Check if spouse or domestic partner is already added in a member
		/// </summary>
		/// <param name="mdsid">User id</param>
		/// <returns></returns>
	    public bool CheckSpouseDomesticPartner(string mdsid)
	    {
		    bool result = false;
		    Log.Info($"Attempting to fetch CheckSpouseDomesticPartner for mdsid:{mdsid}.", this);
			var query = $@"SELECT TRIM(ASSC_INDV_ID) AS ASSC_INDV_ID FROM VIEW_ASSC_INDV@mbcdb WHERE INDV_ID = :mdsid AND INDV_ASSN_TYP_CD IN ('01','35')";
			//var query = $@"SELECT TRIM(ASSC_INDV_ID) AS ASSC_INDV_ID FROM VIEW_ASSC_INDV WHERE INDV_ID = :mdsid AND INDV_ASSN_TYP_CD IN ('01','35')";
			var parameters = new OracleParameter[]
		    {
			    new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsid}
		    };

		    var messageError = $"Error in CheckSpouseDomesticPartner for mdsid:{mdsid}";
		    var oracleResult = ExecuteQuery(query, parameters, messageError);

		    if (oracleResult.HasError)
		    {
			    Log.Error($"Failed to fetch CheckSpouseDomesticPartner for mdsid:{mdsid}.", this);
			    Log.Error(messageError, this);
			    return result;
		    }
		    Log.Info($"Successful to fetch CheckSpouseDomesticPartner for mdsid:{mdsid}.", this);
		    result = oracleResult.Records.Count > 0 ? true : false;
		    return result;
	    }

	    public string ExecuteQueryExistenceMaterialOracle(string materialid)
	    {
		    var query = $"select ITEM_CODE from PRODUCT_MAPPING where ITEM_CODE = :materialid";
		    var parameters = new OracleParameter[]
		    {
			    new OracleParameter(":materialid", OracleDbType.Varchar2) {Value = materialid}
		    };
		    var messageError = $"The execution to Oracle database, material table failed {materialid}";
		    var oracleResult = ExecuteCommand(query, parameters, messageError);
		    return oracleResult.Result;
	    }

        public IEnumerable<ViewUnredeemedRewards> GetUnredeemedRewards(string mdsid)
        {
            Log.Info($"Attempting to fetch GetUnredeemedRewards for mdsid:{mdsid}.", this);
            var result = new List<ViewUnredeemedRewards>();

            var resultFields = new List<string>
            {
                "INDV_ID",
                "REWARDS_NM",
                "DATE_AWARDED",
                "USER_REWARDS_DESC",
                "AWARDED_VAL"
            };

            var sqlQuery =
                $"select * from view_unredeemed_rewards@mbcdb where indv_id = :mdsIndvId";
            //var sqlQuery =
            //    $"select * from view_unredeemed_rewards where indv_id = :mdsIndvId";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":mdsIndvId", OracleDbType.Varchar2) {Value = mdsid}
            };
            var messageError = $"Error in GetUnredeemedRewards {mdsid}";
            var oracleResult = ExecuteQuery(sqlQuery, parameters, messageError);

            if (!oracleResult.HasError)
            {
                result = oracleResult.Records.Select(x => new ViewUnredeemedRewards(x, resultFields)).ToList();
                Log.Info($"Successful to fetch GetUnredeemedRewards for mdsid:{mdsid}.", this);
            }
            else
            {
                Log.Error($"Failed to fetch GetUnredeemedRewards for mdsid:{mdsid}.", this);
                Log.Error(messageError, this);
            }
            return result;
        }
        public int GetUnredeemedRewardsTotal(string mdsid)
        {
            Log.Info($"Attempting to call GetUnredeemedRewardsTotal for mdsid:{mdsid}.", this);

            var query = $"select awarded_val AS awarded_val_total from view_unredeemed_total@mbcdb where indv_id = :mdsid";
            //var query = $"select awarded_val AS awarded_val_total from view_unredeemed_total where indv_id = :mdsid";
            var parameters = new OracleParameter[]
            {
                new OracleParameter(":mdsid", OracleDbType.Varchar2) {Value = mdsid}
            };
            var messageError = $"Error in GetUnredeemedRewardsTotal for mdsid:{mdsid}";
            var oracleResult = ExecuteCommand(query, parameters, messageError);
            if (!oracleResult.HasError)
            {
                Log.Info($"Successful call to Oracle for GetUnredeemedRewardsTotal for mdsid:{mdsid}.", this);
                return int.TryParse(oracleResult.Result, out var totalRewards) ? totalRewards : 0;
            }
            else
            {
                Log.Error($"Failed to fetch GetUnredeemedRewardsTotal for mdsid:{mdsid}.", this);
                return 0;
            }
        }
    }
}