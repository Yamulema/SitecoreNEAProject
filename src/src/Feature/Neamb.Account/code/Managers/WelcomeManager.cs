using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.GeneralContent.Enums;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Enums;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;

namespace Neambc.Neamb.Feature.Account.Managers
{
    [Service(typeof(IWelcomeManager))]
    public class WelcomeManager : IWelcomeManager
    {

        #region Fields
        private const string JwpPrefix = "https://content.jwplatform.com/players/";
        private readonly ISessionAuthenticationManager _sessionManager;
        private readonly IOracleDatabase _oracleManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IRetrieveUserManager _retrieveUserManager;
        #endregion

        #region Constructor
        public WelcomeManager(ISessionAuthenticationManager sessionManager,
            IOracleDatabase oracleManager,
            IAuthenticationAccountManager authenticationAccountManager,
            IGlobalConfigurationManager globalConfigurationManager,
            IAuthenticationManager authenticationManager,
            IRetrieveUserManager retrieveUserManager)
        {
            _sessionManager = sessionManager;
            _oracleManager = oracleManager;
            _authenticationAccountManager = authenticationAccountManager;
            _globalConfigurationManager = globalConfigurationManager;
            _authenticationManager = authenticationManager;
            _retrieveUserManager = retrieveUserManager;
        }
        #endregion

        #region Member Welcome
        public MemberWelcome MemberWelcomeModel(Item datasource)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            var model = new MemberWelcome(datasource)
            {
                SearchMessage = GetSearechMessage(datasource, accountMembership),
                Status = GetStatus(accountMembership),
                Imsid = GetImsid(accountMembership),
                VideoType = GetVideoType(datasource),
                NotYouLinkUrl = GetNotYouUrl(),
                SupportEmail = GetSupportEmail(),
                SocialShare = new SocialShareModel(datasource)
            };
            return model;
        }

        public MemberWelcome MemberWelcomeRegister(MemberWelcome memberWelcome)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            var status = GetStatus(accountMembership);
            if (status == WelcomeStatus.None)
            {
                if (!string.IsNullOrEmpty(memberWelcome.Imsid?.Trim()))
                {
                    var mdsid = GetMdsid(memberWelcome.Imsid);
                    if (!string.IsNullOrEmpty(mdsid))
                    {
                        var userResponse = _retrieveUserManager.RetrieveUserNeamb(mdsid);
                        if (userResponse != null)
                        {
                            memberWelcome.Status = WelcomeStatus.NewMember;
                            memberWelcome.Mdsid = mdsid;
                            return memberWelcome;
                        }
                    }

                }
            }
            else
            {
                memberWelcome.Status = status;
                memberWelcome.Mdsid = accountMembership.Mdsid;
                return memberWelcome;
            }

