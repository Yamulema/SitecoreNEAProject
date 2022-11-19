using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife;
using Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility;

namespace Neambc.Neamb.Foundation.Eligibility.Manager
{
    [Service(typeof(IEligibilityManager))]
    public class EligibilityManager : IEligibilityManager
    {
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IProductEligibilityService _productEligibilityService;
        private readonly ICompIntroLifeService _compIntroProductService;

        public EligibilityManager(IGlobalConfigurationManager globalConfigurationManager,
            IProductEligibilityService productEligibilityService, ICompIntroLifeService compIntroProductService)
        {
            _globalConfigurationManager = globalConfigurationManager;
            _productEligibilityService = productEligibilityService;
            _compIntroProductService = compIntroProductService;
        }

        public EligibilityResultEnum IsMemberEligible(string mdsid, string productcode, int months = 12)
        {
            int.TryParse(mdsid, out int mdsidInt);
            var compIntroResult = _compIntroProductService.GetCompIntroEligibility(mdsid);

            if (productcode.Equals(_globalConfigurationManager.ComplimentaryLifeProductCode,
                StringComparison.InvariantCultureIgnoreCase))
                return compIntroResult.CompEligible ? EligibilityResultEnum.Eligible : EligibilityResultEnum.NotEligible;

            if (productcode.Equals(_globalConfigurationManager.IntroLifeProductCode,
                StringComparison.InvariantCultureIgnoreCase))

                return compIntroResult.IntroEligible ? EligibilityResultEnum.Eligible : EligibilityResultEnum.NotEligible;
            var resultEligibility= _productEligibilityService.GetEligibility(mdsidInt, productcode);
            return resultEligibility ? EligibilityResultEnum.Eligible : EligibilityResultEnum.NotEligible;
        }
    }
}