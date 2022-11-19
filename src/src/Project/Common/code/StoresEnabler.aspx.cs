using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.StringExtensions;

namespace Neamb.Project.Common
{
    public partial class StoresEnabler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GetStoresStatus(object sender, EventArgs e)
        {
            var storeImportManager =
                (IStoreImportManager)ServiceLocator.ServiceProvider.GetService(typeof(IStoreImportManager));

            var storeList = storeImportManager.GetAllStoresInSitecore();
            var excelProcessor = new ExcelProcessor();
            var result = excelProcessor.Process(storeList);
            using (var msA = new MemoryStream())
            {
                result.SaveAs(msA);
                msA.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "StoresStatus.xlsx"));
                Response.AddHeader("Content-Length", msA.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(msA.ToArray());
                Response.End();
            }
        }

        protected void EnableStores(object sender, EventArgs e)
        {
            var pathInput = "D:\\temp\\AutoPublishReport.xlsx";

            XLWorkbook workbook = new XLWorkbook(pathInput);
            var worksheet = workbook.Worksheet(1);

            var storesToEnable = new List<string>();
            int counter = 1;
            foreach (var row in worksheet.Rows()) {
                var sitecoreIdCell = row.Cell("M");
                var sitecoreIdValue = sitecoreIdCell.Value.ToString();
                storesToEnable.Add(sitecoreIdValue);
                counter++;
            }

            var storeImportManager =
                (IStoreImportManager)ServiceLocator.ServiceProvider.GetService(typeof(IStoreImportManager));

            var storesEnabled = storeImportManager.EnableStoresFromList(storesToEnable);
            var excelProcessor = new ExcelProcessor();
            var result = excelProcessor.Process(storesEnabled);
            using (var msA = new MemoryStream())
            {
                result.SaveAs(msA);
                msA.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "StoresEnabled.xlsx"));
                Response.AddHeader("Content-Length", msA.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(msA.ToArray());
                Response.End();
            }
        }
    }

    public class ExcelProcessor
    {
        public XLWorkbook Process(List<StoreReport> storeList)
        {
            var workbookResult = new XLWorkbook();
            var worksheetResult = workbookResult.Worksheets.Add("Stores");
            int counter = 1;

            worksheetResult.Cell("A" + counter).Value = "StoreId";
            worksheetResult.Cell("B" + counter).Value = "Store name";
            worksheetResult.Cell("C" + counter).Value = "NEAMB Enable";
            //worksheetResult.Cell("D" + counter).Value = "SEIUMB Enable";
            worksheetResult.Cell("E" + counter).Value = "Item Processed";

            counter++;
            foreach (var store in storeList)
            {

                worksheetResult.Cell("A" + counter).Value = store.Id;
                worksheetResult.Cell("B" + counter).Value = store.Name;
                worksheetResult.Cell("C" + counter).Value = !store.NeambEnable.IsNullOrEmpty() ? "Y" : "N";
                //worksheetResult.Cell("D" + counter).Value = !store.SeiumbEnable.IsNullOrEmpty() ? "Y" : "N";
                worksheetResult.Cell("E" + counter).Value = store.SitecoreId;

                counter++;
            }
            return workbookResult;
        }
    }
}