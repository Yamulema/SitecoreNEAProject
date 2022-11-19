using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using Sitecore.Data.Items;
using System.Linq;
using Sitecore.Data;
using Sitecore.SecurityModel;

namespace Neamb.Project.Common
{
    public partial class ArchiveVersions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Process_Click(object sender, EventArgs e)
        {
            List<ID> excludePathsNeambId = new List<ID>();
            List<ID> excludePathsSeiumbId = new List<ID>();
            List<ID> excludeTemplatesNeambId = new List<ID>();
            List<ID> excludeTemplatesSeiumbId = new List<ID>();

            ///-----------------------VARIABLES START-------------------
            //variables
            //General variables
            int monthsOlder = 24;
            bool allowArchive = false;

            //NEAMB variables
            string pathInputNeamb = "/sitecore/content/NEAMB//*";
            string itemsToExcludeNeamb = "{E1618CC1-E158-4487-AD2C-C4C38FF195E1}|{EC7726EA-9712-4F93-9914-4F7B185CE2AD}";
            string templatesToExcludeNeamb = "{0B3845C3-566D-4100-A961-0C32C9018320}|{363A6942-7013-4607-857A-888CBF6DB81B}|{DE367237-CE39-4DC4-83BE-58562C57A458}|{3821604B-5F69-4D8B-91DB-2D9D6907C134}|{CEFF0DD5-20FD-40A9-ACD3-C624736B57A2}";

            //SEIUMB variables
            string pathInputSeiumb = "/sitecore/content/NEAMBC//*";
            string itemsToExcludeSeiumb = "";
            string templatesToExcludeSeiumb = "";
            ///-----------------------VARIABLES END-------------------


            //Add paths to exclude NEAMB
            var itemsToBeExcludedNeamb = itemsToExcludeNeamb.Split('|');
            foreach (var itemToBeExcludedNeamb in itemsToBeExcludedNeamb)
            {
                if (!string.IsNullOrEmpty(itemToBeExcludedNeamb))
                {
                    excludePathsNeambId.Add(new ID(itemToBeExcludedNeamb));
                }
            }

            //Add paths to exclude SEIUMB
            var itemsToBeExcludedSeiumb = itemsToExcludeSeiumb.Split('|');
            foreach (var itemToBeExcludedSeiumb in itemsToBeExcludedSeiumb)
            {
                if (!string.IsNullOrEmpty(itemToBeExcludedSeiumb))
                {
                    excludePathsSeiumbId.Add(new ID(itemToBeExcludedSeiumb));
                }
            }

            //Add templates to exclude NEAMB
            var templatesToBeExcludedNeamb = templatesToExcludeNeamb.Split('|');
            foreach (var templateToBeExcludedNeamb in templatesToBeExcludedNeamb)
            {
                if (!string.IsNullOrEmpty(templateToBeExcludedNeamb))
                {
                    excludeTemplatesNeambId.Add(new ID(templateToBeExcludedNeamb));
                }
            }

            //Add templates to exclude Seiumb
            var templatesToBeExcludedSeiumb = templatesToExcludeSeiumb.Split('|');
            foreach (var templateToBeExcludedSeiumb in templatesToBeExcludedSeiumb)
            {
                if (!string.IsNullOrEmpty(templateToBeExcludedSeiumb))
                {
                    excludeTemplatesSeiumbId.Add(new ID(templateToBeExcludedSeiumb));
                }
            }

