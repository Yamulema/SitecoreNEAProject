namespace Neambc.Neamb.Foundation.MBCData.Model.ProductEligibility
{
    public class ProductEligibilityModel
    {
        public bool Eligible { get; set; }
        public bool CurrentMember { get; set; }
        public bool MembershipTypeMatch { get; set; }
        public bool ClearedByState { get; set; }
    }
}