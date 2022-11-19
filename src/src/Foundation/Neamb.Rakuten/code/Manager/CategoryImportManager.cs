using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Rakuten.Model;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    [Service(typeof(ICategoryImportManager))]
    public class CategoryImportManager: ICategoryImportManager
    {
        private readonly IRakutenImportOperation _rakutenImportOperation;

        public CategoryImportManager(IRakutenImportOperation rakutenImportOperation) {
            _rakutenImportOperation = rakutenImportOperation;
        }
        /// <summary>
        /// Get the information from the Csv file
        /// </summary>
        /// <param name="stream">Csv file</param>
        /// <param name="categoryImportResult">Processing result</param>
        /// <returns>Items read from the csv file</returns>
        public List<CategoryExcelItem> ProcessCsvFile(Stream stream, CategoryImportResult categoryImportResult)
        {
            if (stream == null || categoryImportResult == null) {
                throw new ArgumentException($"Parameters for  ProcessCsvFile are incorrect");
            }
            List<CategoryExcelItem> listReturn = new List<CategoryExcelItem>();
            try
            {
                StreamReader reader = new StreamReader(stream);
                using (var csv = new CachedCsvReader(reader, true, ','))
                {
                    int fieldCount = csv.FieldCount;
                    string[] headers = csv.GetFieldHeaders();
                    if (headers.Length == 0)
                    {
                        categoryImportResult.Errors.Add($"Error no header in csv file");
                    }
                    while (csv.ReadNextRecord())
                    {
                        var id = "";
                        var parentId = "";
                        var name = "";

                        for (int i = 0; i < fieldCount; i++)
                        {
                            switch (headers[i].Trim())
                            {
                                case "id":
                                    id = csv[i];
                                    break;
                                case "parent_id":
                                    parentId = csv[i];
                                    break;
                                case "name":
                                    name = csv[i];
                                    break;
                            }
                        }
                        CategoryExcelItem item = new CategoryExcelItem
                        {
                            Id = id,
                            Name = name,
                            ParentId = parentId
                            
                        };
                        listReturn.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                categoryImportResult.Errors.Add($"Error processing the Csv file. {e.Message}");
                Log.Error("Category Rakuten import.Error processing the Csv file.",e,this);
                throw;
            }
            return listReturn;
        }

        /// <summary>
        /// Set the nested level in items retrieved from the Csv file
        /// </summary>
        /// <param name="listCategoryItems">Data retrieved from the csv file </param>
        public void SetCsvItemLevel(List<CategoryExcelItem> listCategoryItems, CategoryImportResult categoryImportResult)
        {
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            Item[] allItemsCategories = master.SelectItems("/sitecore/content/MBCShared/Rakuten Categories//*");

            foreach (var categoryExcelItem in listCategoryItems) {
                if (categoryExcelItem.Id == categoryExcelItem.ParentId) {
                    Log.Error($"Category Rakuten import. Error item with the same parent id {categoryExcelItem.Id}, category name {categoryExcelItem.Name}", this);
                    categoryImportResult.Errors.Add($"Error item with the same parent id {categoryExcelItem.Id}, category name {categoryExcelItem.Name}");
                } else {
                    var itemFoundSitecore = allItemsCategories.FirstOrDefault(item => item[Templates.RakutenCategory.Fields.Id] == categoryExcelItem.Id);
                    categoryExcelItem.ItemSitecore = itemFoundSitecore;
                    OrderListExecution(listCategoryItems, categoryExcelItem, allItemsCategories, categoryImportResult);
                }
            }
        }

        /// <summary>
        /// Recursive method to get the parent and set the level that is the parent level +1
        /// </summary>
        /// <param name="listCategoryItems">Data retrieved from the CSV</param>
        /// <param name="categoryExcelItem">Current item from the CSV to be processed</param>
        /// <param name="allItemsCategories">Items retrieved from Sitecore to set if that already exist</param>
        private void OrderListExecution(List<CategoryExcelItem> listCategoryItems, CategoryExcelItem categoryExcelItem, Item[] allItemsCategories, CategoryImportResult categoryImportResult)
        {

            if (categoryExcelItem.ParentId.Equals("-1"))
            {
                categoryExcelItem.Level = 0;
            }
            else
            {
                var parentCategory = listCategoryItems.FirstOrDefault(item => item.Id == categoryExcelItem.ParentId);
                if (parentCategory?.Level != null)
                {
                    var levelParent = parentCategory.Level + 1;
                    categoryExcelItem.Level = levelParent;
                    var itemFoundParentSitecore = allItemsCategories.FirstOrDefault(item => item[Templates.RakutenCategory.Fields.Id] == parentCategory.Id);
                    categoryExcelItem.ItemParentSitecore = itemFoundParentSitecore;
                }
                else
                {
                    if (parentCategory != null) {
                        OrderListExecution(listCategoryItems, parentCategory, allItemsCategories,categoryImportResult);
                        var levelParent = parentCategory.Level + 1;
                        categoryExcelItem.Level = levelParent;
                        var itemFoundParentSitecore = allItemsCategories.FirstOrDefault(item => item[Templates.RakutenCategory.Fields.Id] == parentCategory.Id);
                        categoryExcelItem.ItemParentSitecore = itemFoundParentSitecore;

                    } else {
                        Log.Error($"Category Rakuten import. Error with no parent category id {categoryExcelItem.Id}, category name {categoryExcelItem.Name}",this);
                        categoryImportResult.Errors.Add($"Error with no parent category id {categoryExcelItem.Id}, category name {categoryExcelItem.Name}");
                    }
                }
            }
        }

        public void ProcessSitecoreItems(List<CategoryExcelItem> listCategoryItems, CategoryImportResult categoryImportResult) {
            //Process creation or update
            ProcessCreationUpdateSitecoreItems(listCategoryItems, categoryImportResult);
            //Process delete
            ProcessDeleteSitecoreItems(listCategoryItems, categoryImportResult);
            //Process publish
            _rakutenImportOperation.PublishItem(Templates.RakutenCategoryParentItem.ID);
        }
        private  void ProcessDeleteSitecoreItems(List<CategoryExcelItem> listCategoryItems, CategoryImportResult categoryImportResult) {
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            Item[] allItemsCategoriesSitecore = master.SelectItems("/sitecore/content/MBCShared/Rakuten Categories//*");
            var categoriesSitecoreMatch= allItemsCategoriesSitecore.Where(item => listCategoryItems.Select(x => x.Id).Contains(item[Templates.RakutenCategory.Fields.Id])).ToList();
            var itemsToDelete = allItemsCategoriesSitecore.Except(categoriesSitecoreMatch);
            using (new Sitecore.SecurityModel.SecurityDisabler()) {
                foreach (var categoryToDelete in itemsToDelete) {

                    _rakutenImportOperation.DeleteItemSitecore(categoryToDelete);
                    categoryImportResult.DeletedItems.Add(categoryToDelete);
                }
            }
        }

        private void ProcessCreationUpdateSitecoreItems(List<CategoryExcelItem> listCategoryItems, CategoryImportResult categoryImportResult)
        {
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            Item[] allItemsCategories = master.SelectItems("/sitecore/content/MBCShared/Rakuten Categories//*");
            var maxLevel = listCategoryItems.Max(item => item.Level);
            List<Item> newItems = new List<Item>();
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                for (int i = 0; i <= maxLevel; i++)
                {
                    var itemsCategory = listCategoryItems.Where(item => item.Level == i);
                    foreach (var itemListCategory in itemsCategory)
                    {
                        Item parentItem = null;

                        parentItem = GetParentItem(itemListCategory, newItems, allItemsCategories);
                        if (parentItem != null)
                        {

                            //Creation
                            if (itemListCategory.ItemSitecore == null)
                            {
                                var newItem = CreateCategoryItemSitecore(parentItem, itemListCategory);
                                if (newItem != null)
                                {
                                    newItems.Add(newItem);
                                }
                            }
                            //Update
                            else
                            {
                                UpdateCategoryItemSitecore(parentItem, itemListCategory.ItemSitecore, itemListCategory);
                                categoryImportResult.UpdatedItems.Add(itemListCategory.ItemSitecore);
                            }

                        }
                        else
                        {
                            categoryImportResult.Errors.Add($"Error with no parent category id {itemListCategory.Id}, category name {itemListCategory.Name}");
                            Log.Error($"Category Rakuten import. Error with no parent category id {itemListCategory.Id}, category name {itemListCategory.Name}",this);
                        }
                    }
                }
                categoryImportResult.NewItems = newItems;

            }
        }

        private bool IsItemRoot(CategoryExcelItem itemListCategory)
        {
            return itemListCategory.ParentId.Equals("-1");
        }
        private Item GetParentRoot(CategoryExcelItem itemListCategory, Database master)
        {
            Item parentItem = null;
            if (itemListCategory.ParentId.Equals("-1"))
            {
                parentItem = master.GetItem(Templates.RakutenCategoryParentItem.ID);
            }
            return parentItem;
        }

        private Item GetParentInner(CategoryExcelItem itemListCategory, List<Item> newItems,
            Item[] allCategoriesSitecore)
        {
            var finalItems = newItems.Union(allCategoriesSitecore);
            return finalItems.FirstOrDefault(item => item[Templates.RakutenCategory.Fields.Id] == itemListCategory.ParentId);
        }
        private Item GetParentItem(
            CategoryExcelItem itemListCategory,
            List<Item> newItems,
            Item[] allItemsCategories
        )
        {
            Item parentItem = null;
            Database master = Database.GetDatabase("master");
            if (itemListCategory.ItemParentSitecore != null)
            {
                parentItem = itemListCategory.ItemParentSitecore;
            }
            else
            {
                bool isItemRoot = IsItemRoot(itemListCategory);
                if (isItemRoot)
                {
                    parentItem = GetParentRoot(itemListCategory, master);
                }
                else
                {
                    parentItem = GetParentInner(itemListCategory, newItems, allItemsCategories);
                }
            }
            
            return parentItem;
        }
        private void UpdateCategoryItemSitecore(Item itemParent, Item itemToUpdate, CategoryExcelItem categoryExcelItem)
        {
            using (new EditContext(itemToUpdate, false, false))
            {
                itemToUpdate[Templates.RakutenCategory.Fields.Id] = categoryExcelItem.Id;
                itemToUpdate[Templates.RakutenCategory.Fields.Name] = categoryExcelItem.Name;
            }

            //case required to move to the right parent
            if (itemToUpdate.Parent.ID != itemParent.ID)
            {
                itemToUpdate.MoveTo(itemParent);
            }
        }


        private Item CreateCategoryItemSitecore(Item itemFoundSitecoreParent, CategoryExcelItem categoryExcelItem)
        {
            string sanitizedName = ItemUtil.ProposeValidItemName(categoryExcelItem.Name);
            TemplateID templateId = new TemplateID(Templates.RakutenItemTemplate.ID);
            Item newItem = itemFoundSitecoreParent.Add(sanitizedName, templateId);
            if (newItem != null)
            {
                using (new EditContext(newItem, false, false))
                {
                    newItem[Templates.RakutenCategory.Fields.Id] = categoryExcelItem.Id;
                    newItem[Templates.RakutenCategory.Fields.Name] = categoryExcelItem.Name;
                    newItem[Templates.RakutenCategory.Fields.NeambEnable] = "1";
                    newItem[Templates.RakutenCategory.Fields.SeiumbEnable] = "1";
                }
            }
            return newItem;
        }

        
        
    }
}