using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Seiumb.Feature.Forms.GTM;
using Neambc.Seiumb.Feature.Forms.GTM.Model;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Project.Seiu.Model;
using Neambc.Seiumb.Project.Web;
using Sitecore.Links;
using Sitecore.Mvc.Controllers;
using System;
using System.Web.Mvc;
using static System.String;
using IBase64Service = Neambc.Neamb.Foundation.MBCData.Services.IBase64Service;

namespace Neambc.Seiumb.Project.Seiu.Controller
{
    public class HeaderController : SitecoreController
    {
        private readonly IBase64Service _base64Service;
        private readonly IUserRepository _userRepository;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IRetrieveUserManager _retrieveUserManager;

        public HeaderController(IBase64Service base64Service, IUserRepository userRepository, ISeiumbProfileManager seiumbProfileManager,
            IRetrieveUserManager retrieveUserManager)
        {
            _base64Service = base64Service;
            _userRepository = userRepository;
            _seiumbProfileManager = seiumbProfileManager;
            _retrieveUserManager = retrieveUserManager;
        }

        public ActionResult Header()
        {
            //logout parameter received from zip code verification page where it is required to logout the user
            var logoutParameter = Request.QueryString[ConstantsNeamb.LogoutParameter];
            if (!IsNullOrEmpty(logoutParameter)) _userRepository.LogoutUser();
            var seiuProfile = _seiumbProfileManager.GetProfile();
            var model = new Header
            {
                UserStatus = _userRepository.GetUserStatus(),
                SeaName = seiuProfile.SeaName,
                FirstName = seiuProfile.FirstName
            };
            //Verify if the user is cold and it is authenticated
            if (Sitecore.Context.User.IsAuthenticated && model.UserStatus.Equals(UserStatusCons.COLD) && !Sitecore.Context.PageMode.IsExperienceEditor && !Sitecore.Context.PageMode.IsPreview)
            {
                Request.Cookies.Remove(FormConstants.NEA_COOKIE_MDSID);
                _userRepository.LogoutUser();
            }

            model.SiteSettings = Sitecore.Context.Database.GetItem(Templates.SitecoreExtensions.SiteSettingsGlobal.ID);
            model.Mdsidqs = Empty;

            var refQuery = Request.QueryString[ConstantsSeiumb.Ref];
            if (!IsNullOrEmpty(refQuery)) {
                LogoutUser();
                model.Mdsidqs = refQuery;
            }

            var mdsidco = GetCookie(FormConstants.NEA_COOKIE_MDSID);
            var mdsid = model.Mdsidqs.Equals(Empty) ? _base64Service.Decode(mdsidco) : model.Mdsidqs.PadLeft(9, '0');
            var userMdsid = _seiumbProfileManager.GetProfile().MdsIdInt.ToString();
                //model.Mdsidqs.Equals(Empty) ? _base64Service.Decode(mdsidco) : model.Mdsidqs;
            //validate that what we got is an mdsid --- just so you know the site doesn't go down ;)
            if (_retrieveUserManager.TryRetrieveUserDataSeiumb(mdsid, out var userData) && userData != null && userData.IaId != Empty)
            {
                if (!IsNullOrEmpty(model.Mdsidqs) && !model.UserStatus.Equals(UserStatusCons.COLD))
                {
                    _userRepository.LogoutUser();
                    model.UserStatus = _userRepository.GetUserStatus();
                }
            }
            else
            {
                //could not retrieve data from this mdsid --- remove all data
                mdsidco = Empty;
                mdsid = Empty;
                model.Mdsidqs = Empty;  // invalid mdsid query string, discard it
            }
            var homeItem = Sitecore.Context.Database.GetItem(Templates.Home.ID);
            var profileItem = Sitecore.Context.Database.GetItem(Templates.ProfilePage.ID);
            model.Mdsidco = mdsidco;
            model.Mdsid = mdsid;
            model.UserMdsid = userMdsid == "0" ? "none" : userMdsid;
            model.HomeUrl = homeItem != null ? LinkManager.GetItemUrl(homeItem) : Empty;
            model.ProfileUrl = profileItem != null ? LinkManager.GetItemUrl(profileItem) : Empty;
            model.RegistrationLink = model.SiteSettings.Fields[Templates.SitecoreExtensions.SiteSettings.Fields.Registration];
            model.RegistrationLinkUrl = Empty;
            if (model.RegistrationLink != null && model.RegistrationLink.TargetItem != null)
                model.RegistrationLinkUrl = LinkManager.GetItemUrl(model.RegistrationLink.TargetItem);

            model.LoginLink = model.SiteSettings.Fields[Templates.SitecoreExtensions.SiteSettings.Fields.MobileLogin];
            model.LoginLinkUrl = model.LoginLink.TargetItem != null ? LinkManager.GetItemUrl(model.LoginLink.TargetItem) : Empty;
            model.Salutations = model.SiteSettings.Fields[Templates.SitecoreExtensions.SiteSettings.Fields.Welcome].Value.Split('|');
            model.FirstSalutation = model.Salutations.Length > 1 ? model.Salutations[0] : "";

            //campaing code
            var campaignCode = !IsNullOrEmpty(Request.QueryString[ConstantsSeiumb.UtmCampaign]) ? Request.QueryString[ConstantsSeiumb.UtmCampaign] : Empty;
            if (!IsNullOrEmpty(campaignCode)) {
                if (campaignCode.Length > 8) campaignCode = campaignCode.Substring(0, 8);
                Session[ConstantsSeiumb.CampaignCode] = campaignCode;
            }
            
            //Cell code
            var cellCode = !IsNullOrEmpty(Request.QueryString[ConstantsSeiumb.UtmSource]) ? Request.QueryString[ConstantsSeiumb.UtmSource] : Empty;
            if (!IsNullOrEmpty(cellCode)) {
                if (cellCode.Length > 8) cellCode = cellCode.Substring(0, 8);
                Session[ConstantsSeiumb.SourceCode] = cellCode;
            } 

            var UtmMedium = !IsNullOrEmpty(Request.QueryString[ConstantsSeiumb.UtmMedium]) ? Request.QueryString[ConstantsSeiumb.UtmMedium] : Empty;
            if (!IsNullOrEmpty(UtmMedium)) Session[ConstantsSeiumb.UtmMedium] = UtmMedium;

            var UtmTerm = !IsNullOrEmpty(Request.QueryString[ConstantsSeiumb.UtmTerm]) ? Request.QueryString[ConstantsSeiumb.UtmTerm] : Empty;
            if (!IsNullOrEmpty(UtmTerm)) Session[ConstantsSeiumb.UtmTerm] = UtmTerm;

            if (!model.UserStatus.Equals(UserStatusCons.HOT) && (!IsNullOrEmpty(model.Mdsid) || model.UserStatus.Equals(UserStatusCons.WARM_HOT) ||
                model.UserStatus.Equals(UserStatusCons.WARM_COLD)) && !Sitecore.Context.User.IsAuthenticated)
            {
                var user = _retrieveUserManager.RetrieveUserSeiumb(model.Mdsid);
                var profile = _retrieveUserManager.ToProfileModel(user);
                _userRepository.CreateVirtualUser(user.Email, seiuProfile);
                _userRepository.FillUserData(profile, seiuProfile, model.Mdsid, false);
                CookieStore.SetCookie(FormConstants.SEIUMBUsername, Empty, TimeSpan.FromDays(Convert.ToInt32(-7)));
                CookieStore.SetCookie(FormConstants.SEIUMBUsername, user.Email, TimeSpan.FromDays(Convert.ToInt32(7)));
            }

            if (!model.UserStatus.Equals(UserStatusCons.HOT) && (!IsNullOrEmpty(mdsidco) || !IsNullOrEmpty(model.Mdsidqs) ||
                model.UserStatus.Equals(UserStatusCons.WARM_HOT) || model.UserStatus.Equals(UserStatusCons.WARM_COLD)) && !IsNullOrEmpty(mdsid))
            {
                var user = _retrieveUserManager.RetrieveUserSeiumb(mdsid);
                var profile = _retrieveUserManager.ToProfileModel(user);
                _userRepository.CreateVirtualUser(user.Email, seiuProfile);
                _userRepository.FillUserData(profile, seiuProfile, mdsid, false);
                CookieStore.SetCookie(FormConstants.SEIUMBUsername, Empty, TimeSpan.FromDays(System.Convert.ToInt32(-7)));
                CookieStore.SetCookie(FormConstants.SEIUMBUsername, user.Email, TimeSpan.FromDays(System.Convert.ToInt32(7)));
            }

            model.UserStatus = _userRepository.GetUserStatus();
            model.SeaName = seiuProfile.SeaName;
            model.FirstName = seiuProfile.FirstName;
            if (model.Salutations.Length == 2) model.SecondSalutation = !IsNullOrEmpty(seiuProfile.SeaName) ? ", " + model.Salutations[1] : "";

            var gtmService = new GTMServiceSeiumb();
            model.OnClickEventContentLogin = gtmService.GetOnClickEvent(new AccountCore
            {
                Event = "account",
                AccountSection = "authentication",
                AccountAction = model.LoginLink.Text.ToLower()
            });
            model.OnClickEventContentRegistration = gtmService.GetOnClickEvent(new AccountCore
            {
                Event = "account",
                AccountSection = "authentication",
                AccountAction = model.RegistrationLink.Text.ToLower()
            });
            return View("/Views/Web/Renderings/Header.cshtml", model);
        }

        private string GetCookie(string cookieName)
        {
            var result = Request.Cookies[cookieName];
            return result != null ? result.Value : Empty;
        }

        private void LogoutUser() {
            CookieStore.SetCookie(FormConstants.NEA_COOKIE_MDSID, string.Empty, TimeSpan.FromDays(Convert.ToInt32(-7)));
            _userRepository.LogoutUser();
        }
    }
}