using Neambc.Neamb.Feature.GeneralContent.Repositories;
using Neambc.Neamb.Foundation.Configuration.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class TestYourKnowledgeController: BaseController
    {
        private readonly ITestYourKnowledgeRepository _testYourKnowledgeRepository;
        private const string QuizView = "~/Views/Neamb.GeneralContent/TestYourKnowledge/Quiz.cshtml";


        public TestYourKnowledgeController(ITestYourKnowledgeRepository testYourKnowledgeRepository)
        {
            _testYourKnowledgeRepository = testYourKnowledgeRepository;
        }

        public ActionResult TestYourKnowledge()
        {
            var model = _testYourKnowledgeRepository.GetKnownledgeContent();
            
            return View(QuizView, model);
        }
    }
}