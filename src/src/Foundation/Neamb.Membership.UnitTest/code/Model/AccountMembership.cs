using System;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.Membership.Enums;
using NUnit.Framework;
using ServiceStack.Common.Extensions;
using SUT = Neambc.Neamb.Foundation.Membership.Model;


namespace Neambc.Neamb.Foundation.Membership.UnitTest.Model {

	[TestFixture]
	public class AccountMembership {

		[Test]
		public void ConstructorSetsDefaults() {
			var ac = new SUT.AccountMembership();
			Assert.AreEqual(SUT.StatusEnum.Unknown, ac.Status);
			Assert.IsNotNull(ac.Profile);
			Assert.IsNotNull(ac.Beneficiaries);
			Assert.AreEqual(0, ac.Beneficiaries.Count);
		}

		[Test]
		public void IsUserWithKnownState_ReturnsAppropriate() {
			var ac = new SUT.AccountMembership();
			foreach (var val in Enum.GetValues(typeof(SUT.StatusEnum)).ToList<SUT.StatusEnum>()) {
				ac.Status = val;
				switch (val) {
					case SUT.StatusEnum.Hot:
					case SUT.StatusEnum.WarmHot:
					case SUT.StatusEnum.WarmCold:
						Assert.IsTrue(ac.IsUserWithKnownState());
						break;
					default:
						Assert.IsFalse(ac.IsUserWithKnownState());
						break;
				}
			}
		}

		[Test]
		public void CloneCreatesNewProfile() {
			var ac = new SUT.AccountMembership {
				Profile = new SUT.Profile {
					EditingStatus = EditingStatus.Saved,
					Email = "one@two.three",
					GenderCode = "X",
					LastName = "tester",
					Phone = "2125551212",
					FirstName = "tester",
					DateOfBirth = "19800101",
					StateCode = "NY",
					AccountState = "messy",
					City = "Buffalo",
					ZipCode = "11011",
					StreetAddress = "1456 NE Main Ave",
					ComplifesignDate = "20190101",
					EmailPermissionIndicator = "Y",
					IAId = "WUT",
					Introlifeenddate = "20190101",
					MembershipCategoryCode = "WUT",
					IsNeaCurrentMember = true,
					NeaMembershipType = "WUT",
					NewEnvInd = "WUT",
					Newmembersegmentindicator = "WUT",
					IsRegistered = true,
					SeaName = "IDK",
					SeaNumber = "IDK",
					IsSeiuCurrentMember = false,
					SeiuLocalName = "IDK",
					SeiuLocalNumber = "IDK",
					UnionId = "IDK",
					Webuserid = "IDK"
				},
				Mdsid = "123",
				Status = SUT.StatusEnum.Cold,
				Beneficiaries = new List<SUT.Beneficiary> {
					new SUT.Beneficiary {
						DisplayName = "one",
						DisplayRelationship = "two",
						DisplayShare = "share",
						Email = "good@bad.ugly",
						FirstName = "first",
						Id = "id",
						LastName = "last",
						MiddleInitial = "m",
						OtherEntityName = "otherwise",
						Relationship = "self",
						Type = BeneficiaryType.OtherEntity

					}
				},
				
				Username = "renegade master"

			};
			var result = ac.Clone() as SUT.AccountMembership;
			Assert.IsNotNull(result);
			Assert.AreEqual(ac.Profile.EditingStatus, result.Profile.EditingStatus);
			Assert.AreEqual(ac.Profile.Email, result.Profile.Email);
			Assert.AreEqual(ac.Profile.GenderCode, result.Profile.GenderCode);
			Assert.AreEqual(ac.Profile.LastName, result.Profile.LastName);
			Assert.AreEqual(ac.Profile.Phone, result.Profile.Phone);
			Assert.AreEqual(ac.Profile.FirstName, result.Profile.FirstName);
			Assert.AreEqual(ac.Profile.DateOfBirth, result.Profile.DateOfBirth);
			Assert.AreEqual(ac.Profile.StateCode, result.Profile.StateCode);
			Assert.AreEqual(ac.Profile.AccountState, result.Profile.AccountState);
			Assert.AreEqual(ac.Profile.City, result.Profile.City);
			Assert.AreEqual(ac.Profile.ZipCode, result.Profile.ZipCode);
			Assert.AreEqual(ac.Profile.StreetAddress, result.Profile.StreetAddress);
			Assert.AreEqual(ac.Profile.ComplifesignDate, result.Profile.ComplifesignDate);
			Assert.AreEqual(ac.Profile.EmailPermissionIndicator, result.Profile.EmailPermissionIndicator);
			Assert.AreEqual(ac.Profile.IAId, result.Profile.IAId);
			Assert.AreEqual(ac.Profile.Introlifeenddate, result.Profile.Introlifeenddate);
			Assert.AreEqual(ac.Profile.MembershipCategoryCode, result.Profile.MembershipCategoryCode);
			Assert.AreEqual(ac.Profile.IsNeaCurrentMember, result.Profile.IsNeaCurrentMember);
			Assert.AreEqual(ac.Profile.NeaMembershipType, result.Profile.NeaMembershipType);
			Assert.AreEqual(ac.Profile.NewEnvInd, result.Profile.NewEnvInd);
			Assert.AreEqual(ac.Profile.Newmembersegmentindicator, result.Profile.Newmembersegmentindicator);
			Assert.AreEqual(ac.Profile.IsRegistered, result.Profile.IsRegistered);
			Assert.AreEqual(ac.Profile.SeaName, result.Profile.SeaName);
			Assert.AreEqual(ac.Profile.SeaNumber, result.Profile.SeaNumber);
			Assert.AreEqual(ac.Profile.IsSeiuCurrentMember, result.Profile.IsSeiuCurrentMember);
			Assert.AreEqual(ac.Profile.SeiuLocalName, result.Profile.SeiuLocalName);
			Assert.AreEqual(ac.Profile.SeiuLocalNumber, result.Profile.SeiuLocalNumber);
			Assert.AreEqual(ac.Profile.UnionId, result.Profile.UnionId);
			Assert.AreEqual(ac.Profile.Webuserid, result.Profile.Webuserid);

			//
			Assert.AreEqual(ac.Beneficiaries.Count, result.Beneficiaries.Count);
			for (var i = 0; i < ac.Beneficiaries.Count; i++) {
				var expected = ac.Beneficiaries[i];
				var actual = result.Beneficiaries[i];
				Assert.AreEqual(expected.DisplayName, actual.DisplayName);
				Assert.AreEqual(expected.DisplayRelationship, actual.DisplayRelationship);
				Assert.AreEqual(expected.DisplayShare, actual.DisplayShare);
				Assert.AreEqual(expected.Email, actual.Email);
				Assert.AreEqual(expected.FirstName, actual.FirstName);
				Assert.AreEqual(expected.Id, actual.Id);
				Assert.AreEqual(expected.LastName, actual.LastName);
				Assert.AreEqual(expected.Id, actual.Id);
				Assert.AreEqual(expected.MiddleInitial, actual.MiddleInitial);
				Assert.AreEqual(expected.OtherEntityName, actual.OtherEntityName);
				Assert.AreEqual(expected.Relationship, actual.Relationship);
				Assert.AreEqual(expected.Type, actual.Type);
			}
			Assert.AreEqual(ac.Profile.UnionId, result.Profile.UnionId);
		}
	}
}
