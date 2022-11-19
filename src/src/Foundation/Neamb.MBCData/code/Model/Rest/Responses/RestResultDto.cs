using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses
{
    public class RestResultDto<T>
    {
        public T Result;
        public bool Success;
        public HttpStatusCode StatusCode;
        public string ExceptionDetail;
        public IList<Parameter> Headers;
    }
}