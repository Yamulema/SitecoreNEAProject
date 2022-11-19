using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;

namespace Neambc.Neamb.Project.Web.Controllers
{
    public class UserStateController : BaseController
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;

        public UserStateController(
            ISessionAuthenticationManager sessionAuthenticationManager)
        {
            _sessionAuthenticationManager = sessionAuthenticationManager;
        }

        [HttpGet]
        public ActionResult GetUserprofileInformation()
        {
            var result = _sessionAuthenticationManager.GetAccountMembership();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}