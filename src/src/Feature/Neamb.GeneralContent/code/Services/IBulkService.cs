using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Services
{
    public interface IBulkService {
        int RemoveOnClickEvent(string eventName, bool performPublish);
    }
}