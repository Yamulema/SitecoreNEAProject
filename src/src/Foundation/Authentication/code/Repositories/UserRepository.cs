using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using ServiceStack.Redis;
using Sitecore.Analytics;
using Sitecore.Configuration;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Web;
using SC = Sitecore;
using SitecoreAuthentication = Sitecore.Security.Authentication;

namespace Neambc.Seiumb.Foundation.Authentication.Repositories
{
    [Service(typeof(IUserRepository))]
	public class UserRepository : IUserRepository {
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly PooledRedisClientManager _connectionRedis;
        public UserRepository(ISeiumbProfileManager seiumbProfileManager) {
            _seiumbProfileManager = seiumbProfileManager;
            _connectionRedis = new PooledRedisClientManager(Settings.GetSetting("Seiumb.Redis.CacheConnection"));
        }

        public bool CreateVirtualUser(string username, SeiuProfile seiuProfile, LoginResponse response = null, bool duplicateRegistrationProcess = false) {
			if (SC.Context.User.IsAuthenticated) LogoutUser();
            var validatedUsername = string.IsNullOrEmpty(username) ? "NA" : username;
            var virtualUser = SitecoreAuthentication.AuthenticationManager.BuildVirtualUser(validatedUsername, true);
			virtualUser.Profile.Email = validatedUsername;
            virtualUser.Profile.Save();
            SitecoreAuthentication.AuthenticationManager.LoginVirtualUser(virtualUser);

            seiuProfile.MdsId = response != null ? response.Data.MdsIdAsString : string.Empty;
            seiuProfile.Email = !string.IsNullOrEmpty(username) ? username : string.Empty;
            seiuProfile.Status = duplicateRegistrationProcess ? UserStatusCons.HOT : UserStatusCons.COLD;

            if (response != null && !string.IsNullOrEmpty(response.Data.RegistrationDuplicateOldFormat)) //virtualUser.Profile.SetCustomProperty(UserDataCons.EMAIL_LIST, response.Registrations);
            {
                var userData = response.Data.RegistrationDuplicateOldFormat.Split(';')[0].Split('|');
                Log.Info($"CreateVirtualUser method email_list:{response.Data.RegistrationDuplicateOldFormat}", this);
                Log.Info($"CreateVirtualUser method duplicateRegistrationProcess:{duplicateRegistrationProcess}", this);
                seiuProfile.Emails = response.Data.RegistrationDuplicateOldFormat;
                seiuProfile.FirstName = userData[1];
                seiuProfile.LastName = userData[2];
            }
            _seiumbProfileManager.SaveProfile(seiuProfile);
            virtualUser.Profile.Save();
			SitecoreAuthentication.AuthenticationManager.LoginVirtualUser(virtualUser);
			return true;
		}

		public void FillUserData(Profile user, SeiuProfile seiuProfile, string mdsid, bool fromLogin, string registrations = null) {
			if (SC.Context.User.IsAuthenticated) LogoutUser();

            var validatedUsername = string.IsNullOrEmpty(user.Email) ? "NA" : user.Email;
            var virtualUser = SitecoreAuthentication.AuthenticationManager.BuildVirtualUser(validatedUsername, true);
			virtualUser.Profile.Email = validatedUsername;
            virtualUser.Profile.Save();
            SitecoreAuthentication.AuthenticationManager.LoginVirtualUser(virtualUser);

            //if (fromLogin)
            //{
            //    try 
            //    { 
            //        FillContactInformation(user, mdsid); 

            //    } catch (Exception e) 
            //    { 
            //        Log.Error("Error saving custom facets", e, this); 
            //    }
            //}

            virtualUser.Profile.Name = user.FirstName + " " + user.LastName;
            seiuProfile.MdsId = mdsid;
            seiuProfile.StreetAddress = string.IsNullOrEmpty(user.StreetAddress) ? string.Empty : user.StreetAddress;
            seiuProfile.FirstName = user.FirstName;
            seiuProfile.LastName = user.LastName;
            seiuProfile.DateOfBirth = user.DateOfBirth;
            seiuProfile.City = user.City;
            seiuProfile.StateCode = user.StateCode;
            seiuProfile.ZipCode = user.ZipCode;
            seiuProfile.Phone = user.Phone;
            seiuProfile.Registered = user.IsRegistered ? "Y" : string.Empty;
            seiuProfile.MembershipCategoryCode = user.MembershipCategoryCode;
            seiuProfile.MembershipType = user.NeaMembershipType;
            seiuProfile.SeiuCurrentMember = user.IsSeiuCurrentMember ? "Y" : string.Empty;
            seiuProfile.SeaNumber = user.SeaNumber;
            seiuProfile.SeaName = user.SeaName;
            seiuProfile.Webuserid = user.Webuserid;
            seiuProfile.SeiuLocalName = user.SeiuLocalName;
            seiuProfile.SeiuLocalNumber = user.SeiuLocalNumber;
            seiuProfile.EmailPermission = user.EmailPermissionIndicator;
            seiuProfile.Email = string.IsNullOrEmpty(user.Email) ? string.Empty : user.Email;
            seiuProfile.GenderCode = user.GenderCode;
            seiuProfile.IntroLifeEndDate = user.Introlifeenddate;
            seiuProfile.NewMemberSegmentIndicator = user.Newmembersegmentindicator;
            seiuProfile.NewEnvId = user.NewEnvInd;
            seiuProfile.LeaName = user.LeaName;
            seiuProfile.LeaNumber = user.LeaNumber;
            seiuProfile.CompLifeSignDate = user.ComplifesignDate;
            seiuProfile.Emails = string.IsNullOrEmpty(registrations) ? string.Empty : registrations;
            seiuProfile.Status = fromLogin ? UserStatusCons.HOT : (user.IsRegistered ? UserStatusCons.WARM_HOT : UserStatusCons.WARM_COLD);

            _seiumbProfileManager.SaveProfile(seiuProfile);
            virtualUser.Profile.Save();
			SitecoreAuthentication.AuthenticationManager.LoginVirtualUser(virtualUser);
		}

