using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Model
{
    public class ActionRequest
    {
        public string UserName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ActionType { get; set; }
        public string PostData { get; set; }
        public string TargetAction { get; set; }
        public Item RenderingItem { get; set; }
        public ID CtaLinkItemId { get; set; }
        public ID CtaTypeItemId { get; set; }
        public ID PostDataItemId { get; set; }
        public ProductDetailDTO Model { get; set; }
        public ComponentTypeEnum ComponentType { get; set; }
        public string ComponentId { get; set; }
        public ID EligibilityItemId { get; set; }
        public bool IsSpecialOffer { get; set; }
        public ActionButtonTypeEnum ActionButtonType { get; set; }
        public string ActionDescription { get; set; }
        public string ConstantCtaAction { get; set; }
        public string ConstantCtaActionOnclick { get; set; }
        public string ConstantCtaActionTargetBlank { get; set; }
        public string ConstantActionType { get; set; }
        public ID AnonymousItemId { get; set; }
        public string ConstantProductGtmAction { get; set; }
        public bool HasCheckEligibility { get; set; }
        public bool RequiresOnlyLogin { get; set; }
        public bool IsOmni { get; set; }
        public string IsOmniDefault { get; set; }
        public ID GoalPrimaryId { get; set; }
        public ID GoalSecondaryId { get; set; }
    }
}