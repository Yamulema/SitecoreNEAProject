using Neambc.Neamb.Feature.GeneralContent.Enums;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class VideoDto : IRenderingModel
    {
        private const string PLATFORMVIDEO = "https://content.jwplatform.com/players/";
        public string SourceUrl { get; set; }
        public VideoSourceType Type { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }

        public static VideoSourceType GetVideoType(Item item)
        {
            var video = item.Fields[Templates.LightBoxVideo.Fields.Video]?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(video))
            {
                return VideoSourceType.None;
            }
            return video.Contains(PLATFORMVIDEO) ? VideoSourceType.JWPlayer : VideoSourceType.YouTube;
        }

        public static string GetVideoUrl(Item item)
        {
            return item.Fields[Templates.LightBoxVideo.Fields.Video]?.Value ?? string.Empty;
        }
    }
}