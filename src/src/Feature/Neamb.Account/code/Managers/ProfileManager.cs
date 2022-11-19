using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUser;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUserName;
using Neambc.Neamb.Foundation.Membership.Enums;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Neambc.Neamb.Feature.Account.Managers
{

    [Service(typeof(IProfileManager))]
    public class ProfileManager : IProfileManager
    {

        #region Fields
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;
        private readonly IGtmService _gtmService;
        private readonly IActionReminderService _actionReminderService;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IUpdateUserNameService _updateUserNameService;
        private readonly ISearchUserNameService _searchUserNameService;
        private readonly IUpdateUserService _updateUserService;

        #endregion

        #region Constructors
        public ProfileManager(ISessionAuthenticationManager sessionAuthenticationManager, IAccountRepository accountRepository, IAccountServiceProxy serviceManager, IAuthenticationAccountManager authenticationAccountManager, ICacheManager cacheManager, IGtmService gtmService,
            IActionReminderService actionReminderService, IGlobalConfigurationManager globalConfigurationManager, IUpdateUserNameService updateUserNameService, ISearchUserNameService searchUserNameService, IUpdateUserService updateUserService)
        {
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _accountRepository = accountRepository;
            _authenticationAccountManager = authenticationAccountManager;
            _gtmService = gtmService;
            _actionReminderService = actionReminderService;
            _globalConfigurationManager = globalConfigurationManager;
            _updateUserNameService = updateUserNameService;
            _searchUserNameService = searchUserNameService;
            _updateUserService = updateUserService;
        }
        #endregion

        #region Public Methods
        public ProfileDTO GetProfileDto(Item datasource, string newcell, string oldcell, string msrName, bool isDraft = false)
        {
            var accountMembership = isDraft ? _sessionAuthenticationManager.GetAccountMembershipDraft() : _sessionAuthenticationManager.GetAccountMembership();
            ProfileDTO profileDtoModel;
            if (_sessionAuthenticationManager.GetAccountMembership().Status == StatusEnum.Hot)
            //Fill the model to be displayed in the screen
                profileDtoModel = new ProfileDTO(datasource)
                {
                    Address = accountMembership.Profile.StreetAddress,
                    Month = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth) ? accountMembership.Profile.DateOfBirth.Substring(0, 2) : string.Empty,
                    Day = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth) ? accountMembership.Profile.DateOfBirth.Substring(2, 2) : string.Empty,
                    Year = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth) ? accountMembership.Profile.DateOfBirth.Substring(4, 4) : string.Empty,
                    City = accountMembership.Profile.City,
                    Email = accountMembership.Username,
                    FirstName = accountMembership.Profile.FirstName,
                    LastName = accountMembership.Profile.LastName,
                    Phone = accountMembership.Profile.Phone,
                    State = accountMembership.Profile.StateCode,
                    Zip = accountMembership.Profile.ZipCode,
                    UserFullName = $"{accountMembership.Profile.FirstName} {accountMembership.Profile.LastName}",
                    EmailPermission = accountMembership.Profile.EmailPermissionIndicator,
                    BirthDate = accountMembership.Profile.DateOfBirth,
                    Webuserid = accountMembership.Profile.Webuserid
                };
            else
                profileDtoModel = new ProfileDTO(datasource);

            profileDtoModel.NewcellParam = newcell;
            profileDtoModel.OldcellParam = oldcell;
            profileDtoModel.MsrNameParam = msrName;
            profileDtoModel.HasTooltipCity = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.CityTooltip]);
            profileDtoModel.HasTooltipAddress = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.AddressTooltip]);
            profileDtoModel.HasTooltipBirthDate = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.BirthDateTooltip]);
            profileDtoModel.HasTooltipEmail = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.EmailTooltip]);
            profileDtoModel.HasTooltipFirstName = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.FirstNameTooltip]);
            profileDtoModel.HasTooltipLastName = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.LastNameTooltip]);
            profileDtoModel.HasTooltipPhone = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.PhoneTooltip]);
            profileDtoModel.HasTooltipState = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.StateTooltip]);
            profileDtoModel.HasTooltipZip = !string.IsNullOrEmpty(datasource[Templates.Profile.Fields.ZipTooltip]);

            return profileDtoModel;
        }

        public ProfileDTO SaveProfileDto(ProfileDTO model, DateParts dateParts,
            bool isValidModelState, ViewDataDictionary viewData, Item item, bool isDraft = false)
        {
            ProfileDTO ret;

            var accountMembership = isDraft
                ? _sessionAuthenticationManager.GetAccountMembershipDraft()
                : _sessionAuthenticationManager.GetAccountMembership();

            FillModel(model, item, accountMembership, dateParts);

            //Authorization indicator according to settings to send marketing emails
            var emailAuthorization = model.OptIn
                        ? ConstantsNeamb.EmailPermissionChecked
                        : ConstantsNeamb.EmailPermissionUnChecked;

            // shortcut that all is validated
            var hasdomainErrors = _accountRepository.HasEmailDomainValidationErrors(model.Email);
            if (hasdomainErrors)
            {
                model.ErrorsEmail.Add(ErrorStatusEnum.InvalidCharacters);
            }
            var isValidModel = isValidModelState &&
                               !_accountRepository.HasDateBirthCustomValidationErrors(model) &&
                               !_accountRepository.HasEmailCustomValidationErrors(model) &&
                               !hasdomainErrors;
            var allowUserNameUpdate = IsModelValid(model, item, accountMembership);

            if (isDraft)
            {
                ret = SaveDraftToAccount(model, accountMembership, isValidModel, allowUserNameUpdate, viewData, emailAuthorization);
            }
            else if (isValidModel && ((!model.Email.Equals(accountMembership.Username) && allowUserNameUpdate) || model.Email.Equals(accountMembership.Username)))
            {
                var mdsId = InvokeUpdateUserService(model, accountMembership, allowUserNameUpdate, emailAuthorization);
                // if user updated successfully then update the information in Session
                if (model.ProcessedSucessfully && !model.HasGeneralError && !model.HasDuplicateAccount)
                {
                    var newAccountMembership = CreateNewAccountMembership(mdsId);
                    model = CreateNewProfile(newAccountMembership, item);
                }
                ret = model;

            }
            else
            {
                if (viewData != null)
                {
                    _accountRepository.SetErrorProfile(model, viewData);
                }
                model.EmailPermission = emailAuthorization;
                ret = model;
            }
            return ret;
        }

        public ProfileDTO SaveProfileDto(ProfileDTO model, bool isValidModelState, ViewDataDictionary viewData, Item item, bool isDraft = false)
        {
            ProfileDTO ret;
            var accountMembership = isDraft
                ? _sessionAuthenticationManager.GetAccountMembershipDraft()
                : _sessionAuthenticationManager.GetAccountMembership();

            ModelHasItemFields(model, item);

            //Authorization indicator according to settings to send marketing emails
            var emailAuthorization = model.OptIn
                        ? ConstantsNeamb.EmailPermissionChecked
                        : ConstantsNeamb.EmailPermissionUnChecked;

            // shortcut that all is validated
            var hasdomainErrors = _accountRepository.HasEmailDomainValidationErrors(model.Email);
            if (hasdomainErrors)
            {
                model.ErrorsEmail.Add(ErrorStatusEnum.InvalidCharacters);
            }
            var isValidModel = isValidModelState &&
                               !_accountRepository.HasDateBirthCustomValidationErrors(model) &&
                               !_accountRepository.HasEmailCustomValidationErrors(model) &&
                               !hasdomainErrors;
            var allowUserNameUpdate = IsModelValid(model, item, accountMembership);

            if (isDraft)
            {
                ret = SaveDraftToAccount(model, accountMembership, isValidModel, allowUserNameUpdate, viewData, emailAuthorization);
            }
            else if (isValidModel && ((!model.Email.Equals(accountMembership.Username) && allowUserNameUpdate) || model.Email.Equals(accountMembership.Username)))
            {
                var mdsId = InvokeUpdateUserService(model, accountMembership, allowUserNameUpdate, emailAuthorization);
                // if user updated successfully then update the information in Session
                if (model.ProcessedSucessfully && !model.HasGeneralError && !model.HasDuplicateAccount)
                {
                    var newAccountMembership = CreateNewAccountMembership(mdsId);
                    model = CreateNewProfile(newAccountMembership, item);
                }
                ret = model;
            }
            else
            {
                if (viewData != null)
                {
                    _accountRepository.SetErrorProfile(model, viewData);
                }
                model.EmailPermission = emailAuthorization;
                ret = model;
            }
            return ret;
        }

        public AccountMembership CreateNewAccountMembership(string mdsId)
        {
            var ret = new AccountMembership();
            _authenticationAccountManager.RetrieveAccount(ret, mdsId);
            _authenticationAccountManager.LogoutUser();
            _authenticationAccountManager.InitializeAccountMemberData(ret);
            _authenticationAccountManager.LoginSitecoreContext(ret.Username);
            try
            {
                _authenticationAccountManager.SaveCustomFacets(ret);
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Error saving custom facets", e, this);
            }
            return ret;
        }

        /// <summary>
        /// Get the GTM action when the user profile is updated
        /// </summary>
        /// <param name="isFormPassword">Flag to see what information was updated</param>
        /// <param name="contextItem">Profile context item</param>
        /// <returns>GTM action</returns>
        public string GetGtmAction(string isFormPassword, Item contextItem)
        {
            string accountAction = !string.IsNullOrEmpty(isFormPassword) && isFormPassword.Equals("0") ? "update profile" : "update password";
            string buttonText = !string.IsNullOrEmpty(isFormPassword) && isFormPassword.Equals("0") ? contextItem[Templates.ProfilePassword.Fields.ProfileSubmit] : contextItem[Templates.ProfilePassword.Fields.PasswordSubmit];
            ProfileUpdate profileUpdate = new ProfileUpdate
            {
                Event = "account",
                AccountSection = "profile & password",
                AccountAction = accountAction,
                CtaText = buttonText
            };
            return _gtmService.GetGtmEvent(profileUpdate);
        }

        #endregion

        //change Name
        #region Private/Protected Methods
        protected virtual void ModelHasItemFields(ProfileDTO model, Item item)
        {
            model.Initialize(item);
            model.HasTooltipCity = !string.IsNullOrEmpty(item[Templates.Profile.Fields.CityTooltip]);
            model.HasTooltipAddress = !string.IsNullOrEmpty(item[Templates.Profile.Fields.AddressTooltip]);
            model.HasTooltipBirthDate = !string.IsNullOrEmpty(item[Templates.Profile.Fields.BirthDateTooltip]);
            model.HasTooltipEmail = !string.IsNullOrEmpty(item[Templates.Profile.Fields.EmailTooltip]);
            model.HasTooltipFirstName = !string.IsNullOrEmpty(item[Templates.Profile.Fields.FirstNameTooltip]);
            model.HasTooltipLastName = !string.IsNullOrEmpty(item[Templates.Profile.Fields.LastNameTooltip]);
            model.HasTooltipPhone = !string.IsNullOrEmpty(item[Templates.Profile.Fields.PhoneTooltip]);
            model.HasTooltipState = !string.IsNullOrEmpty(item[Templates.Profile.Fields.StateTooltip]);
            model.HasTooltipZip = !string.IsNullOrEmpty(item[Templates.Profile.Fields.ZipTooltip]);
            model.SubmitProfileButton = item[Templates.ProfilePassword.Fields.ProfileSubmit];
            model.SubmitPasswordButton = item[Templates.ProfilePassword.Fields.PasswordSubmit];
        }
        
        protected virtual bool IsModelValid(ProfileDTO model, Item item, AccountMembership accountMembership) {
            var ret = false;
            if (model.HasGeneralError || model.Email.Equals(accountMembership.Username)) return ret;
            //Call the webservice to verify the availability of the username
            var userInfo = _searchUserNameService.SearchUserName(model.Email);
            if (userInfo != null && userInfo.Success)
            {
                if (userInfo.Data.Registered) {
                    model.ErrorsEmail.Add(ErrorStatusEnum.EmailInUse);
                    model.ErrorMessageEmailInUse = string.Format(item[Templates.Profile.Fields.EmailInUse], model.Email);
                    model.ProcessedSucessfully = false;
                } else {
                    ret = true;
                }
            }

            if (userInfo?.ErrorCode != SearchUsernameErrorCodes.InputDataValidationError) return ret;
            model.ErrorsEmail.Add(ErrorStatusEnum.EmailFormat);
            model.ProcessedSucessfully = false;
            return ret;
        }
        
        protected virtual void FillModel(ProfileDTO model, Item item, AccountMembership accountMembership, DateParts dateParts)
        {
            ModelHasItemFields(model, item);
            model.UserFullName = $"{accountMembership.Profile.FirstName} {accountMembership.Profile.LastName}";
            model.Day = dateParts.Day;
            model.Month = dateParts.Month;
            model.Year = dateParts.Year;
            model.BirthDate = dateParts.AsBirthDate();
        }
        
        protected virtual ProfileDTO SaveDraftToAccount(ProfileDTO model, AccountMembership accountMembership, bool isValidModel,
            bool allowUserNameUpdate, ViewDataDictionary viewData, string canEmail)
        {
            if (isValidModel)
            {
                accountMembership.Profile.FirstName = model.FirstName;
                accountMembership.Profile.LastName = model.LastName;
                accountMembership.Profile.StreetAddress = model.Address;
                accountMembership.Profile.City = model.City;
                accountMembership.Profile.StateCode = model.State;
                accountMembership.Profile.ZipCode = model.Zip;
                accountMembership.Profile.DateOfBirth = model.BirthDate;
                accountMembership.Profile.Phone = model.Phone.AsFormattedPhoneNumber();
                accountMembership.Profile.EditingStatus |= EditingStatus.YourInformationChanged;

                if (allowUserNameUpdate) accountMembership.Username = model.Email;
                _sessionAuthenticationManager.SaveAccountMembershipDraft(accountMembership);
                model.ProcessedSucessfully = true;
            }
            else
            {
                if (viewData != null) _accountRepository.SetErrorProfile(model, viewData);
                model.EmailPermission = canEmail;
            }
            return model;
        }
        
        /// <summary>
        /// Call the remote UpdateUser API and checks the result for any new MdsId
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountMembership"></param>
        /// <param name="allowUserNameUpdate"></param>
        /// <param name="canEmail"></param>
        /// <returns>Null or a new MdsId from the User Server if successful</returns>
        
        protected virtual string InvokeUpdateUserService(ProfileDTO model, AccountMembership accountMembership, bool allowUserNameUpdate, string canEmail)
        {
            string ret = null;
            // Call to the webservice to update the user

            var serviceResponse = _updateUserService.UpdateUser(accountMembership.Username, Convert.ToInt32(accountMembership.Profile.Webuserid),
                model.FirstName, model.LastName,
                 model.Address,
                 model.City,
                 model.State,
                 model.Zip,
                 model.BirthDate,
                 model.Phone.AsFormattedPhoneNumber(),
                 canEmail,
                 Convert.ToInt32(_globalConfigurationManager.Unionid));


            // Evaluate response from UpdateUser webservice method
            if (serviceResponse != null && serviceResponse.Success)
            {
                if (serviceResponse.Data != null && serviceResponse.Data.updated)
                {
                    if (serviceResponse.Data.registrationCount <= 1)
                    {
                        model.ProcessedSucessfully = true;
                        ret = serviceResponse.Data.newMdsId;
                    }
                    else  // duplicate account
                    {
                        model.HasDuplicateAccount = true;
                        model.Registrations = serviceResponse.Data.registrations;
                        model.NewMdsid = serviceResponse.Data.newMdsId;
                        ret = serviceResponse.Data.newMdsId;
                    }
                }
                else   // error
                {
                    Sitecore.Diagnostics.Log.Warn($"Error webservice UpdateUser. User {accountMembership.Username}", this);
                    model.HasGeneralError = true;
                    model.ProcessedSucessfully = false;
                }
            }
            else   //general error
            {
                model.HasGeneralError = true;
                model.ProcessedSucessfully = false;
            }
            if (allowUserNameUpdate && (ret != null)) ret = ChangeUserName(model, accountMembership);

            return ret;
        }
        
        protected virtual ProfileDTO CreateNewProfile(AccountMembership accountMembership, Item item)
        {
            var ret = new ProfileDTO
            {
                Address = accountMembership.Profile.StreetAddress,
                Month = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth)
                    ? accountMembership.Profile.DateOfBirth.Substring(0, 2)
                    : string.Empty,
                Day = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth)
                    ? accountMembership.Profile.DateOfBirth.Substring(2, 2)
                    : string.Empty,
                Year = !string.IsNullOrEmpty(accountMembership.Profile.DateOfBirth)
                    ? accountMembership.Profile.DateOfBirth.Substring(4, 4)
                    : string.Empty,
                City = accountMembership.Profile.City,
                Email = accountMembership.Username,
                FirstName = accountMembership.Profile.FirstName,
                LastName = accountMembership.Profile.LastName,
                Phone = accountMembership.Profile.Phone,
                State = accountMembership.Profile.StateCode,
                Zip = accountMembership.Profile.ZipCode,
                UserFullName = $"{accountMembership.Profile.FirstName} {accountMembership.Profile.LastName}",
                EmailPermission = accountMembership.Profile.EmailPermissionIndicator
            };
            ret.Initialize(item);
            ret.ProcessedSucessfully = true;
            return ret;
        }
        /// <summary>
        /// Change the username when it is different than the original one
        /// </summary>
        /// <param name="model">model to be returned to the view</param>
        /// <param name="accountMembership">Data of the user logged</param>
        /// <returns></returns>
        private string ChangeUserName(ProfileDTO model, AccountMembership accountMembership)
        {
            var mdsid = string.Empty;
            //Call the webservice for changing the user name
            var responseChangeUsername = _updateUserNameService.UpdateUserNameStatus(accountMembership.Username, model.Email, model.Email, _globalConfigurationManager.Unionid);

            if (responseChangeUsername == true)
            {
                //Sending the email to Exact target
                _accountRepository.SendExactTargetChangeUserName(accountMembership.Profile.FirstName,
                accountMembership.Profile.LastName, accountMembership.Mdsid, model.Email, accountMembership.Username,
                model.NewcellParam, model.OldcellParam);
                model.ProcessedSucessfully = true;
                mdsid = accountMembership.Mdsid;
                _actionReminderService.RemoveVisited(PageType.Profile, accountMembership.Username);
                if (!model.HasDuplicateAccount) return mdsid;
                foreach (var data in model.Registrations.Where(data => data.Email == accountMembership.Username)) data.Email = model.Email;
            }

            else
            {
                model.HasGeneralError = true;
                model.ProcessedSucessfully = false;
            }

            return mdsid;
        }

        #endregion
    }
}