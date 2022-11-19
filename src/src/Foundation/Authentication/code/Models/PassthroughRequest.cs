using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Foundation.Authentication.Models
{
    public class PassthroughRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProductCode { get; set; }
        public IDictionary<string, string> QueryStringParameters { get; set; }
    }
}