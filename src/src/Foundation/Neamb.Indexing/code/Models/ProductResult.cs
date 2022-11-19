using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Indexing.Models
{
    public class ProductResult : BaseModel
    {
        [IndexField("productcode_t")]
        public string _ProductCode { get; set; }

        [IndexField("_parent")]
        public string _Parent { get; set; }

        [IndexField("product_code_sm")]
        public string[] ProductCodeSm { get; set; }
    }
}