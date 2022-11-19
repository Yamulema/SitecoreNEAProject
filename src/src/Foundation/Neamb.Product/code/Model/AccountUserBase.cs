using System;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Product.Model
{
	[Serializable]
	public class AccountUserBase
	{
		public string Username { get; set; }
		public string Mdsid { get; set; }
		public Profile Profile { get; set; }
	}
}