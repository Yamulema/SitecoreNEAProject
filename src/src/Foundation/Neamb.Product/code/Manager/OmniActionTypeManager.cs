using System;
using System.Linq;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(IOmniActionTypeManager))]
	public class OmniActionTypeManager : IOmniActionTypeManager
    {

		#region Fields
		private readonly IEligibilityManager _eligibilityManager;
		private readonly IEligibilityOmni _eligibilityOmni;
		#endregion

		#region Constructor
		public OmniActionTypeManager(
			IEligibilityManager eligibilityManager,
            IEligibilityOmni eligibilityOmni) {
			_eligibilityManager = eligibilityManager;
            _eligibilityOmni = eligibilityOmni;
		}
		#endregion

		#region Public Methods
		public OperationResult GetUrl(OmniLinkModel ominLinkModel) {
            var operationResult = new OperationResult();

            var resultEligibility =
                _eligibilityManager.IsMemberEligible(ominLinkModel.AccountUser.Mdsid, ominLinkModel.ProductCodeLink);
            if (resultEligibility == EligibilityResultEnum.Eligible)
            {
                var resultEligibilityOmni= _eligibilityOmni.CheckEligibility(ominLinkModel.AccountUser.Mdsid, ominLinkModel.ProductCodeLink);
                operationResult.Url = resultEligibilityOmni!=null? resultEligibilityOmni.FirstOrDefault()?.WebAppUrl:"";
            }
            else
            {
                operationResult.ResultUrl = ResultUrlEnum.UnForbidden;
            }

            return operationResult;
        }
		#endregion
	}
}