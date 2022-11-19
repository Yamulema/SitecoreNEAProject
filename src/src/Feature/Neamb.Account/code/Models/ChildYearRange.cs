using System;
using System.Collections.Generic;
using System.Linq;

namespace Neambc.Neamb.Feature.Account.Models {
	[Serializable]
	public class ChildYearRange : Dictionary<string, string> {

		public ChildYearRange(IDictionary<string, string> otherDict) : base(otherDict) { }

		public static ChildYearRange NormalRange() {
			return new ChildYearRange(Enumerable.Range(DateTime.Now.Year - 79, 80)
				.OrderByDescending(x => x)
				.ToDictionary(x => x.ToString(), x => x.ToString()));
		}
	}
}