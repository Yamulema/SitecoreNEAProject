using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Account.Models {
	public class BackCTA {
		public string FriendlyUrl;
		public string Text;

        public BackCTA() {
        }

  //      public BackCTA(Item item) {
		//	var i = item ?? throw new ArgumentNullException(nameof(item));
		//	FriendlyUrl = ((LinkField)i.Fields[Templates.EditUpdateInformation.Fields.Back])?.GetFriendlyUrl();
		//	Text = ((LinkField)i.Fields[Templates.EditUpdateInformation.Fields.Back])?.Text;
		//}
	}
}