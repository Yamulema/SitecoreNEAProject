using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models {
	public class FieldRepDTO : IRenderingModel {
		public ICacheManager _cacheManager {
			get; set;
		}
		public Rendering Rendering {
			get; set;
		}
		public Item Item {
			get; set;
		}
		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		public string State {
			get; set;
		}
		public bool HasTooltip {
			get; set;
		}
		public List<ErrorStatusEnum> Errors {
			get; set;
		}
		public List<SelectListItem> StateList {
			get; set;
		}
		public FieldRepDTO(ICacheManager cacheManager) {
			_cacheManager = cacheManager;
		}
		public FieldRepDTO() {

		}
		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			StateList = GetSelectList(Items.AffiliateRepStates, State);
			HasTooltip = !string.IsNullOrEmpty(Item[Templates.FieldRepForm.Fields.Tooltip]);
		}
		private List<SelectListItem> GetSelectList(ID itemID, string field) {
			var listItems = GetListFromSitecore(itemID, field);
			return listItems;
		}
		private List<SelectListItem> GetListFromSitecore(ID itemID, string field) {
			var listItems = new List<SelectListItem>();
			var list = Sitecore.Context.Database.GetItem(itemID).GetChildren();
			foreach (Item item in list) {
				if (item != null) {
					listItems.Add(new SelectListItem {
						Text = item.Name,
						Value = item[Templates._PageCategoryItem.Fields.Page],
						Selected = !string.IsNullOrEmpty(field) && (item.Name == field)
					});
				}
			}
			return listItems;
		}
	}
}