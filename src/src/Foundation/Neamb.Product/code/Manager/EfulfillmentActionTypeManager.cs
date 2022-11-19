using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(IEfulfillmentActionTypeManager))]
	public class EfulfillmentActionTypeManager : IEfulfillmentActionTypeManager {

		#region Fields
		private readonly IEligibilityManager _eligibilityManager;
		private readonly IProductManager _productmanager;
        private readonly IPdfManager _pdfmanager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;

        private readonly string _cacheKeyGroup = "Efulfillment";
        #endregion

        #region Constructor
        public EfulfillmentActionTypeManager(
			IEligibilityManager eligibilityManager,
			IProductManager productmanager,
            IPdfManager pdfmanager, IGlobalConfigurationManager globalConfigurationManager) {
			_eligibilityManager = eligibilityManager;
			_productmanager = productmanager;
            _pdfmanager = pdfmanager;
            _globalConfigurationManager = globalConfigurationManager;
        }
		#endregion

		#region Protected Methods
		/// <summary>
		/// Get the age between the current date and a specific date (birth date of the user)
		/// </summary>
		/// <param name="dateOfBirth">User date of birth</param>
		/// <returns></returns>
		protected virtual int GetAgeInYears(DateTime dateOfBirth) {
			var tsAge = CurrentLocalNow().Subtract(dateOfBirth);
			return new DateTime(tsAge.Ticks).Year - 1;
		}
		protected virtual DateTime CurrentLocalNow() {
			return DateTime.Now;
		}
		#endregion

		#region Public Methods
		public EfulfillmentResult GetPdfFile(EfulfillmentModel efulfillmentModel) {
			var efulfillmentResult = new EfulfillmentResult();
			var custom1 = string.Empty;
			var custom2 = string.Empty;
			var resultEligibility = efulfillmentModel.CheckEligibility
				? _eligibilityManager.IsMemberEligible(efulfillmentModel.AccountUser.Mdsid, efulfillmentModel.ProductCode)
				: EligibilityResultEnum.NotEligible;
			//Check the result of the webservice call
			if ((efulfillmentModel.CheckEligibility && resultEligibility == EligibilityResultEnum.Eligible) ||
				!efulfillmentModel.CheckEligibility) {
				//Set the uniqueName to be stored in redis
				var uniqueName =
					string.Format("{0}:{1}-{2}", _cacheKeyGroup, efulfillmentModel.AccountUser.Mdsid,
						efulfillmentModel.MaterialId);
				
				//Get the pdf file from a webservice
				var pdfFile = _pdfmanager.VerifyExistencePdfFile(uniqueName, _globalConfigurationManager.UrlEfulfillmentS3,true);
				if (pdfFile == null) {
					//Get the age if the material id = 625
					if (efulfillmentModel.MaterialId.Equals("625") && efulfillmentModel.AccountUser.Profile.DateOfBirth.Length == 8) {
						var dob = new DateTime(
							Convert.ToInt32(efulfillmentModel.AccountUser.Profile.DateOfBirth.Substring(4, 4)),
							Convert.ToInt32(efulfillmentModel.AccountUser.Profile.DateOfBirth.Substring(0, 2)),
							Convert.ToInt32(efulfillmentModel.AccountUser.Profile.DateOfBirth.Substring(2, 2)));
						var age = GetAgeInYears(dob);
						custom1 = age.ToString();
					}
					//Material id == 022 set the sea name
					else if (efulfillmentModel.MaterialId.Equals("022")) {
						custom1 = efulfillmentModel.AccountUser.Profile.SeaName;
					} else if (efulfillmentModel.MaterialId.Equals("883") || efulfillmentModel.MaterialId.Equals("390")) {
						var now = CurrentLocalNow();
						var month = now.Month;
						var year = now.Year;
						var nextYear = now.Year + 1;
						custom2 = $"August 31, {year}";
						custom1 = efulfillmentModel.AccountUser.Profile.SeaName;
						if (month > 8) {
							custom2 = $"August 31, {nextYear}";
						}
					}
                    int.TryParse(efulfillmentModel.AccountUser.Mdsid, out int mdsidInt);

                    var pdfRequest = new PdfRequest
                    {
                        ProductIemId = efulfillmentModel.MaterialId,
                        Email = efulfillmentModel.AccountUser.Username,
                        PdTransDate = DateTime.Now.ToString("MM/dd/yyyy"),
                        PdFirstName = efulfillmentModel.AccountUser.Profile.FirstName,
                        PdLastName = efulfillmentModel.AccountUser.Profile.LastName,
                        PdDob = efulfillmentModel.AccountUser.Profile.DateOfBirth,
                        PdMdsid = mdsidInt,
                        PdAddress = efulfillmentModel.AccountUser.Profile.StreetAddress,
                        PdCity = efulfillmentModel.AccountUser.Profile.City,
                        PdState = efulfillmentModel.AccountUser.Profile.StateCode,
                        PdZip = efulfillmentModel.AccountUser.Profile.ZipCode,
                        PdMemberType = efulfillmentModel.AccountUser.Profile.NeaMembershipType,
                        Custom1 = custom1,
                        Custom2 = custom2
                    };

                    pdfFile = _pdfmanager.GetPdfFile(efulfillmentModel.MaterialId, uniqueName, pdfRequest,
                        _globalConfigurationManager.UrlEfulfillmentS3,custom1, custom2);
				}

				//Insert into order_fulfill
				_productmanager.ExecuteMdsLoggingProcessMaterial(efulfillmentModel.MaterialId);

				if (pdfFile != null && pdfFile.Length > 0) {
                    var fileName= _pdfmanager.GetPdfUrl(uniqueName);
                    if (!string.IsNullOrEmpty(fileName)) {
                        efulfillmentResult.PdfSucessUrl = fileName;
                    } else {
                        efulfillmentResult.ResultUrl = ResultUrlEnum.NoUrl;
                    }
                } else {
					efulfillmentResult.ResultUrl = ResultUrlEnum.NoUrl;
				}
			} else {
				efulfillmentResult.ResultUrl = ResultUrlEnum.UnForbidden;
			}


			return efulfillmentResult;
		}
		#endregion
	}
}