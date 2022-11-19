using System.Runtime.Serialization;

namespace Neambc.Neamb.Foundation.MBCData.Model.ProductEligibility
{
    public class ProductEligibilityBaseRequest
    {
        [DataMember(Name = "mdsId")]
        public int MdsId { get; set; }
    }
}