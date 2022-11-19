using Neambc.Neamb.Feature.GeneralContent.Models;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class InteractiveQuestionnaireController : BaseController
    {
        #region ActionResult Methods
        public ActionResult InteractiveQuestionnaire()
        {
            InteractiveQuestionnaireDto model = new InteractiveQuestionnaireDto();
            model.Initialize(RenderingContext.Current.Rendering);

            //Fills the model.
            model.QuestionItems = GetInteractiveQuestionItems(model.Item);
            model.NumQuestionItems = model.QuestionItems.Count;

            return View("/Views/Neamb.GeneralContent/InteractiveQuestionnaire/InteractiveQuestionnaire.cshtml", model);
        }
        #endregion

        private List<InteractiveQuestionItem> GetInteractiveQuestionItems(Item datasource)
        {
            List<InteractiveQuestionItem> result = new List<InteractiveQuestionItem>();
            foreach (Item child in datasource.GetChildren())
            {
                InteractiveQuestionItem interactiveQuestionItem = new InteractiveQuestionItem { QuestionItem = child, IsLastQuestion = child.Fields[Templates.InteractiveQuestion.Fields.IsLastQuestion].IsChecked() };
                result.Add(interactiveQuestionItem);
            }           

            return result;

        }

    }
}