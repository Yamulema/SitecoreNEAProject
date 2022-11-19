using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
	public class RequestS3: BaseRequestS3
	{
		public string ContentBody { get; set; }
		public string ContentType { get; set; }
		public Stream InputStream { get; set; }
	}
}