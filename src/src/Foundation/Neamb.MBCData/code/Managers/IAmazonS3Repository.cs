using System.Collections.Generic;
using Amazon.S3.Model;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
	public interface IAmazonS3Repository {
		bool CreateObjectS3(RequestS3 request);
		T GetObjectS3<T>(BaseRequestS3 request);
		bool DeleteObjectS3(BaseRequestS3 request);
		bool CopyObjectS3(CopyRequestS3 request);
		IEnumerable<S3File> GetFiles(FilesS3 filesS3);
	}
}