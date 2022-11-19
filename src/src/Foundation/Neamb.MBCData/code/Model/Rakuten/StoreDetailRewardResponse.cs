namespace Neambc.Neamb.Foundation.MBCData.Model.Rakuten
{
    public class StoreDetailRewardResponse
    {
        public StoreDetailRewardBaseResponse Base { get; set; }
        public StoreDetailRewardBaseResponse Global { get; set; }
        public StoreDetailRewardBaseResponse Channel { get; set; }
        public StoreDetailRewardBaseResponse Total { get; set; }
    }
}