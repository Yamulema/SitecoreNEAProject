using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Controllers
{
    public class RetirementSeminarController : BaseController {
        private readonly IRetirementSeminarRepository _retirementSeminarRepository;
        
        public RetirementSeminarController(
            IRetirementSeminarRepository retirementSeminarRepository
        ) {
            _retirementSeminarRepository = retirementSeminarRepository;
        }

        /// <summary>
        /// Get method
        /// </summary>
        /// <returns></returns>
        public ActionResult RetirementSeminar() {
            var model = new RetirementSeminarDTO();
            var rendering = RenderingContext.Current.Rendering;
            model.Initialize(rendering);
            var componentId = Request.QueryString["componentId"];

            _retirementSeminarRepository.SetPropertiesRetirementSeminar(ref model, rendering.Item, rendering.UniqueId,componentId);
            model.ContextItem = RenderingContext.Current.Rendering.Item.ID.ToString();
            return View("/Views/Neamb.Product/Renderings/RetirementSeminar.cshtml", model);
        }
        /// <summary>
        /// ExecuteReminderSeminar
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExecuteReminderSeminar(RetirementSeminarDTO model) {
            var response = _retirementSeminarRepository.ExecuteRegistrationRetirementSeminar(model);
            return Json(new
            {
                hasError = response.HasError,
                processedSucessfully = response.ProcessedSucessfully,
                errorauthentication = response.ErrorAuthentication
            });
        }
    }
}