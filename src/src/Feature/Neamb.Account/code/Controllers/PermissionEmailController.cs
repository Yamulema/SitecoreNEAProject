using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers {
	public class PermissionEmailController : BaseController {
		private readonly ISessionAuthenticationManager _sessionManager;
		private readonly IOracleDatabase _oracleDatabase;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly IAuthenticationAccountManager _authenticationAccountManager;

		public PermissionEmailController(ISessionAuthenticationManager sessionManager, IOracleDatabase oracleDatabase
			,IGlobalConfigurationManager globalConfigurationManager, IAuthenticationAccountManager authenticationAccountManager) {
			_sessionManager = sessionManager;
			_oracleDatabase = oracleDatabase;
			_globalConfigurationManager = globalConfigurationManager;
			_authenticationAccountManager = authenticationAccountManager;
		}
		public ActionResult PermissionEmailAction() {
			var model = new PermissionEmailDTO();
			model.Initialize(RenderingContext.Current.Rendering);
			var accountMembership = _sessionManager.GetAccountMembership();
			model.NeambPermissionMail = accountMembership.Profile.NeambPermissionMail;
			model.UserStatus = accountMembership.Status;
			return View("/Views/Neamb.Account/PermissionEmailAction.cshtml", model);
		}

		

		[HttpPost]		
		public JsonResult StopEmail() {
			var resultProcess = ChangePermissionEmail(_globalConfigurationManager.IndicatorNoPermissionEmail);
			if(resultProcess)
            {
				
				return Json(new { wasProcessed = true, message = "Processed" });
			}
			else
            {
				return Json(new { wasProcessed = false, message = "An error has ocurred" });
			}		
						
		}

		[HttpPost]
		public JsonResult ResumeEmail()
		{
			var resultProcess = ChangePermissionEmail(_globalConfigurationManager.IndicatorYesPermissionEmail);
			if (resultProcess)
			{
				return Json(new { wasProcessed = true, message = "Processed" });
			}
			else
			{
				return Json(new { wasProcessed = false, message = "An error has ocurred" });
			}		

		}

		/// <summary>
		/// Call the Oracle sp to change the permission to receive or not the emails
		/// </summary>
		/// <param name="indicatorPermissionEmail">Yes to allow to receive the email. No to stop receiving</param>
		/// <returns>True that is executed sucessfully. Otherwise false</returns>
		private bool ChangePermissionEmail(string indicatorPermissionEmail)
		{
			//It is required warm or hot status
			var accountMembership = _sessionManager.GetAccountMembership();
			if (accountMembership == null || string.IsNullOrEmpty(accountMembership.Mdsid))
			{
				return false;
			}
			try
			{
				PermissionEmailItemJson permissionEmailItemJson = new PermissionEmailItemJson { Indicator = indicatorPermissionEmail, Type = _globalConfigurationManager.TypePermissionEmail };
				var permissionItemList = new List<PermissionEmailItemJson>();
				permissionItemList.Add(permissionEmailItemJson);
				PermissionEmailJson permissionemailJson = new PermissionEmailJson { IndvId = Convert.ToInt32(accountMembership.Mdsid), BusinessUnit = _globalConfigurationManager.BusinessUnitPermissionEmail, Permissions = permissionItemList };
				//Convert the object into json
				var serializeObject = SerializeObject(permissionemailJson);
				//call the Oracle stored procedure
				var responseOracle = _oracleDatabase.PermisionUpdate(serializeObject);
				if (responseOracle != null)
				{
					var resultOracle = JsonConvert.DeserializeObject<ResultPermissionUpdate>(responseOracle,
					new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore
					});
					_authenticationAccountManager.RetrieveAccount(accountMembership, accountMembership.Mdsid);
					_authenticationAccountManager.InitializeAccountMemberData(accountMembership);
					//Return result from the execution of the store procedure.
					return resultOracle.Success;
				}
				else
				{
					return false;
				}

			}
			catch (Exception)
			{
				return false;
			}
		}
		private string SerializeObject(object @object)
		{
			if (@object == null)
			{
				return null;
			}

			//@object = RemoveHtml(@object);

			var sb = new StringBuilder();
			using (var sw = new StringWriter(sb))
			using (var writer = new JsonTextWriter(sw))
			{
				writer.QuoteChar = '\'';
				var ser = new JsonSerializer()
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				};
				ser.Serialize(writer, @object);
			}
			return sb.ToString();
		}
	}
}