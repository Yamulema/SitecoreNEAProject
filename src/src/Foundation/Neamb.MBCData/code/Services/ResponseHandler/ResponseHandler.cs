using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler {
    [Service(typeof(IResponseHandler))]
    public class ResponseHandler: IResponseHandler
    {
        public void LogErrorResponse(RestError errorResponse, string method, ILog logService) {
            if (errorResponse != null) {
                string errorCodeMessage = $"Error code from {method}: {errorResponse.Code}";
                string errorMessageConcatenated = string.Join(",", errorResponse.Messages);
                string errorMessage = $"Error code from {method}: {errorMessageConcatenated}";
                switch (errorResponse.Code) {
                    case 10001:
                    case 10002:
                    case 12005:
                    case 13001: {
                        logService.Warn(errorCodeMessage, this);
                        logService.Warn(errorMessage, this);
                        break;
                    }
                    case 12002:
                    case 12003:
                    case 12004: {
                        logService.Info(errorCodeMessage, this);
                        logService.Info(errorMessage, this);
                        break;
                    }
                    default: {
                        logService.Error(errorCodeMessage, this);
                        logService.Error(errorMessage, this);
                        break;
                    }
                }
            } else {
                logService.Error("Method LogErrorResponse is receiving errorResponse null", this);
            }
        }
    }
}