using System.Web.Mvc;
using System.Reflection;

namespace Neambc.Seiumb.Foundation.Sitecore.Extensions
{
    public class ValidateFormHandler : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var controller = controllerContext.HttpContext.Request.Form["fhController"];
            var action = controllerContext.HttpContext.Request.Form["fhAction"];

            return !string.IsNullOrWhiteSpace(controller)
                    && !string.IsNullOrWhiteSpace(action)
                    && controller == controllerContext.Controller.GetType().Name
                    && methodInfo.Name == action;
        }
    }
}