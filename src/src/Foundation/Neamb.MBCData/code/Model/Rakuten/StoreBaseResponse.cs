namespace Neambc.Neamb.Foundation.MBCData.Model.Rakuten
{
    public class StoreBaseResponse
    {
        public string DetailEtag { get; set; }
        public string MobileAppStoreDetailEtag { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public StoreDetailRewardResponse Reward { get; set; }
        public string ShoppingURL { get; set; }
        public string MobileOnly { get; set; }
        public StoreDetailResponse Detail { get; set; }
    }
}