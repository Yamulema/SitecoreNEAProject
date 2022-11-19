using System.Linq;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	[Service(typeof(IBaseUrlActionOmni))]
	public class BaseUrlActionOmni : IBaseUrlActionOmni
    {
        private readonly IEligibilityOmni _eligibilityOmni;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        public BaseUrlActionOmni(IEligibilityOmni eligibilityOmni, ISessionAuthenticationManager sessionAuthenticationManager)
        {
            _eligibilityOmni = eligibilityOmni;
            _sessionAuthenticationManager = sessionAuthenticationManager;
        }

        public string GetBaseUrlPartner(string productCode)
		{
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            if (accountMembership.Status == StatusEnum.Hot ||
                accountMembership.Status == StatusEnum.WarmCold ||
                accountMembership.Status == StatusEnum.WarmHot) {
                var resultEligibilityOmni = _eligibilityOmni.CheckEligibility(accountMembership.Mdsid, productCode);
                return resultEligibilityOmni != null ? resultEligibilityOmni.FirstOrDefault()?.WebAppUrl : "";
            } else {
                return "";
            }
            
		}
	}
}