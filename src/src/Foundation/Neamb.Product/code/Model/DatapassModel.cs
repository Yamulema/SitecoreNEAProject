
using System;

namespace Neambc.Neamb.Foundation.Product.Model
{
	[Serializable]
	public class DatapassModel
	{
		public string ProductCode { get; set; }
		public int ComponentType { get; set; }
		public string PrimarySecondaryActionType { get; set; }
		public AccountUserBase AccountUser { get; set; }
	}
}