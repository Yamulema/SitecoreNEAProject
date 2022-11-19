using Amazon.S3.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
	public interface IAmazonS3Proxy {
		bool PutObjectS3(PutObjectRequest putRequest);
		GetObjectResponse GetObjectS3(GetObjectRequest request);
		CopyObjectResponse CopyObjectS3(CopyObjectRequest request);
		bool DeleteObjectS3(DeleteObjectRequest request);
		ListObjectsResponse GetListObjects(ListObjectsRequest listObjectsRequest);
	}
}