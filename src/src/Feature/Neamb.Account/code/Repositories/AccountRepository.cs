using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;
using Neambc.Neamb.Foundation.MBCData.Model.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Services.DeleteUser;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Newtonsoft.Json;

namespace Neambc.Neamb.Feature.Account.Repositories
{
    [Service(typeof(IAccountRepository))]
    public class AccountRepository : IAccountRepository
    {
        private readonly ICookieManager _cookieManager;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly ISessionManager _sessionManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;
        private readonly IRegistrationManager _registrationManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IExactTargetClient _exactTargetClient;
        private readonly IAmazonS3Repository _amazonS3Repository;
        private readonly IBase64Service _base64Service;
        private readonly IRegistrationRedirection _registrationRedirection;
        private readonly ISearchUserNameService _searchUserNameService;
        private readonly ICompIntroLifeService _compIntroLifeService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IOracleDatabase _oracleManager;

        public AccountRepository(
            ICookieManager cookieManager,
            ISessionAuthenticationManager sessionAuthenticationManager,
            IAuthenticationAccountManager authenticationAccountManager,
            IRegistrationManager registrationManager,
            IGlobalConfigurationManager globalConfigurationManager,
            IExactTargetClient exactTargetClient,
            IAmazonS3Repository amazonS3Repository,
            IBase64Service base64Service,
            ISessionManager sessionManager,
            IRegistrationRedirection registrationRedirection,
            ISearchUserNameService searchUserNameService,
            ICompIntroLifeService compIntroLifeService,
            IDeleteUserService deleteUserService,
            IOracleDatabase oracleManager
        )
        {
            _amazonS3Repository = amazonS3Repository;
            _base64Service = base64Service;
            _cookieManager = cookieManager;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _authenticationAccountManager = authenticationAccountManager;
            _registrationManager = registrationManager;
            _globalConfigurationManager = globalConfigurationManager;
            _exactTargetClient = exactTargetClient;
            this._sessionAuthenticationManager = sessionAuthenticationManager;
            _sessionManager = sessionManager;
            _registrationRedirection = registrationRedirection;
            _searchUserNameService = searchUserNameService;
            _compIntroLifeService = compIntroLifeService;
            _deleteUserService = deleteUserService;
            _oracleManager = oracleManager;
        }

        /// <summary>
        /// Authenticate the user name and password and evaluate the different responses
        /// </summary>
        /// <param name="account">User logged</param>
        /// <param name="username">user name to be logged</param>
        /// <param name="password">password to be logged</param>
        /// <param name="pathReset">reset link defined as field in the template</param>
        /// <param name="rememberme">flag to remember the user name</param>
        /// <param name="executeLogout">Flag to logout the user</param>
        /// <returns>AccountMembership object with the user logged</returns>
        public LoginResponse AuthenticateUser(
            AccountMembership account,
            string username,
            string password,
            string pathReset,
            bool rememberme,
            bool executeLogout = true
        )
        {
            var cellcode = _sessionAuthenticationManager.GetCellCode();

            //Execute the authentication of the user with the services
            var response = _authenticationAccountManager.AuthenticateAccount(username, password, account, cellcode);

            //If the user was sucessfully authenticated save the information in a cookie for warm and save the info in Session. Also login in sitecore
            if (account.Status == StatusEnum.Hot || account.Status == StatusEnum.WarmCold)
            {
                //Case no duplicate registration
                if (response.Data.RegistrationCount <= 1)
                {
                    _cookieManager.SaveWarmUser(_base64Service.Encode(response.Data.MdsIdAsString), _globalConfigurationManager.TimeWarmCookie);
                }
                if (executeLogout)
                {
                    _authenticationAccountManager.LogoutUser(false);
                }

                //Flag to remember the user name saving in a cookie
                if (rememberme)
                {
                    _cookieManager.SaveRememberMe(_base64Service.Encode(username), _globalConfigurationManager.TimeRemembermeCookie);
                }
                //Remove from the cookie the remember me option
                else
                {
                    _cookieManager.RemoveRememberMe();
                }

                account.Profile = RetrieveRakutenProfile(account);
                _authenticationAccountManager.InitializeAccountMemberData(account);
                _authenticationAccountManager.LoginSitecoreContext(account.Username);
                
            }
            else
            {
                _authenticationAccountManager.ProcessErrorAuthentication(response, account, username, pathReset);
            }

            return response;
        }        

