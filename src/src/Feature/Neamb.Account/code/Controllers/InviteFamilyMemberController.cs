using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Mvc.Presentation;


namespace Neambc.Neamb.Feature.Account.Controllers {
	public class InviteFamilyMemberController : BaseController {
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly ISessionManager _sessionManager;
		private readonly IOracleDatabase _oracleDatabase;
		private readonly IAccountRepository _accountRepository;
        private readonly ISearchUserNameService _searchUserNameService;

        public InviteFamilyMemberController(
			ISessionAuthenticationManager sessionAuthenticationManager,
			ISessionManager sessionManager,
			IOracleDatabase oracleDatabase,
			IAccountServiceProxy serviceManager,
			IAccountRepository accountRepository,
            ISearchUserNameService searchUserNameService
        ) {
			_accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
			_sessionAuthenticationManager = sessionAuthenticationManager ?? throw new ArgumentNullException(nameof(sessionAuthenticationManager));
			_sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
			_oracleDatabase = oracleDatabase ?? throw new ArgumentNullException(nameof(oracleDatabase));
            _searchUserNameService = searchUserNameService ?? throw new ArgumentNullException(nameof(searchUserNameService));
        }

		/// <summary>
		/// List invite family member (get)
		/// </summary>
		/// <returns></returns>
		public ActionResult ListFamilyMembers() {
			var model = new InviteFamilyMemberDTO();
			model.Initialize(RenderingContext.Current.Rendering);
			var accountUser = _sessionAuthenticationManager.GetAccountMembership();
			model.UserStatus = accountUser.Status;
			var familyNumberLimit = model.Item[Templates.InviteFamilyMember.Fields.FamilyNumberLimit];

			int.TryParse(familyNumberLimit, out var numberLimit);
			numberLimit = numberLimit > 0 ? numberLimit : 6;
			model.LimitRecords = numberLimit;

			//Get the list of members from database
			var listMembers = _oracleDatabase.SelectFamilyMembersList(accountUser.Mdsid);

			if (listMembers == null) {
				model.HasGeneralError = true;
			} else {
				foreach (var memberItem in listMembers) {
					//Get the first data that is separated by |
					var queryMembers = memberItem.Split('|');
					if (queryMembers.Length == 2) {
						//Get the invite family member information from the database passing the id as parameter
						var resultMember = _oracleDatabase.SelectFamilyMemberInfo(queryMembers[0]);
						if (resultMember != null && !string.IsNullOrEmpty(resultMember.Email)) {
                            //Add the family member to the result list
                            var userInfo = _searchUserNameService.SearchUserName(resultMember.Email);
                            var resultResendInvitation = userInfo != null && userInfo.Success && !userInfo.Data.Registered;
                            var familyMemberItemList = new FamilyMemberItemList {
								Identifier = resultMember.IndividualId,
								Name = $"{resultMember.FirstName} {resultMember.LastName}",
								Email = resultMember.Email.ToLower(),
								HasFlagResendInvitation = resultResendInvitation
							};
							model.FamilyMemberList.Add(familyMemberItemList);
							if (model.FamilyMemberList.Count >= numberLimit) {
								break;
							}
						}
					}
				}
			}

			//Verify if this view is displayed as a result of the redirection from add family member page
			var resultRedirectionAdd = _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.AddFamilyMember);
			if (!string.IsNullOrEmpty(resultRedirectionAdd)) {
				model.IsRedirectionAdd = true;
				_sessionManager.Remove(ConstantsNeamb.AddFamilyMember);
			}

			return View("/Views/Neamb.Account/InviteFamilyMember.cshtml", model);
		}

		/// <summary>
		/// Delete Family Member
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		
		public ActionResult DeleteFamilyMember(string memberId) {
			var accountUser = _sessionAuthenticationManager.GetAccountMembership();
			var responseDeleteExecution = _oracleDatabase.DeleteFamilyMember(accountUser.Mdsid, memberId);
			//if (responseDeleteExecution) {
			// TODO: Send an email
			//}
			return Json(new {
				result = responseDeleteExecution ? "ok" : "error"
			}, JsonRequestBehavior.AllowGet
			);
		}

		[HttpPost]
		
		public ActionResult ResendInvitationFamilyMember(string memberId) {
			var accountUser = _sessionAuthenticationManager.GetAccountMembership();
			var resultResend = _accountRepository.SendExactTargetAddFamilyMember(
				accountUser.Profile.FirstName,
				accountUser.Profile.LastName,
				accountUser.Mdsid,
				accountUser.Username
			);
			return Json(
				new {
					result = resultResend ? "ok" : "error"
				},
				JsonRequestBehavior.AllowGet
			);
		}
	}
}