using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.SchemaMarkup.Models
{
    public class QuestionFAQModel
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("acceptedAnswer")]
        public AnswerFAQModel AcceptedAnswer { get; set; }

        public QuestionFAQModel()
        {
            AcceptedAnswer = new AnswerFAQModel();
        }
    }
}