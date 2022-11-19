using System;

namespace Neambc.Neamb.Foundation.Membership.Model {
	public class Reward {
		public int Mdsid {
			get; set;
		}
		public string Name {
			get; set;
		}
		public DateTime Date {
			get; set;
		}
		public string Description {
			get; set;
		}
		public int Value {
			get; set;
		}
	}
}