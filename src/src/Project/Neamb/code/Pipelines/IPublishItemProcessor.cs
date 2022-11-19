using Sitecore.Publishing.Pipelines.PublishItem;

namespace Neambc.Neamb.Project.Web.Pipelines
{
    public interface IPublishItemProcessor {
        void Process(PublishItemContext context);
        bool IsEnabled { get; set; }
    }
}