using Newtonsoft.Json;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.SchemaMarkup.Models
{
    public class BreadCrumbModel
    {
        [JsonProperty("@context")]
        public string Context { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("itemListElement")]
        public List<BreadCrumbItemModel> ItemElementList { get; set; }

        public BreadCrumbModel()
        {
            ItemElementList = new List<BreadCrumbItemModel>();
        }
    }
}