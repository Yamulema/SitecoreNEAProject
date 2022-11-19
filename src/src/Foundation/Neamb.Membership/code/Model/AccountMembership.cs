using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.Membership.Model {
	/// <summary>
	/// Class that contains the information of the user retrieved from the webservice RetrieveUserData
	/// </summary>
	[Serializable]
	public class AccountMembership : ICloneable {
		public StatusEnum Status {
			get; set;
		}
		public string Username {
			get; set;
		}
		public string Mdsid {
			get; set;
		}
		public Profile Profile {
			get; set;
		}
		public List<Beneficiary> Beneficiaries {
			get; set;
		}
		public AccountMembership() {
			Status = StatusEnum.Unknown;
			Profile = new Profile();
			Beneficiaries = new List<Beneficiary>();
		}

		public object Clone() {
			return new AccountMembership() {
				Profile = new Profile() {
					EditingStatus = Profile.EditingStatus,
					Email = Profile.Email,
					GenderCode = Profile.GenderCode,
					LastName = Profile.LastName,
					Phone = Profile.Phone,
					FirstName = Profile.FirstName,
					DateOfBirth = Profile.DateOfBirth,
					StateCode = Profile.StateCode,
					AccountState = Profile.AccountState,
					City = Profile.City,
					ZipCode = Profile.ZipCode,
					StreetAddress = Profile.StreetAddress,
					ComplifesignDate = Profile.ComplifesignDate,
					EmailPermissionIndicator = Profile.EmailPermissionIndicator,
					IAId = Profile.IAId,
					Introlifeenddate = Profile.Introlifeenddate,
					MembershipCategoryCode = Profile.MembershipCategoryCode,
                    IsNeaCurrentMember = Profile.IsNeaCurrentMember,
					NeaMembershipType = Profile.NeaMembershipType,
					NeaMembershipTypeName= Profile.NeaMembershipTypeName,
					NewEnvInd = Profile.NewEnvInd,
					Newmembersegmentindicator = Profile.Newmembersegmentindicator,
                    IsRegistered = Profile.IsRegistered,
					SeaName = Profile.SeaName,
					SeaNumber = Profile.SeaNumber,
                    IsSeiuCurrentMember = Profile.IsSeiuCurrentMember,
					SeiuLocalName = Profile.SeiuLocalName,
					SeiuLocalNumber = Profile.SeiuLocalNumber,
					UnionId = Profile.UnionId,
					Webuserid = Profile.Webuserid,
                    LeaName = Profile.LeaName,
                    LeaNumber = Profile.LeaNumber
				},
				Mdsid = Mdsid,
				Status = Status,
				Beneficiaries = Beneficiaries,
				Username = Username
			};
		}

		public bool IsUserWithKnownState() {
			return (Status == StatusEnum.Hot) ||
				(Status == StatusEnum.WarmHot) ||
				(Status == StatusEnum.WarmCold);
		}
	}
}