using System;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Neamb.Project.Common
{
    public partial class AutoPublishReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Process_Click(object sender, EventArgs e) {
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            Sitecore.Data.Database web = Sitecore.Data.Database.GetDatabase("web");
            var workbookResult = new XLWorkbook();
            GetReport(workbookResult, master, web, "/sitecore/content/NEAMB//*", "Processing Neamb", "{DCE49FC2-EC5E-451C-926C-375A21A2EA25}");
            GetReport(workbookResult, master, web, "/sitecore/content/NEAMBC//*", "Processing Seiumb", "{41912D5D-5105-4306-81BF-525E25E0A607}");
            using (var msA = new MemoryStream()) {
                workbookResult.SaveAs(msA);
                msA.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "AutoPublishReport.xlsx"));
                Response.AddHeader("Content-Length", msA.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(msA.ToArray());
                Response.End();
            }
        }
        private static void GetReport(XLWorkbook workbookResult, Database master, Database web, string pathContent,string worksheetName, string workflowStateName) {
            bool hasWorkflow;
            bool isInApprovedWorkflow;
            bool hasPublishinDates;
            bool hasPublishingDatesLatesVersion;
            bool hasNeverPublish;
            bool hasDifferentVersions;
            var worksheetResult = workbookResult.Worksheets.Add(worksheetName);
            int counter = 1;

            worksheetResult.Cell("A" + counter).Value = "Item ID";
            worksheetResult.Cell("B" + counter).Value = "Item Name";
            worksheetResult.Cell("C" + counter).Value = "Item Path";
            worksheetResult.Cell("D" + counter).Value = "Has Default Workflow";
            worksheetResult.Cell("E" + counter).Value = "Has Worflow status Approved";
            worksheetResult.Cell("F" + counter).Value = "Has Publish Date Item";
            worksheetResult.Cell("G" + counter).Value = "Publish Date Item";
            worksheetResult.Cell("H" + counter).Value = "UnPublish Date Item";
            worksheetResult.Cell("I" + counter).Value = "Has Publish Date Last Version";
            worksheetResult.Cell("J" + counter).Value = "Publish Date Last Version";
            worksheetResult.Cell("K" + counter).Value = "UnPublish Date Last Version";
            worksheetResult.Cell("L" + counter).Value = "Never Publish";
            worksheetResult.Cell("M" + counter).Value = "Has Different Versions";
            

            Item[] allItemsHomeMaster = master.SelectItems(pathContent);
            //Log.Info($"count master {allItemsHomeMaster.Length}","");
            Item[] allItemsHomeWeb = web.SelectItems(pathContent);
            //var listPublishQueue = Sitecore.Publishing.PublishManager.GetPublishQueueEntries(DateTime.MinValue, DateTime.MaxValue, master);
            //Log.Info($"count web {allItemsHomeMaster.Length}", "");
            foreach (var itemContent in allItemsHomeMaster) {
                var itemInWeb = allItemsHomeWeb.FirstOrDefault(item => item.ID == itemContent.ID);
                
                if (itemInWeb == null || itemInWeb.Version.Number != itemContent.Version.Number) {
                    hasWorkflow = false;
                    isInApprovedWorkflow = false;
                    hasPublishinDates = false;
                    var defaultWorkflow = itemContent[FieldIDs.DefaultWorkflow];
                    hasNeverPublish = false;
                    hasDifferentVersions = false;
                    hasPublishingDatesLatesVersion = false;

                    var workflowState = itemContent[FieldIDs.WorkflowState];
                    var neverPublish = itemContent[FieldIDs.NeverPublish];
                    var publishDate = itemContent[FieldIDs.PublishDate];
                    var unPublishDate = itemContent[FieldIDs.UnpublishDate];
                    //var publishDateLastVersion = itemContent.Versions.GetLatestVersion()[FieldIDs.PublishDate];
                    //var unPublishDateLastVersion = itemContent.Versions.GetLatestVersion()[FieldIDs.UnpublishDate];

                    var publishDateLastVersion = itemContent.Versions.GetLatestVersion().Publishing.ValidFrom;
                    var unPublishDateLastVersion = itemContent.Versions.GetLatestVersion().Publishing.ValidTo;
                    //var publishDate2 = itemContent.Versions.GetLatestVersion().Publishing.ValidFrom;
                    
                    //var publishDate3 = itemContent.Versions.GetLatestVersion().Publishing.ValidTo;

                    //var unpublishDate = itemContent.Publishing.UnpublishDate;

                    if (itemInWeb != null && itemInWeb.Version.Number != itemContent.Version.Number) {
                        hasDifferentVersions = true;
                    }
                    if (neverPublish.Equals("1")) {
                        hasNeverPublish = true;
                    }
                    if (!string.IsNullOrEmpty(defaultWorkflow)) {
                        hasWorkflow = true;
                    }
                    if (workflowState.Equals(workflowStateName)) {
                        isInApprovedWorkflow = true;
                    }
                    if (!string.IsNullOrEmpty(publishDate) || !string.IsNullOrEmpty(unPublishDate)) {
                        hasPublishinDates = true;
                    }
                    if (publishDateLastVersion!=DateTime.MinValue || unPublishDateLastVersion.Year!=9999)
                    {
                        hasPublishingDatesLatesVersion = true;
                    }
                    
                    publishDate =!string.IsNullOrEmpty(publishDate)? Sitecore.DateUtil.ToServerTime(Sitecore.DateUtil.IsoDateToDateTime(publishDate)).ToString("MM/dd/yyyy HH:mm"):publishDate;
                    unPublishDate= !string.IsNullOrEmpty(unPublishDate) ? Sitecore.DateUtil.ToServerTime(Sitecore.DateUtil.IsoDateToDateTime(unPublishDate)).ToString("MM/dd/yyyy HH:mm"): unPublishDate;

                    string publishDateLastVersionValue = publishDateLastVersion != DateTime.MinValue ? Sitecore.DateUtil.ToServerTime(publishDateLastVersion).ToString("MM/dd/yyyy HH:mm"): "";
                    string unPublishDateLastVersionValue = unPublishDateLastVersion.Year != 9999 ? Sitecore.DateUtil.ToServerTime(unPublishDateLastVersion).ToString("MM/dd/yyyy HH:mm") : "";

                    if (hasPublishinDates || hasWorkflow || isInApprovedWorkflow) {
                        counter++;
                        //var resultFoundQueue = listPublishQueue.FirstOrDefault(item => item.ItemId == itemContent.ID);

                        worksheetResult.Cell("A" + counter).Value = itemContent.ID.ToString();
                        worksheetResult.Cell("B" + counter).Value = itemContent.Name;
                        worksheetResult.Cell("C" + counter).Value = itemContent.Paths.FullPath;
                        worksheetResult.Cell("D" + counter).Value = hasWorkflow ? "Y" : "N";
                        worksheetResult.Cell("E" + counter).Value = isInApprovedWorkflow ? "Y" : "N";
                        worksheetResult.Cell("F" + counter).Value = hasPublishinDates ? "Y" : "N";
                        worksheetResult.Cell("G" + counter).Value = publishDate;
                        worksheetResult.Cell("H" + counter).Value = unPublishDate;
                        worksheetResult.Cell("I" + counter).Value = hasPublishingDatesLatesVersion ? "Y" : "N";
                        worksheetResult.Cell("J" + counter).Value = publishDateLastVersionValue;
                        worksheetResult.Cell("K" + counter).Value = unPublishDateLastVersionValue;
                        worksheetResult.Cell("L" + counter).Value = hasNeverPublish ? "Y" : "N";
                        worksheetResult.Cell("M" + counter).Value = hasDifferentVersions ? "Y" : "N";
                        //worksheetResult.Cell("I" + counter).Value = resultFoundQueue != null ? "Y" : "N";
                    }
                }
            }
        }
    }
}