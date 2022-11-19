using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Db;

namespace Neambc.Neamb.Foundation.MBCData.Model {
	public class ViewUnredeemedRewards {

		#region Properties
		public int IndvId {
			get; set;
		}
		public string RewardsNm {
			get; set;
		}
		public DateTime DateAwarded {
			get; set;
		}
		public string UserRewardsDesc {
			get; set;
		}
		public int AwardedVal {
			get; set;
		}

		public const string INDV_ID = "INDV_ID";
		public const string REWARDS_NM = "REWARDS_NM";
		public const string DATE_AWARDED = "DATE_AWARDED";
		public const string USER_REWARDS_DESC = "USER_REWARDS_DESC";
		public const string AWARDED_VAL = "AWARDED_VAL";
		public const string DATE_AWARDED_FORMAT = "yyyy/MM/dd";
		#endregion

		#region Constructor
		public ViewUnredeemedRewards() {

		}
		public ViewUnredeemedRewards(IReadOnlyList<object> record, IReadOnlyList<string> columnNames) {
			if (record.Count == columnNames.Count) {
				for (var i = 0; i < record.Count; i++) {
					var field = record[i];
					var parameter = columnNames[i];
					switch (parameter.ToUpper()) {
						case INDV_ID:
							IndvId = int.TryParse(field.ToString().Trim(), out var indvid) ? indvid : 0;
							break;
						case REWARDS_NM:
							RewardsNm = (field as string)?.Trim();
							break;
						case DATE_AWARDED:
							var parsed = DateTime.TryParseExact(
								field.ToString().Trim(),
								new[] { DATE_AWARDED_FORMAT },
								DateTimeFormatInfo.InvariantInfo,
								DateTimeStyles.None, out var dateAwarded);
							DateAwarded = parsed ? dateAwarded : DateTime.MinValue;
							break;
						case USER_REWARDS_DESC:
							UserRewardsDesc = (field as string)?.Trim();
							break;
						case AWARDED_VAL:
							AwardedVal = int.TryParse(field.ToString().Trim(), out var awardedVal)
								? ((awardedVal < 0) ? 0 : awardedVal)
								: 0;
							break;
					}
				}
			}
		}
		#endregion

		#region Public Methods

		public static IEnumerable<ViewUnredeemedRewards> DigestCommandResult(ICommandResult result) {
			IEnumerable<ViewUnredeemedRewards> ret = null;
			if ((null != result) && !result.HasError) {
				ret = result.Records
					.Select(x => new ViewUnredeemedRewards(x, result.ColumnNames))
					.ToList();
			}
			return ret;
		}

		#endregion
	}
}