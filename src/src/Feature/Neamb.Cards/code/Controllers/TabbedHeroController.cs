using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Controllers
{
    public class TabbedHeroController : BaseController
    {
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly IGtmService _gtmService;

		public TabbedHeroController(IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService)
        {
			_globalConfigurationManager = globalConfigurationManager;
			_gtmService = gtmService;
		}

        #region ActionResult Methods
        public ActionResult TabbedHero()
        {
            TabbedHeroDto model = new TabbedHeroDto();
            model.Initialize(RenderingContext.Current.Rendering);

            //Fills the model.
            model.TabbedItems = GetTabbedItems(model.Item);

            //   Converts the provided value to miliseconds or defaults it to 4000ms.
            model.Timer = int.TryParse(model.Item.Fields[Templates.TabbedHero.Fields.Timer]?.Value, out var timer)
                ? timer * 1000
                : 4000;

            return View("/Views/Neamb.Cards/TabbedHero.cshtml", model);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Gets a fixed number of TabbedHeroItems under a TabbedHero item.
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        private List<TabbedHeroItem> GetTabbedItems(Item datasource)
        {
            return datasource.GetChildren()
                .Take(_globalConfigurationManager.MaxTabbedHeroItems)
                .Where(x => x != null)
                .Select(GetTabbedHeroItem).ToList();
        }

        /// <summary>
        /// Maps a given sitecore Item into TabbedHeroItem.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TabbedHeroItem GetTabbedHeroItem(Item item)
        {
            var result = new TabbedHeroItem();
            var link = (LinkField)item.Fields[Templates.TabbedHeroItem.Fields.Destination];

            if (link != null && link.TargetItem != null) {
				string textHero = string.IsNullOrEmpty(link?.Text)
					? link.TargetItem.Fields[Templates.PageInfo.Fields.PageTitle]?.Value
					: link.Text;

				result = new TabbedHeroItem()
                {
                    Text =textHero,
                    ImageAlt = GetImageAltField(item),
                    ImageSrc = item.ImageUrl(Templates.TabbedHeroItem.Fields.Image),
                    Cta = LinkManager.GetItemUrl(link.TargetItem),
                    Target = link.Target,
					GtmAction = _gtmService.GetOnClickEvent(new Foundation.Analytics.Gtm.ContentArticle()
					{
						Event = "content",
						ContentTitle = textHero,
						ContentLocation = "homepage hero banner"
					})
				};
            }
            return result;
        }
        private string GetImageAltField(Item item)
        {
            var imgField = (ImageField)item.Fields["Image"];
            return imgField.Alt;
        }
        #endregion
    }
}