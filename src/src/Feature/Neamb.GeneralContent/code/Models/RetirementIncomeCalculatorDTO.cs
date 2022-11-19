using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models {
	public class RetirementIncomeCalculatorDTO : IRenderingModel {
		private readonly ISessionAuthenticationManager _sessionManager;
		public Rendering Rendering {
			get; private set;
		}
		public Item Item {
			get; private set;
		}
		public bool IsHotOrWarm {
			get; private set;
		}
		public string MdsId {
			get; private set;
		}
		public string IFrame {
			get; private set;
		}
		public RetirementIncomeCalculatorDTO(ISessionAuthenticationManager sessionAuthenticationManager) {
			_sessionManager = sessionAuthenticationManager;
		}
		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			IsHotOrWarm = GetIsHotOrWarm();
			MdsId = GetMdsid();
			IFrame = Item[Templates.RetirementIncomeCalculator.Fields.IFrame];
		}
		private string GetMdsid() {
			var accountMembership = _sessionManager.GetAccountMembership();
			if (IsHotOrWarm) {
				return accountMembership.Mdsid.PadLeft(9, '0');
			}
			return string.Empty;
		}

		private bool GetIsHotOrWarm() {
			var isHotOrWarm = false;
			var accountMembership = _sessionManager.GetAccountMembership();
			if (accountMembership.IsUserWithKnownState()) {
				isHotOrWarm = true;
			}
			return isHotOrWarm;
		}
	}
}