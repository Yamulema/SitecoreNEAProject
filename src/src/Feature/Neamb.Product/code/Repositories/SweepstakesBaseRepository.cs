using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Product.Repositories
{
    public class SweepstakesBaseRepository 
    {
        protected void SetBasicProperties(ref SweepstakesBaseDTO sweepstakesBaseDto, Item renderingItem, AccountMembership accountMembership) {
            sweepstakesBaseDto.SocialShare = new SocialShareModel(renderingItem);
            sweepstakesBaseDto.ShowAcknowledgement = renderingItem.Fields[Templates.LandingPageCta.Fields.Acknowledgement].IsChecked();
            sweepstakesBaseDto.ShowContactInfo = renderingItem.Fields[Templates.LandingPageCta.Fields.ShowContactInfo].IsChecked();
            //get the cta link text 
            sweepstakesBaseDto.CtaText = renderingItem[Templates.LandingPageCta.Fields.CtaLink];
            //Get the link of the login
            LinkField loginLink = renderingItem.Fields[Templates.LandingPageCta.Fields.Login];
            if (loginLink != null && loginLink.TargetItem!=null)
            {
                sweepstakesBaseDto.LoginText = loginLink.Text;
                sweepstakesBaseDto.LoginUrl = LinkManager.GetItemUrl(loginLink.TargetItem);
            }

            sweepstakesBaseDto.HasTermsAndConditions =
                !string.IsNullOrEmpty(renderingItem[Templates.LandingPageCta.Fields.TermsAndConditions]) ? true : false;
            sweepstakesBaseDto.Cellcode = renderingItem[Templates.LandingPageCta.Fields.Cellcode];
            sweepstakesBaseDto.Campaigncode = renderingItem[Templates.LandingPageCta.Fields.Campaigncode];
            sweepstakesBaseDto.Video = renderingItem[Templates.LandingPageCta.Fields.Video];
            sweepstakesBaseDto.IsPositionRight = renderingItem[Templates.LandingPageCta.Fields.Placement] ==
                Templates.TextPlacements.Right.ToString();

            sweepstakesBaseDto.IsAuthenticated = accountMembership.Status == StatusEnum.Hot;
            //Get the partner
            var partners =
                ((MultilistField)renderingItem.Fields[Templates.LandingPageCta.Fields.PartnerAtributtion])
                .GetItems();
            foreach (var itemPartner in partners)
            {
                //sweepstakesBaseDto.ListPartners.Add(itemPartner.ImageUrl(Templates.Partner.Fields.Logo));
                sweepstakesBaseDto.ListPartnersItems.Add(itemPartner);
            }
            sweepstakesBaseDto.ComponentId = renderingItem.ID.Guid.ToString("N");
        }
    }
}