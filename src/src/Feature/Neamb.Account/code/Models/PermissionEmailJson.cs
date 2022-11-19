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
    public class PermissionEmailJson
    {
        [JsonProperty("indv_id")]
        public int IndvId { get; set; }
        [JsonProperty("business_unit")]
        public string BusinessUnit { get; set; }
        [JsonProperty("permissions")]
        public List<PermissionEmailItemJson> Permissions { get; set; }
    }
}