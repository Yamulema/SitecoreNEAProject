using System;
using Neambc.Neamb.Foundation.Membership.Enums;

namespace Neambc.Neamb.Foundation.Membership.Model {
	[Serializable]
	public class Beneficiary {

		#region Properties
		public string Id {
			get; set;
		}
		public string OtherEntityName {
			get; set;
		}
		public string FirstName {
			get; set;
		}
		public string LastName {
			get; set;
		}
		public string MiddleInitial {
			get; set;
		}
		public string Email {
			get; set;
		}
		public string Relationship {
			get; set;
		}
		public string DisplayName {
			get; set;
		}
		public string DisplayRelationship {
			get; set;
		}
		public int Share {
			get; set;
		}
		public string DisplayShare {
			get; set;
		}
		public BeneficiaryType Type {
			get; set;
		}
        public string OnEditClickEvent { get; set; }
        public string OnRemoveClickEvent { get; set; }
        #endregion

        #region Public Methods
        public static Beneficiary CreateSample() {
			return new Beneficiary() {
				Id = Guid.NewGuid().ToString(),
				DisplayName = "John Millennial",
				DisplayRelationship = "Domestic Partner",
				DisplayShare = "100% Payout"
			};
		}
		#endregion

	}
}