using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Seiumb.Feature.GeneralContent.Models;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.GeneralContent.Controllers
{
    public class ChatController : BaseController
    {
        private readonly ITokenizationServiceSeiumb _tokenizationService;

        public ChatController(ITokenizationServiceSeiumb tokenizationService)
        {
            _tokenizationService = tokenizationService;
        }
        // GET: Chat
        public override ActionResult Index()
        {
            var model = new Chat();
            model.DetokenizeChat = DetokenizeChat;
            model.Initialize(RenderingContext.Current.Rendering);
            return View("/Views/Seiumb.GeneralContent/Chat/Chat.cshtml", model);
        }

        private string DetokenizeChat(string rawText)
        {
            return _tokenizationService.DeTokenize(rawText);
        }
    }
}