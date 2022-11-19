using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.SecurityModel;
using Sitecore.StringExtensions;

namespace Neamb.Project.Common
{
    public partial class StoresDisable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private List<string> GetStores()
        {
            var pathInput = @"D:\\temp\\Travel Vendors Removal.xlsx";

            XLWorkbook workbook = new XLWorkbook(pathInput);
            var worksheet = workbook.Worksheet(1);

            var storesToEnable = new List<string>();
            foreach (var row in worksheet.Rows())
            {
                if (row.RowNumber() >= 4)
                {

                    var sitecoreIdCell = row.Cell("F");
                    var sitecoreIdValue = sitecoreIdCell.Value.ToString();
                    if (!string.IsNullOrEmpty(sitecoreIdValue))
                    {
                        storesToEnable.Add(sitecoreIdValue);
                    }

                    sitecoreIdCell = row.Cell("H");
                    sitecoreIdValue = sitecoreIdCell.Value.ToString();
                    if (!string.IsNullOrEmpty(sitecoreIdValue))
                    {
                        storesToEnable.Add(sitecoreIdValue);
                    }
                    sitecoreIdCell = row.Cell("B");
                    sitecoreIdValue = sitecoreIdCell.Value.ToString();
                    if (!string.IsNullOrEmpty(sitecoreIdValue))
                    {
                        storesToEnable.Add(sitecoreIdValue);
                    }

                    sitecoreIdCell = row.Cell("D");
                    sitecoreIdValue = sitecoreIdCell.Value.ToString();
                    if (!string.IsNullOrEmpty(sitecoreIdValue))
                    {
                        storesToEnable.Add(sitecoreIdValue);
                    }

                    sitecoreIdCell = row.Cell("J");
                    sitecoreIdValue = sitecoreIdCell.Value.ToString();
                    if (!string.IsNullOrEmpty(sitecoreIdValue))
                    {
                        storesToEnable.Add(sitecoreIdValue);
                    }
                }
            }
            return storesToEnable;
        }
        protected void DisableStores(object sender, EventArgs e)
        {
            var storesToEnable = GetStores();

            var storesEnabled = DisableStoresFromList(storesToEnable, true);
            var excelProcessor = new ExcelProcessorHelper();
            var result = excelProcessor.Process(storesEnabled, "Updated");
            using (var msA = new MemoryStream())
            {
                result.SaveAs(msA);
                msA.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "StoresDisabled.xlsx"));
                Response.AddHeader("Content-Length", msA.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(msA.ToArray());
                Response.End();
            }
        }

        private List<StoreReportLocal> DisableStoresFromList(List<string> storesToEnable, bool update)
        {
            var stores = new List<StoreReportLocal>();
            var master = Database.GetDatabase("master");
            Item[] allStores = master.SelectItems("/sitecore/content/MBCShared/Rakuten Stores//*[@@templateid='{26F6C7C8-D74B-474B-A531-4E11F6A07F64}']");

            using (new SecurityDisabler())
            {
                foreach (var storeItem in storesToEnable)
                {
                    //Get the store in Sitecore
                    var storeItemSitecore = allStores.FirstOrDefault(item => item[TemplatesLocal.RakutenStore.Fields.Name].Trim().ToLower() == storeItem.Trim().ToLower());

                    if (storeItemSitecore == null)
                    {
                        stores.Add(new StoreReportLocal
                        {
                            Name = storeItem,
                            Updated = "No"
                        });
                        continue;
                    }
                    try
                    {
                        if (update)
                        {
                            using (new EditContext(storeItemSitecore, false, false))
                            {
                                storeItemSitecore[TemplatesLocal.RakutenStore.Fields.NeambEnable] = "";
                                storeItemSitecore[TemplatesLocal.RakutenStore.Fields.SeiumbEnable] = "";
                            }
                        }
                        stores.Add(new StoreReportLocal
                        {
                            Id = storeItemSitecore[TemplatesLocal.RakutenStore.Fields.Id],
                            SitecoreId = storeItemSitecore.ID.ToString(),
                            Name = storeItemSitecore[TemplatesLocal.RakutenStore.Fields.Name],
                            NeambEnable = storeItemSitecore[TemplatesLocal.RakutenStore.Fields.NeambEnable],
                            SeiumbEnable = storeItemSitecore[TemplatesLocal.RakutenStore.Fields.SeiumbEnable],
                            Updated = "Yes"
                        });

                    }
                    catch (Exception)
                    {
                        stores.Add(new StoreReportLocal
                        {
                            Id = storeItemSitecore[TemplatesLocal.RakutenStore.Fields.Id],
                            SitecoreId = storeItemSitecore.ID.ToString(),
                            Name = storeItemSitecore[TemplatesLocal.RakutenStore.Fields.Name],
                            NeambEnable = storeItemSitecore[TemplatesLocal.RakutenStore.Fields.NeambEnable],
                            SeiumbEnable = storeItemSitecore[TemplatesLocal.RakutenStore.Fields.SeiumbEnable],
                            Updated = "No"
                        });
                    }
                }
            }
            return stores;
        }

        protected void SearchStores(object sender, EventArgs e)
        {
            var storesToEnable = GetStores();

            var storesEnabled = DisableStoresFromList(storesToEnable, false);
            var excelProcessor = new ExcelProcessorHelper();
            var result = excelProcessor.Process(storesEnabled, "Found item");
            using (var msA = new MemoryStream())
            {
                result.SaveAs(msA);
                msA.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "StoresReportToDisabled.xlsx"));
                Response.AddHeader("Content-Length", msA.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(msA.ToArray());
                Response.End();
            }
        }
    }

    public class StoreReportLocal
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SitecoreId { get; set; }
        public string NeambEnable { get; set; }
        public string SeiumbEnable { get; set; }
        public string Updated { get; set; }
    }
    public class ExcelProcessorHelper
    {
        public XLWorkbook Process(List<StoreReportLocal> storeList, string columnName)
        {
            var workbookResult = new XLWorkbook();
            var worksheetResult = workbookResult.Worksheets.Add("Stores");
            int counter = 1;

            worksheetResult.Cell("A" + counter).Value = "StoreId";
            worksheetResult.Cell("B" + counter).Value = "Store name";
            worksheetResult.Cell("C" + counter).Value = "NEAMB Enable";
            worksheetResult.Cell("D" + counter).Value = "SEIUMB Enable";
            worksheetResult.Cell("E" + counter).Value = "Item Found";
            worksheetResult.Cell("F" + counter).Value = columnName;

            counter++;
            foreach (var store in storeList)
            {

                worksheetResult.Cell("A" + counter).Value = store.Id;
                worksheetResult.Cell("B" + counter).Value = store.Name;
                worksheetResult.Cell("C" + counter).Value = !store.NeambEnable.IsNullOrEmpty() ? "Y" : "N";
                worksheetResult.Cell("D" + counter).Value = !store.SeiumbEnable.IsNullOrEmpty() ? "Y" : "N";
                worksheetResult.Cell("E" + counter).Value = store.SitecoreId;
                worksheetResult.Cell("F" + counter).Value = store.Updated;

                counter++;
            }
            return workbookResult;
        }
    }

    public struct TemplatesLocal
    {
        public struct RakutenStore
        {
            public static readonly ID ID = new ID("{BA4A37A8-53E9-43D8-B4ED-B81B395CCE29}");

            public struct Fields
            {
                public static readonly ID Id = new ID("{3FDDD9D6-0866-4E6D-B02B-E8776F1F799F}");
                public static readonly ID Name = new ID("{365E9AE1-60E6-4889-B838-D66BDE087B8D}");
                public static readonly ID Description = new ID("{5DB6A22E-5709-4AC9-9E8A-8745B3D52176}");
                public static readonly ID ShortDescription = new ID("{9E2D663B-ACF7-4928-862D-D345BA757B9F}");
                public static readonly ID Categories = new ID("{8E5C94E6-FAE2-4495-91B2-F2E0980558E7}");
                public static readonly ID Banner = new ID("{B03CA598-21E3-4271-AF93-863F7075BA17}");
                public static readonly ID SmallLogo = new ID("{C57B9FB4-1A4B-4B05-9067-35B6F90B05F8}");
                public static readonly ID Thumbnail = new ID("{3EF4A5AA-C656-4FFB-A782-B9045E5F3234}");
                public static readonly ID Icon11230 = new ID("{0992A3F9-57A4-4156-B800-41C3628B26B6}");
                public static readonly ID Icon22460 = new ID("{83863813-469F-4F8F-A181-124749E946CA}");
                public static readonly ID Icon33690 = new ID("{80B65725-DB71-4515-9794-5AEFE2590F30}");
                public static readonly ID LogoEmail = new ID("{3DDEDB3C-6E63-4892-ABCE-5288D0F26B3A}");
                public static readonly ID LogoMobile = new ID("{B6641DCD-A356-440E-B377-C0FAB3E00BCD}");
                public static readonly ID LogoMobile2 = new ID("{A100AB13-9695-4088-969B-C23118211EC4}");
                public static readonly ID LogoMobile3 = new ID("{AB7D0A77-6EFC-49C8-A3F0-1529712EFC91}");
                public static readonly ID FeedSquareLogo = new ID("{A40FE503-7E00-46D5-9652-5AE9CCDF48A6}");
                public static readonly ID MembersOnlyNEA = new ID("{EE2C4820-7A60-4614-8217-90FA5E9EC46B}");
                public static readonly ID MembersOnlySEIU = new ID("{32A4A597-B46A-4C1F-A6C4-F8A53DB0CB2B}");
                public static readonly ID NeambEnable = new ID("{EA93F5B7-DC52-49CB-A848-A6DFBE79FC8B}");
                public static readonly ID SeiumbEnable = new ID("{03DD7EA8-9DD1-4768-890C-53BE237A6C73}");
                //calculated
                public static readonly ID BaseReward = new ID("{429902D7-7451-48CB-823F-14D61A7E2B7D}");
                public static readonly ID TotalReward = new ID("{F5619FA7-55E0-40E7-B2E2-F4FB989D3030}");
                public static readonly ID TypeReward = new ID("{338AA4DC-EE49-4A8F-9655-F27E0E96FF64}");
                public static readonly ID TierReward = new ID("{DABFF084-D8C5-4512-99FC-3E67785D62A1}"); //check box
                public static readonly ID ShoppingUrl = new ID("{4C71C274-0D09-4849-A270-666D5042B276}");
                public static readonly ID ShoppingUrlSeiumb = new ID("{D6C16A05-EC6A-4067-814F-42DBBA75EC2D}");
            }
        }
    }
}