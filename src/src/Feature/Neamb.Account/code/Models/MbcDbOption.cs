using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class MbcDbOption : Option
    {
        public MbcDbOption()
        {
            Values = new Dictionary<string, string>();
        }

        public MbcDbOption(Item item, ID labelFieldId, ID valuesFieldId, string selectedValue = null)
        {
            Label = item.Fields[labelFieldId]?.Value;
            SelectedValue = string.IsNullOrEmpty(selectedValue) ? null : selectedValue;
            Values = GetCategoriesToDictionary(
                ((MultilistField)item.Fields[valuesFieldId])
                .GetItems());
        }
        private Dictionary<string, string> GetCategoriesToDictionary(Item[] getItems)
        {
            try
            {
                return getItems.Where(x => x.Template.BaseTemplates.Any(y => y.ID == Templates.MbcDbField.ID))
                    .ToDictionary(x => x.Fields[Templates.MbcDbField.Fields.MbcDbId]?.Value, x => x.Fields[Templates.CategoryItem.Fields.Value]?.Value);
            }
            catch (Exception e)
            {
                Log.Error("Error while converting categories to Dictionary.", e, this);
            }
            return new Dictionary<string, string>();
        }
    }
}