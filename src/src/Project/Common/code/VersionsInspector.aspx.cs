using System;
using System.IO;
using ClosedXML.Excel;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neamb.Project.Common
{
    public partial class VersionsInspector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Process_Click(object sender, EventArgs e) {
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            var workbookResult = new XLWorkbook();
            GetReport(workbookResult, master, "/sitecore/content/NEAMB//*", "Processing Neamb");
            GetReport(workbookResult, master, "/sitecore/content/NEAMBC//*", "Processing Seiumb");
            using (var msA = new MemoryStream()) {
                workbookResult.SaveAs(msA);
                msA.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "VersionsInspector.xlsx"));
                Response.AddHeader("Content-Length", msA.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(msA.ToArray());
                Response.End();
            }
        }
        private static void GetReport(XLWorkbook workbookResult, Database master, string pathContent,string worksheetName) {
            var worksheetResult = workbookResult.Worksheets.Add(worksheetName);
            int counter = 1;

            worksheetResult.Cell("A" + counter).Value = "Item ID";
            worksheetResult.Cell("B" + counter).Value = "Item Name";
            worksheetResult.Cell("C" + counter).Value = "Item Path";
            worksheetResult.Cell("D" + counter).Value = "Versions";
            
            Item[] allItemsHomeMaster = master.SelectItems(pathContent);
            
            foreach (var itemContent in allItemsHomeMaster) {
                if (itemContent.Versions.Count >= 10) {
                    counter++;
                    worksheetResult.Cell("A" + counter).Value = itemContent.ID.ToString();
                    worksheetResult.Cell("B" + counter).Value = itemContent.Name;
                    worksheetResult.Cell("C" + counter).Value = itemContent.Paths.FullPath;
                    worksheetResult.Cell("D" + counter).Value = itemContent.Versions.Count;
                }
            }
        }
    }
}