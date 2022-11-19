using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Services.AuthenticatePassword;
using Neambc.Neamb.Foundation.MBCData.Services.DeleteUser;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Feature.Forms.Repositories;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.WebServices;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class DuplicateRegistrationFormController : BaseController
    {
        private readonly IFormsRepository _formsRepository;
        private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IAuthenticatePasswordService _authenticatePasswordService;
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IUserRepository _userRepository;

        public DuplicateRegistrationFormController(IFormsRepository formsRepository, IAuthenticationManager authenticationManager,
            IWebServicesConfiguration webServicesConfiguration, IDeleteUserService deleteUserService, IAuthenticatePasswordService authenticatePasswordService,
            ISeiumbProfileManager seiumbProfileManager, IUserRepository userRepository)
        {
            _formsRepository = formsRepository;
            _authenticationManager = authenticationManager;
            _deleteUserService = deleteUserService;
            _webServicesConfiguration = webServicesConfiguration;
            _authenticatePasswordService = authenticatePasswordService;
            _seiumbProfileManager = seiumbProfileManager;
            _userRepository = userRepository;
        }

        public ActionResult DuplicateRegistrationForm(string redirect)
        {
            var profile = _seiumbProfileManager.GetProfile();
            var model = new DuplicateRegistrationFormModel { RedirectAction = redirect };
            model.Initialize(RenderingContext.Current.Rendering);
            model.CurrentEmail = profile.DuplicateEmail;
            Sitecore.Diagnostics.Log.Info($"DuplicateRegistration method login:{model.CurrentEmail}", this);//Debug data
            Sitecore.Diagnostics.Log.Info($"DuplicateRegistration method emails:{profile.Emails}", this);
            model.EmailList = new List<EmailDuplicate>();
            if (string.IsNullOrEmpty(profile.Emails)) return View("/Views/Forms/DuplicateRegistrationForm.cshtml", model);
            var emailRawList = profile.Emails.Split(';');
            foreach (var data in emailRawList)
            {
                var userdata = new EmailDuplicate();
                var rowData = data.Split('|');
                userdata.Email = !string.IsNullOrEmpty(rowData[0]) ? rowData[0] : string.Empty;
                userdata.FirstName = !string.IsNullOrEmpty(rowData[1]) ? rowData[1] : string.Empty;
                userdata.LastName = !string.IsNullOrEmpty(rowData[2]) ? rowData[2] : string.Empty;
                userdata.Dob = !string.IsNullOrEmpty(rowData[3]) ? rowData[3] : string.Empty;
                if (userdata.Email.Equals(model.CurrentEmail))
                    model.EmailList.Insert(0, userdata);
                else
                    model.EmailList.Add(userdata);
            }
            return View("/Views/Forms/DuplicateRegistrationForm.cshtml", model);
        }

        [HttpPost]
        public JsonResult CheckPassword(string username, string password)
        {
            var response = _authenticatePasswordService.AuthenticatePasswordStatus(username, password, Convert.ToInt32(_webServicesConfiguration.UnionId));
            return Json(new { Response = response.Data.authenticated ? 0 : 1 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to remove the characters that are after the |
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>string cut</returns>
        private string DepureEmail(string input)
        {
            var result = string.Empty;
            var index1 = input.IndexOf("|", StringComparison.InvariantCultureIgnoreCase);
            if (index1 != -1) result = input.Remove(index1, input.Length - index1);
            return result;
        }

        [HttpPost]
        public JsonResult FormSubmit(string selectedid, string password)
        {
            HttpContext.Session[ConstantsSeiumb.DuplicateLogin] = "0";

            var seiuProfile = _seiumbProfileManager.GetProfile();

            var errors = new List<KeyValuePair<bool, string>>();

            var emailList = new List<string>();

            if (string.IsNullOrEmpty(password))
            {
                Log.Error($"method: FormSubmit, model password is empty.", this);
            }

            if (!string.IsNullOrEmpty(seiuProfile.Emails))
            {
                emailList = seiuProfile.Emails.Split(';').Select(DepureEmail).ToList();
            }

            if (errors.Any())
            {
                return Json(new { errors, status = "ok", selectedUser = selectedid }, JsonRequestBehavior.AllowGet);
            }

            var cleanList = emailList.Where(x => !x.Contains(selectedid)).ToList();

            if (cleanList.Any())
            {
                errors.AddRange(from deleteRegister in cleanList
                                select deleteRegister.Split('|') into userdata
                                select _deleteUserService.DeleteUserStatus(userdata[0], Convert.ToInt32(_webServicesConfiguration.UnionId))
                                into deleteResponse
                                where deleteResponse.Data.deleted == false
                                select new KeyValuePair<bool, string>(true, deleteResponse.Error.Messages.ToString()));

                //exact target call
                if (errors.Count == 0)
                {
                    _formsRepository.UpdateUserStatus(selectedid);
                    //_formsRepository.DuplicateRegistrationExactTarget(seiuProfile.MdsId, selectedid, seiuProfile.FirstName, 
                    //                                                  seiuProfile.LastName, string.Empty, string.Empty, cleanList);
                }

            }

            if (!selectedid.Equals(seiuProfile.Email))
            {
                _userRepository.LogoutUser();

                var cellCode = HttpContext.Session[ConstantsSeiumb.SourceCode] != null ? HttpContext.Session[ConstantsSeiumb.SourceCode].ToString() : string.Empty;

                Sitecore.Diagnostics.Log.Info($"Registration cellCode: {cellCode}", this);

                if (!string.IsNullOrEmpty(password))
                {
                    var loginResponse = _authenticationManager.AuthenticateUser(seiuProfile, selectedid, password, cellCode);

                    if (loginResponse.Success && loginResponse.Data.LoggedIn)
                    {
                        _authenticationManager.FillUserData(seiuProfile, loginResponse.Data.MdsIdAsString, FormConstants.NEA_COOKIE_MDSID, true, loginResponse.Data.RegistrationDuplicateOldFormat);
                    }
                }
            }
            else
            {
                _authenticationManager.FillUserData(seiuProfile, seiuProfile.MdsId, FormConstants.NEA_COOKIE_MDSID, true, seiuProfile.Emails);
            }

            return Json(new { errors, status = "ok", selectedUser = selectedid }, JsonRequestBehavior.AllowGet);
        }
    }
}