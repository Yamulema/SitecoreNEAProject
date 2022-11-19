using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Feature.Account.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Controllers {
	public class AddFamilyMemberController : BaseController {
		private const string ADD_FAMILY_MEMBER_VIEW = "/Views/Neamb.Account/AddFamilyMember.cshtml";
		private readonly IAccountRepository _accountRepository;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly ISessionManager _sessionManager;
		private readonly IOracleDatabase _oracleManager;

		public AddFamilyMemberController(IAccountRepository accountRepository,
			ISessionAuthenticationManager sessionAuthenticationManager,
			ISessionManager sessionManager, IOracleDatabase oracleManager) {
			_accountRepository = accountRepository;
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_sessionManager = sessionManager;
			_oracleManager = oracleManager;
		}

		/// <summary>
		/// Add family member get method
		/// </summary>
		/// <returns></returns>
		public ActionResult AddFamilyMember() {
			var model = new FamilyMemberDTO();
			model.Initialize(RenderingContext.Current.Rendering);
			var accountUser = _sessionAuthenticationManager.GetAccountMembership();
			//Get the relationship list
			model.RelationshipList = GetRelationshipItems(model.Relationship, accountUser.Mdsid);
			model.UserStatus = accountUser.Status;
			//Verify the tooltips
			SetTooltipProperties(model, RenderingContext.Current.Rendering.Item);

			return View(ADD_FAMILY_MEMBER_VIEW, model);
		}

		private static void SetTooltipProperties(FamilyMemberDTO model, Item contextItem) {
			model.HasTooltipFirstName = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.FirstNameTooltip]);
			model.HasTooltipLastName = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.LastNameTooltip]);
			model.HasTooltipEmail = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.EmailTooltip]);
			model.HasTooltipBirthDate = !string.IsNullOrEmpty(contextItem[Templates.Profile.Fields.BirthDateTooltip]);
			model.HasTooltipRelationship =
				!string.IsNullOrEmpty(contextItem[Templates.FamilyMember.Fields.RelationshipTooltip]);
		}

		/// <summary>
		///  Add family member post method
		/// </summary>
		/// <param name="model">Data post</param>
		/// <param name="day">birtdate day</param>
		/// <param name="month">birtdate month</param>
		/// <param name="year">birtdate year</param>
		/// <returns></returns>
		[HttpPost]
		
		public ActionResult AddFamilyMember(FamilyMemberDTO model, string day, string month, string year) {
			model.BirthDate = $"{month}{day}{year}";
			model.Initialize(RenderingContext.Current.Rendering);
			SetTooltipProperties(model, RenderingContext.Current.Rendering.Item);
			var accountUser = _sessionAuthenticationManager.GetAccountMembership();
			model.RelationshipList = GetRelationshipItems(model.Relationship, accountUser.Mdsid);
			model.UserStatus = accountUser.Status;
			//Check errors in first name
			model.ErrorsFirstName =
				ValidationFieldHelper.SetErrorsField(ViewData.ModelState[nameof(model.FirstName)], true, true, true);
			//Check errors in last name
			model.ErrorsLastName =
				ValidationFieldHelper.SetErrorsField(ViewData.ModelState[nameof(model.LastName)], true, true, true);
			//Check errors in birthdate
			var customDateBirthErrors = _accountRepository.HasDateBirthCustomValidationErrors(model);
			if (ModelState.IsValid && !customDateBirthErrors) {
				//Execute the insertion in  the database
				var resultOracle = _oracleManager.InsertFamilyMember(
					accountUser.Mdsid,
					model.FirstName,
					model.LastName,
					model.Email,
					model.BirthDate,
					model.Relationship
				);
				//Result success
				if (resultOracle) {
					_accountRepository.SendExactTargetAddFamilyMember(accountUser.Profile.FirstName, accountUser.Profile.LastName, accountUser.Mdsid, accountUser.Username);
					//Save successfully result in session
					_sessionManager.StoreInSession<string>(ConstantsNeamb.AddFamilyMember, "1");
					return Redirect(_accountRepository.GetInviteFamilyPageUrl());
				} else {
					model.HasGeneralError = true;
					return View(ADD_FAMILY_MEMBER_VIEW, model);
				}
			} else {
				return View(ADD_FAMILY_MEMBER_VIEW, model);
			}
		}

		/// <summary>
		/// Get the list of relationship
		/// </summary>
		/// <param name="relationshipValue">Selected value</param>
		/// <param name="mdsid">Mdsid</param>
		/// <returns></returns>
		private List<SelectListItem> GetRelationshipItems(string relationshipValue, string mdsid) {
			var listItems = new List<SelectListItem>();

			//Get the global items from Sitecore
			var relationshipList = Sitecore.Context.Database.GetItem(Templates.RelationshipInviteFamilyGlobal.ID).GetChildren();

			//Remove these ones: Sister , brother, other
			var itemsFilter = relationshipList.Where(item =>
			  (item[Templates.MbcDbField.Fields.MbcDbId] != RelationshipEnum.Sister.GetDescription()) &&
			  (item[Templates.MbcDbField.Fields.MbcDbId] != RelationshipEnum.Brother.GetDescription()) &&
			  (item[Templates.MbcDbField.Fields.MbcDbId] != RelationshipEnum.Other.GetDescription())
			);

			//Check Spouse or domestic partner
			var resultSpouse = _oracleManager.IsSpouseDomesticPartnerAssociated(mdsid);
			//If already has a member spouse or domestic partner don't include in the combo box
			if (resultSpouse) {
				itemsFilter = itemsFilter.Where(item =>
					(item[Templates.MbcDbField.Fields.MbcDbId] != RelationshipEnum.Spouce.GetDescription()) &&
					(item[Templates.MbcDbField.Fields.MbcDbId] != RelationshipEnum.DomesticPartner.GetDescription())
				);
			}

			//Build the items for the combobox
			foreach (var relationship in itemsFilter) {
				listItems.Add(new SelectListItem {
					Text = relationship.Name,
					Value = relationship[Templates.MbcDbField.Fields.MbcDbId],
					Selected = !string.IsNullOrEmpty(relationshipValue) &&
						(relationship[Templates.MbcDbField.Fields.MbcDbId] == relationshipValue)
				});
			}

			return listItems;
		}
	}
}