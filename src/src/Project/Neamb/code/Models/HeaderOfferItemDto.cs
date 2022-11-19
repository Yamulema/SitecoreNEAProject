using System;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Project.Web.Models
{
    [Serializable]
    public class HeaderOfferItemDto
	{
        public bool HasCheckEligibility { get; set; }
        public bool ResultCheckEligibility { get; set; }
        public StatusEnum UserStatus { get; set; }
        public string ActionPrimary { get; set; }
        public string ActionPrimaryDescription { get; set; }
        public string ActionPrimaryTargetBlank { get; set; }
        public string ActionSecondary { get; set; }
        public string ActionSecondaryDescription { get; set; }
        public string ActionSecondaryTargetBlank { get; set; }
        public string ActionClickPrimary { get; set; }
        public string ActionClickSecondary { get; set; }
        public bool HasErrorLink { get; set; }
        public string ComponentId { get; set; }
        public HeaderOfferItemDto() {
            HasErrorLink = false;
            UserStatus = StatusEnum.Cold;
        }
    }
}