        public Profile RetrieveRakutenProfile( AccountMembership account )
        {
            var isRakutenMember = _oracleManager.RakutenRegExists(account.Username);
            var profile = account.Profile;
            profile.IsRakutenMember = isRakutenMember;

            if (isRakutenMember)
            {
                var rakutenProfile = _oracleManager.ViewRakutenRegs( account.Username );
                var stores = JsonConvert.DeserializeObject<List<StoreInfo>>(rakutenProfile.FavoriteStore);
                profile.RakutenProfile = new RakutenMemberModel
                {
                    EmailAddress = rakutenProfile.EmailAddress,
                    Id = rakutenProfile.StoreId,
                    CreatedDate = rakutenProfile.CreateDate,
                    EBToken = rakutenProfile.EBToken,
                    FavoriteStores = stores
                };
            }
            return profile;
        }

        public bool SaveFavoriteStore(string mdsId, string email, List<StoreInfo> stores)
        {
            var favoriteStores = JsonConvert.SerializeObject(stores).Replace("'", "''");
            var success = _oracleManager.Update_Favorite_Stores(mdsId, email, favoriteStores);
            return success;
        }

        public RegisterUserResponse RegisterUser(AccountMembership account, string password)
        {
            //Get the cellcode
            var cellcode = _sessionAuthenticationManager.GetCellCode();

            //Get the campaign code
            var campaigncode = _sessionAuthenticationManager.GetCampaignCode();

            //Execute the authentication of the user with the services
            var serviceResponse = _registrationManager.RegisterAccount(account, password, cellcode, campaigncode);
            return serviceResponse;
        }

        /// <summary>
        /// Send the email to the user is registering in the system 
        /// </summary>
        /// <param name="userName">User email</param>
        /// <param name="mdsInvId">User id</param>
        /// <param name="firstname">First name</param>
        /// <param name="lastname">Last name</param>
        /// <param name="emailOptOut">Permission flag choose in the registration screen</param>
        /// <param name="newEnvInd">newenvind date of the user</param>
        /// <param name="membershipType">membershipType of the user</param>
        /// <param name="complifeDate">complifeDate of the user</param>
        /// <param name="memberflag">neacurrentmember of the user</param>
        /// <param name="newMemberFlag">neacurrentmember of the user</param>
        /// <returns></returns>
        public bool SendExactTargetRegisterEmail(
            string userName,
            string mdsInvId,
            string firstname,
            string lastname,
            string emailOptOut,
            string newEnvInd,
            string neaMembershipType,
            string complifeDate,
            bool neaCurrentMember,
            string newMemberSegmentIndicator
        )
        {
            string cellcode;
            var customerdefinition = "TGNW1025-SEND-DEFINITION";
            var campaingcode = "TGNW1025";
            if (neaCurrentMember)
            {
                // CURRENT MEMBER
                if (neaMembershipType == "AP" || neaMembershipType == "AC" || neaMembershipType == "AS")
                {
                    // FAMILY MEMBER
                    cellcode = "SSGA1025";
                }
                else if (neaMembershipType == "12")
                {
                    // EARLY ENROLLEE
                    cellcode = "SSGB1025";
                }

                else if (newMemberSegmentIndicator == "1" || newMemberSegmentIndicator == "2" || newMemberSegmentIndicator == "3" || newMemberSegmentIndicator == "4")
                {
                    // NEW MEMBER
                    cellcode = "SSGC1025";
                }

                else
                {
                    // ORGANIC MEMBER
                    cellcode = "SSGD1025";
                }
            }
            else
            {
                // NOT A CURRENT MEMBER
                cellcode = "SSGE1025";
            }
            

            var parameters = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("INDIVIDUAL_ID", mdsInvId),
                new KeyValuePair<string, string>("FIRST_NAME", firstname.Trim()),
                new KeyValuePair<string, string>("LAST_NAME", lastname),
                new KeyValuePair<string, string>("EMAIL_ADDRESS", userName),
                new KeyValuePair<string, string>("CELL_CODE", cellcode),
                new KeyValuePair<string, string>("CAMPAIGN_CD", campaingcode)
            };
            Log.Debug("Starting sending email registration user", this);
            Log.Debug($"INDIVIDUAL_ID: {mdsInvId}", this);
            Log.Debug($"FIRST_NAME: {firstname}", this);
            Log.Debug($"LAST_NAME: {lastname}", this);
            Log.Debug($"CELL_CODE: {cellcode}", this);
            Log.Debug($"CAMPAIGN_CD: {campaingcode}", this);
            Log.Debug("Ending sending email registration user", this);
            return _exactTargetClient.SendExactTargetService(customerdefinition, userName, parameters,mdsInvId);
        }

