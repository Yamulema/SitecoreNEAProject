using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Neambc.Seiumb.Foundation.Sitecore;
using NUnit.Framework;
using Oracle.ManagedDataAccess.Client;
using SUT = Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Db;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers {
	[TestFixture]
	public class OracleDatabase {

		#region Fields
		private Mock<IConnectedCommand> _currentCommandMock;
		private Mock<IConnectedCommandFactory> _commandFactoryMock;
		private Mock<ILog> _logMock;
		private SUT.OracleDatabase _mgr;
		#endregion

		#region Private Methods
		private void ConfigureSingleRowMock(string value = null) {
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, true))
				.Returns(new CommandResult(value));
		}
		private void ConfigureMultipleRowMock(List<List<object>> values, string[] columnNames) {
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, false))
				.Returns(new CommandResult(values, columnNames));
		}

		#endregion

		[SetUp]
		public void SetUp() {
			_currentCommandMock = null;
			_logMock = new Mock<ILog>();
			_currentCommandMock = new Mock<IConnectedCommand>();
			_commandFactoryMock = new Mock<IConnectedCommandFactory>();
			_commandFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<OracleParameter[]>()))
				.Returns((string query, OracleParameter[] parameters) => _currentCommandMock.Object);

			_mgr = new SUT.OracleDatabase(_commandFactoryMock.Object, _logMock.Object, "connect!");
		}

		[Test]
		public void Constructor_ThrowsOnBadArgs() {
			// ReSharper disable ObjectCreationAsStatement
			Assert.Throws<ArgumentNullException>(() => new SUT.OracleDatabase(null, _logMock.Object, string.Empty));
			Assert.Throws<ArgumentNullException>(() => new SUT.OracleDatabase(_commandFactoryMock.Object, null, string.Empty));
			Assert.Throws<NullReferenceException>(() => new SUT.OracleDatabase(_commandFactoryMock.Object, _logMock.Object));
			// ReSharper enable ObjectCreationAsStatement
		}

		[Test]
		public void SelectOrderFulfill_ReturnsSuccess() {
			ConfigureSingleRowMock();
			var result = _mgr.SelectOrderFulfill(123, "code", "code", "value4");
			Assert.IsTrue(result);
		}
		[Test]
		public void SelectOrderFulfill_ReturnsFailure() {
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, true))
				.Returns(new CommandResult() {
					ErrorMessage = "whoops"
				});
			var result = _mgr.SelectOrderFulfill(123, "code", "code", "value4");
			Assert.IsFalse(result);
		}

		[Test]
		public void SelectItemCodeForProductCode_ReturnsValue() {
			ConfigureSingleRowMock("item");
			Assert.AreEqual("item", _mgr.SelectItemCodeForProductCode("product"));
		}

		[Test]
		public void InsertReminder_ReturnsSuccess() {
			ConfigureSingleRowMock();
			var result = _mgr.InsertReminder("reminderId", "mdsIndividualId");
			Assert.IsTrue(result);
		}
		[Test]
		public void InsertReminder_ReturnsFailure() {
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, true))
				.Returns(new CommandResult {
					ErrorMessage = "whoops"
				});
			var result = _mgr.InsertReminder("reminderId", "mdsIndividualId");
			Assert.IsFalse(result);
		}

		[Test]
		public void ReminderLogCount_ReturnsZeroOnNull() {
			ConfigureSingleRowMock();
			var result = _mgr.ReminderLogCount("reminderId", "mdsIndividualId");
			Assert.AreEqual(0, result);
		}
		[Test]
		public void ReminderLogCount_ReturnsPositiveIntegerWhenValue() {
			ConfigureSingleRowMock("12");
			var result = _mgr.ReminderLogCount("reminderId", "mdsIndividualId");
			Assert.AreEqual(12, result);
		}

		[Test]
		public void SelectRates_ReturnsNull() {
			ConfigureMultipleRowMock(new List<List<object>>(), new string[0]);
			var result = _mgr.SelectRates("FL", "zip", new[] { 68 }, "A");
			Assert.IsNull(result);
		}
		[Test]
		public void SelectRates_ReturnsColumnsByAge() {
			var data = new List<List<object>> {
				new object[] {"$3,432.33", "$234.54", "$11.02"}.ToList(),
			};
			ConfigureMultipleRowMock(data, null);

			var result = _mgr.SelectRates("CA", "zip", new[] { 25, 35, 55 }, "A").ToList();

			for (var i = 0; i < data[0].Count; i++) {
				Assert.AreEqual(data[0][i], result[i]);
			}
		}
		[Test]
		public void SelectRates_ChecksOptions() {
			var failuresStrings = new[] {
				"d", // lower case
				"DD", // more than one char
				"", // empty string
				"1", // non-letter
			};
			foreach (var failureStr in failuresStrings) {
				Assert.Throws<ArgumentException>(() => _mgr.SelectRates("CA", "97122", new[] { 43 }, failureStr));
			}
		}

		[Test]
		public void SelectLifeInsuranceRates_ReturnsNull() {
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, false))
				.Returns(new CommandResult());
			var result = _mgr.SelectLifeInsuranceRates("coverage", "term", "age", "memberSpouse", "smoker");
			Assert.IsNull(result);
		}
		[Test]
		public void SelectLifeInsuranceRates_ReturnsCollection() {
			var data = new List<List<object>> {
				new object[] {"234.33/year"}.ToList(),
				new object[] {"1222.33/month"}.ToList(),
				new object[] {"0.34/day"}.ToList()
			};
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, false))
				.Returns(new CommandResult(data));
			var result = _mgr.SelectLifeInsuranceRates("coverage", "term", "age", "memberSpouse", "smoker").ToArray();
			Assert.AreEqual(data.Count, result.Length);
			for (var i = 0; i < data.Count; i++) {
				Assert.AreEqual(data[i][0], result[i]);
			}
		}
		[Test]
		public void SelectMdsId_ReturnsNull() {
			ConfigureSingleRowMock();
			var result = _mgr.SelectMdsId("123");
			Assert.IsNull(result);
			Assert.IsNull(_mgr.SelectMdsId(null));
		}
		[Test]
		public void SelectMdsId_ReturnsValue() {
			ConfigureSingleRowMock("432");
			var result = _mgr.SelectMdsId("123");
			Assert.AreEqual("432", result);
		}

		[Test]
		public void SelectImsId_ReturnsNull() {
			ConfigureSingleRowMock();
			var result = _mgr.SelectImsId("123");
			Assert.IsNull(result);
			Assert.IsNull(_mgr.SelectImsId(null));
		}

		[Test]
		public void SelectImsId_ReturnsValue() {
			ConfigureSingleRowMock("432");
			var result = _mgr.SelectImsId("123");
			Assert.AreEqual("432", result);
		}

		[Test]
		public void SelectViewInsReg_ReturnsValues() {
			var data = new List<List<object>> {
				new object[] {"123","EarnerCode","MaritalStatus","IncomeBracket","HousingStatus","SpouseCode","1","LevelCode"}.ToList(),
				new object[] {"124","EarnerCode","MaritalStatus","IncomeBracket","HousingStatus","SpouseCode","1","LevelCode"}.ToList(),
				new object[] {"122","EarnerCode","MaritalStatus","IncomeBracket","HousingStatus","SpouseCode","1","LevelCode"}.ToList(),
				new object[] {"121","EarnerCode","MaritalStatus","IncomeBracket","HousingStatus","SpouseCode","1","LevelCode"}.ToList(),
				new object[] {"120","EarnerCode","MaritalStatus","IncomeBracket","HousingStatus","SpouseCode","1","LevelCode"}.ToList(),
			};
			ConfigureMultipleRowMock(
				data,
				new[] {
					ViewInsReg.INDIVIDUAL_ID,
					ViewInsReg.WAGE_EARNER_CODE,
					ViewInsReg.DTAB_MARITAL_STATUS_CODE,
					ViewInsReg.FAMILY_INCOME_RANGE_CODE,
					ViewInsReg.HOUSING_STATUS_CODE,
					ViewInsReg.SPOUSE_EMPTY_CODE,
					ViewInsReg.DEPENDENCY_CHILD_COUNT,
					ViewInsReg.DTAB_VOCL_LEVEL_CODE
				}
			);			
		}
		[Test]
		public void InsertComplimentaryLife_ReturnsFalse() {
			ConfigureSingleRowMock("-1");
			var cl = new ComplimentaryLifeDb();
			var result = _mgr.InsertComplimentaryLife(cl);
			Assert.IsFalse(result);
		}
		[Test]
		public void InsertComplimentaryLife_ReturnsTrue() {
			ConfigureSingleRowMock("0");
			var cl = new ComplimentaryLifeDb();
			var result = _mgr.InsertComplimentaryLife(cl);
			Assert.IsTrue(result);
		}

		[Test]
		public void SelectBeneficiaries_ReturnsNull() {
			ConfigureSingleRowMock();
			var result = _mgr.SelectBeneficiaries("123");
			Assert.IsNull(result);
			Assert.IsNull(_mgr.SelectBeneficiaries(null));
		}
		[Test]
		public void SelectBeneficiaries_ReturnsValues() {
			var data = new List<List<object>> {
				new object[] {"entityName","firstName","lastName","middleName", "type",
					"e@mail.com","33", "designated_code"}.ToList(),
				new object[] {"entityName","firstName","lastName","middleName", "type",
					"e@mail.com","33", "designated_code"}.ToList(),
				new object[] {"entityName","firstName","lastName","middleName", "type",
					"e@mail.com","33", "designated_code"}.ToList(),
			};
			ConfigureMultipleRowMock(
				data,
				new[] {
					ViewBeneficiary.BENEFICIARY_ENTITY_NAME,
					ViewBeneficiary.BENEFICIARY_FIRST_NAME,
					ViewBeneficiary.BENEFICIARY_LAST_NAME,
					ViewBeneficiary.BENEFICIARY_MIDDLE_NAME,
					ViewBeneficiary.BENEFICIARY_TYPE,
					ViewBeneficiary.BENEFICIARY_EMAIL,
					ViewBeneficiary.BENEFICIARY_DESIGNATED_PTS,
					ViewBeneficiary.BENEFICIARY_DESIGNATED_CODE
				}
			);
			var result = _mgr.SelectBeneficiaries("123").ToList();
			for (var i = 0; i < data.Count; i++) {
				Assert.AreEqual(data[i][0], result[i].EntityName);
				Assert.AreEqual(data[i][1], result[i].FirstName);
				Assert.AreEqual(data[i][2], result[i].LastName);
				Assert.AreEqual(data[i][3], result[i].MiddleName);
				Assert.AreEqual(data[i][4], result[i].BeneficiaryType);
				Assert.AreEqual(data[i][5], result[i].EmailAddress);
				Assert.AreEqual(data[i][6], result[i].DesignatedPts.ToString());
				Assert.AreEqual(data[i][7], result[i].DesignatedCd);
			}
		}

		[Test]
		public void SelectFamilyMembersList_ReturnsNull() {
			var data = new List<List<object>> {
				new object[] {}.ToList()
			};
			ConfigureMultipleRowMock(data, new[] { "IDPIPEDCODE" });
			var result = _mgr.SelectFamilyMembersList("123");
			Assert.AreEqual(0, result.Count());
			Assert.IsNull(_mgr.SelectFamilyMembersList(null));
		}
		[Test]
		public void SelectFamilyMembersList_ReturnsValues() {
			var data = new List<List<object>> {
				new object[] {"family1"}.ToList(),
				new object[] {"family1"}.ToList(),
				new object[] {"family1"}.ToList()
			};
			ConfigureMultipleRowMock(data, new[] { "IDPIPEDCODE" });
			var result = _mgr.SelectFamilyMembersList("123").ToArray();
			for (var i = 0; i < data.Count; i++) {
				Assert.AreEqual(data[i][0], result[i]);
			}
		}

		[Test]
		public void SelectZipCodeCount_ReturnsCount() {
			ConfigureSingleRowMock("123");
			var result = _mgr.SelectZipCodeCount("PA");
			Assert.AreEqual(123, result);
		}

		[Test]
		public void GetLastUpdateDate_ResultsValue() {
			ConfigureSingleRowMock("201");
			var result = _mgr.SelectZipCodeCount("PA");
			Assert.AreEqual(201, result);
		}

		[Test]
		public void SelectFamilyMemberInfo_ReturnsValues() {
			var data = new List<List<object>> {
				new object[] {"123","firstName","lastName","email1@me.com"}.ToList()
			};
			ConfigureMultipleRowMock(
				data,
				new[] {
					ViewIndividual.INDIVIDUAL_ID,
					ViewIndividual.INDIVIDUAL_FIRST_NAME,
					ViewIndividual.INDIVIDUAL_LAST_NAME,
					ViewIndividual.INDIVIDUAL_EMAIL_ADDRESS
				}
			);
			var result = _mgr.SelectFamilyMemberInfo("123");
			Assert.AreEqual(data[0][0], result.IndividualId);
			Assert.AreEqual(data[0][1], result.FirstName);
			Assert.AreEqual(data[0][2], result.LastName);
			Assert.AreEqual(data[0][3], result.Email);
		}
		[Test]
		public void SelectFamilyMemberInfo_ReturnsNull() {
			var data = new List<List<object>>();
			ConfigureMultipleRowMock(
				data,
				new[] {
					ViewIndividual.INDIVIDUAL_ID,
					ViewIndividual.INDIVIDUAL_FIRST_NAME,
					ViewIndividual.INDIVIDUAL_LAST_NAME,
					ViewIndividual.INDIVIDUAL_EMAIL_ADDRESS
				}
			);
			var result = _mgr.SelectFamilyMemberInfo("123");
			Assert.IsNull(result);
			Assert.IsNull(_mgr.SelectFamilyMemberInfo(null));
		}

		[Test]
		public void SelectLastUpdateDate_ReturnsNull() {
			ConfigureSingleRowMock();
			Assert.IsNull(_mgr.SelectLastUpdateDate("123"));
			Assert.IsNull(_mgr.SelectLastUpdateDate(null));
		}
		[Test]
		public void SelectLastUpdateDate_ReturnsValue() {
			ConfigureSingleRowMock("19880101");
			Assert.AreEqual("19880101", _mgr.SelectLastUpdateDate("123"));
		}

		[Test]
		public void SelectUnredeemedRewardsTotal_ReturnsZeroOnError() {
			var data = new List<List<object>>();
			ConfigureMultipleRowMock(data, new string[0]);
			Assert.AreEqual(0, _mgr.SelectUnredeemedRewardsTotal("123"));
			Assert.AreEqual(0, _mgr.SelectUnredeemedRewardsTotal(null));
		}
		[Test]
		public void UnredeemedRewardsTotal_ReturnsInteger() {
			var data = new List<List<object>> {
				new object[] {"4488"}.ToList()
			};
			ConfigureMultipleRowMock(data, new string[0]);
			Assert.AreEqual("4488", _mgr.SelectUnredeemedRewardsTotal("123").ToString());
		}

		[Test]
		public void GetUnredeemedAwards_ReturnsValues() {
			var data = new List<List<object>> {
				new object[] {"123","RewardsName","2015/01/01","Description","123"}.ToList(),
				new object[] {"122","RewardsName","2015/02/01","Description","223"}.ToList(),
				new object[] {"125","RewardsName","2015/03/01","Description","323"}.ToList(),
				new object[] {"17","RewardsName","2015/04/01","Description","423"}.ToList(),
			};
			var resultFields = new[] {
				ViewUnredeemedRewards.INDV_ID,
				ViewUnredeemedRewards.REWARDS_NM,
				ViewUnredeemedRewards.DATE_AWARDED,
				ViewUnredeemedRewards.USER_REWARDS_DESC,
				ViewUnredeemedRewards.AWARDED_VAL
			};
			ConfigureMultipleRowMock(data, resultFields);
			var result = _mgr.SelectUnredeemedRewards("123").ToArray();
			for (var i = 0; i < data.Count; i++) {
				Assert.AreEqual(data[i][0], result[i].IndvId.ToString());
				Assert.AreEqual(data[i][1], result[i].RewardsNm);
				Assert.AreEqual(data[i][2], result[i].DateAwarded.ToString(ViewUnredeemedRewards.DATE_AWARDED_FORMAT));
				Assert.AreEqual(data[i][3], result[i].UserRewardsDesc);
				Assert.AreEqual(data[i][4], result[i].AwardedVal.ToString());
			}
		}

		[Test]
		public void SelectLpgtRates_ReturnsValue() {
			ConfigureSingleRowMock("4488");
			Assert.AreEqual("4488", _mgr.SelectLpgtRates("123", "11"));
		}
		[Test]
		public void SelectLpgtRates_ReturnsNull() {
			ConfigureSingleRowMock();
			Assert.IsNull(_mgr.SelectLpgtRates("123", "11"));
			Assert.IsNull(_mgr.SelectLpgtRates(null, "11"));
			Assert.IsNull(_mgr.SelectLpgtRates("123", null));
		}

		[Test]
		public void IsSpouseDomesticPartnerAssociated_ReturnsTrue() {
			ConfigureSingleRowMock("1");
			Assert.IsTrue(_mgr.IsSpouseDomesticPartnerAssociated("123"));
		}
		[Test]
		public void IsSpouseDomesticPartnerAssociated_ReturnsFalse() {
			ConfigureSingleRowMock("0");
			Assert.IsFalse(_mgr.IsSpouseDomesticPartnerAssociated("123"));
			Assert.IsFalse(_mgr.IsSpouseDomesticPartnerAssociated(null));
		}

		[Test]
		public void EnrollInSweepstakes_ReturnsTrue() {
			ConfigureSingleRowMock("1");
			Assert.IsTrue(_mgr.EnrollInSweepstakes("123", "ssd"));
		}
		[Test]
		public void EnrollInSweepstakes_ReturnsFalse() {
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, true))
				.Returns(new CommandResult { ErrorMessage = "nope" });
			Assert.IsFalse(_mgr.EnrollInSweepstakes("123", "ssd"));
			Assert.IsFalse(_mgr.EnrollInSweepstakes("123", null));
			Assert.IsFalse(_mgr.EnrollInSweepstakes(null, "ssd"));
		}

		[Test]
		public void InsertFamilyMember_ReturnsTrue() {
			ConfigureSingleRowMock("1");
			var result = _mgr.InsertFamilyMember("123", "firstName", "lastName", "e@mail.com", "20080101", "Brother");
			Assert.IsTrue(result);
		}
		[Test]
		public void InsertFamilyMember_ReturnsFalse() {
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, true))
				.Returns(new CommandResult { ErrorMessage = "nope" });
			var result = _mgr.InsertFamilyMember("123", "firstName", "lastName", "e@mail.com", "20080101", "Brother");
			Assert.IsFalse(result);
			Assert.IsFalse(_mgr.InsertFamilyMember(null, "firstName", "lastName", "e@mail.com", "20080101", "Brother"));
		}

		[Test]
		public void DeleteFamilyMember_ReturnsTrue() {
			ConfigureSingleRowMock("1");
			var result = _mgr.DeleteFamilyMember("123", "firstName");
			Assert.IsTrue(result);
		}
		[Test]
		public void DeleteFamilyMember_ReturnsFalse() {
			_currentCommandMock.Setup(x => x.Execute(It.IsAny<string>(), null, true))
				.Returns(new CommandResult { ErrorMessage = "nope" });
			Assert.IsFalse(_mgr.DeleteFamilyMember("123", "ssd"));
			Assert.IsFalse(_mgr.DeleteFamilyMember("123", null));
			Assert.IsFalse(_mgr.DeleteFamilyMember(null, "ssd"));
		}
	}
}
