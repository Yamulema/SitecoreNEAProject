using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class Subscription
    {
        public string Mdsid { get; set; }
        public int ListId { get; set; }
        public SubscriberStatus Status { get; set; }
    }
}