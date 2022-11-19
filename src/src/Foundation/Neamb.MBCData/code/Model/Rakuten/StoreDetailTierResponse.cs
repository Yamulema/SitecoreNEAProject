using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rakuten {
    public class StoreDetailTierResponse {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShoppingURL { get; set; }
        public string ProductURL { get; set; }
        public string ExternalCategoryId { get; set; }
    }
}