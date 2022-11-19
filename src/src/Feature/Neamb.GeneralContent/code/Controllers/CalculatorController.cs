using System;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Enums;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers {
	public class CalculatorController : BaseController {

		#region Fields
		private readonly IOracleDatabase _oracleManager;
		private readonly IRateQuotationManager _rateQuotationManager;
		#endregion

		#region Constructors
		public CalculatorController(IOracleDatabase oracleManager, IRateQuotationManager rateQuotationManager) {
			_oracleManager = oracleManager;
			_rateQuotationManager = rateQuotationManager;
		}
		#endregion

		#region Private Methods
		private bool ValidationDataInsurance(string coverage, string term, string age, string memberspouse, string smoker) {

			var ageValidation = 
				(age.Length == 2) && 
				IsDigitsOnly(age) && 
				(Convert.ToInt32(age) >= 19) && 
				(Convert.ToInt32(age) <= 70);

			var coverageValidation =
				((coverage.Length == 5) || (coverage.Length == 6) || (coverage.Length == 7)) &&
				IsDigitsOnly(coverage) &&
				(Convert.ToInt32(coverage) >= 25000) &&
				(Convert.ToInt32(coverage) <= 1000000);

			var termValidation =
				term.Equals("10") ||
				term.Equals("15") ||
				term.Equals("20");

			var memberSpouseValidation =
				memberspouse.Equals("1") ||
				memberspouse.Equals("2");

			var smokerValidation =
				smoker.Equals("1") ||
				smoker.Equals("2") ||
				smoker.Equals("3") ||
				smoker.Equals("4") ||
				smoker.Equals("5");

			return ageValidation && 
				coverageValidation && 
				termValidation && 
				memberSpouseValidation && 
				smokerValidation;
		}
		private bool ValidationDataInsurancePremium(string coverage, string age) {
			var ageValidation =
				(age.Length == 2) &&
				IsDigitsOnly(age) &&
				(Convert.ToInt32(age) >= 25) &&
				(Convert.ToInt32(age) <= 69);
			var coverageValidation =
				((coverage.Length == 5) || (coverage.Length == 6)) &&
				IsDigitsOnly(coverage) &&
				(Convert.ToInt32(coverage) >= 25000) &&
				(Convert.ToInt32(coverage) <= 500000);
			return ageValidation && coverageValidation;
		}
		private bool IsDigitsOnly(string str) {
			foreach (var c in str) {
				if (c < '0' || c > '9') {
					return false;
				}
			}

			return true;
		}
		#endregion

		#region Public Methods
		public ActionResult GetLifeInsuranceCalculator(string coverage, string term, string age, string memberspouse, string smoker) {
			var resultValidation = ValidationDataInsurance(coverage, term, age, memberspouse, smoker);
			if (resultValidation) {
				var result = _oracleManager.SelectLifeInsuranceRates(coverage, term, age, memberspouse, smoker)
					?? new string[0];
				return Json(new {
					results = result
				}, JsonRequestBehavior.AllowGet);
			} else {
				return Json(new {
					results = "error"
				}, JsonRequestBehavior.AllowGet);
			}
		}
		public ActionResult GetLifeInsurancePlanCalculator(string coverage, string age) {
			var resultValidation = ValidationDataInsurancePremium(coverage, age);
			if (resultValidation) {
				var result = _oracleManager.SelectLpgtRates(coverage, age);
				return Json(new {
					results = result
				}, JsonRequestBehavior.AllowGet);
			} else {
				return Json(new {
					results = "error"
				}, JsonRequestBehavior.AllowGet);
			}
		}
		public ActionResult GetQuote(string state, string zip, string age) {
			var quoteStatus = _rateQuotationManager.Validate(state, zip);

			switch (quoteStatus) {
				case QuoteStatus.NoData:
					return Json(new {
						Status = (int)QuoteStatus.NoData,
						Message = Configuration.NoDataMessage
					}, JsonRequestBehavior.AllowGet);
				case QuoteStatus.InvalidAge:
					return Json(new {
						Status = (int)QuoteStatus.InvalidAge,
						Message = Configuration.InvalidAgeMessage
					}, JsonRequestBehavior.AllowGet);
				case QuoteStatus.Ok:
                    var planQuotes = _rateQuotationManager.GetPlanQuotes(state, zip, age);
					return planQuotes.Any()
						? Json(new {
							Status = (int)QuoteStatus.Ok,
							Plans = planQuotes
						})
						: Json(new {
							Status = (int)QuoteStatus.NoData,
							Message = Configuration.NoDataMessage
						}, JsonRequestBehavior.AllowGet);
				default:
					return Json(new {
						Status = (int)QuoteStatus.NoData,
						Message = Configuration.NoDataMessage
					}, JsonRequestBehavior.AllowGet);
			}
		}
		public ActionResult ValidateState(string state) {
			var stateStatus = _rateQuotationManager.Validate(state);
			switch (stateStatus) {
				case StateStatus.Invalid:
					return Json(new {
						Status = (int)StateStatus.Invalid,
						Message = Configuration.InvalidStateMessage
					}, JsonRequestBehavior.AllowGet);
				default:
					return Json(new {
						Status = (int)stateStatus
					});
			}
		}
		#endregion
	}
}