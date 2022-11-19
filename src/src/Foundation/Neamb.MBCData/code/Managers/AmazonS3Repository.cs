using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.S3.Model;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model;
using Newtonsoft.Json;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
    [Service(typeof(IAmazonS3Repository))]
    public class AmazonS3Repository : IAmazonS3Repository {
		#region Fields

		private readonly IAmazonS3Proxy _proxy;

		#endregion
		#region constructor
		public AmazonS3Repository(IAmazonS3Proxy proxy) {
            _proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Create an object in Amazon S3
		/// </summary>
		/// <param name="request">Data to be set in Amazon S3 object</param>
		public bool CreateObjectS3(RequestS3 request)
		{
			var putRequest = new PutObjectRequest
			{
				BucketName = request.BucketName,
				Key = request.IsEncrypted?request.Key.CalculateMD5Hash():request.Key,
				InputStream = request.InputStream,
				ContentBody = request.ContentBody,
				ContentType = request.ContentType
			};
			return _proxy.PutObjectS3(putRequest);
		}
		/// <summary>
		/// Get an object from Amazon S3
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="request">Object data</param>
		/// <returns></returns>
		public T GetObjectS3<T>(BaseRequestS3 request)
		{
			Type typeByteArray = typeof(byte[]);
			Type typeParameterType = typeof(T);
			byte[] bytesFile = null;
			var getRequest = new GetObjectRequest()
			{
				BucketName = request.BucketName,
				Key = request.IsEncrypted ? request.Key.CalculateMD5Hash() : request.Key
			};
			var response = _proxy.GetObjectS3(getRequest);
            if (response!=null && response.ResponseStream != null) {
                using (var responseStream = response.ResponseStream) {
                    bytesFile = ReadStream(responseStream);
                    if (typeParameterType == typeByteArray) {
                        return (T) Convert.ChangeType(bytesFile, typeof(T));
                    } else {
                        var strFile = Encoding.Default.GetString(bytesFile);
                        return JsonConvert.DeserializeObject<T>(strFile);
                    }
                }
            } else {
                return default(T);
            }
        }
		/// <summary>
		/// Delete an object from Amazon S3
		/// </summary>
		/// <param name="request">Data to identify the object in S3</param>
		public bool DeleteObjectS3(BaseRequestS3 request)
		{
			var deleteObjectRequest = new DeleteObjectRequest
			{
				BucketName = request.BucketName,
				Key = request.IsEncrypted ? request.Key.CalculateMD5Hash() : request.Key,
			};
			return _proxy.DeleteObjectS3(deleteObjectRequest);
		}

		/// <summary>
		/// Copy an object in S3 from a source to a destination
		/// </summary>
		/// <param name="request">Data to identify the object in S3</param>
		public bool CopyObjectS3(CopyRequestS3 request)
		{
			var copyObjectRequest = new CopyObjectRequest()
			{
				SourceBucket = request.SourceBucket,
				SourceKey = request.SourceKey.CalculateMD5Hash(),
				DestinationBucket = request.DestinationBucket,
				DestinationKey = request.DestinationKey.CalculateMD5Hash()
			};
			var response= _proxy.CopyObjectS3(copyObjectRequest);
			return response != null;
		}
		/// <summary>
		/// Get some objects from Amazon S3
		/// </summary>
		/// <param name="filesS3">Information of the objects to retrieve</param>
		/// <returns></returns>
		public IEnumerable<S3File> GetFiles(FilesS3 filesS3)
		{
            List<S3File> resultFiles = new List<S3File>();
            var s3Objects = new List<S3Object>();
			var listObjectsRequest = new ListObjectsRequest()
			{
				BucketName = filesS3.BucketName,
				Prefix = filesS3.Key
			};
			do
			{
				var response = _proxy.GetListObjects(listObjectsRequest);
				if (response != null)
				{
					if (filesS3.TypeFilter.HasFlag(S3ObjectTypeFilter.File))
					{
						s3Objects.AddRange(response.S3Objects.Where(x => x.Size > 0));
					}
					if (filesS3.TypeFilter.HasFlag(S3ObjectTypeFilter.Folder))
					{
						s3Objects.AddRange(response.S3Objects.Where(x => x.Size == 0));
					}
                    if (response.IsTruncated)
                    {
                        listObjectsRequest.Marker = response.NextMarker;
                    }
                    else
					{
						listObjectsRequest = null;
					}
				} else {
                    listObjectsRequest = null;
                }
			} while (listObjectsRequest != null);

            foreach (var s3ObjectItem in s3Objects) {
                var s3File = new S3File {
                    Key = s3ObjectItem.Key,
                    Name = s3ObjectItem.Key.GetName(filesS3.Key),
                    Extension = s3ObjectItem.Key.GetExtension()
                };
                var request =new BaseRequestS3 {
                    BucketName = filesS3.BucketName,
                    Key = s3ObjectItem.Key
                };

                s3File.Content = GetObjectS3<byte[]>(request);
                resultFiles.Add(s3File);
            }
            return resultFiles;
            
        }
		#endregion
		#region Private methods
		private byte[] ReadStream(Stream responseStream) {
			var buffer = new byte[16 * 1024];
			using (var ms = new MemoryStream()) {
				int read;
				while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0) {
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
      #endregion
	}
}