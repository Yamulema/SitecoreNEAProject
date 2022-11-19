using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class LoginModel : IRenderingModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public bool IsMobile { get; set; }
        public List<LoginErrors> Errors { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public bool IsProductDetail { get; set; }
        public LoginAjaxEnum? LoginAjaxProcess { get; set; }
        public string StoreId { get; set; }
        public string RedirectUrlId { get; set; }
        public string HotDeal { get; set; }
        public string CtaText { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            var userRepository = (IUserRepository)ServiceLocator.ServiceProvider.GetService(typeof(IUserRepository));
            IsProductDetail = PageItem.IsDerived(Templates.ProductDetailPageType.ID) && !userRepository.GetUserStatus().Equals(UserStatusCons.HOT);
            if (PageItem.IsDerived(Templates.ProductDetailPageType.ID) && !userRepository.GetUserStatus().Equals(UserStatusCons.HOT))
                LoginAjaxProcess = LoginAjaxEnum.Product;
            else if (PageItem.IsDerived(Templates.MarketPlacePageType.ID))
                LoginAjaxProcess = LoginAjaxEnum.Marketplace;
            else
                LoginAjaxProcess = LoginAjaxEnum.None;
        }
    }
}