using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(ISsoActionTypeManager))]
	public class SsoActionTypeManager : QueryStringManager, ISsoActionTypeManager {
		private readonly IEligibilityManager _eligibilityManager;
		private readonly IUserStatusHandler _userStatusHandler;
		private readonly ISessionManager _sessionManager;
		private readonly IPipelineService _pipelineService;
		public SsoActionTypeManager(IEligibilityManager eligibilityManager,
			IUserStatusHandler userStatusHandler, ISessionManager sessionManager,
			IPipelineService pipelineService) {
			_eligibilityManager = eligibilityManager;
			_userStatusHandler = userStatusHandler;
			_sessionManager = sessionManager;
			_pipelineService = pipelineService;

		}

		public OperationResult GetUrlSso(SsoModel ssoModel) {
			var operationResult = new OperationResult();
			var resultEligibility = _eligibilityManager.IsMemberEligible(ssoModel.AccountUser.Mdsid, ssoModel.ProductCode);
			if (resultEligibility == EligibilityResultEnum.Eligible) {
				var resultPipeline =
					_pipelineService.RunProcessPipelines(ssoModel.ProductCode, ssoModel.AccountUser, ConstantsNeamb.PipelineNameSso);

				if (!string.IsNullOrEmpty(resultPipeline.ActionPrimary)) {
					if (ssoModel.ComponentType == (int)ComponentTypeEnum.SpecialOffer) {
						operationResult.Url = AppendQueryStringParameter(resultPipeline.ActionPrimary, _sessionManager);
					} else {
						operationResult.Url = resultPipeline.ActionPrimary;
					}
				} else {
					operationResult.ResultUrl = ResultUrlEnum.NoUrl;
				}
			} else {
				operationResult.ResultUrl = ResultUrlEnum.UnForbidden;
			}

			return operationResult;
		}
	}
}