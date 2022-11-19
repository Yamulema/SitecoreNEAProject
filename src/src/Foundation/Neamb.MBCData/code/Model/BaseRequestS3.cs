using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
	public class BaseRequestS3
	{
		public string BucketName { get; set; }
		public string Key { get; set; }
		public bool IsEncrypted { get; set; }
	}
}