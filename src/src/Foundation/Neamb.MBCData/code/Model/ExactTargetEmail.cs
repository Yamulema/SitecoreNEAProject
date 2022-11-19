using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class ExactTargetEmail
    {
        public string CustomerKey { get; set; }
        public string SubscriberKey { get; set; }
        public string EmailTo { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}