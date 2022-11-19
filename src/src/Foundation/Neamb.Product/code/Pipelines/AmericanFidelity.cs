using System;
using System.Xml;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.Product.Pipelines {
	public class AmericanFidelity : PartnerBase {
		private readonly IAccountServiceProxy _serviceManager;

		/// <summary>
		/// This method is required because in processor doesn't work DI
		/// </summary>
		public AmericanFidelity() {
			_serviceManager = (IAccountServiceProxy)ServiceLocator.ServiceProvider.GetService(typeof(IAccountServiceProxy));
		}

		public void Process(ProductPipelineArgs args) {
			if (programCodes.Contains(args.ProductCode)) {
				var serviceResponse = _serviceManager.ExecuteEnrollmentQuery(args.MdsId);

				var xmlDocument = new XmlDocument();
				var uniqueId = string.Empty;
				xmlDocument.LoadXml(serviceResponse);
				var applicant = xmlDocument.GetElementsByTagName("Applicant");
				XmlNode firstapplicant = null;

				foreach (XmlNode itemapplicant in applicant) {
					if (itemapplicant["Relationship"]?.InnerText.Trim().ToLower() == "employee") {
						firstapplicant = itemapplicant;
						break;
					}
				}

				if (firstapplicant != null) {
					if (firstapplicant.Attributes != null) {
						uniqueId = firstapplicant.Attributes["UniqueID"].Value;
					}
				} else {
					var response = _serviceManager.ExecuteUpdateEnrollment(args.AddressLine1, args.City, args.State, args.ZipCode, args.EmailAddress,
						args.GivenName, args.FamilyName, args.DateBirth, args.MdsId);
					xmlDocument.LoadXml(response);
					applicant = xmlDocument.GetElementsByTagName("Applicant");
					if (applicant.Count > 0) {
						var firstApplicant = applicant[0];
						if (firstApplicant.Attributes != null) {
							uniqueId = firstApplicant.Attributes["UniqueID"].Value;
						}
					}
				}

				if (!string.IsNullOrEmpty(uniqueId)) {
					try {
						var responseLogin = _serviceManager.ExecuteEnrollmentGetLogin(new Guid(uniqueId));
						Log.Info("American afidelity unique id " + uniqueId, this);

						args.Result = string.Format("https://afenroll.benselect.com/Enroll/Login.aspx?login_guid={0}",
							responseLogin);
					} catch {
						args.Result = string.Empty;
					}
				}
			}
		}
	}
}