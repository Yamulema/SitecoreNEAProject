using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class TestYourKnowledge: IRenderingModel
    {
        public Rendering Rendering { get; set; }

        public Item Item { get; set; }
        
        public Item PageItem { get; set; }

        public string HeadLine { get; set; }

        public string Question { get; set; }

        public string SubmitButton { get; set; }

        public LinkContentField ConfirmationLink { get; set; }

        public string Option1 { get; set; }

        public string Option2 { get; set; }

        public string Option3 { get; set; }

        public string Option4 { get; set; }

        public string Option5 { get; set; }

        public string Option6 { get; set; }

        public string Option7 { get; set; }

        public string Option8 { get; set; }

        public string Option9 { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }

        public TestYourKnowledge()
        {
            ConfirmationLink = new LinkContentField();
        }
    }

    public class LinkContentField
    {
        public string LinkType { get; set; }

        public string Value { get; set; }
    }
}