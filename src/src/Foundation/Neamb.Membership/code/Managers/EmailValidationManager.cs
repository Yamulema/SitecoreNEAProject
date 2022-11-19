using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Services.ValidateEmailDomain;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Managers {
	[Service(typeof(IEmailValidationManager))]
	public class EmailValidationManager : IEmailValidationManager {
		#region Private methods
		private readonly ISearchUserNameService _searchUserNameService;
		private readonly IValidateEmailDomain _validateEmailDomain;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        #endregion

        #region Constructors
        public EmailValidationManager(IValidateEmailDomain validateEmailDomain, ISearchUserNameService searchUserNameService, ISessionAuthenticationManager sessionAuthenticationManager) {
            _validateEmailDomain = validateEmailDomain;
            _searchUserNameService = searchUserNameService;
            _sessionAuthenticationManager = sessionAuthenticationManager;
        }       
        #endregion

        /// <summary>
        /// Uses Neamb ValidateEmailDomain service to determine if an email is valid. 
        /// Used in Jquery remote validator
        /// </summary>
        /// <param name="email">Email to be validated</param>
        /// <param name="validateusername">Flag to validate with user name too</param>
        /// <returns>True for valid emails and the error type when invalid</returns>
        public ResultEmailValidation IsValid(string email, bool? validateusername) {
            ResultEmailValidation resultEmailValidation = new ResultEmailValidation();
            if (string.IsNullOrEmpty(email)) {
                resultEmailValidation.ErrorMessage = "EmailRequired";
                return resultEmailValidation;
            }

            resultEmailValidation = HasReservedDomain(email, resultEmailValidation);

            if (resultEmailValidation.IsValid) {
                var status = _validateEmailDomain.ValidateEmailDomainStatus(email);
                if (status != null && status.Success)
                {
                    if (!status.Data.valid)
                    {
                        resultEmailValidation.IsValid = false;
                        resultEmailValidation.ErrorMessage = "EmailDomainError";
                    }
                }
            }

            if (validateusername.HasValue && validateusername.Value) {
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                if(accountMembership.Profile.Email!= email)
                {
                    SearchUserNameResponse isUsernameAvailableDto = _searchUserNameService.SearchUserName(email);
                    if (isUsernameAvailableDto != null && isUsernameAvailableDto.Success)
                    {
                        if (isUsernameAvailableDto.Data.Registered)
                        {
                            resultEmailValidation.IsValid = false;
                            resultEmailValidation.ErrorMessage = "EmailAlreadyRegistered";
                        }
                    }
                }
            }
            return resultEmailValidation;
        }

        private ResultEmailValidation HasReservedDomain(string email, ResultEmailValidation validation)
        {
            validation.IsValid = true;
            var domain = email.Substring(email.LastIndexOf('@') + 1);
            if (domain.Contains("k12") || domain.Contains(".edu"))
            {
                validation.IsValid = false;
                validation.ErrorMessage = "EmailReservedDomainError";
            }
            return validation;
        }
    }
}