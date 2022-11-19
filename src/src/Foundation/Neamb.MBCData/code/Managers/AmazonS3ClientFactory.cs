using Amazon.S3;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
	[Service(typeof(IAmazonS3ClientFactory))]
	public class AmazonS3ClientFactory: IAmazonS3ClientFactory {
		public IAmazonS3 CreateClient() {
			return new AmazonS3Client();
		}
	}
}