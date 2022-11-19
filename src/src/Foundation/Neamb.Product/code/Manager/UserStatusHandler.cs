using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Sitecore.Links;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(IUserStatusHandler))]
	public class UserStatusHandler : IUserStatusHandler {
		#region Fields
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		#endregion

		#region Constructors
		public UserStatusHandler(ISessionAuthenticationManager sessionAuthenticationManager) {
			_sessionAuthenticationManager = sessionAuthenticationManager;
		}
		#endregion

		#region Public Methods
		public string GetUrlUserNotAuthenticated() {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			var urlReturn = "";
			if (accountMembership != null && accountMembership.Status != StatusEnum.Hot) {
				urlReturn = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));
			}

			return urlReturn;
		}
		#endregion
	}
}