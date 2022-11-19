
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Product.Model
{
	public class ProductRedirectResponse
    {
        public string UrlRedirect { get; set; }
        public Dictionary<string, object> PostData { get; set; }
    }
}