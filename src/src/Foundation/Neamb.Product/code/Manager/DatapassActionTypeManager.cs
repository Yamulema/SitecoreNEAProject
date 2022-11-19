using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(IDatapassActionTypeManager))]
	public class DatapassActionTypeManager : QueryStringManager, IDatapassActionTypeManager {
		private readonly IEligibilityManager _eligibilityManager;
		private readonly IUserStatusHandler _userStatusHandler;
		private readonly ISessionManager _sessionManager;
		private readonly IPipelineService _pipelineService;

		public DatapassActionTypeManager(IEligibilityManager eligibilityManager,
			IUserStatusHandler userStatusHandler, ISessionManager sessionManager,
			IPipelineService pipelineService) {
			_eligibilityManager = eligibilityManager;
			_userStatusHandler = userStatusHandler;
			_sessionManager = sessionManager;
			_pipelineService = pipelineService;
		}

		public OperationResult GetUrlDatapass(DatapassModel datapassModel) {
			var operationResult = new OperationResult();


			var resultEligibility =
				_eligibilityManager.IsMemberEligible(datapassModel.AccountUser.Mdsid, datapassModel.ProductCode);
			if (resultEligibility == EligibilityResultEnum.Eligible) {
				var resultPipeline = _pipelineService.RunProcessPipelines(datapassModel.ProductCode, datapassModel.AccountUser,
					ConstantsNeamb.PipelineNameDatapass);
				if (!string.IsNullOrEmpty(resultPipeline.ActionPrimary) || !string.IsNullOrEmpty(resultPipeline.ActionSecondary)) {
					if (datapassModel.PrimarySecondaryActionType.Equals(ConstantsNeamb.DataFirst)) {
						operationResult.Url = resultPipeline.ActionPrimary;
					} else {
						operationResult.Url = !string.IsNullOrEmpty(resultPipeline.ActionSecondary)
							? resultPipeline.ActionSecondary
							: resultPipeline.ActionPrimary;
					}

					if (datapassModel.ComponentType == (int)ComponentTypeEnum.SpecialOffer) {
						operationResult.Url = AppendQueryStringParameter(operationResult.Url, _sessionManager);
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