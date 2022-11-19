using System;
using System.Web.Mvc;
using Sitecore.Mvc.Controllers;

namespace Neambc.Neamb.Foundation.Configuration.Utility
{
    public class BaseController: SitecoreController
    {
        protected override void HandleUnknownAction(string actionName)
        {
            try
            {
                this.ActionInvoker.InvokeAction(this.ControllerContext, "Redirection");
            }
            catch (Exception) {
                throw;
            }
        }

        public ActionResult Redirection()
        {
            new HttpNoFoundResult();
            return Redirect("/404");
        }
    }
}