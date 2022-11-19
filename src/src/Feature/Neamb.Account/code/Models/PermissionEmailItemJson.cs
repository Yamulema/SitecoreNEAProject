using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using Newtonsoft.Json;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class PermissionEmailItemJson 
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("indicator")]
        public string Indicator { get; set; }        
    }
}