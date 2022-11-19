using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using Sitecore.Data.Items;
using System.Linq;
using Sitecore.Data;
using Sitecore.SecurityModel;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neamb.Project.Common
{
    public partial class PopulateProgramCodesFinal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Process_Click(object sender, EventArgs e)
        {
            List<string> templatesToCompareList = new List<string>();
            

            ///-----------------------VARIABLES START-------------------
            //NEAMB variables
            string pathInputNeamb = "/sitecore/content/NEAMB//*";
            string templatesToCompare = "{CDCEBEEF-02EB-4CF2-86A3-41BC0D31F613}|{BE6F5ACA-A166-4D70-918A-FC2C330111C5}|{C23718EB-13D3-417A-80EB-A8942BE2B236}";
            
            
            ///-----------------------VARIABLES END-------------------


            //Add paths to exclude NEAMB
            var itemsTemplatesToCompareAll = templatesToCompare.Split('|');
            foreach (var itemsTemplatesToCompare in itemsTemplatesToCompareAll)
            {
                if (!string.IsNullOrEmpty(itemsTemplatesToCompare))
                {
                    templatesToCompareList.Add(itemsTemplatesToCompare);
                }
            }

            

            var workbookResult = new XLWorkbook();
             GetReport(workbookResult, "Processing Neamb", pathInputNeamb, templatesToCompareList);
            
            
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

        private void GetReport(
            XLWorkbook workbookResult,
            string worksheetName,
            string pathInput,
            List<string> templatesToCompareList
        
        )
        {
            Sitecore.Data.Database web = Sitecore.Data.Database.GetDatabase("web");
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            string fileName = "C:\\temp\\Program_Codes_Website.XLSX";
            ID programCodeNew = new ID("{B01AD396-BC36-486A-839E-889926842C54}");
            ID parentProgramCodes = new ID("{C4D7E815-5666-4991-9C17-949EF49970D3}");
            ID templateProgramCodesId = new ID("{9144B452-185D-42BE-A59A-7CA2AC559BF6}");
            ID programCodeTitleId = new ID("{A7612FAF-7445-4ADB-BEFC-1810C14E5414}");
            ID programCodeDataId = new ID("{B01AD396-BC36-486A-839E-889926842C54}");
            ID productCtaProgramCode = new ID("{D7125B4C-E4AA-4C56-A7E2-A5BC2369B88B}");
            ID productOfferCardProgramCode = new ID("{5EEE1604-783B-49C1-AB00-60BA69EE79F2}");
            ID specialOfferProgramCode = new ID("{C281C8C8-E549-47DD-A707-5099B2B28C22}");
            var templateProgramCodes = new TemplateID(templateProgramCodesId);
            var rootParentProgramCodes = master.GetItem(parentProgramCodes);

            var workbook = new XLWorkbook(fileName);
            var ws1 = workbook.Worksheet(1);
            var ws3 = workbook.Worksheet(3);

            foreach (IXLRow row in ws1.Rows())
            {
                var productCodeInput = row.Cell(4).GetValue<string>();
                
                if (!string.IsNullOrEmpty(productCodeInput) && !productCodeInput.Equals("-"))
                {
                    var titleInput = row.Cell(3).GetValue<string>();
                    var titleInput2 = row.Cell(2).GetValue<string>();

                    string sanitizedName = "";
                    if(!string.IsNullOrEmpty(titleInput))
                    sanitizedName= ItemUtil.ProposeValidItemName(String.Format("{0} {1}", titleInput, productCodeInput));
                    else
                        sanitizedName = ItemUtil.ProposeValidItemName(String.Format("{0} {1}", titleInput2, productCodeInput));


                    CreateGlobalProgramCode(programCodeTitleId, programCodeDataId, templateProgramCodes, productCodeInput, sanitizedName, titleInput, rootParentProgramCodes);

                }
            }

            foreach (IXLRow row in ws3.Rows())
            {
                var productCodeInput = row.Cell(4).GetValue<string>();

                if (!string.IsNullOrEmpty(productCodeInput) && !productCodeInput.Equals("-"))
                {
                    var titleInput = row.Cell(3).GetValue<string>();
                    var titleInput2 = row.Cell(2).GetValue<string>();

                    string sanitizedName = "";
                    if (!string.IsNullOrEmpty(titleInput))
                        sanitizedName = ItemUtil.ProposeValidItemName(String.Format("{0} {1}", titleInput, productCodeInput));
                    else
                        sanitizedName = ItemUtil.ProposeValidItemName(String.Format("{0} {1}", titleInput2, productCodeInput));


                    CreateGlobalProgramCode(programCodeTitleId, programCodeDataId, templateProgramCodes, productCodeInput, sanitizedName, titleInput, rootParentProgramCodes);

                }
            }
            //Get the data from row 6 column I
            var row6 = ws1.Row(6);
            var productCodeInputRow6 = row6.Cell(9).GetValue<string>();
            var titleInputRow6 = row6.Cell(8).GetValue<string>();
            string sanitizedNameRow6 = ItemUtil.ProposeValidItemName(titleInputRow6);
            CreateGlobalProgramCode(programCodeTitleId, programCodeDataId, templateProgramCodes, productCodeInputRow6, sanitizedNameRow6, titleInputRow6, rootParentProgramCodes);

            Item[] allItemsMaster = master.SelectItems(pathInput);
            string pathGlobalProgramCodes = "/sitecore/content/NEAMB/Global/Product Codes//*";
            Item[] allProgramCodesMaster = master.SelectItems(pathGlobalProgramCodes);
            
            
            var worksheetResult = workbookResult.Worksheets.Add(worksheetName);
            int counter = 1;
            worksheetResult.Cell("A" + counter).Value = "Item ID";
            worksheetResult.Cell("B" + counter).Value = "Item Path";
            worksheetResult.Cell("C" + counter).Value = "Item Name";
            worksheetResult.Cell("D" + counter).Value = "Product Code";
            worksheetResult.Cell("E" + counter).Value = "Item in Global";
            worksheetResult.Cell("F" + counter).Value = "Item in Global Path";
            worksheetResult.Cell("G" + counter).Value = "Item in Global Name";
            
            string productCode = "";
            Item itemProgramCodeGlobal = null;
            foreach (var itemContentMaster in allItemsMaster)
            {
                productCode = "";
                string templateFound= templatesToCompareList.FirstOrDefault(x => itemContentMaster.IsDerived(new ID(x)));
                if (!string.IsNullOrEmpty(templateFound))
                {
                    allProgramCodesMaster = master.SelectItems(pathGlobalProgramCodes);

                    //Check for ProductCta
                    if (templateFound.Equals("{CDCEBEEF-02EB-4CF2-86A3-41BC0D31F613}"))
                    {
                        //get the current product code data
                        productCode = itemContentMaster[new ID("{14EB4F0E-41C9-48C1-98E1-032E873F94B3}")];
                        itemProgramCodeGlobal = VerifyGlobalProgramCode(allProgramCodesMaster, programCodeNew, programCodeTitleId, programCodeDataId, templateProgramCodes, productCode, rootParentProgramCodes);
                        if (itemProgramCodeGlobal != null)
                        {
                            UpdateProgramCodeContent(productCtaProgramCode, itemContentMaster, itemProgramCodeGlobal);
                        }
                    }

                    //Check for ProductOfferCard
                    if (templateFound.Equals("{BE6F5ACA-A166-4D70-918A-FC2C330111C5}"))
                    {
                        //get the current product code data
                        productCode = itemContentMaster[new ID("{5C6D4AB6-D18F-486D-82BD-9E944D21D56D}")];
                        itemProgramCodeGlobal = VerifyGlobalProgramCode(allProgramCodesMaster, programCodeNew, programCodeTitleId, programCodeDataId, templateProgramCodes, productCode, rootParentProgramCodes);
                        if (itemProgramCodeGlobal != null)
                        {
                            UpdateProgramCodeContent(productOfferCardProgramCode, itemContentMaster, itemProgramCodeGlobal);
                        }
                        
                    }

                    //Check for ProductOfferCard
                    if (templateFound.Equals("{C23718EB-13D3-417A-80EB-A8942BE2B236}"))
                    {
                        //get the current product code data
                        productCode = itemContentMaster[new ID("{1ECD6E58-F256-471C-A87E-62B5FCBE2192}")];
                        itemProgramCodeGlobal = VerifyGlobalProgramCode(allProgramCodesMaster, programCodeNew, programCodeTitleId, programCodeDataId, templateProgramCodes, productCode, rootParentProgramCodes);
                        if (itemProgramCodeGlobal != null)
                        {
                            UpdateProgramCodeContent(specialOfferProgramCode, itemContentMaster, itemProgramCodeGlobal);
                        }
                        
                    }
                    counter++;
                    worksheetResult.Cell("A" + counter).Value = itemContentMaster.ID.ToString();
                    worksheetResult.Cell("B" + counter).Value = itemContentMaster.Paths.FullPath;
                    worksheetResult.Cell("C" + counter).Value = itemContentMaster.Name;
                    worksheetResult.Cell("D" + counter).Value = productCode;
                    if (itemProgramCodeGlobal != null)
                    {
                        worksheetResult.Cell("E" + counter).Value = itemProgramCodeGlobal.ID.ToString();
                        worksheetResult.Cell("F" + counter).Value = itemProgramCodeGlobal.Paths.FullPath;
                        worksheetResult.Cell("G" + counter).Value = itemProgramCodeGlobal.Name;
                    }
                    else
                    {
                        worksheetResult.Cell("E" + counter).Value = "No";
                        worksheetResult.Cell("F" + counter).Value = "No";
                        worksheetResult.Cell("G" + counter).Value = "No";
                    }
                }


            }

        }

        private static Item VerifyGlobalProgramCode(Item[] allProgramCodesMaster, ID programCodeNew, ID programCodeTitleId, ID programCodeDataId, TemplateID templateProgramCodes, string productCode, Item rootParentProgramCodes)
        {
            Item itemProgramCodeGlobal = allProgramCodesMaster.FirstOrDefault(item => item[programCodeNew] == productCode);
            if (itemProgramCodeGlobal == null || itemProgramCodeGlobal.ID == Sitecore.Data.ID.Null)
            {
                if(string.IsNullOrEmpty(productCode))
                {
                    return null;
                }
                //else
                //{
                //    //create it
                //    itemProgramCodeGlobal = CreateGlobalProgramCode(programCodeTitleId, programCodeDataId, templateProgramCodes, productCode, productCode, productCode, rootParentProgramCodes);

                //}
            }

            return itemProgramCodeGlobal;
        }

        private static void UpdateProgramCodeContent(ID productCtaProgramCode, Item itemContentMaster, Item itemProgramCodeGlobal)
        {
            using (new SecurityDisabler())
            {
                using (new EditContext(itemContentMaster, false, false))
                {
                    itemContentMaster[productCtaProgramCode] = itemProgramCodeGlobal.ID.ToString();
                }
            }
        }

        private static Item CreateGlobalProgramCode(ID programCodeTitleId, ID programCodeDataId, TemplateID templateProgramCodes, string productCode, string itemName, string itemTitle, Item rootParentProgramCodes)
        {
            Item itemProgramCodeGlobal;
            using (new SecurityDisabler())
            {
                var programCodeNewItem = rootParentProgramCodes.Add(itemName, templateProgramCodes);

                using (new EditContext(programCodeNewItem, false, false))
                {
                    programCodeNewItem[programCodeTitleId] = itemTitle;
                    programCodeNewItem[programCodeDataId] = productCode;
                }
                itemProgramCodeGlobal = programCodeNewItem;
            }

            return itemProgramCodeGlobal;
        }
    }
}