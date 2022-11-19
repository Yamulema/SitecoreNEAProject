using System.Collections.Generic;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Account.Models {
	public class BeneficiariesDTO {

		#region Properties
		public Item Item {
			get; set;
		}
		public string Add {
			get; set;
		}
		public string AddCta {
			get; set;
		}
		public string Tooltip {
			get; set;
		}
		public List<Beneficiary> Beneficiaries {
			get; set;
		}
        public int TotalPayout { get; set; }

		public string SelectedBeneficiaryId {
			get; set;
		}
		public string Action {
			get; set;
		}
		public bool IsCtaEnabled {
			get; set;
		}
        public string OnClickEvent { get; set; }
		#endregion

		#region Constructors
		public BeneficiariesDTO() {
			Beneficiaries = new List<Beneficiary>();
		}
		#endregion

		#region Public Methods
		public static BeneficiariesDTO CreateFromItem(Item item) {
			return new BeneficiariesDTO() {
				Add = item.Fields[Templates.ComplementaryLifeInsurance.Fields.Add]?.Value,
				Tooltip = item.Fields[Templates.ComplementaryLifeInsurance.Fields.Beneficiaries_Tooltip]?.Value,
				SelectedBeneficiaryId = System.Guid.Empty.ToString(),
				Item = item,
				Beneficiaries = new List<Beneficiary> {
					Beneficiary.CreateSample()
				},
				AddCta = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.AddBeneficiary))
			};
		}
		#endregion

	}
}