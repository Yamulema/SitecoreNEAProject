using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class AddUpdateDataExtensionRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "SubscriberKeyIsRequired")]
        [StringLength(100)]
        public string SubscriberKey { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "EmailIsRequired")]
        [StringLength(254)]
        public string Email { get; set; }
        public Item Newsletter { get; set; }
        public SubscriberStatus NewStatus { get; set; }
    }
}