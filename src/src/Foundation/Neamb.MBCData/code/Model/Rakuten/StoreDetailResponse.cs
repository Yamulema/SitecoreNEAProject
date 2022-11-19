using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rakuten
{
    public class StoreDetailResponse
    {
        public string Status { get; set; }
        public bool AffiliatizerEnabled { get; set; }
        public bool AndroidTrackable { get; set; }
        public List<int> Categories { get; set; }
        public StoreDetailConditionsResponse Conditions { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string DetailEtag { get; set; }
        public int Id { get; set; }
        public StoreDetailImagesResponse Images { get; set; }
        public bool InStoreEnabled { get; set; }
        public bool IosTrackable { get; set; }
        public bool Luxury { get; set; }
        public string LargeLogo { get; set; }
        public bool MobileEnabled { get; set; }
        public bool MobileOnly { get; set; }
        public string Name { get; set; }
        public string OrderConfirmation { get; set; }
        public string OrderConfirmationDOMRegex { get; set; }
        public string OrderConfirmationURLRegex { get; set; }
        public string ShoppingURL { get; set; }
        public string SmallLogo { get; set; }
        public bool TabletEnabled { get; set; }
        public string UrlName { get; set; }
        public bool UscEnabled { get; set; }
        public bool ItpEnabled { get; set; }
        public StoreDetailTrackableDeviceResponse Trackable { get; set; }
        public bool IcbEnabled { get; set; }
        public List<StoreDetailTierResponse> Tiers { get; set; }
    }
}
