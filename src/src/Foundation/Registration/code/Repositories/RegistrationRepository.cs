using System;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;
using Neambc.Neamb.Foundation.MBCData.Services.RegisterUser;
using Neambc.Seiumb.Foundation.Registration.Interfaces;
using Neambc.Seiumb.Foundation.WebServices;


namespace Neambc.Seiumb.Foundation.Registration.Repositories
{
    [Service(typeof(IRegistrationRepository))]
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly IAccountServiceProxy _neambServiceManager;
		private readonly IWebServicesConfiguration _config;
        private readonly IRegisterUserService _registerUserService;
        public RegistrationRepository(IAccountServiceProxy neambServiceManager, IWebServicesConfiguration config,
            IRegisterUserService registerUserService)
        {
            _neambServiceManager = neambServiceManager;
			_config = config;
            _registerUserService = registerUserService;
        }

        public RegisterUserResponse RegisterUser(string firstName, string lastName, string streetAddress, string city, string stateCode,
            string zipCode, string dob, string phone, string username, string password, string permissionIndicator,
            string campcode, string cellCode)
        {
            return _registerUserService.RegisterUserData(firstName, lastName, streetAddress, city, stateCode, zipCode, dob, phone, username, password, permissionIndicator, campcode, cellCode, Convert.ToInt32(_config.UnionId), _config.Webusersource);
        }
        
    }
}