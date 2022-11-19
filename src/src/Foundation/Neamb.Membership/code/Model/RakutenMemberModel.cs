using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.Membership.Model
{
    [Serializable]
    public class StoreInfo {
        public Guid StoreGuid { get; set; }
        public string StoreName { get; set; }
    }

    [Serializable]
    public class RakutenMemberModel
    {
        public string Id { get; set; }
        public long CreatedDate { get; set; }
        public string EmailAddress { get; set; }
        public string EBToken { get; set; }
        public List<StoreInfo> FavoriteStores { get; set; }
    }
}