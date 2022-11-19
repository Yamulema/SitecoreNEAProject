using System;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models {
	public class ComplimentaryLifeDTO : IRenderingModel {

		#region Properties
		public StatusEnum AccountStatus {
			get; set;
		}
		public string AnonymousUser {
			get; set;
		}
		public BeneficiariesDTO Beneficiaries {
			get; set;
		}
		public ErrorStatusEnum ProfileStatus {
			get; set;
		}
		public ErrorStatusEnum PayoutStatus {
			get; set;
		}
		public ErrorStatusEnum ErrorStatus {
			get; set;
		}
		public bool WasSaved {
			get; set;
		}
		public int PayoutTotal {
			get; set;
		}
		public EditingStatus EditingStatus {
			get; set;
		}
		public string Subhead {
			get; set;
		}

		public Rendering Rendering {
			get; set;
		}
		public Item Item {
			get; set;
		}
		public Item PageItem {
			get; set;
		}
		public MembershipType MembershipType {
			get; set;
		}
		public string PathComplimentary {
			get; set;
		}
		public EligibilityResultEnum EligibilityResult {
			get; set;
		}
		public string PhoneNumber {
			get; set;
		}
		public string SupportEmail {
			get; set;
		}
        public string OnClickEvent { get; set; }
        public bool IsWizardStep { get; set; }
		public string GtmAction
		{
			get; set;
		}
		#endregion

		#region Constructors
		public ComplimentaryLifeDTO() {

		}
		public ComplimentaryLifeDTO(Rendering rendering, bool absorbItem = false) {
			Initialize(rendering);
			if (absorbItem) {
				AbsorbItem(rendering.Item);
			}
		}
		#endregion

		#region Public Methods
		public void AbsorbItem(Item item) {
		}
		public void Initialize(Rendering rendering) {
			rendering = rendering ?? throw new ArgumentNullException(nameof(rendering));
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			EligibilityResult = EligibilityResultEnum.NotEligible;
		}
		#endregion
	}
}