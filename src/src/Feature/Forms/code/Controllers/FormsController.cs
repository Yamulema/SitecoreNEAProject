using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Forms.Repositories;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class FormsController : BaseController
    {
        private readonly IFormsRepository _formsRepository;

        public FormsController(IFormsRepository formsRepository)
        {
            _formsRepository = formsRepository;
        }

        /// <summary>
        /// Method for Calculator   
        /// </summary>
        /// <param name="smoker">true/false</param>
        /// <param name="age">age</param>
        /// <param name="coverage">coverage</param>
        /// <returns>smoker_rate/nonsmoker_rate</returns>
        [HttpPost]
        
		public ActionResult GetDataCalculator(bool smoker, string age, string coverage)
        {
            var data = _formsRepository.GetDataCalculator(smoker, age, coverage);
            return Json(new { results = data }, JsonRequestBehavior.AllowGet);
        }
    }
}