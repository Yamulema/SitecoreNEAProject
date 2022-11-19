using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Forms.Models {
	public class PasswordDisavowModel : IRenderingModel {

		#region Properties

		public Item Item { get; set; }
		public bool IsCanceled { get; set; }

		#endregion

		#region Constructors

		public PasswordDisavowModel() { }

		public PasswordDisavowModel(Rendering rendering) {
			if (rendering != null) {
				Initialize(rendering);
			}
		}
		#endregion

		#region Public Methods
		public void Initialize(Rendering rendering) {
			IsCanceled = false;
			Item = rendering.Item;
		}
		#endregion
	}
}