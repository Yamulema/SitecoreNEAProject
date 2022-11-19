using System;
using Moq;
using Neambc.Neamb.Feature.Rakuten.Controllers;
using Neambc.Neamb.Feature.Rakuten.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Rakuten.UnitTest.Controllers
{
    [TestFixture]

    public class ExclusiveOffersCarouselControllerTest
    {
        #region Fields
        private Mock<ISessionAuthenticationManager> _sessionAuthenticationManager;
        private const string _itemID = "{1396BA97-35F5-491C-B198-3F523685ED80}";
        private const string _offer1ID = "{AFBEBEC2-0287-403C-B373-F1367198C048}";
        private const string _offer2ID = "{7D82B630-48AD-4068-A94A-D9AF98F3869F}";
        private const string _offer3ID = "{37E91502-1BF5-41BC-A826-EE835DB33792}";
        private const string _store1ID = "{B7C3390F-7AA3-4C50-A747-84226531F52C}";
        private const string _store2ID = "{FD458EBD-CB42-4AF2-BA93-8D5F715033A4}";
        private const string _store3ID = "{ED05A6D6-4DDB-49C5-A5F5-4C97BD59087B}";
        #endregion

        [SetUp]
        public void SetUp()
        {
            _sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
        }

        public static IEnumerable<TestCaseData> ExclusiveOffersCarouselController_GetExclusiveOfferCards_Data() {
            var model = new ExclusiveOffersCarousel();

            var o1 = CreateOfferCard(1, _store1ID, "Was 3%", "5% Cash Back", "Percentage", true, "btn-gray", "Everyday Deal");
            var o2 = CreateOfferCard(2, _store2ID, "Was $3", "$5 Cash Back", "Cash", true, "btn-gray", "Everyday Deal");

            var r1 = new List<ExclusiveOfferCard>{};
            var r2 = new List<ExclusiveOfferCard>{o1};
            var r3 = new List<ExclusiveOfferCard>{o1, o2};
            var r4 = new List<ExclusiveOfferCard>{o1};

            yield return new TestCaseData(model, "", r1);
            yield return new TestCaseData(model, $"{_offer1ID}", r2);
            yield return new TestCaseData(model, $"{_offer1ID}|{_offer2ID}", r3);
            yield return new TestCaseData(model, $"{_offer1ID}|{_offer3ID}", r4);
        }

        [Test, TestCaseSource("ExclusiveOffersCarouselController_GetExclusiveOfferCards_Data")]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExclusiveOffersCarouselController_GetExclusiveOfferCards(ExclusiveOffersCarousel model, string items, List<ExclusiveOfferCard> expected){
            using (var db = new Db()) {
                FillDatabase(db, items);
                model.Item = db.GetItem(new ID(_itemID));
                var sut = new ExclusiveOffersCarouselController(_sessionAuthenticationManager.Object);

                var result = sut.GetExclusiveOfferCards(model, "token");

                Assert.AreEqual(expected.Count, result.Count);
                for (int i = 0; i < result.Count; i++) {
                    Assert.AreEqual(expected[i].Headline, result[i].Headline);
                    Assert.AreEqual(expected[i].BackgroundColor, result[i].BackgroundColor);
                    Assert.AreEqual(expected[i].Offer.Id, result[i].Offer.Id);
                    Assert.AreEqual(expected[i].Offer.MembersOnly, result[i].Offer.MembersOnly);
                    Assert.AreEqual(expected[i].Offer.BaseReward, result[i].Offer.BaseReward);
                    Assert.AreEqual(expected[i].Offer.TotalReward, result[i].Offer.TotalReward);
                    Assert.AreEqual(expected[i].Offer.Type, result[i].Offer.Type);
                    Assert.AreEqual(expected[i].Offer.Tier, result[i].Offer.Tier);
                    Assert.AreEqual(expected[i].Offer.ShoppingUrl, result[i].Offer.ShoppingUrl);
                    Assert.AreEqual(expected[i].Offer.Name, result[i].Offer.Name);
                    Assert.AreEqual(expected[i].Offer.Banner, result[i].Offer.Banner);
                    Assert.AreEqual(expected[i].Offer.SmallLogo, result[i].Offer.SmallLogo);
                    Assert.AreEqual(expected[i].Offer.Thumbnail, result[i].Offer.Thumbnail);
                    Assert.AreEqual(expected[i].Offer.Icon11230, result[i].Offer.Icon11230);
                    Assert.AreEqual(expected[i].Offer.Icon22460, result[i].Offer.Icon22460);
                    Assert.AreEqual(expected[i].Offer.Icon33690, result[i].Offer.Icon33690);
                    Assert.AreEqual(expected[i].Offer.LogoEmail, result[i].Offer.LogoEmail);
                    Assert.AreEqual(expected[i].Offer.LogoMobile, result[i].Offer.LogoMobile);
                    Assert.AreEqual(expected[i].Offer.LogoMobile2x, result[i].Offer.LogoMobile2x);
                    Assert.AreEqual(expected[i].Offer.LogoMobile3x, result[i].Offer.LogoMobile3x);
                    Assert.AreEqual(expected[i].Offer.FeedSquareLogo, result[i].Offer.FeedSquareLogo);
                    Assert.AreEqual(expected[i].Offer.BtnClass, result[i].Offer.BtnClass);
                    Assert.AreEqual(expected[i].Offer.CardText, result[i].Offer.CardText);
                }
            }
        }

        private static ExclusiveOfferCard CreateOfferCard(int index, string offerID, string baseReward, string totalReward, 
            string type, bool tier, string btnClass, string cardText) {
            var result = new ExclusiveOfferCard {
                Headline = "Trending",
                BackgroundColor = "#005daa",
                Offer = new ExclusiveOffer
                {
                    Id = Guid.Parse(offerID),
                    MembersOnly = false,
                    BaseReward = baseReward,
                    TotalReward = totalReward,
                    Type = type,
                    Tier = tier,
                    ShoppingUrl = "shopping_url/token",
                    Name = $"Store{index}",
                    Banner = "Banner",
                    SmallLogo = "SmallLogo",
                    Thumbnail = "Thumbnail",
                    Icon11230 = "Icon11230",
                    Icon22460 = "Icon22460",
                    Icon33690 = "Icon33690",
                    LogoEmail = "LogoEmail",
                    LogoMobile = "LogoMobile",
                    LogoMobile2x = "LogoMobile2x",
                    LogoMobile3x = "LogoMobile3x",
                    FeedSquareLogo = "FeedSquareLogo",
                    BtnClass = btnClass,
                    CardText = cardText,
                }
            };
            return result;
        }

        private void FillDatabase(Db db, string items) {
            db.Add(new DbItem("CarouselOffers", new ID(_itemID)) {
                Fields = {
                    new DbField("CarouselOffers", Templates.ExclusiveOffersCarousel.CarouselOffers) { Value = items },
                }
            });

            db.Add(CreateStoreItem(1, _store1ID, "Percentage", "1", "1"));
            db.Add(CreateStoreItem(2, _store2ID, "Cash", "1", "1"));
            db.Add(CreateStoreItem(3, _store3ID, "Percentage", "1", "0"));

            db.Add(CreateOfferItem(1, _offer1ID, _store1ID));
            db.Add(CreateOfferItem(2, _offer2ID, _store2ID));
            db.Add(CreateOfferItem(3, _offer3ID, _store3ID));
        }

        private DbItem CreateStoreItem(int index, string storeID, string type, string tier, string neambEnable) {
            var result = new DbItem($"Store{index}", new ID(storeID))
            {
                Fields = {
                    new DbField("BaseReward", Templates.Store.Fields.BaseReward) { Value = "3" },
                    new DbField("TotalReward", Templates.Store.Fields.TotalReward) { Value = "5" },
                    new DbField("Type", Templates.Store.Fields.Type) { Value = type },
                    new DbField("Tier", Templates.Store.Fields.Tier) { Value = tier },
                    new DbField("ShoppingUrl", Templates.Store.Fields.ShoppingUrl) { Value = "shopping_url/" },
                    new DbField("Name", Templates.Store.Fields.Name) { Value = $"Store{index}" },
                    new DbField("Banner", Templates.Store.Fields.Banner) { Value = "Banner" },
                    new DbField("SmallLogo", Templates.Store.Fields.SmallLogo) { Value = "SmallLogo" },
                    new DbField("Thumbnail", Templates.Store.Fields.Thumbnail) { Value = "Thumbnail" },
                    new DbField("Icon11230", Templates.Store.Fields.Icon11230) { Value = "Icon11230" },
                    new DbField("Icon22460", Templates.Store.Fields.Icon22460) { Value = "Icon22460" },
                    new DbField("Icon33690", Templates.Store.Fields.Icon33690) { Value = "Icon33690" },
                    new DbField("LogoEmail", Templates.Store.Fields.LogoEmail) { Value = "LogoEmail" },
                    new DbField("LogoMobile", Templates.Store.Fields.LogoMobile) { Value = "LogoMobile" },
                    new DbField("LogoMobile2x", Templates.Store.Fields.LogoMobile2x) { Value = "LogoMobile2x" },
                    new DbField("LogoMobile3x", Templates.Store.Fields.LogoMobile3x) { Value = "LogoMobile3x" },
                    new DbField("FeedSquareLogo", Templates.Store.Fields.FeedSquareLogo) { Value = "FeedSquareLogo" },
                    new DbField("MembersOnly", Templates.Store.Fields.MembersOnlyNEA) { Value = "0" },
                    new DbField("NeambEnable", Templates.Store.Fields.NeambEnable) { Value = neambEnable },
                }
            };
            return result;
        }

        private DbItem CreateOfferItem(int index, string offerID, string storeID) {
            var result = new DbItem($"Offer{index}", new ID(offerID))
            {
                Fields = {
                    new DbField("Headline", Templates.ExclusiveOffersCarousel.Fields.CardHeadline) { Value = "Trending" },
                    new DbField("CardColor", Templates.ExclusiveOffersCarousel.Fields.CardColor) { Value = "#005daa" },
                    new DbField("CardOffer", Templates.ExclusiveOffersCarousel.Fields.CardOffer) { Value = storeID },
                }
            };
            return result;
        }
    }
}
