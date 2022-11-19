using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Account.Interfaces {
	public interface IProfileManager {
		ProfileDTO GetProfileDto(Item datasource, string newcell, string oldcell, string msrName, bool isDraft = false);
		//delete
		ProfileDTO SaveProfileDto(ProfileDTO model, 
			DateParts dateParts,
			bool IsValidModelState, ViewDataDictionary viewData, Item item, bool isDraft = false);
		ProfileDTO SaveProfileDto(ProfileDTO model,
			bool isValidModelState, ViewDataDictionary viewData, Item item, bool isDraft = false);
		string GetGtmAction(string isFormPassword, Item contextItem);
		AccountMembership CreateNewAccountMembership(string mdsId);
	}
}