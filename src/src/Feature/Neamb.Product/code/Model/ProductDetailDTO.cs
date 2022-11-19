using System;
using System.Collections.Generic;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Model
{
    [Serializable]
    public class ProductDetailDTO
    {
        public string Title { get; set; }    // copied from ProductAnchoredDTO
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public StatusEnum UserStatus { get; set; }
        public string ActionPrimary { get; set; }
        public string ActionSecondary { get; set; }
        public string ActionPrimaryDescription { get; set; }
        public string ActionSecondaryDescription { get; set; }
        public string ActionClickPrimary { get; set; }
        public string ActionClickSecondary { get; set; }
        public string ActionPrimaryTargetBlank { get; set; }
        public string ActionSecondaryTargetBlank { get; set; }
        public string CtaPrimaryColor { get; set; }
        public string CtaSecondaryColor { get; set; }
        public string CtaAnonymousColor { get; set; }
        public bool HasCheckEligibility { get; set; }
        public bool ResultCheckEligibility { get; set; }
        public string PhoneNumber { get; set; }
        public string SupportEmail { get; set; }
        public bool HasClassEligibility { get; set; }
        public string EyebrownName { get; set; }
        public string EyebrownLink { get; set; }
        public string EyebrownTarget { get; set; }
        public bool HasCommingSoon { get; set; }
        public bool HasAlreadyNotified { get; set; }
        public string NotifyProductAvailableAction { get; set; }
        public List<string> ListPartners { get; set; }
        public List<Item> ListPartnersItems { get; set; }
        public string NotifyProductAvailableLink { get; set; }
        public string ComponentId { get; set; }
        public string AnonymousText { get; set; }
        public string AnonymousUrl { get; set; }
        public string AnonymousGtmAction { get; set; }
        public string AnonymousFunctionAction { get; set; }
        public string GtmActionPage { get; set; }
        public bool IsProductPage { get; set; }
        public bool HasError { get; set; }
        public bool HasErrorLink { get; set; }
        public bool HasBrokenLink { get; set; }
        public bool IsEligibleOmni { get; set; }
        public bool IsOmni { get; set; }
        public bool RequiresOnlyLogin { get; set; }
        public String ProductContactDetails { get; set; }

        public virtual void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            UserStatus = StatusEnum.Cold;
            HasCheckEligibility = false;
            ResultCheckEligibility = false;
            HasAlreadyNotified = false;
            ListPartners = new List<string>();
            ListPartnersItems = new List<Item>();
            HasClassEligibility = false;
            HasError = false;
            HasErrorLink = false;
            HasBrokenLink = false;
            IsEligibleOmni = false;
            IsOmni = false;
        }
    }
}