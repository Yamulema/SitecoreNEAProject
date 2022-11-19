using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Search.Models
{
    public class StoreResult
    {
        public IEnumerable<StoreResultCard> stores;
        public bool IsRakutenMember;
        public bool IsHotState;
        public bool hasOneMorePage { get; set; }
    }
}