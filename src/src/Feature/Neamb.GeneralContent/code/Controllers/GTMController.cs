using System;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class GTMController : BaseController
    {
        private readonly IGTMLog _gtmLog;

        public GTMController(IGTMLog gtmLog) {
            _gtmLog = gtmLog;
        }

        [HttpPost]
        public ActionResult Tracking(string eventData)
        {
            
            try
            {
                _gtmLog.Info($"{DateTime.Now.Date:d} - Tracking GTM Event: {eventData}");
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _gtmLog.Error($"Error in Tracking GTM Event", e);
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}