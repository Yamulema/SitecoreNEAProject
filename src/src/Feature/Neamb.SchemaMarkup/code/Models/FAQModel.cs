using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.SchemaMarkup.Models
{
    public class FAQModel
    {
        [JsonProperty("@context")]
        public string Context { get; set; }


        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("mainEntity")]
        public List<QuestionFAQModel> MainEntity { get; set; }

        public FAQModel()
        {
            MainEntity = new List<QuestionFAQModel>();
        }
    }
}