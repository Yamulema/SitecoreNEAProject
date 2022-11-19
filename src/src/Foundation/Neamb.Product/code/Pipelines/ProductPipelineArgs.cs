using Sitecore.Pipelines;

namespace Neambc.Neamb.Foundation.Product.Pipelines {
	public class ProductPipelineArgs : PipelineArgs {
		public string ProductCode {
			get => CustomData["ProductCode"]?.ToString();
			set => CustomData["ProductCode"] = value;
		}

		public string MdsId {
			get => CustomData["MdsId"]?.ToString();
			set => CustomData["MdsId"] = value;
		}

		public string Result {
			get => CustomData["Result"]?.ToString();
			set => CustomData["Result"] = value;
		}
		public string GivenName {
			get => CustomData["GivenName"]?.ToString();
			set => CustomData["GivenName"] = value;
		}
		public string FamilyName {
			get => CustomData["FamilyName"]?.ToString();
			set => CustomData["FamilyName"] = value;
		}
		public string EmailAddress {
			get => CustomData["EmailAddress"]?.ToString();
			set => CustomData["EmailAddress"] = value;
		}
		public string AddressLine1 {
			get => CustomData["AddressLine1"]?.ToString();
			set => CustomData["AddressLine1"] = value;
		}
		public string City {
			get => CustomData["City"]?.ToString();
			set => CustomData["City"] = value;
		}
		public string State {
			get => CustomData["State"]?.ToString();
			set => CustomData["State"] = value;
		}
		public string PostalCode {
			get => CustomData["PostalCode"]?.ToString();
			set => CustomData["PostalCode"] = value;
		}
		public string Telephone {
			get => CustomData["Telephone"]?.ToString();
			set => CustomData["Telephone"] = value;
		}
		public string SecondaryUrl {
			get => CustomData["SecondaryUrl"]?.ToString();
			set => CustomData["SecondaryUrl"] = value;
		}

		public string ZipCode {
			get => CustomData["ZipCode"]?.ToString();
			set => CustomData["ZipCode"] = value;
		}
		public string DateBirth {
			get => CustomData["DateBirth"]?.ToString();
			set => CustomData["DateBirth"] = value;
		}
		public string WebserviceUrl {
			get => CustomData["WebserviceUrl"]?.ToString();
			set => CustomData["WebserviceUrl"] = value;
		}
        public string ReferrerId
        {
            get => CustomData["ReferrerId"]?.ToString();
            set => CustomData["ReferrerId"] = value;
        }
        public string HeaderTrueCarToken
        {
            get => CustomData["HeaderTrueCarToken"]?.ToString();
            set => CustomData["HeaderTrueCarToken"] = value;
        }
        public string ReturnUrl {
			get => CustomData["ReturnUrl"]?.ToString();
			set => CustomData["ReturnUrl"] = value;
		}
	}
}