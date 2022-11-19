using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.Enums;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
	public class FilesS3:BaseRequestS3
	{
		public S3ObjectTypeFilter TypeFilter { get; set; }
	}
}