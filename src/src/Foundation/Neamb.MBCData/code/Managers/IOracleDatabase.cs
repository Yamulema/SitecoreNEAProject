using System.Collections.Generic;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	public interface IOracleDatabase {
		/// <summary>
		/// Execution of the stored procedure  MDS_UTIL.ORDER_FULFILL@MBCDB 
		/// </summary>
		/// <param name="msdId">User id</param>
		/// <param name="itemCode"></param>
		/// <param name="cellCode"></param>
		/// <param name="value4"></param>
		/// <returns></returns>
		bool SelectOrderFulfill(int msdId, string itemCode, string cellCode, string value4);

		string SelectItemCodeForProductCode(string productCode);
		bool InsertReminder(string reminderId, string mdsIndividualId);
		int ReminderLogCount(string reminderId, string mdsIndividualId);
		bool EnrollInSweepstakes(string msdId, string sweepstakesId);
		IReadOnlyList<ViewBeneficiary> SelectBeneficiaries(string mdsIndividualId);
		bool InsertComplimentaryLife(ComplimentaryLifeDb complimentaryLifeDb);
		string SelectLastUpdateDate(string mdsId);
		IReadOnlyList<string> SelectLifeInsuranceRates(string coverage, string term, string age, string memberSpouse, string smoker);
		string SelectLpgtRates(string coverage, string age);
		string SelectMdsId(string individualId);
		string SelectImsId(string accountMembershipMdsId);
		IEnumerable<string> SelectRates(string state, string zip, int[] ages, string option);
		int SelectZipCodeCount(string state);
		bool InsertFamilyMember(string msdId, string firstName, string lastName, string email, string dateOfBirth, string relationshipCode);
		IEnumerable<string> SelectFamilyMembersList(string mdsId);
		ViewIndividual SelectFamilyMemberInfo(string familyMemberId);
		bool DeleteFamilyMember(string msdId, string memberId);
		bool IsSpouseDomesticPartnerAssociated(string mdsId);
		bool SelectItemCodeExists(string itemCode);
		//bool SelectMaterialIdExists(string materialId);
		IEnumerable<ViewUnredeemedRewards> SelectUnredeemedRewards(string mdsId);
		decimal SelectUnredeemedRewardsTotal(string mdsId);
        IReadOnlyList<ViewSeminar> ViewAllSeminar();
        bool InsertInSeminar(int msdId, string leaCode);
        IReadOnlyList<ViewSeminarReg> ViewAllSeminarReg();
        bool DeleteAllTestUsers();
        IList<ViewOmni> ExecuteViewOmni(string mdsid, string productCode);
        bool Rakuten_Registration(string mdsId, string emailAddress, string regId, string ebToken,
                             string unionId, string cellCode);
        bool RakutenRegExists(string emailAddress);
        ViewRakutenReg ViewRakutenRegs(string emailAddress);
        bool Update_Favorite_Stores(string mdsId, string emailAddress, string favoriteStores);
        string Search_Favorite_Stores(string emailAddress);
		string PermisionUpdate(string jsonObject);

	}
}