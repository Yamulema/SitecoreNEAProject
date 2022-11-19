
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.SchemaMarkup.Models
{
    public class ArticleModel
    {
        [JsonProperty("@context")]
        public string Context { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("mainEntityOfPage")]
        public MainEntityOfPage MainEntityOfPage { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Image { get; set; }

        [JsonProperty("datePublished")]
        public DateTime DatePublished { get; set; }

        [JsonProperty("dateModified")]
        public DateTime DateModified { get; set; }

        [JsonProperty("author")]
        public AuthorModel Author { get; set; }

        public ArticleModel()
        {
            MainEntityOfPage = new MainEntityOfPage();
            //Image = new List<string>();
            Author = new AuthorModel();
        }
    }



}