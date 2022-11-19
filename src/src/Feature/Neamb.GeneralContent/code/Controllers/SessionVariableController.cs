using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.StringExtensions;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class SessionVariableController : BaseController
    {
        private ISessionManager _sessionManager;

        public SessionVariableController(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public ActionResult SessionVariable()
        {
            var model = new SessionVariableDTO();
            model.Initialize(RenderingContext.Current.Rendering);

            model.SessionVariableDictionary = new Dictionary<string, string>();

            if (!model.SessionVariable.IsNullOrEmpty())
            {
                String[] SessionVariables = model.SessionVariable.Split('\n');

                foreach (var SessionVariable in SessionVariables)
                {
                    String[] Variables = SessionVariable.Split('=');
                    _sessionManager.StoreInSession<string>(Variables[0], Variables[1]);
                    model.SessionVariableDictionary.Add(Variables[0], Variables[1]);
                }
            }

            return View("/Views/Neamb.GeneralContent/Renderings/SessionVariable.cshtml", model);
        }
    }
}