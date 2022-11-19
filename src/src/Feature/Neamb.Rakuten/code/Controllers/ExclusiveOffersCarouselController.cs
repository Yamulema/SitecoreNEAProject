using Neambc.Neamb.Feature.Rakuten.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using static System.Decimal;

namespace Neambc.Neamb.Feature.Rakuten.Controllers
{
    public class ExclusiveOffersCarouselController : BaseController
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;

        public ExclusiveOffersCarouselController(ISessionAuthenticationManager sessionAuthenticationManager)
        {
            _sessionAuthenticationManager = sessionAuthenticationManager;
        }

        public override ActionResult Index()
        {
            var member = _sessionAuthenticationManager.GetAccountMembership();
            var ebToken = member.Status == StatusEnum.Hot && member.Profile.IsRakutenMember && member.Profile.RakutenProfile != null ?
                member.Profile.RakutenProfile.EBToken : string.Empty;

            var model = GetExclusiveOfferCarouselModel(RenderingContext.Current.Rendering, ebToken);
            return View("/Views/Neamb.Marketplace/ExclusiveOffersCarousel.cshtml", model);
        }

        public ExclusiveOffersCarousel GetExclusiveOfferCarouselModel(Rendering currentRendering, string ebToken)
        {

            var model = new ExclusiveOffersCarousel();
            model.Initialize(currentRendering);
            model.Headline = model.Item.Fields[Templates.ExclusiveOffersCarousel.CarouselHeadline]?.Value;
            model.Cards = GetExclusiveOfferCards(model, ebToken);
            return model;
        }

        public List<ExclusiveOfferCard> GetExclusiveOfferCards(ExclusiveOffersCarousel model, string ebToken)
        {
            var result = new List<ExclusiveOfferCard>();
            try
            {
                var offersFolder = (MultilistField)model.Item.Fields[Templates.ExclusiveOffersCarousel.CarouselOffers];

                if (offersFolder != null)
                    foreach (var item in offersFolder.GetItems())
                    {
                        var offer = GetOfferCard(item, ebToken);
                        if (offer.Offer != null) result.Add(offer);
                    }
            }
            catch (Exception e)
            {
                Log.Error($"Error getting exclusive offers", e, this);
            }

            return result;
        }

        private ExclusiveOfferCard GetOfferCard(Item item, string ebToken)
        {
            var offer = new ExclusiveOfferCard
            {
                Headline = item[Templates.ExclusiveOffersCarousel.Fields.CardHeadline],
                BackgroundColor = GetBackgroundColor(item[Templates.ExclusiveOffersCarousel.Fields.CardColor]),
                Offer = GetOffer(item.Fields[Templates.ExclusiveOffersCarousel.Fields.CardOffer], ebToken)
            };
            return offer;
        }

        private string GetBackgroundColor(string color)
        {
            var result = string.IsNullOrEmpty(color) ? "005daa" : color;
            return result.Contains("#") ? result : $"#{result}";
        }

        private ExclusiveOffer GetOffer(Field field, string ebToken)
        {
            var store = ((MultilistField)field)?.GetItems().FirstOrDefault();
            if (store == null) return null;

            var neambEnable = ((CheckboxField)store.Fields[Templates.Store.Fields.NeambEnable]).Checked;
            if (!neambEnable) return null;

            var shoppingUrl = store.Fields[Templates.Store.Fields.ShoppingUrl]?.Value;
            var tier = ((CheckboxField)store.Fields[Templates.Store.Fields.Tier]).Checked;
            var type = store.Fields[Templates.Store.Fields.Type]?.Value;

            TryParse(store.Fields[Templates.Store.Fields.TotalReward]?.Value, out var totalReward);
            TryParse(store.Fields[Templates.Store.Fields.BaseReward]?.Value, out var baseReward);
            var memberOnly = ((CheckboxField)store.Fields[Templates.Store.Fields.MembersOnlyNEA]).Checked;

            var offerReward = totalReward - baseReward;
            var btnClass = "btn-gray";
            var cardText = "Can't Miss Deal";
            var totalRewardText = "";
            var baseRewardText = "&nbsp;";

            if (offerReward >= 0 && offerReward <= (decimal)2.99)
                cardText = "Everyday Deal";
            else if (offerReward >= 3 && offerReward <= (decimal)4.99)
                cardText = "Great Deal";

            if (type == "Percentage")
            {
                totalRewardText = $"{totalReward}% Cash Back";
                if (baseReward != totalReward)
                    baseRewardText = $"(Non-members: {baseReward}%)";
            }
            else
            {
                if (int.TryParse(totalReward.ToString(CultureInfo.InvariantCulture), out _))
                    totalRewardText = $"${totalReward} Cash Back";
                else
                    totalRewardText = $"${totalReward:n2} Cash Back";

                if (baseReward != totalReward)
                {
                    if (int.TryParse(baseReward.ToString(CultureInfo.InvariantCulture), out _))
                        baseRewardText = $"(Non-members: ${baseReward})";
                    else
                        baseRewardText = $"(Non-members: ${baseReward:n2})";
                }
                cardText = "Everyday Deal";
            }

            if (memberOnly)
            {
                btnClass = "btn-orange";
                cardText = "For Members Only";
            }

            if (totalReward == 0)
            {
                totalRewardText = "";
                baseRewardText = "";
            }

            var result = new ExclusiveOffer
            {
                Type = type,
                Id = store.ID.Guid,
                BtnClass = btnClass,
                CardText = cardText,
                MembersOnly = memberOnly,
                ShoppingUrl = $"{shoppingUrl}{ebToken}",
                BaseReward = baseRewardText,
                TotalReward = tier ? "Up to " + totalRewardText : totalRewardText,
                Tier = tier,
                Name = store.Fields[Templates.Store.Fields.Name]?.Value,
                Banner = store.Fields[Templates.Store.Fields.Banner]?.Value,
                SmallLogo = store.Fields[Templates.Store.Fields.SmallLogo]?.Value,
                Thumbnail = store.Fields[Templates.Store.Fields.Thumbnail]?.Value,
                Icon11230 = store.Fields[Templates.Store.Fields.Icon11230]?.Value,
                Icon22460 = store.Fields[Templates.Store.Fields.Icon22460]?.Value,
                Icon33690 = store.Fields[Templates.Store.Fields.Icon33690]?.Value,
                LogoEmail = store.Fields[Templates.Store.Fields.LogoEmail]?.Value,
                LogoMobile = store.Fields[Templates.Store.Fields.LogoMobile]?.Value,
                LogoMobile2x = store.Fields[Templates.Store.Fields.LogoMobile2x]?.Value,
                LogoMobile3x = store.Fields[Templates.Store.Fields.LogoMobile3x]?.Value,
                FeedSquareLogo = store.Fields[Templates.Store.Fields.FeedSquareLogo]?.Value,
            };
            return result;
        }
    }
}