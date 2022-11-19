using System.Collections.Generic;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Controllers {
	public class ThreeContentItemsController : BaseController {
		private readonly ICacheManager _cacheManager;
		private readonly ISessionAuthenticationManager _sessionManager;
		private AccountMembership accountMembership;
		private const string ITEM_ID_SEPARATOR = "|";
		private const char CHAR_ITEM_ID_SEPARATOR = '|';
		private const int NUMBER_OF_CONTENT_ITEMS = 3;

		public ThreeContentItemsController(ICacheManager cacheManager, ISessionAuthenticationManager sessionManager) {
			_cacheManager = cacheManager;
			_sessionManager = sessionManager;
			SetAccountMembership();
		}
		public ActionResult ThreeContentItems() {
			return View("/Views/Neamb.Cards/ThreeContentItems.cshtml", CreateModel());
		}

		private ThreeContentItemsDTO CreateModel() {
			var threeContentItemsDTO = new ThreeContentItemsDTO();
			threeContentItemsDTO.Initialize(RenderingContext.Current.Rendering);
			if (!IsAuthenticated()) {
				threeContentItemsDTO.NoDisplay = true;
				return threeContentItemsDTO;
			}
			return RetrieveContentItems(threeContentItemsDTO);
		}

		private ThreeContentItemsDTO RetrieveContentItems(ThreeContentItemsDTO threeContentItemsDTO) {
			var value = RetrieveCache(GetID());
			if (value == null || value.Split(CHAR_ITEM_ID_SEPARATOR).Length != NUMBER_OF_CONTENT_ITEMS) {
				threeContentItemsDTO.NoDisplay = true;
			} else {
				threeContentItemsDTO.NoDisplay = false;
				threeContentItemsDTO.FillItems(value);
			}
			return threeContentItemsDTO;
		}

		private string GetID() {
			return ConstantsNeamb.ThreeColContentItemsTrackerKeyPrefix + accountMembership.Mdsid;
		}
        [System.ObsoleteAttribute]
		public void Track() {
			if (!IsAuthenticated()) {
				return;
			}
			AddPageIdToCache();
		}

		private void AddPageIdToCache() {
			var key = GetID();
			if (ExistInCache(key)) {
				AddItemID(key);
			} else {
				CreateKey(key);
			}
		}

		private void CreateKey(string key) {
			var item = PageContext.Current.Item;
			_cacheManager.StoreInCache<string>(key, item.ID.ToString());
		}

		private void AddItemID(string key) {
			var pagesIds = new List<string>(RetrieveCache(key).Split(CHAR_ITEM_ID_SEPARATOR));
			var item = PageContext.Current.Item;
			if (pagesIds.Contains(item.ID.ToString())) {
				return;
			}
			if (pagesIds.Count == NUMBER_OF_CONTENT_ITEMS) {
				pagesIds.RemoveAt(0);
			}
			pagesIds.Add(item.ID.ToString());
			StoreInCache(key, pagesIds);
		}

		private void StoreInCache(string key, List<string> pagesId) {
			_cacheManager.StoreInCache<string>(key, string.Join(ITEM_ID_SEPARATOR, pagesId));
		}

		private bool ExistInCache(string key) {
			return _cacheManager.ExistInCache(key);
		}

		private string RetrieveCache(string key) {
			return _cacheManager.RetrieveFromCache<string>(key);
		}

		private bool IsAuthenticated() {
			return (accountMembership.Status == StatusEnum.Hot ||
				accountMembership.Status == StatusEnum.WarmHot);
		}

		private void SetAccountMembership() {
			accountMembership = _sessionManager.GetAccountMembership();
		}
	}
}