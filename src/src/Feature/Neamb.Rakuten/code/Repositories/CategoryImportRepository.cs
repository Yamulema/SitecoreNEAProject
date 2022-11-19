using System;
using System.IO;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Rakuten.Model;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Feature.Rakuten.Repositories
{
    [Service(typeof(ICategoryImportRepository))]
    public class CategoryImportRepository: ICategoryImportRepository
    {
        private readonly ICategoryImportManager _categoryImportManager;
        public CategoryImportRepository(ICategoryImportManager categoryImportManager) {
            _categoryImportManager = categoryImportManager;
        }

        public CategoryImportResult ExecuteImportProcess(Stream stream) {
            try {

                CategoryImportResult categoryImportResult = new CategoryImportResult();
                //Execute import process
                var categoryCsvItems = _categoryImportManager.ProcessCsvFile(stream, categoryImportResult);
                if (categoryCsvItems.Count > 0 && categoryImportResult.Errors.Count == 0) {
                    //Set level value in Csv item list items
                    _categoryImportManager.SetCsvItemLevel(categoryCsvItems, categoryImportResult);
                    //Process Sitecore items
                    _categoryImportManager.ProcessSitecoreItems(categoryCsvItems, categoryImportResult);
                }
                return categoryImportResult;
            } catch (Exception e) {
                Log.Error("Category Rakuten import.", e, this);
                throw;
            }
        }
    }
}