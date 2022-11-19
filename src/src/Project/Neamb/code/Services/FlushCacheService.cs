using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Project.Web.Enums;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;
using Sitecore.Resources.Media;

namespace Neambc.Neamb.Project.Web.Services
{
    [Service(typeof(IFlushCacheService))]
    public class FlushCacheService : IFlushCacheService
    {
        private readonly ICdnService _cdnService;
        private readonly ICacheManager _cacheManager;
        public bool IsEnabled { get; set; } = Configuration.EnableFlushCacheService;
        public FlushCacheService(ICdnService cdnService, ICacheManager cacheManager)
        {
            _cdnService = cdnService;
            _cacheManager = cacheManager;
        }
        public void Process(PublishItemContext publishItemContext) {
            Item item = null;

            if (!IsEnabled)
                return;

            if (publishItemContext.Result.Operation == PublishOperation.None || 
                publishItemContext.Result.Operation == PublishOperation.Skipped)
                return;
            Log.Debug($"FlushCacheService Pipeline - PublishOperation Success ", this);
            if (publishItemContext.Action == PublishAction.PublishSharedFields) {
                item = publishItemContext.PublishHelper.GetSourceItem(publishItemContext.ItemId);
            } else {
                item = publishItemContext.VersionToPublish;
            }
            if (item != null)
            {
                Process(item);
            } else {
                Log.Error($"FlushCacheService item: {publishItemContext.ItemId}'",this);
            }
        }
        private void Process(Item item) {
            var contentType = GetContentType(item);

            if (contentType.HasFlag(ContentType.Media)) {
                Log.Debug($"FlushCacheService Pipeline - Process HasFlag Media: " + ContentType.Media, this);
                Log.Debug($"FlushCacheService Pipeline - Process EnableCdnInvalidation: " + Configuration.EnableCdnInvalidation, this);
                if (Configuration.EnableCdnInvalidation) {                   
                    FlushMedia(item);                   
                }
            }
                
            if (contentType.HasFlag(ContentType.ContentFilter))
                FlushContentFilter(item);
        }
        private ContentType GetContentType(Item item)
        {
            var result = ContentType.None;
            if (item.Paths.IsMediaItem)
                result |= ContentType.Media;
            if (item.Template.ID == Templates.ContentFilter.ID ||
                item.Template.BaseTemplates.Any(x => x.ID == Templates.ContentFilter.ID))
                result |= ContentType.ContentFilter;
            return result;
        }
        private void FlushMedia(Item item)
        {
            Log.Debug($"FlushCacheService Pipeline - FlushMedia started", this);
            var path = MediaManager.GetMediaUrl(item, new MediaUrlOptions()
            {
                AlwaysIncludeServerUrl = false
            });
            Log.Debug($"FlushCacheService Pipeline - FlushMedia path: " + path, this);
            _cdnService.InvalidateAsync(new List<string> {
                path
            });
            Log.Debug($"FlushCacheService Pipeline - FlushMedia finished", this);

        }
        private void FlushContentFilter(Item item)
        {
            var pattern = $"ContentFilter:{item.ID.Guid.ToString()}_*";
            var keys = _cacheManager.SearchPatternInCache(pattern);
            foreach (var key in keys)
                _cacheManager.Remove(key);
        }
    }
}