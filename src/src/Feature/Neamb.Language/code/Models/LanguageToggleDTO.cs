using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Language.Models {
	public class LanguageToggleDTO : IRenderingModel {

		#region Fields
		private const string SELECTED_BUTTON_CLASS = "btn btn-toggle-accent";
		private const string NO_SELECTED_BUTTON_CLASS = "btn btn-white";
		private const string TOP_GROUP_CLASS = "btn-group";
		#endregion

		#region Properties
		public Rendering Rendering { get; private set; }
		public Item Item { get; private set; }
		public string EnglishLink { get; private set; }
		public string EnglishClass { get; private set; }
		public string EnglishName { get; private set; }
		public string SpanishLink { get; private set; }
		public string SpanishClass { get; private set; }
		public string SpanishName { get; private set; }
		public string GroupTopClass { get; private set; }
		public string SelectedLanguage { get; internal set; }
		public bool RedirectEnglish { get; internal set; }
		public bool RedirectSpanish { get; internal set; }
		#endregion

		#region Public Methods
		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			SetLinks();
			SetLanguageNames();
			SetGroupClass();
			SetLanguageClasses();
		}
		#endregion

		#region Private Methods

		private void SetLanguageNames() {
			EnglishName = GetLanguageName(Items.English);
			SpanishName = GetLanguageName(Items.Spanish);
		}

		private string GetLanguageName(ID language) {
			var languageItem = Sitecore.Context.Database.GetItem(language);
			return languageItem.Fields[Templates.CategoryItem.Fields.Value].Value;
		}

		private void SetGroupClass() {
			GroupTopClass = TOP_GROUP_CLASS;
		}

		private void SetLinks() {
			EnglishLink = GetUrl(Templates.LanguageToggle.Fields.English);
			SpanishLink = GetUrl(Templates.LanguageToggle.Fields.Spanish);
		}

		private void SetLanguageClasses() {
			var activeLanguagefield = Item.Fields[Templates.LanguageToggle.Fields.Default].Value;
			var defaultID = Items.English;
			ID.TryParse(activeLanguagefield, out defaultID);
			if (defaultID == Items.Spanish) {
				SpanishClass = SELECTED_BUTTON_CLASS;
				EnglishClass = NO_SELECTED_BUTTON_CLASS;
			} else {
				SpanishClass = NO_SELECTED_BUTTON_CLASS;
				EnglishClass = SELECTED_BUTTON_CLASS;
			}
		}

		private string GetUrl(ID language) {
			string url = null;
			LinkField languageField = Item.Fields[language];
			if (languageField?.TargetItem != null) {
				url = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(languageField.TargetItem.ID));
			}
			return url;
		}
		#endregion
	}
}