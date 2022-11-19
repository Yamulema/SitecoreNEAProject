using Newtonsoft.Json;


namespace Neambc.Neamb.Feature.SchemaMarkup.Models
{
    public class BreadCrumbItemModel
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("item")]
        public string Item { get; set; }
    }
}