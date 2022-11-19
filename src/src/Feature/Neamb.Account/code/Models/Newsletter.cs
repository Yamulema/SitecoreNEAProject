using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class Newsletter
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public string Subscribe { get; set; }
        public string Unsubscribe { get; set; }
        public SubscriberStatus SubscriberStatus { get; set; }
    }
}