using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rakuten
{
    public class StoreResponse
    {
        public string Etag { get; set; }
        public IList<StoreBaseResponse> Store { get; set; }
        public bool CanContinueProcess { get; set; }
        public StoreResponse() {
            Store= new List<StoreBaseResponse>();
            CanContinueProcess = false;
        }
    }
}