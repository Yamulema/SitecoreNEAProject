using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler
{
    public interface IResponseHandler
    {
        void LogErrorResponse(RestError errorResponse, string method, ILog logService);
    }
}
