using System.Net;
using System.Web.Mvc;

namespace Neambc.Seiumb.Foundation.SitecoreExtensions.Extensions
{
    public class HttpForbiddenResult : HttpStatusCodeResult
    {
        public HttpForbiddenResult()
            : this(null)
        {
        }

        public HttpForbiddenResult(string statusDescription)
            : base(HttpStatusCode.Forbidden, statusDescription)
        {
        }
    }
}