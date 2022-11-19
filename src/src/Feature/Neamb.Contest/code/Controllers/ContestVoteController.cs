using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Contest.Interfaces;
using Neambc.Neamb.Feature.Contest.Model;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Contest.Controllers
{
    public class ContestVoteController : BaseController
    {
        private readonly IContestVoteManager _contestVoteManager;

        public ContestVoteController(IContestVoteManager contestVoteManager)
        {
            _contestVoteManager = contestVoteManager;
        }

        public ActionResult ContestVote()
        {
            var model = _contestVoteManager.GetContestModel(RenderingContext.Current.Rendering, 0);
            return View("/Views/Neamb.Contest/ContestVote.cshtml", model);
        }
        [HttpPost]
        [ValidateFormHandler]
        public ActionResult ContestVote(int page)
        {
            var model = _contestVoteManager.GetContestModel(RenderingContext.Current.Rendering, page);
            return View("/Views/Neamb.Contest/ContestVote.cshtml", model);
        }
    }
}