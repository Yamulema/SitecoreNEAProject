using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Controllers
{
    public class FlipCardController : BaseController
    {
        #region ActionResult Methods
        public ActionResult FlipCard()
        {
            FlipCardDto model = new FlipCardDto();
            model.Initialize(RenderingContext.Current.Rendering);

            //Fills the model.
            model.PageCards = GetFlipCardItemItems(model.Item);

            return View("/Views/Neamb.Cards/FlipCards.cshtml", model);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the children card items
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        private List<FlipCard> GetFlipCardItemItems(Item datasource)
        {
            return datasource.GetChildren()
                .Select(GetFlipCardItem).ToList();
        }

        /// <summary>
        /// Maps a given sitecore Item into FlipCard.
        /// </summary>
        /// <param name="item">Flip child item</param>
        /// <returns></returns>
        private FlipCard GetFlipCardItem(Item item)
        {
            var result = new FlipCard { FlipItem = item };
            var backgroundSelected = item.Fields[Templates.FlipCardItem.Fields.FlipBackgroundColor]?.Value;

            if (!string.IsNullOrEmpty(backgroundSelected))
            {
                result.BackgroundColor = $"btn btn-{backgroundSelected}";
            }
            return result;
        }
        
        #endregion
    }
}