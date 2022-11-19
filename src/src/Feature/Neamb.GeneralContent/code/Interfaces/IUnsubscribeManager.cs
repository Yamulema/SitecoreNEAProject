using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.GeneralContent.Enums;
using Neambc.Neamb.Feature.GeneralContent.Models;

namespace Neambc.Neamb.Feature.GeneralContent.Interfaces
{
    public interface IUnsubscribeManager
	{
		void UnsubscribeList(int listid, string mdsid, UnsubscribeDTO unsubscribeModel);
	}
}