using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Feature.GeneralContent.Services;
using Neambc.Neamb.Foundation.Configuration.Services.ActionReminder;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Diagnostics;
using Neambc.Neamb.Foundation.MBCData.Managers;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class MiscController : BaseController
    {
        private readonly IActionReminderService _actionReminderService;
        private readonly IBulkService _bulkService;
        private readonly IOracleDatabase _oracleManager;
        private readonly ISmartPublish _smartPublish;
        private readonly ISmartPublishLog _smartPublishLog;

        public MiscController(IActionReminderService actionReminderService, IBulkService bulkService, IOracleDatabase oracleManager, ISmartPublish smartPublish, ISmartPublishLog smartPublishLog)
        {
            _actionReminderService = actionReminderService;
            _bulkService = bulkService;
            _oracleManager = oracleManager;
            _smartPublish = smartPublish;
            _smartPublishLog = smartPublishLog;
        }
        [HttpPost]
        public ActionResult Migrate()
        {

            var result = _actionReminderService.Migrate();
            return Json(result);
        }
        [HttpPost]
        public ActionResult RemoveGa(RemoveRequest request)
        {
            var result = _bulkService.RemoveOnClickEvent("ga_event", request.Publish);
            return Json(result);
        }
        [HttpPost]
        public ActionResult RemoveVisited(RemoveVisitedRequest request)
        {
            try
            {
                Log.Warn($"RemoveVisited for {request.PageType}", this);
                var current = Sitecore.Context.Database;
                if (Request.Url.Host.Equals(Configuration.HostForVisitedPage))
                {
                    _actionReminderService.RemoveVisited(request.PageType);
                }
                else
                {
                    Log.Warn($"Skipped RemoveVisited db name is not the host", this);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error in RemoveVisited {e.Message}", e, this);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult DeleteAllTestUsers()
        {
            var ret = false;
            try
            {
                ret = _oracleManager.DeleteAllTestUsers();
            }
            catch (Exception e)
            {
                Log.Error($"Error in RemoveTestUsers {e.Message}", e, this);
            }
            return Json(new
            {
                result = ret ? "ok" : "error"
            }, JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public ActionResult Autopublish()
        {
            _smartPublish.PublishItem(new Sitecore.Data.ID(Sitecore.Configuration.Settings.GetSetting("NeambItemId"))); //neamb root
            _smartPublish.PublishItem(new Sitecore.Data.ID(Sitecore.Configuration.Settings.GetSetting("SeiumbItemId")));  //neambc root

            return Json("Check logs for publish report");
        }
    }
}