        private void FillContactInformation(Profile profile, string mdsid) {
            var globalConfiguration = (IGlobalConfigurationManager)ServiceLocator.ServiceProvider.GetService(typeof(IGlobalConfigurationManager));
            if (!globalConfiguration.EnableCustomFacets) return;
            var userName = SC.Context.User?.Name;
            //var contactIdentificationRepository = (IContactIdentificationRepository) ServiceLocator.ServiceProvider.GetService(typeof(IContactIdentificationRepository));
            //var resultIdentification = contactIdentificationRepository.IdentifyUser(userName);
            var userCustomFacetData = new UserCustomFacetData {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                City = profile.City,
                ComplifesignDate = profile.ComplifesignDate,
                DateOfBirth = profile.DateOfBirth,
                Email = profile.Email,
                EmailPermissionIndicator = profile.EmailPermissionIndicator,
                GenderCode = profile.GenderCode,
                IAId = profile.IAId,
                Introlifeenddate = profile.Introlifeenddate,
                Username = profile.Email,
                Mdsid = mdsid,
                MembershipCategoryCode = profile.MembershipCategoryCode,
                IsNeaCurrentMember = profile.IsNeaCurrentMember,
                NeaMembershipType = profile.NeaMembershipType,
                NewEnvInd = profile.NewEnvInd,
                Newmembersegmentindicator = profile.Newmembersegmentindicator,
                Phone = profile.Phone,
                IsRegistered = profile.IsRegistered,
                SeaName = profile.SeaName,
                SeaNumber = profile.SeaNumber,
                IsSeiuCurrentMember = profile.IsSeiuCurrentMember,
                SeiuLocalName = profile.SeiuLocalName,
                SeiuLocalNumber = profile.SeiuLocalNumber,
                StateCode = profile.StateCode,
                StreetAddress = profile.StreetAddress,
                UnionId = profile.UnionId,
                Webuserid = profile.Webuserid,
                ZipCode = profile.ZipCode,
                LeaName = profile.LeaName,
                LeaNumber = profile.LeaNumber
            };
            //if (!resultIdentification) return;
            var contactId = Tracker.Current.Contact.ContactId;
            //var t = Task.Run(() => contactIdentificationRepository.SaveCustomData(userCustomFacetData, contactId));
            //t.Wait();
            //contactIdentificationRepository.ReloadContact(contactId);
        }
		/// <summary>
		/// returns user status (warm, hot, cold)
		/// </summary>
		/// <returns></returns>
		public string GetUserStatus() {
            if (SC.Context.PageMode.IsExperienceEditor || SC.Context.PageMode.IsPreview || !SC.Context.User.IsAuthenticated) return UserStatusCons.COLD;
            var seiuProfile = _seiumbProfileManager.GetProfile();
            return !string.IsNullOrEmpty(seiuProfile.Status) ? seiuProfile.Status : UserStatusCons.COLD;
        }

		/// <summary>
		/// Logs out user
		/// </summary>
		public void LogoutUser() {
			_seiumbProfileManager.DeleteProfile();
			SitecoreAuthentication.AuthenticationManager.Logout();
			HttpContext.Current.Session.Remove(ConstantsSeiumb.DuplicateLogin);
		}
    }
}