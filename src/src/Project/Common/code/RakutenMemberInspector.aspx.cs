using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neamb.Project.Common
{
    public class SimpleStore
    {
        public Guid StoreGuid { get; set; }
        public string StoreName { get; set; }
    }

    public partial class RakutenMemberInspector : System.Web.UI.Page
    {

        private ICacheManagerSeiumb cacheManagerSeiumb;
        private ICacheManager cacheManagerNeamb;

        protected void Page_Load(object sender, EventArgs e)
        {
            cacheManagerSeiumb = (ICacheManagerSeiumb)ServiceLocator.ServiceProvider.GetService(typeof(ICacheManagerSeiumb));
            cacheManagerNeamb = (ICacheManager)ServiceLocator.ServiceProvider.GetService(typeof(ICacheManager));
        }

        protected void Process_Click(object sender, EventArgs e)
        {
            Database web = Database.GetDatabase("web");
            var workbookResult = new XLWorkbook();
            Item[] allStores = web.SelectItems("/sitecore/content/MBCShared/Rakuten Stores//*[@@templateid='{26F6C7C8-D74B-474B-A531-4E11F6A07F64}']");
            //GetReport(workbookResult,  "Processing Neamb");
            GetReport(workbookResult, "Processing Neamb", allStores);
            GetReport(workbookResult, "Processing Seiumb", allStores);
            using (var msA = new MemoryStream())
            {
                workbookResult.SaveAs(msA);
                msA.Position = 0;
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "RakutenMembers.xlsx"));
                Response.AddHeader("Content-Length", msA.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(msA.ToArray());
                Response.End();
            }
        }

        protected void Process_Click_OnePage(object sender, EventArgs e)
        {
            Database web = Database.GetDatabase("web");
            Item[] allStores = web.SelectItems("/sitecore/content/MBCShared/Rakuten Stores//*[@@templateid='{26F6C7C8-D74B-474B-A531-4E11F6A07F64}']");
            IList<string> txtLines = new List<string>();
            IList<string> keys = new List<string>();

            for (int unionId = 1; unionId <= 2; unionId++)
            {
                keys = unionId == 1 ? cacheManagerNeamb.SearchPatternInCache("RakutenKey:*") : cacheManagerSeiumb.SearchPatternInCache("RakutenKey:*");

                foreach (var key in keys)
                {
                    MemberCreationResponse rakutenMemberItem;
                    rakutenMemberItem = unionId == 1 ? cacheManagerNeamb.RetrieveFromCache<MemberCreationResponse>(key) : cacheManagerSeiumb.RetrieveFromCache<MemberCreationResponse>(key);

                    var storesJson = "";
                    if (rakutenMemberItem.FavoriteStores != null && rakutenMemberItem.FavoriteStores.Count > 0)
                    {
                        IList<SimpleStore> favoriteStores = new List<SimpleStore>();
                        foreach (Guid storeId in rakutenMemberItem.FavoriteStores)
                        {
                            var favoriteStore = allStores.FirstOrDefault(x => x.ID == new ID(storeId));

                            if (favoriteStore == null) continue;
                            var store = new SimpleStore
                            {
                                StoreGuid = storeId,
                                StoreName = favoriteStore[TemplatesLocal.RakutenStore.Fields.Name]
                            };
                            favoriteStores.Add(store);
                        }

                        storesJson = new StringBuilder(Newtonsoft.Json.JsonConvert.SerializeObject(favoriteStores))
                            //.Replace('"', '\'')
                            .ToString();
                    }

                    string data = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                        key.Replace("RakutenKey:", ""),
                        rakutenMemberItem.EmailAddress,
                        GetDatetimeFromEpoch(rakutenMemberItem.CreatedDate).ToString("MM/dd/yyyy"),
                        unionId,
                        rakutenMemberItem.Id,
                        rakutenMemberItem.EBtoken,
                        storesJson);

                    txtLines.Add(data);
                }
            }

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                foreach (string line in txtLines)
                    streamWriter.WriteLine(line);

                streamWriter.Flush();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("cache-control", "must-revalidate");
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "RakutenMembers.txt"));
                Response.AddHeader("Content-Length", memoryStream.Length.ToString());
                Response.ContentType = "application/otc-stream";
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }
        }

        private void GetReport(XLWorkbook workbookResult, string worksheetName, Item[] allStores)
        {
            var worksheetResult = workbookResult.Worksheets.Add(worksheetName);
            int counter = 1;
            string favoriteStoresNames = "";
            string delimiter = ",";
            IList<string> keys = new List<string>();
            if (worksheetName.Equals("Processing Neamb"))
            {
                keys = cacheManagerNeamb.SearchPatternInCache("RakutenKey:*");
            }
            else
            {
                keys = cacheManagerSeiumb.SearchPatternInCache("RakutenKey:*");
            }
            worksheetResult.Cell("A" + counter).Value = "Mdsid";
            worksheetResult.Cell("B" + counter).Value = "Email";
            worksheetResult.Cell("C" + counter).Value = "Create date";
            worksheetResult.Cell("D" + counter).Value = "ID";
            worksheetResult.Cell("E" + counter).Value = "EBtoken";
            worksheetResult.Cell("F" + counter).Value = "Favorite Stores";
            foreach (var key in keys)
            {
                counter++;
                favoriteStoresNames = "";
                MemberCreationResponse rakutenMemberItem = null;
                if (worksheetName.Equals("Processing Neamb"))
                {
                    rakutenMemberItem = cacheManagerNeamb.RetrieveFromCache<MemberCreationResponse>(key);
                }
                else
                {
                    rakutenMemberItem = cacheManagerSeiumb.RetrieveFromCache<MemberCreationResponse>(key);
                }

                worksheetResult.Cell("A" + counter).Value = key.Replace("RakutenKey:", "");
                worksheetResult.Cell("B" + counter).Value = rakutenMemberItem.EmailAddress;
                worksheetResult.Cell("C" + counter).Value = GetDatetimeFromEpoch(rakutenMemberItem.CreatedDate).ToString("MM/dd/yyyy");
                worksheetResult.Cell("D" + counter).Value = rakutenMemberItem.Id;
                worksheetResult.Cell("E" + counter).Value = rakutenMemberItem.EBtoken;

                if (rakutenMemberItem.FavoriteStores != null)
                {
                    foreach (Guid storeId in rakutenMemberItem.FavoriteStores)
                    {

                        var favoriteStore = allStores.FirstOrDefault(x => x.ID == new ID(storeId));


                        if (favoriteStore != null)
                        {
                            if (string.IsNullOrEmpty(favoriteStoresNames))
                                favoriteStoresNames = favoriteStore[TemplatesLocal.RakutenStore.Fields.Name];
                            else
                                favoriteStoresNames = favoriteStoresNames + delimiter + favoriteStore[TemplatesLocal.RakutenStore.Fields.Name];
                        }
                    }
                }
                worksheetResult.Cell("F" + counter).Value = favoriteStoresNames;
            }

        }

        private DateTime GetDatetimeFromEpoch(long epoch)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
            return dateTimeOffset.DateTime;
        }
    }
}