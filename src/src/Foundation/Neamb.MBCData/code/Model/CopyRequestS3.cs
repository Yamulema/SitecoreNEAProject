using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
	public class CopyRequestS3
	{
		public string SourceBucket { get; set; }
		public string SourceKey { get; set; }
		public string DestinationBucket { get; set; }
		public string DestinationKey { get; set; }
	}
}