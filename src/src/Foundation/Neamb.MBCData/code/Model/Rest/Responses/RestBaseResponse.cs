using System.Net;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses
{
    public class RestBaseResponse
    {
        public bool Success;
        public HttpStatusCode StatusCode;
        public string ExceptionDetail;
        public RestError Error { get; set; }
    }
}