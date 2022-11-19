using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.Indexing.Models
{
    public class StoreSearchResult : BaseModel
    {
        [IndexField("store_name_s")]
        public string StoreNameIndex { get; set; }

        [IndexField("name_t")]
        public string StoreName { get; set; }

        [IndexField("categories_sm")]
        public List<Guid> StoreCategories { get; set; }

        [IndexField("shoppingurl_t")]
        public string ShoppingUrl { get; set; }

        [IndexField("tier_b")]
        public bool Tier { get; set; }

        [IndexField("neamb_enable_b")]
        public bool NEAMBEnable { get; set; }

        [IndexField("members_only_neamb_b")]
        public bool MemberOnly { get; set; }

        [IndexField("popular_offer_neamb_b")]
        public bool PopularOffer { get; set; }

        #region Reward
        [IndexField("base_reward_tf")]
        public float BaseReward { get; set; }

        [IndexField("total_reward_tf")]
        public float TotalReward { get; set; }

        [IndexField("type_t")]
        public string Type { get; set; }
        #endregion

        #region Image Fields
        [IndexField("banner_t")]
        public string Banner { get; set; }

        [IndexField("smalllogo_t")]
        public string SmallLogo { get; set; }

        [IndexField("thumbnail_t")]
        public string Thumbnail { get; set; }

        [IndexField("icon11230_t")]
        public string Icon11230 { get; set; }

        [IndexField("icon22460_t")]
        public string Icon22460 { get; set; }

        [IndexField("icon33690_t")]
        public string Icon33690 { get; set; }

        [IndexField("logoemail_t")]
        public string LogoEmail { get; set; }

        [IndexField("logomobile_t")]
        public string LogoMobile { get; set; }

        [IndexField("logomobile2x_t")]
        public string LogoMobile2x { get; set; }

        [IndexField("logomobile3x_t")]
        public string LogoMobile3x { get; set; }

        [IndexField("feedsquarelogo_t")]
        public string FeedSquareLogo { get; set; }
        #endregion
    }
}