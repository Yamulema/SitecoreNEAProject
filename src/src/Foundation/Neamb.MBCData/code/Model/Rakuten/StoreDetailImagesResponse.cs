using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rakuten
{
    public class StoreDetailImagesResponse
    {
        public string Banner { get; set; }
        public string SmallLogo { get; set; }
        public string Thumbnail { get; set; }
        [JsonProperty("icon-112x30")]
        public string Icon11230 { get; set; }
        [JsonProperty("icon-224x60")]
        public string Icon22460 { get; set; }
        [JsonProperty("icon-336x90")]
        public string Icon33690 { get; set; }
        [JsonProperty("logo-email")]
        public string LogoEmail { get; set; }
        [JsonProperty("logo-mobile")]
        public string LogoMobile { get; set; }
        [JsonProperty("logo-mobile2x")]
        public string LogoMobile2x { get; set; }
        [JsonProperty("logo-mobile3x")]
        public string LogoMobile3x { get; set; }
        [JsonProperty("feed_square_logo")]
        public string FeedSquareLogo { get; set; }
    }
}
