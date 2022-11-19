using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Services.Rakuten;
using Sitecore.Collections;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;


namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    [Service(typeof(IStoreImportManager))]
    public class StoreImportManager : IStoreImportManager
    {
        private readonly IRakutenStoreRestRepository _rakutenStoreRestRepository;
        private readonly IGlobalConfigurationManager _config;
        private readonly IStoreProcessChecker _storeProcessChecker;
        private readonly IRakutenLog _rakutenLog;
        private readonly IRakutenImportOperation _rakutenImportOperation;

        public StoreImportManager(IRakutenStoreRestRepository rakutenStoreRestRepository, IGlobalConfigurationManager config, IStoreProcessChecker storeProcessChecker, IRakutenLog rakutenLog, IRakutenImportOperation rakutenImportOperation)
        {
            _rakutenStoreRestRepository = rakutenStoreRestRepository;
            _config = config;
            _storeProcessChecker = storeProcessChecker;
            _rakutenLog = rakutenLog;
            _rakutenImportOperation = rakutenImportOperation;
        }

        public void ExecuteImportStoreSitecore()
        {
            _rakutenLog.Debug("Starting import store process");
            var stores = GetAllStoresAndDetail();
            if (stores?.Result?.Store != null && stores.Result.CanContinueProcess && stores.Result.Store.Count > 0)
            {
                ProcessStoreInSitecore(stores);
            }
            _rakutenLog.Debug("Ending import store process");
        }

        /// <summary>
        /// Retrieve from the Rakuten api the store and detail list
        /// </summary>
        /// <returns>Stores and details</returns>
        private RestResultDto<StoreResponse> GetAllStoresAndDetail()
        {
            var parameters = new List<KeyValuePair<string, string>>();
            var response = new RestResultDto<StoreResponse>();
            //Build the parameters, url, server to set the Rest object
            parameters.Add(new KeyValuePair<string, string>("channel", _config.RakutenStoreChannel));
            

            //Execute the get rest action to retrieve the store list
            _rakutenLog.Debug("Getting stores from the api");
            _rakutenLog.Debug("Server: " + _config.RakutenServerApiUrl);
            _rakutenLog.Debug("Action: " + _config.RakutenStoreApiUrl);
            var etagStored= _storeProcessChecker.GetEtag();
            if (!string.IsNullOrEmpty(etagStored))
            {
                _rakutenLog.Debug($"Etag stored Redis: {etagStored}");
            }
            else
            {
                _rakutenLog.Debug($"No etag stored Redis");
            }
            response = _rakutenStoreRestRepository.GetStore(etagStored);

            if (response?.Result?.Store == null)
            {
                _rakutenLog.Debug("No stores retrieved from the API");
                if (response.StatusCode== HttpStatusCode.NotModified)
                {
                    _rakutenLog.Debug("Response not modified from the API");
                }                
                return response;
            }

            _rakutenLog.Debug($"Stores number retrieved: {response.Result.Store.Count}");

            var etagFromApi = response.Headers.FirstOrDefault(item => item.Name == "ETag");
            var canContinueProcess = response.StatusCode== HttpStatusCode.OK ? _storeProcessChecker.CanContinueImportProcess(etagFromApi):false;
            response.Result.CanContinueProcess = canContinueProcess;
            if (canContinueProcess)
            {
                _rakutenLog.Debug($"Saving etag information in Redis {etagFromApi} ");
                //Save the etag in Sitecore
                _storeProcessChecker.SaveEtagCache(etagFromApi);
                _rakutenLog.Debug("Getting store detail from the api ");
                //Foreach store retrieve its detail
                foreach (var store in response.Result.Store)
                {
                    parameters = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("id", store.Id)
                    };
                    //Build the request with parameters, server, action
                    var restRequestDto = new RestRequestDto
                    {
                        Action = _config.RakutenStoreDetailApiUrl,
                        Parameters = parameters,
                        Server = _config.RakutenServerApiUrl,
                        ParseJson = true
                    };
                    //Execute the rest action to get the store detail
                    var resultStoreDetail = _rakutenStoreRestRepository.GetStoreDetail(restRequestDto);
                    var storeDetail = resultStoreDetail.Result.Store.FirstOrDefault();
                    if (storeDetail != null)
                    {
                        _rakutenLog.Debug($"Store detail retrieved {storeDetail.Id} ");
                        store.Detail = storeDetail;
                    }
                    else
                    {
                        _rakutenLog.Debug($"No store detail from the api. Store id: {store.Id} ");
                    }
                }
            }
            else
            {
                _rakutenLog.Debug("Cannot continue with the import store process");
            }

            return response;
        }

        /// <summary>
        /// Check if the stores will be created or updated
        /// </summary>
        /// <param name="storeResponse">List of stores and details</param>
        private void ProcessStoreInSitecore(RestResultDto<StoreResponse> storeResponse)
        {
            var master = Database.GetDatabase("master");
            var rootItem = master.GetItem(Templates.RakutenStoreParentItem.ID);
            var templateId = new TemplateID(Templates.RakutenStoreItemTemplate.ID);
            var rakutenRewardTemplateId = new TemplateID(Templates.RakutenRewardItemTemplate.ID);
            var mediaUrl = _config.RakutenMediaServerUrl;
            var shoppingUrl = _config.RakutenShoppingBaseUrl;
            var channel = _config.RakutenStoreChannel;
            var partnerId = _config.RakutenPartnerId;
            var seiumbChannelId = _config.RakutenSeiumbStoreChannel;
            var seiumbPartnerId = _config.RakutenSeiumbPartnerId;

            //var allStores = GetAllStoresInBucket(master, Templates.RakutenStoreParentItem.ID, Templates.RakutenStoreItemTemplate.ID);
            Item[] allStores = master.SelectItems("/sitecore/content/MBCShared/Rakuten Stores//*[@@templateid='{26F6C7C8-D74B-474B-A531-4E11F6A07F64}']");
            var allItemsCategories = master.SelectItems("/sitecore/content/MBCShared/Rakuten Categories//*");

            IList<Item> itemsCreated = new List<Item>();
            string storesToBeSkipped = _config.StoresSkipImportProcess;
            var storesToBeSkippedList = storesToBeSkipped.Split('|').ToList();
            foreach (var storeToSkip in storesToBeSkippedList) {
                _rakutenLog.Debug($"Store skipped in Sitecore. Store id: {storeToSkip}  ");
            }
            var itemMatchesNotProcess = storeResponse.Result.Store.Where(item => storesToBeSkippedList.Contains(item.Id)).ToList();
            _rakutenLog.Debug($"Store matches not process :{itemMatchesNotProcess.Count}  ");

            var itemMatchesProcess = storeResponse.Result.Store.Except(itemMatchesNotProcess).ToList();
            _rakutenLog.Debug($"Store matches process :{itemMatchesProcess.Count}  ");
            foreach (var storeItem in itemMatchesProcess)
            {
                //Get the store in Sitecore
                var storeItemSitecore = allStores.FirstOrDefault(item => item[Templates.RakutenStore.Fields.Id] == storeItem.Id);
                //Get the categories to be set in store item category field
                var categoriesItem = GetCategories(allItemsCategories, storeItem);
                //Creation
                if (storeItemSitecore == null) {
                    _rakutenLog.Debug($"Store creation in Sitecore. Store id: {storeItem.Id}  ");
                    //Create the store item in Sitecore
                    var newStoreItem = CreateStoreItemSitecore(rootItem,
                        storeItem,
                        categoriesItem,
                        templateId,
                        mediaUrl,
                        channel,
                        shoppingUrl,
                        partnerId,
                        seiumbChannelId,
                        seiumbPartnerId);
                    //Create the base, global and total rewards
                    CreateRewardItemSitecore(newStoreItem, storeItem.Reward.Base, "Base", rakutenRewardTemplateId);
                    CreateRewardItemSitecore(newStoreItem, storeItem.Reward.Global, "Global", rakutenRewardTemplateId);
                    CreateRewardItemSitecore(newStoreItem, storeItem.Reward.Total, "Total", rakutenRewardTemplateId);
                    itemsCreated.Add(newStoreItem);
                }
                //Update
                else {
                    _rakutenLog.Debug($"Store update in Sitecore. Id: {storeItem.Id}");
                    UpdateStoreItemSitecore(storeItemSitecore,
                        storeItem,
                        categoriesItem,
                        mediaUrl,
                        channel,
                        partnerId,
                        shoppingUrl,
                        seiumbChannelId,
                        seiumbPartnerId);
                    UpdateStoreReward(storeItemSitecore, storeItem.Reward, templateId);
                }
            }
            //Get the stores to be deleted
            var allStoreItemsSitecore = allStores.Union(itemsCreated).ToList();
            var itemMatches = allStoreItemsSitecore.Where(item => storeResponse.Result.Store.Select(x => x.Id).Contains(item[Templates.RakutenStore.Fields.Id])).ToList();
            var itemsToDelete = allStoreItemsSitecore.Except(itemMatches);
            foreach (var storeToDelete in itemsToDelete) {
                _rakutenImportOperation.DeleteItemSitecore(storeToDelete);
            }
            //Publish the stores in Sitecore
            _rakutenImportOperation.PublishItem(rootItem);

        }

        /// <summary>
        /// Get the category field value to be set in Store item in Sitecore
        /// </summary>
        /// <param name="allItemsCategories">All categories</param>
        /// <param name="storeBaseResponse">Object that has the categories retrieved from the Rakuten api</param>
        /// <returns></returns>
        private string GetCategories(Item[] allItemsCategories, StoreBaseResponse storeBaseResponse)
        {
            var categories = storeBaseResponse.Detail.Categories;
            var matchCategories = allItemsCategories.Where(item => categories.Contains(Convert.ToInt32(item[Templates.RakutenCategory.Fields.Id]))).Select(x => x.ID);
            return string.Join("|", matchCategories);
        }

        /// <summary>
        /// Create a new Store in Sitecore
        /// </summary>
        /// <param name="storeParent">Parent item of the Stores in Sitecore</param>
        /// <param name="storeBaseResponse">Store data retrieved from the api</param>
        /// <param name="categoryValue">Category value to be set in Store item field</param>
        /// <returns></returns>
        private Item CreateStoreItemSitecore(Item storeParent, StoreBaseResponse storeBaseResponse, string categoryValue, TemplateID templateId, string mediaUrl, string channel, string shoppingUrl, string partnerId, string seiumbChannelId, string seiumbPartnerId)
        {
            var sanitizedName = ItemUtil.ProposeValidItemName(storeBaseResponse.Name);

            var newItem = storeParent.Add(sanitizedName, templateId);
            if (newItem == null)
            {
                _rakutenLog.Warn($"Could not create store item, name {sanitizedName}, parent {storeParent.ID}");
                return null;
            }
            using (new SecurityDisabler()) {
                using (new EditContext(newItem, false, false)) {
                    SetStoreItemFields(storeBaseResponse,
                        categoryValue,
                        newItem,
                        mediaUrl,
                        channel,
                        partnerId,
                        shoppingUrl,
                        seiumbChannelId, 
                        seiumbPartnerId);
                }
            }

            return newItem;
        }

        /// <summary>
        /// Create a new reward of the Store in Sitecore
        /// </summary>
        /// <param name="store">Store item in Sitecore</param>
        /// <param name="storeDetailRewardBaseResponse">Reward data retrieved from the api</param>
        /// <param name="rewardName">Reward name</param>
        /// <returns></returns>
        private void CreateRewardItemSitecore(Item store, StoreDetailRewardBaseResponse storeDetailRewardBaseResponse, string rewardName, TemplateID templateId)
        {
            if (storeDetailRewardBaseResponse == null)
            {
                _rakutenLog.Debug($"Reward Base created. Store id: {store.ID}");
                return;
            }

            var reward = store.Add(rewardName, templateId);
            if (reward == null)
            {
                _rakutenLog.Warn($"Reward item failed to create, name: {rewardName}, store: {store.ID}");
                return;
            }
            using (new SecurityDisabler()) {
                using (new EditContext(reward, false, false)) {
                    reward[Templates.RakutenReward.Fields.Name] = rewardName;
                    reward[Templates.RakutenReward.Fields.Amount] = storeDetailRewardBaseResponse.Amount.ToString(CultureInfo.InvariantCulture);
                    reward[Templates.RakutenReward.Fields.Display] = storeDetailRewardBaseResponse.Display;
                }
            }
        }

        /// <summary>
        /// Update the reward item in Sitecore
        /// </summary>
        /// <param name="itemToUpdate">Reward item to be updated</param>
        /// <param name="storeDetailRewardBaseResponse">Reward data retrieved from the api</param>
        /// <param name="itemName">Reward name</param>
        private void UpdateAwardItemSitecore(Item itemToUpdate, StoreDetailRewardBaseResponse storeDetailRewardBaseResponse, string itemName)
        {
            using (new SecurityDisabler()) {
                using (new EditContext(itemToUpdate, false, false)) {
                    itemToUpdate[Templates.RakutenReward.Fields.Name] = itemName;
                    itemToUpdate[Templates.RakutenReward.Fields.Amount] = storeDetailRewardBaseResponse.Amount.ToString(CultureInfo.InvariantCulture);
                    itemToUpdate[Templates.RakutenReward.Fields.Display] = storeDetailRewardBaseResponse.Display;
                }
            }
        }

        /// <summary>
        /// Set the Store item fields to be updated in the item in Sitecore
        /// </summary>
        /// <param name="storeBaseResponse">Store data retrieved from the api</param>
        /// <param name="categoryValue">Category value to be updated in the field</param>
        /// <param name="storeItem">Item to be updated in Sitecore</param>
        private void SetStoreItemFields(StoreBaseResponse storeBaseResponse, string categoryValue, Item storeItem, string mediaUrl, string channel, string partnerId, string shoppingBaseUrl, string seiumbChannelId, string seiumbPartnerId)
        {
            try { 
            storeItem[Templates.RakutenStore.Fields.Id] = storeBaseResponse.Id;
            storeItem[Templates.RakutenStore.Fields.Name] = storeBaseResponse.Name;
            storeItem[Templates.RakutenStore.Fields.ShortDescription] = storeBaseResponse.Detail.ShortDescription;
            storeItem[Templates.RakutenStore.Fields.Description] = storeBaseResponse.Detail.Description;
            storeItem[Templates.RakutenStore.Fields.Categories] = categoryValue;
            storeItem[Templates.RakutenStore.Fields.Banner] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.Banner);
            storeItem[Templates.RakutenStore.Fields.SmallLogo] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.SmallLogo);
            storeItem[Templates.RakutenStore.Fields.Thumbnail] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.Thumbnail);
            storeItem[Templates.RakutenStore.Fields.Icon11230] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.Icon11230);
            storeItem[Templates.RakutenStore.Fields.Icon22460] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.Icon22460);
            storeItem[Templates.RakutenStore.Fields.Icon33690] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.Icon33690);
            storeItem[Templates.RakutenStore.Fields.LogoEmail] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.LogoEmail);
            storeItem[Templates.RakutenStore.Fields.LogoMobile] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.LogoMobile);
            storeItem[Templates.RakutenStore.Fields.LogoMobile2] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.LogoMobile2x);
            storeItem[Templates.RakutenStore.Fields.LogoMobile3] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.LogoMobile3x);
            storeItem[Templates.RakutenStore.Fields.FeedSquareLogo] = AddServerUrlIfNotNull(mediaUrl, storeBaseResponse.Detail.Images.FeedSquareLogo);
            
            //calculated

            decimal baseValue;
            decimal globalValue;
            decimal totalValue;
            decimal channelValue = 0;
            var hasChannel = storeBaseResponse.Reward?.Channel != null;
            var hasRange = false;

            if (storeBaseResponse.Reward?.Base?.Range != null) {
                baseValue = (decimal)storeBaseResponse.Reward?.Base?.Range?.High;
                hasRange = true;
            }
            else {
                baseValue = storeBaseResponse.Reward?.Base?.Amount ?? 0;
            }

            if (storeBaseResponse.Reward?.Global?.Range != null)
            {
                globalValue = (decimal)storeBaseResponse.Reward?.Global?.Range?.High;
                hasRange = true;
            }
            else
            {
                globalValue = storeBaseResponse.Reward?.Global?.Amount ?? 0;
            }

            if (storeBaseResponse.Reward?.Total?.Range != null)
            {
                totalValue = (decimal) storeBaseResponse.Reward?.Total?.Range.High;
                hasRange = true;
            }
            else
            {
                totalValue = storeBaseResponse.Reward?.Total?.Amount ?? 0;
            }

            if (storeBaseResponse.Reward?.Channel?.Range != null)
            {
                channelValue = (decimal) storeBaseResponse.Reward?.Channel?.Range.High;
                hasRange = true;
            } else {
                channelValue = storeBaseResponse.Reward?.Channel?.Amount ?? 0;
            }

            var wasValue = baseValue;
            if (hasChannel)
                if (channelValue > globalValue) wasValue = globalValue;

            storeItem[Templates.RakutenStore.Fields.BaseReward] = wasValue.ToString(CultureInfo.InvariantCulture);
            storeItem[Templates.RakutenStore.Fields.TotalReward] = totalValue.ToString(CultureInfo.InvariantCulture);
            storeItem[Templates.RakutenStore.Fields.TierReward] = hasRange ? "1" : "0";
            storeItem[Templates.RakutenStore.Fields.TypeReward] = storeBaseResponse.Reward?.Base?.Display;
            storeItem[Templates.RakutenStore.Fields.ShoppingUrl] = CreateShoppingUrl(storeBaseResponse, shoppingBaseUrl, channel, partnerId);
            storeItem[Templates.RakutenStore.Fields.ShoppingUrlSeiumb] = CreateShoppingUrlSeiumb(storeBaseResponse, shoppingBaseUrl, seiumbChannelId, seiumbPartnerId);
            }
            catch (Exception e)
            {
                _rakutenLog.Error($"Error in store {storeBaseResponse.Id} {storeItem.Name}", e);
                throw;
            }
        }

        private static string AddServerUrlIfNotNull(string serverUrl, string image) { return string.IsNullOrEmpty(image) ? image : $"{serverUrl}{image}"; }

        private static string CreateShoppingUrl(StoreBaseResponse store, string shoppingBaseUrl, string channel, string partnerId)
        {
            return $"{shoppingBaseUrl}{store.ShoppingURL}?sourceId=6991112&sourceName=Web-Desktop-NEA&channel={channel}&epid={partnerId}&ebtoken=";
        }

        private static string CreateShoppingUrlSeiumb(StoreBaseResponse store, string shoppingBaseUrl, string channel, string partnerId)
        {
            return $"{shoppingBaseUrl}{store.ShoppingURL}?sourceId=6991135&sourceName=Web-Desktop-SEIU&channel={channel}&epid={partnerId}&ebtoken=";
        }

        /// <summary>
        /// Update store item in Sitecore
        /// </summary>
        /// <param name="itemToUpdate">Store to be updated</param>
        /// <param name="storeBaseResponse">Store data to be updated in Sitecore</param>
        /// <param name="categoryValue">Category value to be updated</param>
        private void UpdateStoreItemSitecore(Item itemToUpdate, StoreBaseResponse storeBaseResponse, string categoryValue, string mediaUrl, string channel, string partnerId, string shoppingBaseUrl, string seiumbChannelId, string seiumbPartnerId)
        {
            using (new SecurityDisabler()) {
                using (new EditContext(itemToUpdate, false, false)) {
                    SetStoreItemFields(storeBaseResponse,
                        categoryValue,
                        itemToUpdate,
                        mediaUrl,
                        channel,
                        partnerId,
                        shoppingBaseUrl,
                        seiumbChannelId,
                        seiumbPartnerId);
                }
            }
        }

        /// <summary>
        /// Update the reward item in Sitecore
        /// </summary>
        /// <param name="storeItem">Store item in Sitecore</param>
        /// <param name="storeRewardResponse">Reward data retrieved from the api to be updated in Sitecore</param>
        private void UpdateStoreReward(Item storeItem, StoreDetailRewardResponse storeRewardResponse, TemplateID templateId)
        {
            var rewardItems = storeItem.GetChildren();
            if (storeRewardResponse.Base != null)
            {
                UpdateStoreRewardInner(storeRewardResponse.Base, rewardItems, "Base", storeItem, templateId);
            }
            else
            {
                _rakutenLog.Debug($"Reward Base deleted. Store id: {storeItem.ID}");
                DeleteRewardInner(rewardItems, "Base");
            }

            if (storeRewardResponse.Global != null)
            {
                UpdateStoreRewardInner(storeRewardResponse.Global, rewardItems, "Global", storeItem, templateId);
            }
            else
            {
                _rakutenLog.Debug($"Reward Global deleted. Store id: {storeItem.ID}");
                DeleteRewardInner(rewardItems, "Global");
            }

            if (storeRewardResponse.Total != null)
            {
                UpdateStoreRewardInner(storeRewardResponse.Total, rewardItems, "Total", storeItem, templateId);
            }
            else
            {
                _rakutenLog.Debug($"Reward Total deleted. Store id: {storeItem.ID}");
                DeleteRewardInner(rewardItems, "Total");
            }
            //Get the rewards that exist in Sitecore and doesn't exists in the information retrieved from the api
            var rewardsToDelete = rewardItems.Where(item => item.Name != "Base" && item.Name != "Global" && item.Name != "Total");
            foreach (var rewardItemToDelete in rewardsToDelete)
            {
                _rakutenLog.Debug($"Reward deleted {rewardItemToDelete.ID}");
                _rakutenImportOperation.DeleteItemSitecore(rewardItemToDelete);
            }
        }

        /// <summary>
        /// Delete the reward item in Sitecore
        /// </summary>
        /// <param name="rewardItems">Child items of the Store</param>
        /// <param name="rewardName">Reward name</param>
        private void DeleteRewardInner(ChildList rewardItems, string rewardName)
        {
            var rewardItem = rewardItems.FirstOrDefault(item => item.Name == rewardName);
            if (rewardItem != null)
            {
                _rakutenLog.Debug($"Reward deleted. Id: {rewardItem.ID}");
                _rakutenImportOperation.DeleteItemSitecore(rewardItem);
            }
        }

        /// <summary>
        /// Check if the reward will be created or updated
        /// </summary>
        /// <param name="storeRewardResponse"></param>
        /// <param name="rewardItems"></param>
        /// <param name="rewardName"></param>
        /// <param name="storeParentItem"></param>
        private void UpdateStoreRewardInner(
            StoreDetailRewardBaseResponse storeRewardResponse,
            ChildList rewardItems,
            string rewardName,
            Item storeParentItem,
            TemplateID templateId
        )
        {
            var rewardBase = rewardItems.FirstOrDefault(item => item.Name == rewardName);
            if (rewardBase == null)
            {
                CreateRewardItemSitecore(storeParentItem, storeRewardResponse, "Base", templateId);
                _rakutenLog.Debug($"Reward created. Name: {rewardName}. Parent Id: {storeParentItem.ID}");
            }
            else
            {
                UpdateAwardItemSitecore(rewardBase, storeRewardResponse, "Base");
                _rakutenLog.Debug($"Reward updated. Name: {rewardName}. Parent Id: {storeParentItem.ID}");
            }
        }

        private IEnumerable<Item> GetAllStoresInBucket(Database sitecoreDb, ID bucketItemId, ID storeTemplateId)
        {
            var bucketItem = sitecoreDb.GetItem(bucketItemId);
            var indexableItem = new SitecoreIndexableItem(bucketItem);
            using (var context = ContentSearchManager.GetIndex(indexableItem).CreateSearchContext())
            {
                var results = context.GetQueryable<SearchResultItem>().Where(x => x.TemplateId == storeTemplateId).Select(y => y.GetItem());
                return results.ToArray();
            }
        }
        public List<StoreReport> GetAllStoresInSitecore()
        {
            var master = Database.GetDatabase("master");
            Item[] allStores = master.SelectItems("/sitecore/content/MBCShared/Rakuten Stores//*[@@templateid='{26F6C7C8-D74B-474B-A531-4E11F6A07F64}']");

            var stores = new List<StoreReport>();

            foreach (var item in allStores)
                stores.Add(new StoreReport
                {
                    Id = item[Templates.RakutenStore.Fields.Id],
                    SitecoreId = item.ID.ToString(),
                    Name = item[Templates.RakutenStore.Fields.Name],
                    NeambEnable = item[Templates.RakutenStore.Fields.NeambEnable],
                    SeiumbEnable = item[Templates.RakutenStore.Fields.SeiumbEnable]
                });
            return stores;
        }

        public List<StoreReport> EnableStoresFromList(List<string> storesToEnable)
        {
            var stores = new List<StoreReport>();
            var master = Database.GetDatabase("master");
            Item[] allStores = master.SelectItems("/sitecore/content/MBCShared/Rakuten Stores//*[@@templateid='{26F6C7C8-D74B-474B-A531-4E11F6A07F64}']");

            using (new SecurityDisabler())
            {
                foreach (var storeItem in storesToEnable)
                {
                    //Get the store in Sitecore
                    var storeItemSitecore = allStores.FirstOrDefault(item => item.ID.ToString() == storeItem);

                    if (storeItemSitecore == null) continue;
                    using (new EditContext(storeItemSitecore, false, false))
                    {
                        storeItemSitecore[Templates.RakutenStore.Fields.NeambEnable] = "1";
                        stores.Add(new StoreReport
                        {
                            Id = storeItemSitecore[Templates.RakutenStore.Fields.Id],
                            SitecoreId = storeItemSitecore.ID.ToString(),
                            Name = storeItemSitecore[Templates.RakutenStore.Fields.Name],
                            NeambEnable = storeItemSitecore[Templates.RakutenStore.Fields.NeambEnable],
                            SeiumbEnable = storeItemSitecore[Templates.RakutenStore.Fields.SeiumbEnable]
                        });
                    }
                }
            }
            return stores;
        }
    }
}