using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Project.Web.Services;
using Sitecore.Diagnostics;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Neambc.Neamb.Project.Web.Pipelines
{
    public class PublishItem : PublishItemProcessor
    {
        private readonly IFlushCacheService _flushCacheService;
        public PublishItem(IFlushCacheService flushCacheService) {
            _flushCacheService = flushCacheService;
        }
        public override void Process(PublishItemContext context) {
            try {
                //Flush cached Items
                _flushCacheService.Process(context);
            }
            catch (Exception e) {
                Log.Error("Error while running PublishItem pipeline.", e, this);
            }
        }
    }
}