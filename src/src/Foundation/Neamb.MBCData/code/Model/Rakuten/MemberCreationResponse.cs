using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rakuten
{
    public class MemberCreationResponse
    {
        public long CreatedDate { get; set; }
        public string EmailAddress { get; set; }
        public string Id { get; set; }
        public List<Guid> FavoriteStores { get; set; }
        public string EBtoken { get; set; }
    }
}