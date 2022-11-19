using System;
using Amazon.S3.Model;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	[Service(typeof(IAmazonS3Proxy))]
	public class AmazonS3Proxy: IAmazonS3Proxy
	{
		#region Fields

		private readonly ILog _log;
		private readonly IAmazonS3ClientFactory _clientFactory;

		#endregion

		#region Constructor
		public AmazonS3Proxy(IAmazonS3ClientFactory clientFactory, ILog log)
		{
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _log = log ?? throw new ArgumentNullException(nameof(log));
		}
		#endregion

		#region Public methods
		public bool PutObjectS3(PutObjectRequest putRequest) {
            string key = putRequest != null ? putRequest.Key : "";
            try {
				using (var clientS3 = _clientFactory.CreateClient()) {
                    
                    var response = clientS3.PutObject(putRequest);
                    if (response != null) {
                        return true;
                    } else {
                        
                        _log.Warn($"Put Object S3 {key} was not inserted response null", this);
                        return false;
                    }
                }
			} catch (Exception ex) {
				_log.Error($"Put Object S3 {key}", ex, this);
				return false;
			}
		}

		public GetObjectResponse GetObjectS3(GetObjectRequest request) {
			GetObjectResponse getObjectResponse= null;
            string key = request != null ? request.Key : "";
            try {
				using (var clientS3 = _clientFactory.CreateClient()) {
					getObjectResponse = clientS3.GetObject(request);
				}
			} catch (Exception ex) {
				_log.Debug($"Get Object S3 {key} {ex.Message}",  this);
			}
			return getObjectResponse;
		}

		public CopyObjectResponse CopyObjectS3(CopyObjectRequest request) {
			CopyObjectResponse copyObjectResponse= null;
            string sourceKey = request != null ? request.SourceKey : "";
            string destinationKey = request != null ? request.DestinationKey : "";
            try {
				using (var clientS3 = _clientFactory.CreateClient()) {
					copyObjectResponse = clientS3.CopyObject(request);
				}
			} catch (Exception ex) {
				_log.Error($"Copy Object S3 {sourceKey} {destinationKey}", ex, this);
			}
			return copyObjectResponse;
		}

		public bool DeleteObjectS3(DeleteObjectRequest request) {
            string key = request != null ? request.Key : "";
            try
			{
				using (var clientS3 = _clientFactory.CreateClient())
				{
					clientS3.DeleteObject(request);
                    return true;
				}
			}
			catch (Exception ex)
			{
				_log.Error($"Delete Object S3 {key}", ex, this);
				return false;
			}
		}

		public ListObjectsResponse GetListObjects(ListObjectsRequest listObjectsRequest) {
			ListObjectsResponse response = null;
			try
			{
				using (var clientS3 = _clientFactory.CreateClient())
				{
					response = clientS3.ListObjects(listObjectsRequest);
				}
			}
			catch (Exception ex)
			{
				_log.Error("List Objects S3", ex, this);
			}
			return response;
		}
		#endregion
	}
}