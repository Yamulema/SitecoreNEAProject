using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Manager
{
    [Service(typeof(IProductGtmManager))]
    public class ProductGtmManager : IProductGtmManager
    {
        private readonly IGtmService _gtmService;
        private readonly IPageSitecoreContext _pageSitecoreContext;

        public ProductGtmManager(IGtmService gtmService, IPageSitecoreContext pageSitecoreContext)
        {
            _gtmService = gtmService;
            _pageSitecoreContext = pageSitecoreContext;
        }

        /// <summary>
        /// Get the GTM action click method as a string
        /// </summary>
        /// <param name="componentType">Product component type: Product, Multirow card, Offer link</param>
        /// <param name="renderingItem">Rendering item</param>
        /// <param name="clickHref">ClickHref</param>
        /// <param name="cta">Data to be sent to GTM</param>
        /// <param name="statusUser">User status: cold, warm, hot</param>
        /// <returns></returns>
        public string GetGtmFunction(ComponentTypeEnum componentType, Item renderingItem, string clickHref, ProductCtaBase cta = null, StatusEnum statusUser = StatusEnum.Cold)
        {
            string clickAction = "";
            string productName;
            if (renderingItem != null && ((renderingItem.TemplateID == Templates.ProductCtaLite.ID ||
                renderingItem.TemplateID == Templates.PromoToutLite.ID) && _pageSitecoreContext.Current.TemplateID != Templates.ProductPage.ID
            ))
            {
                productName = cta?.ProductName;
            }
            else
            {
                productName = !string.IsNullOrEmpty(_pageSitecoreContext.Current.DisplayName) ? _pageSitecoreContext.Current.DisplayName : _pageSitecoreContext.Current.Name;
            }

            switch (componentType)
            {
                case ComponentTypeEnum.Cta:
                    {
                        if (cta != null)
                        {
                            cta.ClickHref = clickHref;
                            cta.ProductName = productName;
                            if (statusUser == StatusEnum.WarmHot || 
                                statusUser == StatusEnum.Cold || 
                                statusUser == StatusEnum.Unknown) cta.Event = "login cta";
                            clickAction = _gtmService.GetGtmEvent(cta);
                        }
                        break;
                    }
                case ComponentTypeEnum.CtaSeiumb:
                    {
                        if (cta != null)
                        {
                            cta.ClickHref = clickHref;
                            cta.ProductName = productName;
                            if (statusUser == StatusEnum.WarmHot ||
                                statusUser == StatusEnum.Cold ||
                                statusUser == StatusEnum.Unknown) cta.Event = "login cta";
                            clickAction = _gtmService.GetGtmEvent(cta);
                        }
                        break;
                    }
                case ComponentTypeEnum.CtaSecondary:
                    {
                        if (cta != null)
                        {
                            var ctaSecondary = new ProductCtaSecondary
                            {
                                ClickHref = clickHref,
                                ProductName = productName,
                                CtaText = cta.CtaText
                            };
                            if (statusUser == StatusEnum.WarmHot ||
                                statusUser == StatusEnum.Cold ||
                                statusUser == StatusEnum.Unknown) ctaSecondary.Event = "login cta";
                            clickAction = _gtmService.GetGtmEvent(ctaSecondary);
                        }
                        break;
                    }
                case ComponentTypeEnum.CtaSecondarySeiumb:
                    {
                        if (cta != null)
                        {
                            var ctaSecondary = new ProductCtaSecondary
                            {
                                ClickHref = clickHref,
                                ProductName = productName,
                                CtaText = cta.CtaText
                            };
                            if (statusUser == StatusEnum.WarmHot ||
                                statusUser == StatusEnum.Cold ||
                                statusUser == StatusEnum.Unknown) ctaSecondary.Event = "login cta";
                            clickAction = _gtmService.GetGtmEvent(ctaSecondary);
                        }
                        break;
                    }
                case ComponentTypeEnum.Anonymous:
                    {
                        if (cta != null)
                        {
                            cta.Event = "login cta";
                            cta.ClickHref = clickHref;
                            cta.ProductName = productName;
                            clickAction = _gtmService.GetGtmEvent(cta);
                        }
                        break;
                    }
                case ComponentTypeEnum.AnonymousSeiumb:
                    {
                        if (cta != null)
                        {
                            ProductCtaBase productCtaBase = new ProductCtaBase();
                            productCtaBase.CtaText = cta.CtaText;
                            productCtaBase.Event = "login cta";
                            productCtaBase.ProductName = productName;
                            productCtaBase.ClickHref = clickHref;
                            clickAction = _gtmService.GetGtmEvent(productCtaBase);
                        }
                        break;
                    }
                case ComponentTypeEnum.MultiRow:
                    {
                        LinkField productCtaConfigured = renderingItem.Fields[Templates.ProductOfferCard.Fields.Link];
                        string processedTitle = renderingItem[Templates.ProductOfferCard.Fields.Title];
                        string processedDescription = renderingItem[Templates.ProductOfferCard.Fields.Description];
                        //this condition is for carousel product card component
                        var isCarouselCardComponent = string.IsNullOrEmpty(processedTitle) && string.IsNullOrEmpty(processedDescription);
                        if (isCarouselCardComponent)
                        {
                            processedTitle = renderingItem[Templates.CarouselOfferItem.Fields.Title];
                            processedDescription = renderingItem[Templates.CarouselOfferItem.Fields.Description];
                            var multiOfferCard = new CarouselMultiOfferCard
                            {
                                CtaText = processedDescription,
                                Event = "multi-offer card",
                                CardTitle = $"{processedTitle}|{productCtaConfigured.Text}",
                                ClickHref = clickHref,
                                Position = "0"
                            };
                            clickAction = _gtmService.GetGtmEvent(multiOfferCard);
                        }
                        else
                        {
                            var multiOfferCard = new MultiOfferCard
                            {
                                CtaText = processedDescription,
                                Event = "multi-offer card",
                                CardTitle = $"{processedTitle}|{productCtaConfigured.Text}",
                                ClickHref = clickHref
                            };
                            clickAction = _gtmService.GetGtmEvent(multiOfferCard);
                        }
                        break;
                    }
                case ComponentTypeEnum.OfferLink:
                    {
                        LinkField productCtaConfigured = renderingItem.Fields[Templates.ProductOfferCard.Fields.Link];
                        OfferLink offerLink = new OfferLink
                        {
                            Event = "account",
                            AccountSection = "manage products and services",
                            AccountAction = "product",
                            CtaText = productCtaConfigured.Text,
                            ClickHref = clickHref
                        };
                        clickAction = _gtmService.GetGtmEvent(offerLink);
                        break;
                    }
                case ComponentTypeEnum.OfferLinkHeader:
                {
                    LinkField productCtaConfigured = renderingItem.Fields[Templates.ProductOfferCard.Fields.Link];
                    OfferLinkHeader offerLink = new OfferLinkHeader
                    {
                        NavText = productCtaConfigured.Text,
                    };
                    clickAction = _gtmService.GetGtmEvent(offerLink);
                    break;
                }
                case ComponentTypeEnum.GuideCta:
                    {
                        string filename = $"{renderingItem.Fields[Templates.GuideCta.Fields.MaterialId]}";

                        LinkField primaryLink = renderingItem.Fields[Templates.GuideCta.Fields.Cta];

                        if (string.IsNullOrEmpty(filename))
                        {
                            if (primaryLink.IsMediaLink)
                            {
                                filename = Sitecore.Resources.Media.MediaManager.GetMediaUrl(primaryLink.TargetItem);
                            }
                            else  //Get the internal link url
                            {
                                filename = Sitecore.Links.LinkManager.GetItemUrl(primaryLink.TargetItem);
                            }
                        }
                        GuideCta guideCta = new GuideCta
                        {
                            Event = "downloads",
                            CtaText = primaryLink.Text,
                            FileName = System.IO.Path.GetFileName(filename),
                            ClickHref = clickHref
                        };

                        clickAction = _gtmService.GetGtmEvent(guideCta);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return clickAction;
        }
    }
}