
using System;

namespace Neambc.Neamb.Foundation.Product.Model
{
	[Serializable]
	public class EfulfillmentModel
	{
		public string ProductCode { get; set; }
		public string MaterialId { get; set; }
		public bool CheckEligibility { get; set; }
		public AccountUserBase AccountUser { get; set; }
		public bool CheckLogin { get; set; }
	}
}