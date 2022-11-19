
using System;

namespace Neambc.Neamb.Foundation.Product.Model
{
	[Serializable]
	public class PassthroughModel
    {
		public string UtmSource { get; set; }
		public string UtmCampaign { get; set; }
		public string UtmTerm { get; set; }
        public string UtmMedium { get; set; }
        public string Sob { get; set; }
        public string Gclid { get; set; }
    }
}