            memberWelcome.Status = WelcomeStatus.None;
            memberWelcome.ErrorStatus |= WelcomeErrorStatus.InvalidCode;
            memberWelcome.VideoType = GetVideoType(memberWelcome.Item);
            memberWelcome.SocialShare = new SocialShareModel(memberWelcome.Item);
            memberWelcome.SearchMessage = GetSearechMessage(memberWelcome.Item, accountMembership);
            memberWelcome.NotYouLinkUrl = GetNotYouUrl();
            memberWelcome.SupportEmail = GetSupportEmail();
            return memberWelcome;
        }
        #endregion

        #region Member Verification
        public MemberVerification MemberVerificationModel(Item datasource)
        {
            var accountMembership = _sessionManager.GetAccountMembership();

            //Initialize model
            var model = new MemberVerification(datasource);
            InitializeMemberVerification(accountMembership, datasource, ref model);
            return model;
        }
        public void VerifyZip(ref MemberVerification model, ModelStateDictionary modelState)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            var datasource = model.Item;

            //Initialize model
            InitializeMemberVerification(accountMembership, datasource, ref model);

            //Validate model
            ValidateModel(model, modelState);

            //Process
            VerifyZip(accountMembership, ref model);
        }
        public AuthenticationResultEnum VerifyPassword(ref MemberVerification model, ViewDataDictionary viewData)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            var datasource = model.Item;

            //Initialize model
            InitializeMemberVerification(accountMembership, datasource, ref model);

            //Validate model
            ValidateModel(model, viewData.ModelState);

            if (model.ErrorStatus == WelcomeErrorStatus.None)
            {
                //Process
                return SignIn(accountMembership, model, viewData);
            }
            else
            {
                return AuthenticationResultEnum.ErrorNotValid;
            }
        }
        #endregion

        #region Membership Card
        public MembershipCard MembershipCard(Item datasource)
        {
            var accountMembership = _sessionManager.GetAccountMembership();

            return new MembershipCard(datasource)
            {
                Status = GetStatus(accountMembership),
                Imsid = GetImsid(accountMembership)
            };
        }
        public MembershipCard MembershipCardRegister(MembershipCard membershipCard)
        {
            var mdsid = GetMdsid(membershipCard.Imsid);
            var userResponse = _retrieveUserManager.RetrieveUserNeamb(mdsid);
            if (userResponse != null)
            {
                //Zip validation flag needs to be set as true to prevent a bounce from the registration page.
                _sessionManager.SaveZipCodeValidationSuccess();
                membershipCard.Status = WelcomeStatus.NewMember;
                membershipCard.Mdsid = mdsid;
            }
            else
            {
                membershipCard.Status = WelcomeStatus.None;
                membershipCard.ErrorStatus |= WelcomeErrorStatus.InvalidCode;
            }

            return membershipCard;
        }
        #endregion

        #region Membership Card Login
        public AuthenticationResultEnum VerifyPassword(ref MembershipCardLogin model, ViewDataDictionary viewData)
        {
            var accountMembership = _sessionManager.GetAccountMembership();
            var datasource = model.Item;

            //Validate model
            ValidateModel(model, viewData.ModelState);

            if (model.ErrorStatus == WelcomeErrorStatus.None)
            {
                //Process
                return SignIn(accountMembership, model, viewData);
            }
            else
            {
                return AuthenticationResultEnum.ErrorNotValid;
            }
        }
        #endregion

        #region Private Methods
        private string GetSupportEmail()
        {
            var siteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
            return siteSettings[Templates.SiteSettings.Fields.Email];
        }

        private string GetSearechMessage(Item datasource, AccountMembership accountMembership)
        {
            var warmMessage = datasource.Fields[Templates.MemberWelcome.Fields.Warm]?.Value ?? string.Empty;

            return GetStatus(accountMembership) != WelcomeStatus.None
                ? warmMessage.Replace("[]", _sessionManager.GetAccountMembership().Profile.FirstName)
                : datasource.Fields[Templates.MemberWelcome.Fields.Cold]?.Value ?? string.Empty;
        }
        private string GetNotYouUrl()
        {
            var url = $"{LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.MemberWelcome))}";
            return url;
        }

        private static VideoSourceType GetVideoType(Item item)
        {
            var video = item.Fields[Templates.MemberWelcome.Fields.Video]?.Value ?? string.Empty;

            if (string.IsNullOrEmpty(video))
            {
                return VideoSourceType.None;
            }

            return video.Contains(JwpPrefix) ? VideoSourceType.JWPlayer : VideoSourceType.YouTube;
        }
        private static WelcomeStatus GetStatus(AccountMembership accountMembership)
        {
            switch (accountMembership?.Status ?? StatusEnum.Unknown)
            {
                case StatusEnum.LockedNewToken:
                case StatusEnum.LockedOldToken:
                    return WelcomeStatus.None;
                case StatusEnum.Duplicated:
                    return WelcomeStatus.ExistingMember;
                case StatusEnum.Registered:
                    return WelcomeStatus.ExistingMember;
                case StatusEnum.Unknown:
                    return WelcomeStatus.None;
                case StatusEnum.Hot:
                    return WelcomeStatus.ExistingMember;
                case StatusEnum.WarmHot:
                    return WelcomeStatus.ExistingMember;
                case StatusEnum.WarmCold:
                    return WelcomeStatus.NewMember;
                case StatusEnum.Cold:
                    return WelcomeStatus.NewMember;
                default:
                    return WelcomeStatus.None;
            }
        }
        private string GetImsid(AccountMembership accountMembership)
        {
            return string.IsNullOrEmpty(accountMembership?.Mdsid)
                ? string.Empty
                : _oracleManager.SelectImsId(accountMembership.Mdsid);
        }
        private string GetMdsid(string individualId)
        {
            return string.IsNullOrEmpty(individualId)
                ? string.Empty
                : _oracleManager.SelectMdsId(individualId);
        }
        private void ValidateModel(MembershipCardLogin model, ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(model.Imsid))
            {
                model.ErrorStatus |= WelcomeErrorStatus.InvalidCode;
            }
            switch (model.Action)
            {
                case WelcomeAction.None:
                    model.ErrorStatus |= WelcomeErrorStatus.GeneralError;
                    Log.Warn($"No handle for Action:{model.Action}", this);
                    break;
                case WelcomeAction.VerifyPassword:
                    if (ValidationFieldHelper.GetAttributeError(modelState[nameof(model.Password)]) != ModelErrorType.None)
                    {
                        model.ErrorStatus |= WelcomeErrorStatus.InvalidPassword;
                    }
                    break;
                default:
                    model.ErrorStatus |= WelcomeErrorStatus.GeneralError;
                    Log.Warn($"No handle for Action:{model.Action}", this);
                    break;
            }
        }
        private AuthenticationResultEnum SignIn(AccountMembership accountMembership, MembershipCardLogin model, ViewDataDictionary viewData)
        {

            var accountDto = new AccountDTO()
            {
                Email = accountMembership.Username,
                Password = model.Password
            };
            var options = new ItemUrlBuilderOptions
            {
                AlwaysIncludeServerUrl = true,

            };


            var pathReset = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.ResetPassword), options);

            var result = _authenticationManager.ExecuteAuthentication(viewData.ModelState, string.Empty, pathReset, accountDto, viewData, false);

            //Map errors
            MapSignInErrors(model, accountDto);

            return result;
        }
        private static void MapSignInErrors(MembershipCardLogin model, AccountDTO accountDto)
        {
            if (accountDto.IsValid)
            {
                model.WasProcessed = true;
                return;
            }
            if (accountDto.HasErrorInvalidCredentials)
            {
                model.ErrorStatus |= WelcomeErrorStatus.InvalidPassword;
            }
            if (accountDto.HasLockedError)
            {
                model.ErrorStatus |= WelcomeErrorStatus.AccountLocked;
            }
            if (accountDto.HasAlreadyLockedErrorTokenValid)
            {
                model.ErrorStatus |= WelcomeErrorStatus.AccountAlreadyLockedValidToken;
            }
            if (accountDto.HasErrorTimeout)
            {
                model.ErrorStatus |= WelcomeErrorStatus.TimeOut;
            }
        }
        private void InitializeMemberVerification(AccountMembership accountMembership, Item datasource, ref MemberVerification model)
        {
            model.Salutation = GetSalutation(datasource, accountMembership);
            model.Status = GetStatus(accountMembership);
            model.Imsid = GetImsid(accountMembership);
            model.Instructions = GetInstructions(datasource, accountMembership);
            model.NotYou = GetNotYou(datasource, accountMembership);
        }
        private void VerifyZip(AccountMembership accountMembership, ref MemberVerification model)
        {
            //If the zipcode of the user logged matches with the entered in the screen redirect to the registration page with the information of the user logged
            if (accountMembership.Profile.ZipCode.Equals(model.Zip))
            {
                _sessionManager.SaveZipCodeValidationSuccess();
                model.WasProcessed = true;
            }
            else
            {
                //Get the attempt counter from session
                var attemptNumber = _sessionManager.GetAttemptZipCodeValidation();
                attemptNumber++;
                if (attemptNumber >= _globalConfigurationManager.AttemptZipCodeValidation)
                {
                    //If the attempt number is greater than 3 redirect logout the user and redirect to the registration page
                    _authenticationAccountManager.LogoutUser(false);
                    model.ErrorStatus |= WelcomeErrorStatus.TooManyAttempts;
                }
                else
                {
                    //If the attempt number is less than 3 redirect to the same screen
                    _sessionManager.SaveAttemptZipCodeValidation(attemptNumber);
                    model.Attempts = attemptNumber;
                    model.ErrorStatus |= WelcomeErrorStatus.ZipMatchNotFound;
                }
            }
        }
        private static void MapSignInErrors(MemberVerification model, AccountDTO accountDto)
        {
            if (accountDto.IsValid)
            {
                model.WasProcessed = true;
                return;
            }
            if (accountDto.HasErrorInvalidCredentials)
            {
                model.ErrorStatus |= WelcomeErrorStatus.InvalidPassword;
            }
            if (accountDto.HasLockedError)
            {
                model.ErrorStatus |= WelcomeErrorStatus.AccountLocked;
            }
            if (accountDto.HasAlreadyLockedErrorTokenValid)
            {
                model.ErrorStatus |= WelcomeErrorStatus.AccountAlreadyLockedValidToken;
            }
            if (accountDto.HasErrorTimeout)
            {
                model.ErrorStatus |= WelcomeErrorStatus.TimeOut;
            }
        }

        private AuthenticationResultEnum SignIn(AccountMembership accountMembership, MemberVerification model, ViewDataDictionary viewData)
        {
            var accountDto = new AccountDTO()
            {
                Email = accountMembership.Username,
                Password = model.Password
            };
            //accountDto.Initialize(RenderingContext.Current.Rendering);
            var options = new ItemUrlBuilderOptions
            {
                AlwaysIncludeServerUrl = true,

            };

            var pathReset = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Items.ResetPassword), options);

            var result = _authenticationManager.ExecuteAuthentication(viewData.ModelState, string.Empty, pathReset, accountDto, viewData, false);

            //Map errors
            MapSignInErrors(model, accountDto);

            return result;
        }

        private void ValidateModel(MemberVerification model, ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(model.Imsid))
            {
                model.ErrorStatus |= WelcomeErrorStatus.InvalidCode;
            }

            switch (model.Action)
            {
                case WelcomeAction.None:
                    model.ErrorStatus |= WelcomeErrorStatus.GeneralError;
                    Log.Warn($"No handle for Action:{model.Action}", this);
                    break;
                case WelcomeAction.VerifyZip:
                    if (ValidationFieldHelper.GetAttributeError(modelState[nameof(model.Zip)]) != ModelErrorType.None)
                    {
                        model.ErrorStatus |= WelcomeErrorStatus.InvalidZip;
                    }
                    break;
                case WelcomeAction.VerifyPassword:
                    if (ValidationFieldHelper.GetAttributeError(modelState[nameof(model.Password)]) != ModelErrorType.None)
                    {
                        model.ErrorStatus |= WelcomeErrorStatus.InvalidPassword;
                    }
                    break;
                default:
                    model.ErrorStatus |= WelcomeErrorStatus.GeneralError;
                    Log.Warn($"No handle for Action:{model.Action}", this);
                    break;
            }
        }

        private string GetNotYou(Item datasource, AccountMembership accountMembership)
        {
            var result = datasource.Fields[Templates.MemberVerificationForm.Fields.NotYou]?.Value ?? string.Empty;
            return GetStatus(accountMembership) != WelcomeStatus.None
                ? result.Replace("[]", _sessionManager.GetAccountMembership().Profile.FirstName)
                : datasource.Fields[Templates.MemberVerificationForm.Fields.NotYou]?.Value ?? string.Empty;
        }

        private static string GetInstructions(Item datasource, AccountMembership accountMembership)
        {
            switch (GetStatus(accountMembership))
            {
                case WelcomeStatus.None:
                    return string.Empty;
                case WelcomeStatus.NewMember:
                    return datasource.Fields[Templates.MemberVerificationForm.Fields.InstructionsNotRegistered]?.Value;
                case WelcomeStatus.ExistingMember:
                    return datasource.Fields[Templates.MemberVerificationForm.Fields.InstructionsRegistered]?.Value;
                default:
                    return string.Empty;
            }
        }
        private string GetSalutation(Item datasource, AccountMembership accountMembership)
        {
            var result = datasource.Fields[Templates.MemberVerificationForm.Fields.Salutation]?.Value ?? string.Empty;
            return GetStatus(accountMembership) != WelcomeStatus.None
                ? result.Replace("[]", _sessionManager.GetAccountMembership().Profile.FirstName)
                : string.Empty;
        }
        #endregion
    }
}