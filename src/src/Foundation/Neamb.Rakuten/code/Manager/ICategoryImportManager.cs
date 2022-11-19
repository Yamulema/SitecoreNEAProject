using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Neamb.Foundation.Rakuten.Model;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    public interface ICategoryImportManager {
        List<CategoryExcelItem> ProcessCsvFile(Stream stream, CategoryImportResult categoryImportResult);
        void SetCsvItemLevel(List<CategoryExcelItem> listCategoryItems, CategoryImportResult categoryImportResult);
        void ProcessSitecoreItems(List<CategoryExcelItem> listCategoryItems, CategoryImportResult categoryImportResult);
    }
}
