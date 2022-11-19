using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Feature.Cards.Repositories.Enums;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Neambc.Neamb.Feature.Cards.Repositories.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Feature.Cards.Controllers
{
    public class PageCardController : BaseController
    {
        public IPageCardDealerFactory PageCardDealerFactory { get; }

        public PageCardController(IPageCardDealerFactory pageCardDealerFactory)
        {
            PageCardDealerFactory = pageCardDealerFactory;
        }
        #region ActionResult Methods
        public ActionResult RelatedContent()
        {
            var model = new RelatedContentDto();
            model.Initialize(RenderingContext.Current.Rendering);

            // Checks if the render has datasource, if not it uses the PageItem instead.
            var datasource = model.Item ?? model.PageItem;

            //Fills the model.
            var cardDealer = PageCardDealerFactory.GetCardDealer(PageCardDealerType.RelatedContent);
            model.PageCards = cardDealer.GetPageCards<RelatedContentCard>(model.PageItem, datasource);
            model.BackgroundColor = GetBackgroundColor1Class(model.Item);

            return View("/Views/Neamb.Cards/RelatedContent.cshtml", model);
        }
        public ActionResult ProductCardCarousel()
        {
            var model = new ProductCardCarouselDto();
            model.Initialize(RenderingContext.Current.Rendering);

            // Checks if the render has datasource, if not it uses the PageItem instead.
            var datasource = model.Item ?? model.PageItem;

            //Fills the model.
            var cardDealer = PageCardDealerFactory.GetCardDealer(PageCardDealerType.ProductCardCarousel);
            model.PageCards = cardDealer.GetPageCards<ProductCardCarousel>(model.PageItem, datasource);
            model.BackgroundColor = GetBackgroundColor2Class(model.Item);
            model.Style = GetProductCardCarouselStyle(model.PageCards.Count());

            return View("/Views/Neamb.Cards/ProductCardCarousel.cshtml", model);
        }
        public ActionResult FourColumnProductCardCarousel()
        {
            var model = new ProductCardCarouselDto();
            model.Initialize(RenderingContext.Current.Rendering);

            // Checks if the render has datasource, if not it uses the PageItem instead.
            var datasource = model.Item ?? model.PageItem;

            //Fills the model.
            var cardDealer = PageCardDealerFactory.GetCardDealer(PageCardDealerType.ProductCardCarousel);
            model.PageCards = cardDealer.GetPageCards<ProductCardCarousel>(model.PageItem, datasource);
            //model.BackgroundColor = GetBackgroundColor2Class(model.Item);
            model.Style = GetProductCardCarouselStyle(model.PageCards.Count());

            return View("/Views/Neamb.Cards/FourColumnProductCardCarousel.cshtml", model);
        }
        public ActionResult TwoColumnCarousel()
        {
            var model = new TwoColumnCarouselDto();
            model.Initialize(RenderingContext.Current.Rendering);

            //Fills the model.
            var cardDealer = PageCardDealerFactory.GetCardDealer(PageCardDealerType.TwoColumnCarousel);
            var dataSource = model.HasDatasource ? model.Item : null;
            model.PageCards = cardDealer.GetPageCards<CarouselPageCard>(model.PageItem, dataSource);

            return View("/Views/Neamb.Cards/TwoColumnCarousel.cshtml", model);
        }
        public ActionResult MultiRowProductCards()
        {
            var model = new MultiRowProductCardsDto();
            model.Initialize(RenderingContext.Current.Rendering);

            //Fills the model.
            var cardDealer = PageCardDealerFactory.GetCardDealer(PageCardDealerType.MultiRowProductCards);
            var datasource = model.Item ?? model.PageItem;
            model.PageCards = cardDealer.GetPageCards<MultiRowProductCard>(model.PageItem, datasource);

            return View("/Views/Neamb.Cards/MultiRowProductCards.cshtml", model);
        }
        public ActionResult FiveContentCards()
        {
            var model = new FiveContentCardDto();
            model.Initialize(RenderingContext.Current.Rendering);

            // Checks if the render has datasource, if not it uses the PageItem instead.
            var datasource = model.Item ?? model.PageItem;

            //Fills the model.
            var cardDealer = PageCardDealerFactory.GetCardDealer(PageCardDealerType.FiveContentCard);
            model.PageCards = cardDealer.GetPageCards<FiveContentCard>(model.PageItem, datasource).ToList();
            model.BackgroundColor = GetBackgroundColor1Class(model.Item);

            return View("/Views/Neamb.Cards/FiveContentCards.cshtml", model);
        }
        #endregion
        #region Static Methods
        /// <summary>
        /// Gets the backgound css class for a given Item that inherits _RelatedContent.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string GetBackgroundColor1Class(Item item)
        {
            var result = "bg-white";

            switch (item.Fields[Templates.RelatedContent.Fields.BackgroundColor]?.Value)
            {
                case "Gray":
                    {
                        result = "bg-gray";
                        break;
                    }
                default:
                    result = "bg-white";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the backgound css class for a given Item that inherits _ProductCardsCarousel.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string GetBackgroundColor2Class(Item item)
        {
            var result = "bg-gray";

            switch (item.Fields[Templates.ProductCardsCarousel.Fields.BackgroundColor]?.Value)
            {
                case "Blue":
                    {
                        result = "bg-blue";
                        break;
                    }
                default:
                    result = "bg-gray";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the css style for the carousel
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string GetProductCardCarouselStyle(int count)
        {
            if (count == 1) {
                return "one-product-card-carousel";
            } else if (count == 2 ) {
                return "two-product-card-carousel";
            }
            else {
                return "product-card-carousel";
            }
        }
        #endregion
    }
}