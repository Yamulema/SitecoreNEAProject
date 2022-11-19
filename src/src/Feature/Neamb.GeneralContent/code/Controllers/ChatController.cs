using Neambc.Neamb.Feature.GeneralContent.Models;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Feature.GeneralContent.Repositories;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class ChatController : BaseController
    {
        private readonly IChatRepository _ChatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _ChatRepository = chatRepository;
        }
        public ActionResult Chat()
        {
            return View("/Views/Neamb.GeneralContent/Chat/Chat.cshtml", CreateModel());
        }

        private ChatDTO CreateModel()
        {
            var chatDTO = _ChatRepository.GetChatContent();

            return chatDTO;
        }


    }
}