            var workbookResult = new XLWorkbook();
            if (!string.IsNullOrEmpty(pathInputNeamb))
            {
                GetReport(workbookResult, "Processing Neamb", pathInputNeamb, monthsOlder, allowArchive, excludePathsNeambId, excludeTemplatesNeambId);
            }
            if (!string.IsNullOrEmpty(pathInputSeiumb))
            {
                GetReport(workbookResult, "Processing Seiumb", pathInputSeiumb, monthsOlder, allowArchive, excludePathsSeiumbId, excludeTemplatesSeiumbId);
            }
            using (var msA = new MemoryStream())
            {
                workbookResult.SaveAs(msA);
                msA.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "ArchiveVersions.xlsx"));
                Response.AddHeader("Content-Length", msA.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(msA.ToArray());
                Response.End();
            }
        }

        private bool NoContainsParentToExclude(Sitecore.Data.Items.Item item, List<ID> parentIdToExclude)
        {
            if (item.Parent != null)
            {
                if (parentIdToExclude.Contains(item.ID))
                {
                    return false;
                }
                else
                {
                    // Process parent item
                    return NoContainsParentToExclude(item.Parent, parentIdToExclude);
                }
            }
            else
            {
                return true;
            }
        }

        private void GetReport(
            XLWorkbook workbookResult,
            string worksheetName,
            string pathInput,
            int monthOlder,
            bool archiveAllow,
            List<ID> parentIdToExclude,
            List<ID> templateIdToExclude
        )
        {
            DateTime dateToCompare = DateTime.Now.AddMonths(-monthOlder);
            Sitecore.Data.Database web = Sitecore.Data.Database.GetDatabase("web");
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            Sitecore.Data.Archiving.Archive archive = Sitecore.Data.Database.GetDatabase("master").Archives["archive"];
            Item[] allItemsWeb = web.SelectItems(pathInput);
            Item[] allItemsMaster = master.SelectItems(pathInput);
            var worksheetResult = workbookResult.Worksheets.Add(worksheetName);
            int counter = 1;
            worksheetResult.Cell("A" + counter).Value = "Item ID";
            worksheetResult.Cell("B" + counter).Value = "Item Path";
            worksheetResult.Cell("C" + counter).Value = "Item Name Master";
            worksheetResult.Cell("D" + counter).Value = "Version Web";
            worksheetResult.Cell("E" + counter).Value = "Version to Archive";
            worksheetResult.Cell("F" + counter).Value = "Version Language";
            worksheetResult.Cell("G" + counter).Value = "Version Update date";
            string versionsWeb = "";

            foreach (var itemContentMaster in allItemsMaster)
            {
                if (itemContentMaster.Versions.Count >= 10)
                {
                    //Review if it is required process
                    var noContainsParentToExclude = NoContainsParentToExclude(itemContentMaster, parentIdToExclude);
                    if (noContainsParentToExclude && !templateIdToExclude.Contains(itemContentMaster.TemplateID))
                    {
                        versionsWeb = "";
                        //retrieve the version of the item in web
                        var itemInWeb = allItemsWeb.FirstOrDefault(item => item.ID == itemContentMaster.ID);
                        List<Item> itemsWebVersions = new List<Item>();
                        if (itemInWeb != null && itemInWeb.ID != Sitecore.Data.ID.Null)
                        {
                            itemsWebVersions = itemInWeb.Versions.GetVersions(true).ToList();


                            foreach (var itemInWebByLanguage in itemsWebVersions)
                            {
                                versionsWeb = versionsWeb + "Version:" + itemInWebByLanguage.Version + " Language:" + itemInWebByLanguage.Language.Name + ",";
                            }
                        }
                        var itemsMasterVersions = itemContentMaster.Versions.GetVersions(true);


                        foreach (var itemMasterVersion in itemsMasterVersions)
                        {
                            var itemInWebLanguageVersion = itemsWebVersions.FirstOrDefault(item =>
                                item.Version.Number == itemMasterVersion.Version.Number && item.Language == itemMasterVersion.Language);

                            Sitecore.Data.Fields.DateField updatedField = itemMasterVersion.Fields["__Updated"];
                            string dateTimeString = updatedField.Value;

                            DateTime updatedFieldFinal = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);

                            if ((itemInWebLanguageVersion != null &&
                                    itemInWebLanguageVersion.ID != Sitecore.Data.ID.Null &&
                                    (itemMasterVersion.Language.Name != itemInWebLanguageVersion.Language.Name ||
                                        itemMasterVersion.Version.Number != itemInWebLanguageVersion.Version.Number)) ||
                                (itemInWebLanguageVersion == null ||
                                    itemInWebLanguageVersion.ID == Sitecore.Data.ID.Null) &&
                                updatedFieldFinal <= dateToCompare
                            )
                            {
                                counter++;
                                worksheetResult.Cell("A" + counter).Value = itemMasterVersion.ID.ToString();
                                worksheetResult.Cell("B" + counter).Value = itemMasterVersion.Paths.FullPath;
                                worksheetResult.Cell("C" + counter).Value = itemMasterVersion.Name;
                                worksheetResult.Cell("D" + counter).Value = versionsWeb;
                                worksheetResult.Cell("E" + counter).Value = itemMasterVersion.Version.Number;
                                worksheetResult.Cell("F" + counter).Value = itemMasterVersion.Language.Name;
                                worksheetResult.Cell("G" + counter).Value = updatedFieldFinal.ToShortDateString();

                                if (archiveAllow)
                                {
                                    using (new SecurityDisabler())
                                    {
                                        archive.ArchiveVersion(itemMasterVersion);
                                    }
                                }
                            }

                        }
                    }

                }
            }

        }

    }
}