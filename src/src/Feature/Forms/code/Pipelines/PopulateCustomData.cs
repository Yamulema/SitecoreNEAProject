using System.Data;
using Sitecore.Cintel.Reporting;
using Sitecore.Cintel.Reporting.Processors;

namespace Neambc.Seiumb.Feature.Forms.Pipelines {
	public class PopulateCustomData : ReportProcessorBase {
		public override void Process(ReportProcessorArgs args) {
			var result = args.QueryResult;
			var table = args.ResultTableForView;

			foreach (var row in result.AsEnumerable()) {
				var targetRow = table.NewRow();

				// FirstName
				targetRow["FirstName"] = row["FirstName"];
                // LastName
                targetRow["LastName"] = row["LastName"];
                // Email
                targetRow["Email"] = row["Email"];
                // Iaid
                targetRow["Iaid"] = row["Iaid"];
                // NeaCurrentMember
                targetRow["NeaCurrentMember"] = row["NeaCurrentMember"];
                // UnionId
                targetRow["UnionId"] = row["UnionId"];

                // MdsId
                targetRow["MdsId"] = row["MdsId"];

                // Street Address
                targetRow["StreetAddress"] = row["StreetAddress"];

                // DateOfBirth
                var dob = row["DateOfBirth"];

                if (dob != null && !string.IsNullOrEmpty(dob.ToString()) && dob.ToString().Length == 8)
                {
                    targetRow["DateOfBirth"] = string.Format("{0}/{1}/{2}", dob.ToString().Substring(0, 2), dob.ToString().Substring(2, 2), dob.ToString().Substring(4, 4));
                }

                // City
                targetRow["City"] = row["City"];

                // StateCode
                targetRow["StateCode"] = row["StateCode"];

                // ZipCode
                targetRow["ZipCode"] = row["ZipCode"];

                // Phone
                var phone = row["Phone"];

                if (phone != null && !string.IsNullOrEmpty(phone.ToString()) && phone.ToString().Length == 10)
                {
                    targetRow["Phone"] = string.Format("({0}){1}-{2}", phone.ToString().Substring(0, 3), phone.ToString().Substring(3, 3), phone.ToString().Substring(6, 4));
                }

                // Registered
                targetRow["Registered"] = row["Registered"];

                // MembershipCatCode
                targetRow["MembershipCatCode"] = row["MembershipCatCode"];

                // NeaMembershipType
                targetRow["NeaMembershipType"] = row["NeaMembershipType"];

                // SeiuCurrentMember
                targetRow["SeiuCurrentMember"] = row["SeiuCurrentMember"];

                // SeaNumber
                targetRow["SeaNumber"] = row["SeaNumber"];

                // SeaName
                targetRow["SeaName"] = row["SeaName"];

                // WebUserId
                targetRow["WebUserId"] = row["WebUserId"];

                // SeiuLocalName
                targetRow["SeiuLocalName"] = row["SeiuLocalName"];

                // SeiuLocalNumber
                targetRow["SeiuLocalNumber"] = row["SeiuLocalNumber"];

                // EmailPermission
                targetRow["EmailPermission"] = row["EmailPermission"];
                // NewEnvInd
                targetRow["NewEnvInd"] = row["NewEnvInd"];
                // NewEnvInd
                targetRow["LeaName"] = row["LeaName"];
                // NewEnvInd
                targetRow["LeaNumber"] = row["LeaNumber"];
                // ComplifesignDate
                var datecomplife = row["ComplifesignDate"];

                if (datecomplife != null && !string.IsNullOrEmpty(datecomplife.ToString()) && datecomplife.ToString().Length == 8)
                {
                    targetRow["ComplifesignDate"] = string.Format("{0}/{1}/{2}", datecomplife.ToString().Substring(0, 2),
                        datecomplife.ToString().Substring(2, 2), datecomplife.ToString().Substring(4, 4));
                }
                // GenderCode
                targetRow["GenderCode"] = row["GenderCode"];
                // Introlifeenddate
                var dateintro = row["Introlifeenddate"];

                if (dateintro != null && !string.IsNullOrEmpty(dateintro.ToString()) &&
                    dateintro.ToString().Length == 8)
                {
                    targetRow["Introlifeenddate"] = row["Introlifeenddate"];
                }

                // Newmembersegmentindicator
                targetRow["Newmembersegmentindicator"] = row["Newmembersegmentindicator"];
                table.Rows.Add(targetRow);
			}

			args.ResultSet.Data.Dataset[args.ReportParameters.ViewName] = table;
		}
	}
}