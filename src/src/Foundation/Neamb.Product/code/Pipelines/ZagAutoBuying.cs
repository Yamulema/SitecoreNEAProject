using System;
using System.Collections.Specialized;
using Neambc.Neamb.Foundation.Config.Utility;


namespace Neambc.Neamb.Foundation.Product.Pipelines {
	public class ZagAutoBuying : PartnerBase {
		public void Process(ProductPipelineArgs args) {
			if (programCodes.Contains(args.ProductCode)) {
                //ZNAT000026077
                var referrerId = args.ReferrerId;
				var initialurl = args.WebserviceUrl;
				var parameters =
					$"givenName={args.GivenName}&familyName={args.FamilyName}&emailAddress={args.EmailAddress}" +
					$"&addressLine1={args.AddressLine1}&city={args.City}&state={args.State}&postalCode={args.PostalCode}" +
					$"&telephone={args.Telephone}&memberId={args.MdsId}&referrer_id={referrerId}";
				try {
                    Sitecore.Diagnostics.Log.Info($"{initialurl}{parameters}",this);
                    NameValueCollection nameValueCollection = new NameValueCollection(1);
                    nameValueCollection.Add("x-truecar-app-token", args.HeaderTrueCarToken);
                    var myRequest =
						new WebRequestHelper(initialurl, "POST", parameters,nameValueCollection);
					var actionPrimary = $"http://{myRequest.GetResponse()}";
					SetAction(actionPrimary, args);
				} catch (Exception ex) {
                    Sitecore.Diagnostics.Log.Error($"Calling {initialurl}{parameters}", this);
                    Sitecore.Diagnostics.Log.Error(this + "Error calling the url of TrueCar " + DateTime.Now, ex, this);
				}
			}
		}
	}
}