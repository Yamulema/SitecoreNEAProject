using Amazon.S3;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
	public interface IAmazonS3ClientFactory {
		IAmazonS3 CreateClient();
	}
}