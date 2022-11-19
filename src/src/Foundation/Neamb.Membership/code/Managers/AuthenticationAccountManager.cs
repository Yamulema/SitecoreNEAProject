using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken;
using Neambc.Neamb.Foundation.MBCData.Services.Login;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Diagnostics;
using Sitecore.Links;
using static System.String;
using Convert = System.Convert;
using SC = Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
    [Service(typeof(IAuthenticationAccountManager))]
    public class AuthenticationAccountManager : IAuthenticationAccountManager
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly ICreateResetTokenService _createResetTokenService;
        private readonly ISessionManager _sessionManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IExactTargetClient _exactTargetClient;
        private readonly IBase64Service _base64Service;
        private readonly SC.IAuthenticationManager _authenticationManager;
        private readonly IUrlManager _urlManager;
        //private readonly IContactIdentificationRepository _contactIdentificationRepository;
        private readonly ILoginUserService _loginUserService;
        private readonly IRetrieveUserManager _retrieveUserManager;
        private readonly IIceTravelDollarsManager _iceTravelDollarsManager;
        private readonly ICookieManager _cookieManager;

        public AuthenticationAccountManager(
            ISessionAuthenticationManager sessionAuthenticationManager,
            ICreateResetTokenService createResetTokenService,
            ISessionManager sessionManager,
            IGlobalConfigurationManager globalConfigurationManager,
            IExactTargetClient exactTargetClient,
            IBase64Service base64Service,
            SC.IAuthenticationManager authenticationManager,
            IUrlManager urlManager,
            //IContactIdentificationRepository contactIdentificationRepository,
            ILoginUserService loginUserService,
            IRetrieveUserManager retrieveUserManager,
            IIceTravelDollarsManager iceTravelDollarsManager,
            ICookieManager cookieManager
        )
        {
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _createResetTokenService = createResetTokenService;
            _sessionManager = sessionManager;
            _globalConfigurationManager = globalConfigurationManager;
            _exactTargetClient = exactTargetClient;
            _base64Service = base64Service;
            _authenticationManager = authenticationManager;
            _urlManager = urlManager;
            //_contactIdentificationRepository = contactIdentificationRepository;
            _loginUserService = loginUserService;
            _retrieveUserManager = retrieveUserManager;
            _iceTravelDollarsManager = iceTravelDollarsManager;
            _cookieManager = cookieManager;
        }

        /// <summary>
        /// Authenticate account against the webservices provided by the client
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">password</param>
        /// <param name="account">Account with the information retrieved from the webservices</param>
        /// <param name="cellcode"></param>
        /// <returns>Response of the webservice ValidateUsernameAndPassword</returns>
        public LoginResponse AuthenticateAccount(string username, string password, AccountMembership account, string cellcode)
        {
            var resultLoginRestExecution = _loginUserService.LoginUser(username, password, Convert.ToInt32(_globalConfigurationManager.Unionid), cellcode, _globalConfigurationManager.NeambcKeyMatchRoutineIdentifier);
            if (resultLoginRestExecution.Success && resultLoginRestExecution.Data != null && resultLoginRestExecution.Data.LoggedIn)
                RetrieveAccount(account, resultLoginRestExecution.Data.MdsIdAsString);
            return resultLoginRestExecution;
        }

        public void RetrieveAccount(AccountMembership account, string mdsid)
        {
            var userResponse = _retrieveUserManager.RetrieveUserNeamb(mdsid);
            if (userResponse == null) return;
            var profile = _retrieveUserManager.ToProfileModel(userResponse);
            profile.MembershipType = EnumExtensions.FromDescription<MembershipType>(userResponse.NeaMembershipTypeName);

            account.Username = userResponse.Email;
            account.Mdsid = mdsid.PadLeft(9, '0');
            //Verify flag to see if the user is registered or not
            account.Status = userResponse.Registered ? StatusEnum.Hot : StatusEnum.WarmCold;
            account.Profile = profile;
        }

        /// <summary>
        /// Clear the user from the session and cookies
        /// </summary>
        public void LogoutUser(bool removeTracking = true)
        {
            var headerData = _sessionAuthenticationManager.GetAccountMembership();
            if (headerData != null && !IsNullOrEmpty(headerData.Mdsid))
            {
                var keyEligibilityResult = $"{ConstantsNeamb.EligibilityCompIntroLife}{headerData.Mdsid}";
                _sessionManager.Remove(keyEligibilityResult);
                RemoveSessionOffersMenu(headerData.Mdsid);
            }
            _sessionAuthenticationManager.RemoveAccountMembership();
            _sessionAuthenticationManager.RemoveAccountMembershipDraft();
            _sessionAuthenticationManager.RemoveAttemptZipCodeValidation();
            _sessionAuthenticationManager.RemoveZipCodeValidationSuccess();
            _sessionAuthenticationManager.RemoveRequestedPageLogin();
            _sessionAuthenticationManager.GetRequestedPageRegister();
            if (removeTracking)
            {
                _sessionAuthenticationManager.RemoveCellCode();
                _sessionAuthenticationManager.RemoveCampaignCode();
                _sessionAuthenticationManager.RemoveMedium();
            }
            _sessionManager.Remove(ConstantsNeamb.CtaActionType);
            _sessionManager.Remove(ConstantsNeamb.CtaActionPrimary);
            _sessionManager.Remove(ConstantsNeamb.CtaActionPrimaryOnclick);
            _sessionManager.Remove(ConstantsNeamb.CtaActionPrimaryTargetBlank);
            _sessionManager.Remove(ConstantsNeamb.CtaActionSecondary);
            _sessionManager.Remove(ConstantsNeamb.CtaActionSecondaryOnclick);
            _sessionManager.Remove(ConstantsNeamb.CtaActionSecondaryTargetBlank);
            _cookieManager.RemoveCookie(ConstantsNeamb.IceTravelDollarCookie);
            _cookieManager.RemoveCookie(ConstantsNeamb.IceTravelDollarCookieUser);
            _authenticationManager.Logout();
        }

        public void RemoveSessionOffersMenu(string mdsid)
        {
            var keyOffersList = $"{ConstantsNeamb.OfferHeaderListIds}{mdsid}";
            List<string> offerIdList = _sessionManager.RetrieveFromSession<List<string>>(keyOffersList);
            if (offerIdList != null)
            {
                foreach (var offerId in offerIdList)
                {
                    var keyOffersHeader = $"{ConstantsNeamb.OfferHeader}{mdsid}{offerId}";
                    _sessionManager.Remove(keyOffersHeader);
                }
            }
            _sessionManager.Remove(keyOffersList);
        }

        public void SaveCustomFacets(AccountMembership account)
        {

            if (!_globalConfigurationManager.EnableCustomFacets)
            {
                return;
            }

            if (account.Profile == null)
            {
                return;
            }

            //var resultIdentification = _contactIdentificationRepository.IdentifyUser(userName);
            var userName = Context.User?.Name;

            var userCustomFacetData = new UserCustomFacetData
            {
                City = account.Profile.City,
                ComplifesignDate = account.Profile.ComplifesignDate,
                DateOfBirth = account.Profile.DateOfBirth,
                Email = account.Profile.Email,
                EmailPermissionIndicator = account.Profile.EmailPermissionIndicator,
                FirstName = account.Profile.FirstName,
                GenderCode = account.Profile.GenderCode,
                IAId = account.Profile.IAId,
                Introlifeenddate = account.Profile.Introlifeenddate,
                LastName = account.Profile.LastName,
                Username = account.Username,
                Mdsid = account.Mdsid,
                MembershipCategoryCode = account.Profile.MembershipCategoryCode,
                IsNeaCurrentMember = account.Profile.IsNeaCurrentMember,
                NeaMembershipType = account.Profile.NeaMembershipType,
                NewEnvInd = account.Profile.NewEnvInd,
                Newmembersegmentindicator = account.Profile.Newmembersegmentindicator,
                Phone = account.Profile.Phone,
                IsRegistered = account.Profile.IsRegistered,
                SeaName = account.Profile.SeaName,
                SeaNumber = account.Profile.SeaNumber,
                IsSeiuCurrentMember = account.Profile.IsSeiuCurrentMember,
                SeiuLocalName = account.Profile.SeiuLocalName,
                SeiuLocalNumber = account.Profile.SeiuLocalNumber,
                StateCode = account.Profile.StateCode,
                StreetAddress = account.Profile.StreetAddress,
                UnionId = account.Profile.UnionId,
                Webuserid = account.Profile.Webuserid,
                ZipCode = account.Profile.ZipCode,
                LeaName = account.Profile.LeaName,
                LeaNumber = account.Profile.LeaNumber
            };
            //if (!resultIdentification) return;
            Log.Info("STARTING SAVE CUSTOM FACET DATA", this);
            var contactId = Tracker.Current.Contact.ContactId;
            //var t = Task.Run(() => _contactIdentificationRepository.SaveCustomData(userCustomFacetData, contactId));
            //t.Wait();
            //_contactIdentificationRepository.ReloadContact(contactId);
        }
        /// <summary>
        /// Authenticate the user in Sitecore
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns></returns>
        public void LoginSitecoreContext(string userName)
        {
            var user = _authenticationManager.BuildVirtualUser(userName, true);
            user.Profile.Email = userName;
            user.Profile.Save();
            _authenticationManager.LoginVirtualUser(user);
        }

        public void ProcessErrorAuthentication(LoginResponse response, AccountMembership account, string username, string pathReset)
        {
            if (response.Error != null)
            {
                if (Convert.ToInt32(response.Error.Code) == (int)AuthenticationStatusEnum.AcccountLockedRestService)
                {
                    var serviceResponse = _createResetTokenService.CreateResetToken(username, Convert.ToInt32(_globalConfigurationManager.Unionid));
                    if (serviceResponse == null || !serviceResponse.Success || serviceResponse.Data == null) return;
                    if (serviceResponse.Data.NewToken)
                    {
                        SendExactTargetResetEmail(
                            new ExactTargetResetEmail
                            {
                                UserName = username,
                                FirstName = serviceResponse.Data.FirstName,
                                Token = serviceResponse.Data.ResetToken,
                                ResetPath = pathReset,
                                CancelPath = Empty,
                                ExpiresAt = serviceResponse.Data.ExpiresAt,
                                ResetPasswordEnum = ResetPasswordEnum.Locked
                            });
                        account.Status = StatusEnum.LockedNewToken;
                    }
                    else
                    {
                        account.Status = StatusEnum.LockedOldToken;
                    }
                }
                else
                {
                    account.Status = StatusEnum.Cold;
                }
            }
            else
            {
                Log.Error($"Error in ProcessErrorAuthentication {username} {response.ErrorCodeResponse}", this);
                account.Status = StatusEnum.Cold;
            }

        }

        /// <summary>
        /// Send the email when the account is recent locked
        /// </summary>
        /// <param name="model">Exact Target Reset Email Model</param>
        /// <returns>Response from Exact target</returns>
        public ResultSendResetEmail SendExactTargetResetEmail(ExactTargetResetEmail model)
        {
            //Check user name is not empty
            if (IsNullOrEmpty(model.UserName)) throw new ArgumentException("Error username is empty in Exact target in Reset Email", model.UserName);
            
            //Check the path reset is not empty and it is a valid url
            if (IsNullOrEmpty(model.ResetPath)) throw new ArgumentException("Error pathReset is empty in Exact target in Reset Email", model.ResetPath);
            if (!_urlManager.IsValidUrl(model.ResetPath)) throw new ArgumentException("Error pathReset is invalid in Exact target in Reset Email", model.ResetPath);

            //Check Expires At is not empty
            if (IsNullOrWhiteSpace(model.ExpiresAt)) throw new ArgumentException("Error ExpiresAt is empty in Exact target in Reset Email", model.ExpiresAt);

            //Get the token from the webservice
            var resultSendResetEmail = new ResultSendResetEmail();
            var cellcodeOne = Empty;

            if (model.ResetPasswordEnum == ResetPasswordEnum.Locked)
            {
                cellcodeOne = _globalConfigurationManager.CellCodeResetPasswordLockedOut;
            }
            else
            {
                cellcodeOne = (model.NewToken ?? true) ? _globalConfigurationManager.CellCodeResetPasswordRequestedReset: _globalConfigurationManager.CellCodeResetPasswordRequestedResetOldToken;

                //Check the path cancel is not empty and it is a valid url
                if (IsNullOrEmpty(model.CancelPath)) throw new ArgumentException("Error pathCancel is empty in Exact target in Reset Email", model.CancelPath);
                if (!_urlManager.IsValidUrl(model.CancelPath)) throw new ArgumentException("Error pathCancel is invalid in Exact target in Reset Email", model.CancelPath);
            }

            var campaignCd = _globalConfigurationManager.CampaignResetPassword;
            var encodedUsername = _base64Service.Encode(model.UserName);

            //Build the full path of the reset page				
            var resetUrl = $"{model.ResetPath}?id={encodedUsername}&s={HttpUtility.UrlEncode(model.Token)}";
            Log.Debug("RESET URL " + resetUrl, this);
            var cancelUrl = $"{model.CancelPath}?id={encodedUsername}";
            Log.Debug("CANCEL URL " + cancelUrl, this);
            var customerDefinition = _globalConfigurationManager.CustomerDefinitionResetPassword;
            var parameters = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("FIRST_NAME", model.FirstName),
                new KeyValuePair<string, string>("RESET_URL", resetUrl),
                new KeyValuePair<string, string>("CANCEL_URL", cancelUrl),
                new KeyValuePair<string, string>("CELL_CODE", cellcodeOne),
                new KeyValuePair<string, string>("CAMPAIGN_CD", campaignCd),
                new KeyValuePair<string, string>("TOKEN_EXPIRY_TIME", model.ExpiresAt)
            };
            resultSendResetEmail.ResultExactTarget = _exactTargetClient.SendExactTargetService(customerDefinition, model.UserName, parameters);
            return resultSendResetEmail;
        }
        public void InitializeAccountMemberData(AccountMembership accountMembership)
        {
            _sessionAuthenticationManager.SaveAccountMembership(accountMembership);
        }

        /// <summary>
        /// Verify if the Ice Travell Dollar cookie exists. If not, the value is requested and then the cookie is created
        /// </summary>
        /// <param name="mdsid">User Id</param>
        public void IceTravelDollarCookie(string mdsid)
        {
            if (!ValidateIceTravelDollarCookieUser(mdsid) && _cookieManager.HasCookie(ConstantsNeamb.IceTravelDollarCookie))
            {
                return;
            }

            var balanceTravelDollars = _iceTravelDollarsManager.GetBalance(mdsid);
            if (balanceTravelDollars > 0)
            {
                _cookieManager.SaveCookie(ConstantsNeamb.IceTravelDollarCookie, balanceTravelDollars.ToString(), null, true);
            }
            else
            {
                _cookieManager.RemoveCookie(ConstantsNeamb.IceTravelDollarCookie);
            }


            _cookieManager.SaveCookie(ConstantsNeamb.IceTravelDollarCookieUser, mdsid, null, true);
        }

        public void RemoveIceTravellDollarCookie()
        {
            _cookieManager.RemoveCookie(ConstantsNeamb.IceTravelDollarCookie);
            _cookieManager.RemoveCookie(ConstantsNeamb.IceTravelDollarCookieUser);
        }

        private bool ValidateIceTravelDollarCookieUser(string mdsid)
        {
            var cookieValue = _cookieManager.GetCookie(ConstantsNeamb.IceTravelDollarCookieUser);

            if (!mdsid.Equals(cookieValue))
            {
                _cookieManager.RemoveCookie(ConstantsNeamb.IceTravelDollarCookieUser);
                return true;
            }
            
            return false;
        }
        public bool IsValidRedirection(string currentpath)
        {
            var duplicationPage =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID));

            var registrationPage =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.RegistrationPage.ID));

            var zipcodeVerification =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ZipCodeVerificationPage.ID));

            var loginpage =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));

            var forgotPage =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ForgotPasswordPage.ID));
            var resetPage =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ResetPage.ID));
            var resetDisavowPage =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ResetPageDisavow.ID));
            var forgotEmailPage =
                LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.ForgotEmailPage.ID));

            if (currentpath.Equals(forgotPage, StringComparison.InvariantCultureIgnoreCase) ||
                currentpath.Equals(resetPage, StringComparison.InvariantCultureIgnoreCase) ||
                currentpath.Equals(resetDisavowPage, StringComparison.InvariantCultureIgnoreCase) ||
                currentpath.Equals(duplicationPage, StringComparison.InvariantCultureIgnoreCase) ||
                currentpath.Equals(registrationPage, StringComparison.InvariantCultureIgnoreCase) ||
                currentpath.Equals(zipcodeVerification, StringComparison.InvariantCultureIgnoreCase) ||
                currentpath.Equals(loginpage, StringComparison.InvariantCultureIgnoreCase) ||
                currentpath.Equals(forgotEmailPage, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}