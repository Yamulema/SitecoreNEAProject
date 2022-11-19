using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests
{
    public class RestRequestDto
    {
        public string Server { get; set; }
        public string Token { get; set; }
        public string Action { get; set; }
        public object Body { get; set; }
        public bool ParseJson { get; set; }
        public bool IsBasicAuthentication { get; set; }
        public List<KeyValuePair<string,string>> Parameters { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
    }
}