using System.Collections.Generic;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Pipelines;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models {
	public class SecuredBodyDTO : IRenderingModel {
		public Rendering Rendering { get; private set; }
		public Item Item { get; private set; }
		private readonly ISessionAuthenticationManager _sessionManager;
		public string BodyBackgroundColorClass { get; private set; }
		public bool DisplayComponent { get; private set; }
		public bool IsSecuredBodyEmpty { get; private set; }
		public bool IsUserWarmOrHot { get; private set; }
		public Dictionary<string, string> Tokens { get; set; }
        public IStringProcessor StringProcessor { get; set; }

        public SecuredBodyDTO(ISessionAuthenticationManager sessionAuthenticationManager) {
			_sessionManager = sessionAuthenticationManager;
		}

		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			var body = Item[Templates.PageBody.Fields.PageBodyBody];
			var securedBody = Item[Templates.SecuredContent.Fields.SecuredBody];
			DisplayComponent = GetDisplayComponent(body, securedBody);
			IsSecuredBodyEmpty = string.IsNullOrEmpty(securedBody);
			IsUserWarmOrHot = GetIsUserWarmOrHot();
			BodyBackgroundColorClass = GetBackgroundColorClass();
			Tokens = GetTokens();
		}

		private Dictionary<string, string> GetTokens() {
			var result = new Dictionary<string, string>();

			// Adds MdsidToken
			var accountMembership = _sessionManager.GetAccountMembership();
			if (accountMembership.Status == StatusEnum.Hot || accountMembership.Status == StatusEnum.WarmHot || accountMembership.Status == StatusEnum.WarmCold) {
				result.Add(Configuration.MdsidToken, accountMembership.Mdsid);
				result.Add(Configuration.FirstNameToken, accountMembership.Profile.FirstName);
				result.Add(Configuration.LastNameToken, accountMembership.Profile.LastName);
			}

			return result;
		}

		private string GetBackgroundColorClass() {
			var backgroundColor = Item[Templates.PageBody.Fields.PageBodyBodyBackgroundColor];
			var backgroundColorClass = ConstantsNeamb.WhiteBackgroundColorClass;
			if (backgroundColor == ConstantsNeamb.GrayBackgroundColor) {
				backgroundColorClass = ConstantsNeamb.GrayBackgroundColorClass;
			} else if (backgroundColor == ConstantsNeamb.BlueBackgroundColor) {
				backgroundColorClass = ConstantsNeamb.BlueBackgroundColorClass;
			}
			return backgroundColorClass;
		}
		private bool GetIsUserWarmOrHot() {
			var isUserWarmOrHot = false;
			var accountMembership = _sessionManager.GetAccountMembership();
			if (accountMembership.Status == StatusEnum.Hot || accountMembership.Status == StatusEnum.WarmHot || accountMembership.Status == StatusEnum.WarmCold) {
				isUserWarmOrHot = true;
			}
			return isUserWarmOrHot;
		}

		private bool GetDisplayComponent(string body, string securedBody) {
			return (!string.IsNullOrEmpty(body) || !string.IsNullOrEmpty(securedBody));
		}
	}
}