using Sitecore.Pipelines;


namespace Neambc.Seiumb.Feature.Language.Infrastructure.Pipelines {
	/// <summary>
	/// Information of the Current and New languages
	/// </summary>
	public class ChangeLanguagePipelineArgs : PipelineArgs {
		public ChangeLanguagePipelineArgs(string currentLanguage, string newLanguage) {
			CurrentLanguage = currentLanguage;
			NewLanguage = newLanguage;
		}

		public string CurrentLanguage {
			get => CustomData["CurrentLanguage"]?.ToString();
			set => CustomData["CurrentLanguage"] = value;
		}

		public string NewLanguage {
			get => CustomData["NewLanguage"]?.ToString();
			set => CustomData["NewLanguage"] = value;
		}
	}
}