using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Rakuten.Constants;
using Neambc.Seiumb.Feature.Rakuten.Models;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using static System.Decimal;

namespace Neambc.Seiumb.Feature.Rakuten.Controllers
{
    public class SeiumbExclusiveOffersCarouselController : BaseController
    {
        private readonly ISeiumbProfileManager _seiumbProfileManager;

        public SeiumbExclusiveOffersCarouselController(ISeiumbProfileManager seiumbProfileManager)
        {
            _seiumbProfileManager = seiumbProfileManager;
        }

        public ActionResult ExclusiveOffers()
        {
            var rakutenResponse = _seiumbProfileManager.GetRakutenMemberCreationResponse();
            var ebToken = _seiumbProfileManager.InHotState() && _seiumbProfileManager.IsRakutenMember() ? rakutenResponse.EBtoken : string.Empty;
            var model = GetSeiumbExclusiveOfferCarouselModel(RenderingContext.Current.Rendering, ebToken);
            return View("/Views/Seiumb.Marketplace/ExclusiveOffersCarousel.cshtml", model);
        }

        public SeiumbExclusiveOffersCarousel GetSeiumbExclusiveOfferCarouselModel(Rendering currentRendering, string ebToken)
        {
            var model = new SeiumbExclusiveOffersCarousel();
            model.Initialize(currentRendering);
            model.Headline = model.Item.Fields[Templates.SeiumbExclusiveOffersCarousel.CarouselHeadline]?.Value;
            model.Cards = GetSeiumbSeiumbExclusiveOfferCards(model, ebToken);
            return model;
        }

        public List<SeiumbExclusiveOfferCard> GetSeiumbSeiumbExclusiveOfferCards(SeiumbExclusiveOffersCarousel model, string ebToken)
        {
            var result = new List<SeiumbExclusiveOfferCard>();
            try
            {
                var offersFolder = (MultilistField)model.Item.Fields[Templates.SeiumbExclusiveOffersCarousel.CarouselOffers];

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

        private SeiumbExclusiveOfferCard GetOfferCard(Item item, string ebToken)
        {
            var offer = new SeiumbExclusiveOfferCard
            {
                Headline = item[Templates.SeiumbExclusiveOffersCarousel.Fields.CardHeadline],
                BackgroundColor = GetBackgroundColor(item[Templates.SeiumbExclusiveOffersCarousel.Fields.CardColor]),
                Offer = GetOffer(item.Fields[Templates.SeiumbExclusiveOffersCarousel.Fields.CardOffer], ebToken)
            };
            return offer;
        }

        private string GetBackgroundColor(string color)
        {
            var result = string.IsNullOrEmpty(color) ? "6C4A98" : color;
            return result.Contains("#") ? result : $"#{result}";
        }

        private SeiumbExclusiveOffer GetOffer(Field field, string ebToken)
        {
            var store = ((MultilistField)field)?.GetItems().FirstOrDefault();
            if (store == null) return null;

            var seiumbEnable = ((CheckboxField)store.Fields[Templates.Store.Fields.SeiumbEnable]).Checked;
            if (!seiumbEnable) return null;

            var shoppingUrl = store.Fields[Templates.Store.Fields.ShoppingUrl]?.Value;
            var tier = ((CheckboxField)store.Fields[Templates.Store.Fields.Tier]).Checked;
            var type = store.Fields[Templates.Store.Fields.Type]?.Value;

            TryParse(store.Fields[Templates.Store.Fields.TotalReward]?.Value, out var totalReward);
            TryParse(store.Fields[Templates.Store.Fields.BaseReward]?.Value, out var baseReward);
            var memberOnly = ((CheckboxField)store.Fields[Templates.Store.Fields.MembersOnlySEIU]).Checked;

            var offerReward = totalReward - baseReward;
            var btnClass = "white";
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
                btnClass = "";
                cardText = "For Members Only";
            }

            if (totalReward == 0)
            {
                totalRewardText = "";
                baseRewardText = "";
            }

            var result = new SeiumbExclusiveOffer
            {
                Type = type,
                Id = store.ID.Guid,
                BtnClass = btnClass,
                CardText = cardText,
                MembersOnly = memberOnly,
                ShoppingUrl = $"{shoppingUrl}{ebToken}",
                BaseReward = baseRewardText,
                TotalReward = tier ? "Up to " + totalRewardText : totalRewardText,
                Tier = "&nbsp;",
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