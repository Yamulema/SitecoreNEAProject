using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Interfaces;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Account.UnitTest.Fakes {
	public class FakeProfileManager : IProfileManager {

		#region IProfileManager
		public ProfileDTO GetProfileDto(Item datasource, string newcell, string oldcell, string msrName, bool isDraft = false) {
			throw new System.NotImplementedException();
		}

		public ProfileDTO SaveProfileDto(ProfileDTO model, DateParts dateParts,
			bool isValidModelState, ViewDataDictionary viewData, Item item, bool isDraft = false) {
			throw new System.NotImplementedException();
		}
		public string GetGtmAction(string isFormPassword, Item contextItem) {
			throw new System.NotImplementedException();
		}

		public AccountMembership CreateNewAccountMembership(string mdsId)
		{
			throw new System.NotImplementedException();
		}

        public ProfileDTO SaveProfileDto(ProfileDTO model, bool isValidModelState, ViewDataDictionary viewData, Item item, bool isDraft = false)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }

}
