using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Neambc.Neamb.Foundation.Configuration.Utility
{
	public class HttpError500Result : HttpStatusCodeResult
	{
		public HttpError500Result()
			: this(null)
		{
		}

		public HttpError500Result(string statusDescription)
			: base(HttpStatusCode.InternalServerError, statusDescription)
		{
		}
	}
}