
using System;

namespace Neambc.Neamb.Foundation.Product.Model
{
	[Serializable]
	public class ComingSoonModel
	{
		public string ReminderId { get; set; }
        public string ContextItemId { get; set; }
        public string EligibilityItemId { get; set; }
		public AccountUserBase AccountUser { get; set; }

		public string UrlReturn { get; set; }
	}
}