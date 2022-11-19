

using System;

namespace Neambc.Neamb.Foundation.Product.Model
{
	[Serializable]
	public class SsoModel
	{
		public string ProductCode { get; set; }
		public int ComponentType { get; set; }
		public AccountUserBase AccountUser { get; set; }
	}
}