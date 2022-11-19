using Neambc.Neamb.Feature.Cards.Models;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Feature.Cards.Controllers
{
    public class TwoColumnGridController : BaseController
    {
        public ActionResult TwoColumnGrid()
        {
            return View("/Views/Neamb.Cards/TwoColumnGrid.cshtml", CreateModel());
        }

        private TwoColumnGridDTO CreateModel()
        {
            var twoColumnGridItemDTO = new TwoColumnGridDTO();
            twoColumnGridItemDTO.Initialize(RenderingContext.Current.Rendering);
            return  twoColumnGridItemDTO;
        }
    }
}