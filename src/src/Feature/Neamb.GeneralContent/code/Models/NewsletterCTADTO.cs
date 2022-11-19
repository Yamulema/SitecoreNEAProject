using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class NewsletterCTADTO : IRenderingModel
    {
        public Rendering Rendering { get; private set; }
        public Item Item { get; set; }
        public bool IsSubscribed { get; set; }
        public int NewsletterId { get; set; }
        public SocialShareModel SocialShare { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsPublic { get; set; }
        [MaxLength(100, ErrorMessage = ConstantsNeamb.ValidationLength)]
        [EmailCompare(ErrorMessage = "Email Format")]
        [EmailAddress(ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
        public string Email { get; set; }
        public List<ErrorStatusEnum> ErrorsEmail { get; set; }
        public bool HasGeneralError { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            ErrorsEmail = new List<ErrorStatusEnum>();
            HasGeneralError = false;
        }
        public string OnSubscribeEvent { get; set; }
        public string OnUnSubscribeEvent { get; set; }
    }
}