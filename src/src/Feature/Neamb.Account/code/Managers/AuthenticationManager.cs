using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Feature.Account.Managers
{
    [Service(typeof(IAuthenticationManager))]
    public class AuthenticationManager : IAuthenticationManager
    {

        private readonly IAccountRepository _accountRepository;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IAuthenticationAccountManager _authenticationAccountManager;


		public AuthenticationManager(IAccountRepository accountRepository, ISessionAuthenticationManager sessionAuthenticationManager, IAuthenticationAccountManager authenticationAccountManager)
        {
            _accountRepository = accountRepository;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _authenticationAccountManager = authenticationAccountManager;
		}

		/// <summary>
		/// Execute the authentication for checking the info against the webservice
		/// </summary>
		/// <param name="modelstate">State of the view</param>
		/// <param name="ckbrememberme">Remember option given in the screen</param>
		/// <param name="pathReset">Path for reseting the password</param>
		/// <param name="modelaccount">Model</param>
		/// <param name="viewData">View data to evaluate errors</param>
		/// <param name="executeLogout">Flag to logout the user</param>
		/// <returns></returns>
		public AuthenticationResultEnum ExecuteAuthentication(ModelStateDictionary modelstate, string ckbrememberme, string pathReset, AccountDTO modelaccount, ViewDataDictionary viewData, bool executeLogout=true)
        {
            var result = AuthenticationResultEnum.None;
            if (modelstate.IsValid)
            {
                try
                {
                    var rememberme = !string.IsNullOrEmpty(ckbrememberme) && ckbrememberme.Equals("on") ? true : false;

                    var account = new AccountMembership();
					var responseService = _accountRepository.AuthenticateUser(account, modelaccount.Email, modelaccount.Password, pathReset, rememberme,executeLogout);

	                if (responseService != null && responseService.Data!=null && responseService.Data.LoggedIn)
                    {
						//Case no duplicate registration
						if (responseService.Data.RegistrationCount <= 1 || (responseService.Data.MdsId == 0))
	                    {
						    switch (account.Status)
                            {
                                case StatusEnum.Unknown:
                                case StatusEnum.Cold:
                                    {
                                        modelaccount.HasErrorInvalidCredentials = true;
                                        break;
                                    }
                                case StatusEnum.Hot:
                                    {
                                        modelaccount.IsValid = true;
                                        break;
                                    }
                                default:
                                    {
                                        Sitecore.Diagnostics.Log.Info("Default condition " + account.Status, this);
                                        modelaccount.HasErrorTimeout = true;
                                        break;
                                    }
                            }
                        }
                        //case duplicate registration
                        else
                        {
							Log.Info("Duplicate registration ",this);
                            _sessionAuthenticationManager.SaveDuplicateRegistration(responseService.Data.Registrations);
                            _sessionAuthenticationManager.SaveDuplicateRegistrationEmail(modelaccount.Email.ToLower());
                            _sessionAuthenticationManager.SaveDuplicateRegistrationPassword(modelaccount.Password);
                            _authenticationAccountManager.LogoutUser();
                            result = AuthenticationResultEnum.Duplicated;
                        }
                    }
                    //No return response from service LoginUser
                    else
                    {
                        switch (account.Status)
                        {
                            case StatusEnum.Unknown:
                            case StatusEnum.Cold:
                            {
                                modelaccount.HasErrorInvalidCredentials = true;
                                break;
                            }
                            case StatusEnum.LockedNewToken:
                            {
                                modelaccount.HasLockedError = true;
                                break;
                            }
                            case StatusEnum.LockedOldToken:
                            {
                                modelaccount.HasAlreadyLockedErrorTokenValid = true;
                                break;
                            }
                            default:
                            {
                                Sitecore.Diagnostics.Log.Info("Default condition " + account.Status, this);
                                modelaccount.HasErrorTimeout = true;
                                break;
                            }
                        }
                    }
                }
                catch (NeambWebServiceException ex)
                {
                    Log.Error(ex.Message, ex, this);
                    modelaccount.HasErrorTimeout = true;
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Fatal(ex.Message, ex, this);
                    modelaccount.HasErrorTimeout = true;
                }

	            if (result == AuthenticationResultEnum.None)
	            {
		            result = modelaccount.IsValid
			            ? AuthenticationResultEnum.Valid
			            : AuthenticationResultEnum.ErrorFromService;
	            }
            }
            else
            {
                //Errors with the validation of the form
                var modelStateVal = viewData.ModelState["Email"];
                modelaccount.HasErrorUserName = modelStateVal.Errors.Count > 0;
                modelStateVal = viewData.ModelState["Password"];
                modelaccount.HasErrorPassword = modelStateVal.Errors.Count > 0;
                result = AuthenticationResultEnum.ErrorNotValid;
            }

            return result;
        }
    }
}