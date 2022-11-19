using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System;
using System.Globalization;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Web.UI.WebControls;

namespace Neambc.Neamb.Feature.Cards.Controllers
{
    public class StackableItemController : BaseController
    {
        private const string PLATFORMVIDEO = "https://content.jwplatform.com/players/";

        public ActionResult StackableItem()
        {
            return View("/Views/Neamb.Cards/StackableItem.cshtml", CreateModel());
        }

        public StackableItemDTO CreateModel()
        {
            var stackableItem = RenderingContext.Current.Rendering.Item;
            var textPlacement = stackableItem.Fields[Templates.ContentDisplayComponents.StackableItem.Fields.TextPlacement];
            var image = stackableItem.Fields[Templates.ContentDisplayComponents.StackableItem.Fields.Image];
            var video = stackableItem.Fields[Templates.ContentDisplayComponents.StackableItem.Fields.Video];
            var callout = stackableItem.Fields[Templates.ContentDisplayComponents.StackableItem.Fields.Callout];

            var hasVideo = false;
            string videoUrl = null;
            if (video.Value != string.Empty)
            {
                hasVideo = true;
                videoUrl = video.Value;
            }
            
            var stackableItemDTO = new StackableItemDTO()
            {
                HasTextPlacementLeft = textPlacement.Value == Categories.TextPlacements.Left.ToString() ? true : false,
                HasImage = image.Value != string.Empty ? true : false,
                HasVideo = hasVideo,
                VideoUrl = videoUrl,
                HasCallout = callout.Value != string.Empty ? true : false,
                IsJWPlatformVideo = video.Value.Contains(PLATFORMVIDEO) ? true : false
            };

            stackableItemDTO.Initialize(RenderingContext.Current.Rendering);

            return stackableItemDTO;
        }
    }
}