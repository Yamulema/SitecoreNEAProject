using System;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Project.Web.Repositories
{
	[Service(typeof(IWebRepository))]
	public class WebRepository : IWebRepository
	{
		private readonly ICookieManager _cookieManager;
		private readonly ISessionAuthenticationManager _sessionManager;
		private readonly IAuthenticationAccountManager _authenticationAccountManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IRetrieveUserManager _retrieveUserManager;
        private readonly IBase64Service _base64Service;
		
		public WebRepository(
			ICookieManager cookieManager,
			ISessionAuthenticationManager sessionManager,
			IAuthenticationAccountManager authenticationAccountManager,
            IGlobalConfigurationManager globalConfigurationManager,
            IRetrieveUserManager retrieveUserManager,
            IBase64Service base64Service)
		{
			_cookieManager = cookieManager;
			_sessionManager = sessionManager;
			_authenticationAccountManager = authenticationAccountManager;
            _globalConfigurationManager = globalConfigurationManager;
            _retrieveUserManager = retrieveUserManager;
            _base64Service = base64Service;
		}

		/// <summary>
		/// Process the value sent in ref parameter to retrieve the information for this mdsid and save the account user in session
		/// </summary>
		/// <param name="mdsid">Value sent in ref parameter</param>
		public void SetWarmStatus(string mdsid)
		{
			try {
                var userResponse = _retrieveUserManager.RetrieveUserNeamb(mdsid);
                var account = new AccountMembership();

				if (userResponse != null)
				{
                    var profile = _retrieveUserManager.ToProfileModel(userResponse);
                    profile.MembershipType = EnumExtensions.FromDescription<MembershipType>(userResponse.NeaMembershipTypeName);

                    account.Username = userResponse.Email;
					account.Mdsid = mdsid.PadLeft(9, '0');
                    account.Status = userResponse.Registered ? StatusEnum.WarmHot : StatusEnum.WarmCold;

					account.Profile = profile;
					//Save the cookie warm user
					_cookieManager.SaveWarmUser(_base64Service.Encode(account.Mdsid),_globalConfigurationManager.TimeWarmCookie);

					_authenticationAccountManager.InitializeAccountMemberData(account);
					_authenticationAccountManager.LoginSitecoreContext(account.Username);
                }
			}
			catch (Exception ex)
			{
				Sitecore.Diagnostics.Log.Info($"Ref {mdsid} id is not recognized or not processed", this);
				Sitecore.Diagnostics.Log.Warn(ex.Message, ex, this);
			}
		}

		/// <summary>
		/// Verify if the session has expired but exist a cookie of the latest user logged
		/// </summary>
		public void ProcessCookieWarm()
		{
			var accountMembership = _sessionManager.GetAccountMembership();
			if (accountMembership.Status == StatusEnum.Unknown)
			{
				//Get the value of the Cookie "neambwarmuser"
				var cookieWarm = _cookieManager.GetWarmUser();
				if (!string.IsNullOrEmpty(cookieWarm))
				{
					var mdsid = _base64Service.Decode(cookieWarm);
					SetWarmStatus(mdsid);
				}
			}
		}

		/// <summary>
		/// Get the logo of the state of the user
		/// </summary>
		/// <param name="seaNumber">User data</param>
		/// <returns>name of the logo</returns>
		public string GetLogoImage(string seaNumber)
		{
			return Sitecore.Configuration.Settings.GetSetting(string.Format("{0}{1}", ConstantsNeamb.StateLogoNumber,
				seaNumber));
		}
	}
}