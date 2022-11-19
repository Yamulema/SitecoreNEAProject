using System;
using Neambc.Neamb.Feature.Language.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Language.Models {
	public class SpanishLanguageModalDTO : IRenderingModel {
		private const int FIRST_CULTURE = 0;
		private const int LANGUAGE_POSITION = 0;
		private const char Separator = '-';
		private const string CookieName = "neamb-language";

		public Rendering Rendering {
			get; private set;
		}
		public Item Item {
			get; private set;
		}
		public string Content {
			get; private set;
		}
		public Item SiteSettings {
			get; private set;
		}
		public bool DisplayModal {
			get; private set;
		}

		private readonly ICookieManager _cookieManager;

		public string[] UserLanguages {
			get; private set;
		}

		public SpanishLanguageModalDTO(string[] userLanguages, ICookieManager cookieManager) {
			_cookieManager = cookieManager;
			UserLanguages = userLanguages;
		}

		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			SiteSettings = GetSiteSettings();
			Content = SiteSettings.Fields[Templates.SpanishLanguageModal.Fields.Content].Value;
			DisplayModal = GetDisplayModal();
		}
		private Item GetSiteSettings() {
			Item siteSettings = null;
			try {
				var datasourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
				siteSettings = Sitecore.Context.Database.GetItem(datasourceId);
			} catch (Exception ex) {
				Log.Error("Get SiteSettings item in NEAMB Spanish Language Modal", ex, this);
			}
			return siteSettings;
		}

		private bool GetDisplayModal() {
			var displayModal = false;

			try {
				if (!Sitecore.Context.PageMode.IsExperienceEditor && IsLanguageSpanish(UserLanguages) && !CookieExists()) {
					CreateCookie();
					displayModal = true;
				}
			} catch (Exception ex) {
				Log.Debug("Get Display modal in NEAMB Language Modal, setting display as false", this);
				Log.Debug(ex.StackTrace);
				displayModal = false;
			}

			return displayModal;
		}

		private bool CookieExists() {
			var cookie = _cookieManager.GetCookie(CookieName);
			if (cookie != null) {
				return true;
			}
			return false;
		}

		private void CreateCookie() {
			_cookieManager.CreateCookie(CookieName);
		}

		private bool IsLanguageSpanish(string[] userLanguages) {
			Assert.ArgumentNotNull(userLanguages, "userLanguages");
			Assert.ArgumentCondition(userLanguages.Length > 0, "userLanguages", "No User languages available");

			var culture = userLanguages[FIRST_CULTURE];
			var language = culture.Split(Separator)[LANGUAGE_POSITION];
			return (language == ConstantsNeamb.SpanishLanguage);
		}
	}
}