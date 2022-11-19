
namespace Neambc.Neamb.Feature.Product.Model
{
	public class ProductRedirectRequest
    {
        public string UtmSource { get; set; }
        public string UtmCampaign { get; set; }
        public string UtmTerm { get; set; }
        public string UtmMedium { get; set; }
        public string Sob { get; set; }
        public string Gclid { get; set; }
        public string ProductCode { get; set; }
        public string Mdsid { get; set; }
    }
}