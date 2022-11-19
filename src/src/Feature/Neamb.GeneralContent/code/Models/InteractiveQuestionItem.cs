using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class InteractiveQuestionItem
    {
        public Item QuestionItem { get; set; }
        public bool IsLastQuestion { get; set; }        
    }
}