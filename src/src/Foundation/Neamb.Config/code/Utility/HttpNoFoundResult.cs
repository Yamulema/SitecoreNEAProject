using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Neambc.Neamb.Foundation.Configuration.Utility
{
	public class HttpNoFoundResult : HttpStatusCodeResult
	{
		public HttpNoFoundResult()
			: this(null)
		{
		}

		public HttpNoFoundResult(string statusDescription)
			: base(HttpStatusCode.NotFound, statusDescription)
		{
		}
	}
}