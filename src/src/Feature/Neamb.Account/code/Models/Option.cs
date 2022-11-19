using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Feature.Account.Models {
	public class Option {

		#region Properties
		public string Label {
			get; set;
		}
		public Dictionary<string, string> Values {
			get; set;
		}
		public string SelectedValue {
			get; set;
		}
		#endregion

		#region Constructors
		public Option(Dictionary<string, string> dict, string selected = null) {
			Values = dict;
			SelectedValue = selected ?? string.Empty;
		}

		public Option() {
			Values = new Dictionary<string, string>();
		}

		public Option(Item item, ID labelFieldId, ID valuesFieldId) {
			Label = item.Fields[labelFieldId]?.Value;
			SelectedValue = Guid.Empty.ToString();
			Values = GetCategoriesToDictionary(
				((MultilistField)item.Fields[valuesFieldId])
				.GetItems());
		}
		#endregion

		#region Private Methods
		private Dictionary<string, string> GetCategoriesToDictionary(Item[] getItems) {
			try {
				return getItems.Where(x => x.Template.BaseTemplates.Any(y => y.ID == Templates.CategoryItem.ID))
					.ToDictionary(x => x.ID.Guid.ToString(), x => x.Fields[Templates.CategoryItem.Fields.Value]?.Value);
			} catch (Exception e) {
				Log.Error("Error while converting categories to Dictionary.", e, this);
			}
			return new Dictionary<string, string>();
		}
		#endregion
	}
}