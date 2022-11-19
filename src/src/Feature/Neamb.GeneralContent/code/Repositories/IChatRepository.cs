using Neambc.Neamb.Feature.GeneralContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Repositories
{
    public interface IChatRepository
    {
        ChatDTO GetChatContent();
    }
}