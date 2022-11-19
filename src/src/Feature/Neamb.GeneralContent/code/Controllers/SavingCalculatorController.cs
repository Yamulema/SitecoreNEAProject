using Neambc.Neamb.Feature.GeneralContent.Models;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class SavingCalculatorController : BaseController
    {
        #region ActionResult Methods
        public ActionResult SavingCalculator()
        {
            SavingCalculatorDto model = new SavingCalculatorDto();
            model.Initialize(RenderingContext.Current.Rendering);

            //Fills the model.
            model.SavingCalculatorItems = GetSavingCalculatorItems(model.Item);

            return View("/Views/Neamb.GeneralContent/SavingsCalculator/SavingsCalculator.cshtml", model);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the children saving calculator product cards
        /// </summary>
        /// <param name="datasource">Saving Calculator folder</param>
        /// <returns></returns>
        private List<List<SavingCalculatorItem>> GetSavingCalculatorItems(Item datasource)
        {
            List<List<SavingCalculatorItem>> result = new List<List<SavingCalculatorItem>>();
            int counter = 1;
            List<SavingCalculatorItem> newInnerList = new List<SavingCalculatorItem>();
            foreach (Item child in datasource.GetChildren())
            {
                //Identify if the row is 3 to add a new List
                if(counter % 3 == 0)
                {
                    newInnerList.Add(GetSavingCalculatorItem(child));
                    result.Add(newInnerList);
                    newInnerList = new List<SavingCalculatorItem>();
                }
                else
                {
                    newInnerList.Add(GetSavingCalculatorItem(child));
                }
                counter++;
            }
            if (newInnerList != null && newInnerList.Count > 0)
            {
                result.Add(newInnerList);
            }

            return result;
            
        }

        /// <summary>
        /// Maps a given sitecore Item into SavingCalculatorItem.
        /// </summary>
        /// <param name="item">Saving Calculator item</param>
        /// <returns></returns>
        private SavingCalculatorItem GetSavingCalculatorItem(Item item)
        {
            var result = new SavingCalculatorItem { SavingCalculatorProductItem = item };
            var itemIconGlobal = item[Templates.SavingsCalculatorProduct.Fields.Icon];
            if (!string.IsNullOrEmpty(itemIconGlobal))
            {
                var selectedDivisionRuleItem = Context.Database.GetItem(new ID(itemIconGlobal));
                var iconSelected = selectedDivisionRuleItem[Templates.CategoryItem.Fields.Value];
                if (!string.IsNullOrEmpty(iconSelected))
                {
                    result.Icon = $"fa fa-2x {iconSelected}";
                }
            }            
            return result;
        }

        #endregion
    }
}