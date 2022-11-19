using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Neambc.Neamb.Feature.Account.Pipelines
{
	public class RegisterWebApiRoutes
	{
		public void Process(PipelineArgs args)
		{
			RouteTable.Routes.MapRoute("logout", "api/{controller}/{action}",
				new {controller = "AuthenticationAccount", action = "LogoutForm"});
			RouteTable.Routes.MapRoute("SignInAjax", "api/{controller}/{action}",
				new { controller = "AuthenticationAccount", action = "SignInAjax" });
			RouteTable.Routes.MapRoute("registration", "api/{controller}/{action}",
				new {controller = "Registration", action = "ContinueAction"});
			RouteTable.Routes.MapRoute("checkpassword", "api/{controller}/{action}",
				new {controller = "Duplicate", action = "CheckPassword"});
			RouteTable.Routes.MapRoute("removeduplicateregistration", "api/{controller}/{action}",
				new { controller = "Duplicate", action = "RemoveDuplicateRegistration" });
			RouteTable.Routes.MapRoute("redirectwarmproduct", "api/{controller}/{action}",
				new { controller = "AuthenticationAccount", action = "RedirectWarmProduct" });
			RouteTable.Routes.MapRoute("redirectprofilepage", "api/{controller}/{action}",
				new { controller = "Profile", action = "RedirectProfilePage" });
		    RouteTable.Routes.MapRoute("changeSubscription", "api/{controller}/{action}",
		        new { controller = "SettingsAndSubs", action = "ChangeSubscription" });
		    RouteTable.Routes.MapRoute("EmailValidationRules", "api/{controller}/{action}",
		        new { controller = "EmailValidation", action = "EmailValidationRules" });
			RouteTable.Routes.MapRoute("DeleteFamilyMember", "api/{controller}/{action}",
				new { controller = "InviteFamilyMember", action = "DeleteFamilyMember" });
			RouteTable.Routes.MapRoute("ResendInvitationFamilyMember", "api/{controller}/{action}",
				new { controller = "InviteFamilyMember", action = "ResendInvitationFamilyMember" });
			RouteTable.Routes.MapRoute("DownloadPdfEfulfillment", "api/{controller}/{action}",
				new { controller = "AuthenticationAccount", action = "DownloadPdfEfulfillment" });
            RouteTable.Routes.MapRoute("redirectwarmcoldauthentication", "api/{controller}/{action}",
                new { controller = "AuthenticationAccount", action = "RedirectWarmColdAuthentication" });
			RouteTable.Routes.MapRoute("savepostloginaction", "api/{controller}/{action}",
				new { controller = "AuthenticationAccount", action = "SavePostLoginAction" });
			RouteTable.Routes.MapRoute("stopEmail", "api/{controller}/{action}",
				new { controller = "PermissionEmail", action = "StopEmail" });
			RouteTable.Routes.MapRoute("resumeEmail", "api/{controller}/{action}",
				new { controller = "PermissionEmail", action = "ResumeEmail" });
		}
    }
}