        /// <summary>
        /// Get the path of the duplicate registration page
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateRegistrationPageUrl()
        {
            return LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.DuplicateRegistrationPage.ID));
        }

        /// <summary>
        /// Get the information of the duplicate emails to be shown in the screen
        /// </summary>
        /// <param name="currentEmail">Current Email authenticated</param>
        /// <returns></returns>
        public List<EmailDuplicate> GetDuplicateRegistrationEmails(string currentEmail)
        {
            var listEmailDuplicated = new List<EmailDuplicate>();
            var duplicatedEmails = _sessionAuthenticationManager.GetDuplicateRegistration();
            if (duplicatedEmails != null && duplicatedEmails.Count > 0)
            {
                foreach (var emailDuplicate in duplicatedEmails)
                {
                    var userdata = new EmailDuplicate();
                    userdata.Email = emailDuplicate.Email;
                    userdata.FirstName = emailDuplicate.FirstName;
                    userdata.LastName = emailDuplicate.LastName;
                    userdata.Dob = emailDuplicate.RegistrationDate;
                    userdata.Webuserid = emailDuplicate.WebUserIdAsString;
                    if (userdata.Email.ToLower().Equals(currentEmail.ToLower()))
                    {
                        listEmailDuplicated.Insert(0, userdata);
                    }
                    else
                    {
                        listEmailDuplicated.Add(userdata);
                    }
                }
            }
            return listEmailDuplicated;
        }

        /// <summary>
        /// Delete the duplicate emails
        /// </summary>
        /// <param name="emailSelected">Email selected that was not deleted</param>
        /// <param name="emailsDeleted">Email list with the ones to be deleted</param>
        /// <returns>True (no errors): otherwise (errors)</returns>
        public bool DeleteDuplicateRegistrationEmails(string emailSelected, List<string> emailsDeleted)
        {
            var hasError = false;
            var duplicatedEmails = _sessionAuthenticationManager.GetDuplicateRegistration();
            foreach (var emailDuplicate in duplicatedEmails)
            {
                if (!emailDuplicate.Email.Equals(emailSelected))
                {
                    var serviceResponse = _deleteUserService.DeleteUserStatus(emailDuplicate.Email, Convert.ToInt32(_globalConfigurationManager.Unionid));
                    if (serviceResponse.Data.deleted == false)
                    {
                        hasError = true;
                    }
                    else
                    {
                        emailsDeleted.Add(emailDuplicate.Email);
                        try
                        {
                            BaseRequestS3 baseRequest = new BaseRequestS3
                            {
                                BucketName = _globalConfigurationManager.BucketNameAvatarImages,
                                IsEncrypted = true,
                                Key = emailDuplicate.WebUserIdAsString
                            };
                            _amazonS3Repository.DeleteObjectS3(baseRequest);
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Error deleting S3 avatar image {emailDuplicate.Email}", ex, this);
                        }
                    }
                }
            }
            return !hasError;
        }

        /// <summary>
        /// Send the email when the duplicate registration has finished
        /// </summary>
        /// <param name="accountMembership"></param>
        /// <param name="emailsDeleted"></param>
        /// <param name="emailSelected"></param>
        /// <returns>Response from Exact target</returns>
        public bool SendExactTargetDuplicateRegistrationEmail(
            AccountMembership accountMembership,
            string emailsDeleted,
            string emailSelected
        )
        {
            var targetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var newDT = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
            var parameters = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("FIRST_NAME", accountMembership.Profile.FirstName),
                new KeyValuePair<string, string>("LAST_NAME", accountMembership.Profile.LastName),
                new KeyValuePair<string, string>("CELL_CODE",
                    _globalConfigurationManager.CellCodeDuplicateRegistration),
                new KeyValuePair<string, string>("CAMPAIGN_CD",
                    _globalConfigurationManager.CampaignCodeDuplicateRegistration),
                new KeyValuePair<string, string>("INDIVIDUAL_ID", accountMembership.Mdsid.PadLeft(9, '0')),
                new KeyValuePair<string, string>("DATE_TIME_ACTION",
                    $"{newDT:MM/dd/yyyy} at {newDT:h:mm tt} (CST)"),
                new KeyValuePair<string, string>("REMOVED_USERNAMES", emailsDeleted)
            };
            var customerDefinition = _globalConfigurationManager.CustomerDefinitionDuplicateRegistration;
            return _exactTargetClient.SendExactTargetService(customerDefinition,
                emailSelected,
                parameters,
                accountMembership.Mdsid.PadLeft(9, '0'));
        }

        
        /// <summary>
        /// Validate the user data
        /// </summary>
        /// <param name="model">Model to return the errors</param>
        /// <param name="viewData">Viewdata</param>
        public void SetErrorProfile(ProfileDTO model, ViewDataDictionary viewData)
        {
            SetErrorUserBasicData(model, viewData);
            model.ErrorsAddress =
                ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.Address)], true, true, true);
            model.ErrorsCity = ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.City)], true, true, true);
            model.ErrorsState =
                ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.State)], true, false, false);
            model.ErrorsPhone =
                ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.Phone)], false, true, true);
            model.ErrorsEmail.AddRange(ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.Email)], true, true, true));
        }

        /// <summary>
        /// Validate user data (first name, last name, zip code)
        /// </summary>
        /// <param name="model">Model to return the errors</param>
        /// <param name="viewData">Viewdata</param>
        public void SetErrorUserBasicData<T>(T model, ViewDataDictionary viewData) where T : IUsernameBasicDTO
        {
            model.ErrorsFirstName =
                ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.FirstName)], true, true, true);
            model.ErrorsLastName =
                ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.LastName)], true, true, true);
            model.ErrorsZip = ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.Zip)], true, true, true);
        }

        /// <summary>
        /// Execute special validation in registration forms
        /// </summary>
        /// <param name="model">Registration form data</param>
        /// <returns>True in case of errors: otherwise false</returns>
        public bool HasDateBirthCustomValidationErrors<T>(T model) where T : IDateBirthDTO
        {
            if (string.IsNullOrEmpty(model.BirthDate))
            {
                model.ErrorsBirthDate.Add(ErrorStatusEnum.Required);
            }
            try
            {
                var resultConvert = DateTime.ParseExact(model.BirthDate, "MMddyyyy", CultureInfo.InvariantCulture);
                var resultVal = ValidationFieldHelper.ValidateBirthDate(resultConvert);
                if (!resultVal)
                {
                    model.ErrorsBirthDate.Add(ErrorStatusEnum.AgeRequirement);
                }
            }
            catch (FormatException ex)
            {
                model.ErrorsBirthDate.Add(ErrorStatusEnum.InvalidDate);
                Log.Error("HasDateBirthCustomValidationErrors Error:", ex, this);
            }

            return model.ErrorsBirthDate.Count > 0;
        }

        public bool HasEmailDomainValidationErrors(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            var lowerEmail = email.ToLower();
            return lowerEmail.Contains(".com.com");
        }

        public bool HasEmailCustomValidationErrors(ProfileBasicDTO model)
        {
            if (model?.Email != null)
            {
                var domain = model.Email.Substring(model.Email.LastIndexOf('@') + 1);
                if (domain.Contains(".edu") || domain.Contains("k12"))
                {
                    model.ErrorsEmail.Add(ErrorStatusEnum.Warning);
                }
            }
            return model.ErrorsEmail.Any(x => x != ErrorStatusEnum.Warning);
        }

        /// <summary>
        /// Execute special validation in registration forms
        /// </summary>
        /// <param name="model">Registration form data</param>
        /// <returns>True in case of errors: otherwise false</returns>
        public bool HasPasswordCustomValidationErrors<T>(T model) where T : IPasswordDTO
        {
            if (!ValidationFieldHelper.ValidatePassword(model.Password))
            {
                model.ErrorsPassword.Add(ErrorStatusEnum.PasswordRequirement);
            }

            if (!ValidationFieldHelper.ValidatePassword(model.ConfirmPassword))
            {
                model.ErrorsConfirmPassword.Add(ErrorStatusEnum.PasswordRequirement);
            }
            //Validate the password matches with confirm password
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                model.ErrorsConfirmPassword.Add(ErrorStatusEnum.PasswordNotEqual);
            }
            return model.ErrorsPassword.Count > 0 || model.ErrorsConfirmPassword.Count > 0;
        }

        /// <summary>
        /// Send the email when the account is recent locked
        /// </summary>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="individualId">Mds Inv id</param>
        /// <param name="newUsername">New login</param>
        /// <param name="oldUsername">Old login</param>
        /// <param name="msrName"></param>
        /// <returns>Response from Exact target</returns>
        public void SendExactTargetChangeUserName(
            string firstName,
            string lastName,
            string individualId,
            string newUsername,
            string oldUsername,
            string newcellParam,
            string oldcellParam            
        )
        {
            var customerDefinition = _globalConfigurationManager.CustomerDefinitionChangeUsername;
            var cellCodeNewLogin = string.IsNullOrEmpty(newcellParam) ? _globalConfigurationManager.CellcodeChangeUsernameNewLogin : newcellParam;
            var cellCodeOldLogin = string.IsNullOrEmpty(oldcellParam) ? _globalConfigurationManager.CellcodeChangeUsernameOldLogin : oldcellParam;
            var campaignCd = _globalConfigurationManager.CampaignCodeChangeUsername;
            var targetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var newDt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
            var parameters = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("EmailAddress", newUsername),
                new KeyValuePair<string, string>("FIRST_NAME", firstName),
                new KeyValuePair<string, string>("LAST_NAME", lastName),
                new KeyValuePair<string, string>("CELL_CODE", cellCodeNewLogin),
                new KeyValuePair<string, string>("CAMPAIGN_CD", campaignCd),
                new KeyValuePair<string, string>("INDIVIDUAL_ID", individualId),
                new KeyValuePair<string, string>("NEW_USERNAME", newUsername),
                new KeyValuePair<string, string>("OLD_USERNAME", oldUsername),                
            };
            _exactTargetClient.SendExactTargetService(customerDefinition, newUsername, parameters);
            parameters.RemoveAll(item => item.Key == "CELL_CODE");
            parameters.Add(new KeyValuePair<string, string>("CELL_CODE", cellCodeOldLogin));
            parameters.RemoveAll(item => item.Key == "EmailAddress");
            parameters.Add(new KeyValuePair<string, string>("EmailAddress", oldUsername));
            _exactTargetClient.SendExactTargetService(customerDefinition, oldUsername, parameters);
        }

        /// <summary>
        /// Send the email when it is added a new family member
        /// </summary>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="mdsid">Mds Inv id</param>
        /// <param name="username">User login</param>
        /// <returns>Response from Exact target</returns>
        public bool SendExactTargetAddFamilyMember(
            string firstName,
            string lastName,
            string mdsid,
            string username
        )
        {
            var customerDefinition = _globalConfigurationManager.CustomerDefinitionAddFamilyMember;
            var cellCode = _globalConfigurationManager.CellcodeAddFamilyMember;
            var campaignCd = _globalConfigurationManager.CampaignAddFamilyMember;
            var parameters = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("FIRST_NAME", firstName),
                new KeyValuePair<string, string>("LAST_NAME", lastName),
                new KeyValuePair<string, string>("CELL_CODE", cellCode),
                new KeyValuePair<string, string>("CAMPAIGN_CD", campaignCd),
                new KeyValuePair<string, string>("INDIVIDUAL_ID", mdsid)
            };
            var resultExactTarget = _exactTargetClient.SendExactTargetService(customerDefinition, username, parameters, mdsid);
            return resultExactTarget.Equals("OK");
        }
        /// <summary>
        /// Get the path of the invite family page
        /// </summary>
        /// <returns></returns>
        public string GetInviteFamilyPageUrl()
        {
            return LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.InviteFamilyPage.ID));
        }

        /// <summary>
        /// Set the gtm action according the model data to see if the operation failed or not
        /// </summary>
        /// <param name="model">Model data</param>
        public void SetGtmActionRegistration(RegistrationDTO model)
        {
            if (!model.IsValid || model.HasGeneralError)
            {
                model.GtmAction = _registrationManager.GetGtmActionRegistration(RegistrationEventResultEnum.Failed);
            }
            else if (!model.HasDuplicateAccount)
            {
                model.GtmAction = _registrationManager.GetGtmActionRegistration(RegistrationEventResultEnum.Success);
            }
        }

        /// <summary>
        /// Verify if the registration page has value in the success rule field and this get the url to be redirected
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string HandleItemRedirectionSuccessRule(RegistrationDTO model)
        {
            string itemRedirectionUrl = "";
            //Get the url to be redirected
            var itemRedirection = _registrationRedirection.GetItemRedirection();
            if (itemRedirection != null && itemRedirection.ID != ID.Null && itemRedirection.ID != ID.Undefined)
            {
                //Append the previous page to the url
                itemRedirectionUrl = LinkManager.GetItemUrl(itemRedirection);
                if (!String.IsNullOrEmpty(model.RequestedPage))
                {
                    itemRedirectionUrl = $"{itemRedirectionUrl}?returnurl={model.RequestedPage}";
                }
                //Store in session the url and item redirection id
                _sessionManager.StoreInSession<string>(ConstantsNeamb.RegistrationRedirectionSuccessRule, itemRedirectionUrl);
                _sessionManager.StoreInSession<string>(ConstantsNeamb.RegistrationRedirectionItemId, itemRedirection.ID.ToString());
            }
            return itemRedirectionUrl;
        }

        /// <summary>
        /// Manage the response obtained from Search Username webservice
        /// </summary>
        /// <param name="searchUserNameResponse"></param>
        /// <param name="model"></param>
        private void HandleErrorValidateUserName(SearchUserNameResponse searchUserNameResponse, RegistrationDTO model)
        {
            if (!searchUserNameResponse.Success)
            {
                //email has invalid format
                if (searchUserNameResponse.ErrorCode == SearchUsernameErrorCodes.InputDataValidationError)
                    model.ErrorsEmail.Add(ErrorStatusEnum.EmailFormat);
            }
            else if (searchUserNameResponse.Data.Registered)
            {
                //Email is not available				
                model.ErrorsEmail.Add(ErrorStatusEnum.EmailInUse);
            }
            else
            {
                model.HasGeneralError = true;
            }
        }
        /// <summary>
		/// Logic to execute the registration actions
		/// </summary>
		/// <param name="model">User data input by the user in registration form</param>
		/// <param name="viewData">View dictionary data</param>
		/// <param name="isModelValid">Flag to know if the model is valid</param>
		/// <returns></returns>
		public string ExecuteRegistration(RegistrationDTO model, ViewDataDictionary viewData, bool isModelValid)
        {
            string resultRedirection = "";
            try
            {
                var hasdomainErrors = HasEmailDomainValidationErrors(model.Email);
                if (hasdomainErrors)
                {
                    model.ErrorsEmail.Add(ErrorStatusEnum.InvalidCharacters);
                }

                var customEmailErrors = HasEmailCustomValidationErrors(model);
                var customDateBirthErrors = HasDateBirthCustomValidationErrors(model);
                var customPasswordErrors = HasPasswordCustomValidationErrors(model);

                if (isModelValid && !customEmailErrors && !customPasswordErrors && !customDateBirthErrors && !hasdomainErrors)
                {
                    var accountMembership = new AccountMembership();
                    SearchUserNameResponse searchUserNameResponse = new SearchUserNameResponse();
                    if (ValidateUserName(ref searchUserNameResponse, model))
                    {
                        if (CallRegisterUser(model, accountMembership))
                        {
                            var resultAuthentication = AuthenticationAfterRegistration(accountMembership, model);
                            if (resultAuthentication == RegistrationStatus.Success)
                            {
                                resultRedirection = HandleItemRedirectionSuccessRule(model);
                            }
                            else if (resultAuthentication == RegistrationStatus.Duplicated)
                            {
                                model.HasDuplicateAccount = true;
                                resultRedirection = GetDuplicateRegistrationPageUrl();
                                _sessionManager.StoreInSession<string>(ConstantsNeamb.RegistrationRedirectionSuccessRule, "duplication");
                                _sessionManager.StoreInSession<string>(ConstantsNeamb.RegistrationRedirectionItemId, Templates.DuplicateRegistrationPage.ID.ToString());
                            }
                            else
                            {
                                model.HasGeneralError = true;
                            }
                        }
                        else
                        {
                            model.HasGeneralError = true;
                        }
                    }
                    else
                    {
                        model.IsValid = false;
                        HandleErrorValidateUserName(searchUserNameResponse, model);
                    }
                }
                else
                {
                    model.IsValid = false;
                    SetErrorProfile(model, viewData);
                }
                return resultRedirection;
            }
            catch (NeambWebServiceException ex)
            {
                Log.Error(ex.Message, ex, this);
                model.HasGeneralError = true;
                return resultRedirection;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex, this);
                model.HasGeneralError = true;
                return resultRedirection;
            }
        }

        /// <summary>
        /// Call the webservice method SearchUserName to see if the email is valid or not
        /// </summary>
        /// <param name="isUsernameAvailableDto">Response of the webservice method call</param>
        /// <param name="model">Registration model</param>
        /// <returns></returns>

        public bool ValidateUserName(ref SearchUserNameResponse isUsernameAvailableDto, RegistrationDTO model)
        {
            if (isUsernameAvailableDto == null) throw new ArgumentNullException(nameof(isUsernameAvailableDto));

            isUsernameAvailableDto = _searchUserNameService.SearchUserName(model.Email);
            return (isUsernameAvailableDto != null && isUsernameAvailableDto.Success &&
                (isUsernameAvailableDto.Data.Registered == false));
        }
        /// <summary>
        /// Set the accountMembership with the data input in registration form
        /// </summary>
        /// <param name="model">Registration form data</param>
        /// <param name="accountMembership">Form data processed in another object</param>
        /// <returns></returns>
        public bool CallRegisterUser(RegistrationDTO model, AccountMembership accountMembership)
        {
            accountMembership.Username = model.Email;
            accountMembership.Profile.FirstName = model.FirstName;
            accountMembership.Profile.LastName = model.LastName;
            accountMembership.Profile.DateOfBirth = model.BirthDate;
            accountMembership.Profile.StreetAddress = model.Address;
            accountMembership.Profile.City = model.City;
            accountMembership.Profile.StateCode = model.State;
            accountMembership.Profile.ZipCode = model.Zip;
            accountMembership.Profile.Phone = !string.IsNullOrEmpty(model.Phone) ? model.Phone.Replace(" ", string.Empty) : string.Empty;
            accountMembership.Profile.Email = model.Email;
            accountMembership.Profile.EmailPermissionIndicator = model.OptIn
                ? ConstantsNeamb.EmailPermissionChecked
                : ConstantsNeamb.EmailPermissionUnChecked;

            var registerUserDto = RegisterUser(accountMembership, model.Password);
            return (registerUserDto != null &&
                registerUserDto.Success && registerUserDto.Data != null && registerUserDto.Data.Registered);
        }

        /// <summary>
        /// Process respose obtained after the call to webservice to register the new user
        /// </summary>
        /// <param name="accountMembership">Data of the user registered</param>
        /// <param name="model">Model to be returned to the view</param>
        /// <returns></returns>
        public RegistrationStatus AuthenticationAfterRegistration(AccountMembership accountMembership, RegistrationDTO model)
        {
            var loginServiceResponse =
                AuthenticateUser(accountMembership,
                    accountMembership.Username,
                    model.Password,
                    string.Empty,
                    false);
            if (loginServiceResponse != null && loginServiceResponse.Data != null && loginServiceResponse.Data.LoggedIn)
            {
                long mdsidInt = -1;
                if (loginServiceResponse.Data.RegistrationCount <= 1 || (long.TryParse(accountMembership.Mdsid, out mdsidInt) && mdsidInt == 0))
                {
                    //Successfully logged
                    if (accountMembership.Status == StatusEnum.Hot || accountMembership.Status == StatusEnum.WarmCold)
                    {
                        SendExactTargetRegisterEmail(accountMembership.Username,
                            accountMembership.Mdsid,
                            accountMembership.Profile.FirstName,
                            accountMembership.Profile.LastName,
                            accountMembership.Profile.EmailPermissionIndicator,
                            accountMembership.Profile.NewEnvInd,
                            accountMembership.Profile.NeaMembershipType,
                            accountMembership.Profile.ComplifesignDate,
                            accountMembership.Profile.IsNeaCurrentMember,
                            accountMembership.Profile.Newmembersegmentindicator);
                        model.ProcessedSucessfully = true;
                        return RegistrationStatus.Success;

                    }
                    else
                    {
                        model.HasGeneralError = true;
                        return RegistrationStatus.Error;
                    }
                }
                else
                {
                    //Store in session the url and item redirection id
                    _sessionAuthenticationManager.SaveDuplicateRegistration(loginServiceResponse.Data.Registrations);
                    _sessionAuthenticationManager.SaveDuplicateRegistrationEmail(model.Email.ToLower());
                    _sessionAuthenticationManager.SaveDuplicateRegistrationPassword(model.Password);
                    _authenticationAccountManager.LogoutUser(false);
                    return RegistrationStatus.Duplicated;
                }
            }
            model.HasGeneralError = true;
            return RegistrationStatus.Error;
        }
    }
}