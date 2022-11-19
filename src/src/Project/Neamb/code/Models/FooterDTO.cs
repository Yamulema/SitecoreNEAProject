using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Project.Web.Models {
	public class FooterDTO : IRenderingModel {
		public Rendering Rendering {
			get; set;
		}
		public Item Item {
			get; set;
		}
		public Item SiteSettings {
			get; private set;
		}
		public string Version {
			get; private set;
		}
		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			SiteSettings = GetSiteSettings();
			Version = Configuration.SiteReleaseVersion;
		}
		private Item GetSiteSettings() {
			var datasourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
			return Sitecore.Context.Database.GetItem(datasourceId);
		}
	}
}