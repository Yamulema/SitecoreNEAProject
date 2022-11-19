using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	[Service(typeof(IAmazonS3Manager))]
	public class AmazonS3Manager : IAmazonS3Manager {
		private IAmazonS3 client;

		/// <summary>
		/// Create a folder in S3
		/// </summary>
		/// <param name="bucketName">Bucket name</param>
		/// <param name="path">path/folder to be created</param>
		public void CreateFolder(string bucketName, string path) {
			using (client = new AmazonS3Client()) {
				var putRequest = new PutObjectRequest {
					BucketName = bucketName,
					Key = path,
				};
				putRequest.InputStream = new MemoryStream();
				client.PutObject(putRequest);
			}
		}
		/// <summary>
		/// Upload a file in S3
		/// </summary>
		/// <param name="bucketName">Bucket name</param>
		/// <param name="fileName">File name to be created/updated</param>
		/// <param name="filecontent">File content as Memorystream</param>
		/// <param name="contenttype">Mime type</param>
		/// <param name="encryptName">Flag to encrypt the file name</param>
		public void WritingFile(string bucketName, string fileName, MemoryStream filecontent, string filecontesttext, string contenttype, bool encryptName = true) {
			using (client = new AmazonS3Client()) {
				//Create the object with bucket name, file name and rewrite option
				var putRequest = new PutObjectRequest {
					BucketName = bucketName,
					Key = encryptName ? CalculateMD5Hash(fileName) : fileName,
					//CannedACL = S3CannedACL.PublicReadWrite
				};

				if (!string.IsNullOrEmpty(filecontesttext)) {
					putRequest.ContentBody = filecontesttext;
				} else if (filecontent != null) {
					//Set the content as image

					putRequest.ContentType = contenttype;
					filecontent.Seek(0, SeekOrigin.Begin);
					putRequest.InputStream = filecontent;
				}




				client.PutObject(putRequest);
			}
		}

		/// <summary>
		/// Retrieve an object from S3
		/// </summary>
		/// <param name="bucketName">Bucket name</param>
		/// <param name="fileName">File name to be retrieved</param>
		/// <returns>File as byte []</returns>
		public byte[] GetFile(string bucketName, string fileName, bool encryptName = true) {
			byte[] bytesFile = null;
			//using (client = new AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.USEast1))
			using (client = new AmazonS3Client()) {
				var request = new GetObjectRequest() {
					BucketName = bucketName,
					Key = encryptName ? CalculateMD5Hash(fileName) : fileName,
				};
				try {
					using (var response = client.GetObject(request)) {
						using (var responseStream = response.ResponseStream) {
							bytesFile = ReadStream(responseStream);
						}
					}
				} catch (AmazonS3Exception e) {
					Log.Info($"The key {fileName} doesn't exist in bucket {bucketName}", this);
					Log.Error("Error in GetAnImage", e, this);
				}
			}
			return bytesFile;
		}

		/// <summary>
		/// Delete an object from S3
		/// </summary>
		/// <param name="bucketName">Bucket name</param>
		/// <param name="fileName">File name</param>
		public void DeleteAnObject(string bucketName, string fileName) {
			using (client = new AmazonS3Client()) {
				var request = new DeleteObjectRequest() {
					BucketName = bucketName,
					Key = CalculateMD5Hash(fileName)
				};

				try {
					client.DeleteObject(request);
				} catch (AmazonS3Exception e) {
					Log.Info($"The key {fileName} doesn't exist in bucket {bucketName}", this);
					Log.Error("Error in DeleteAnObject", e, this);
				}
			}
		}

		/// <summary>
		/// Copy an object in a bucket with another key
		/// </summary>
		/// <param name="bucketName"></param>
		/// <param name="sourcekey"></param>
		/// <param name="destinationkey"></param>
		/// <returns></returns>
		public bool CopyAnObject(string bucketName, string sourcekey, string destinationkey) {
			var result = false;
			using (client = new AmazonS3Client()) {
				var request = new CopyObjectRequest() {
					SourceBucket = bucketName,
					SourceKey = CalculateMD5Hash(sourcekey),
					DestinationBucket = bucketName,
					DestinationKey = CalculateMD5Hash(destinationkey),
					//CannedACL = S3CannedACL.PublicRead
				};

				try {
					client.CopyObject(request);
					result = true;
				} catch (AmazonS3Exception e) {
					Log.Info($"The key {sourcekey} doesn't exist in bucket {bucketName}", this);
					Log.Error("Error in CopyAnObject", e, this);
				}
			}

			return result;
		}

		/// <summary>
		/// Convert the stream to a byte[]
		/// </summary>
		/// <param name="responseStream">Input stream</param>
		/// <returns>File as byte[]</returns>
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

		/// <summary>
		/// Calculate the MD5 Hash to a string
		/// </summary>
		/// <param name="input">input string</param>
		/// <returns></returns>
		private string CalculateMD5Hash(string input) {
			// step 1, calculate MD5 hash from input

			var md5 = MD5.Create();
			var inputBytes = Encoding.ASCII.GetBytes(input);
			var hash = md5.ComputeHash(inputBytes);
			// step 2, convert byte array to hex string
			var sb = new StringBuilder();
			for (var i = 0; i < hash.Length; i++) {
				sb.Append(hash[i].ToString("x2"));
			}
			return sb.ToString();
		}

		/// <summary>
		/// Serializes a provided object into a JSON and writes it into a file in S3.
		/// </summary>
		/// <param name="bucketName"></param>
		/// <param name="path"></param>
		/// <param name="fileName"></param>
		/// <param name="content"></param>
		/// <param name="contentType"></param>
		/// <param name="encryptName"></param>
		public void WriteFile(string bucketName, string path, string fileName, object content, S3ContentType contentType, bool encryptName = true) {
			//TODO: Move this logic to Generic WriteFile method.
			#region Refactor
			var contenttype = string.Empty;
			switch (contentType) {
				case S3ContentType.None:
					contenttype = string.Empty;
					break;
				case S3ContentType.TextPlain:
					contenttype = "text/plain";
					break;
				case S3ContentType.ImagePng:
					contenttype = "image/png";
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
			}
			#endregion

			var serializedObject = JsonConvert.SerializeObject(content);
			var key = $"{path}/{fileName}";
			WritingFile(bucketName, key, null, serializedObject, contenttype, false);
		}

		public T GetFile<T>(string bucketName, string path, string fileName, bool encryptName = true) {
			var fullPath = $"{path}/{fileName}";
			var s3File = GetFile(bucketName, fullPath, false);
			if (s3File == null) {
				return default(T);
			}
			var strFile = Encoding.Default.GetString(s3File);
			return JsonConvert.DeserializeObject<T>(strFile);
		}

		public IEnumerable<S3File> GetFiles(string bucketName, string path, S3ObjectTypeFilter typeFilter) {
			try {
				using (client = new AmazonS3Client()) {
					var s3Objects = new List<S3Object>();
					var request = new ListObjectsRequest() {
						BucketName = bucketName,
						Prefix = path
					};
					do {
						var response = client.ListObjects(request);
						if (typeFilter.HasFlag(S3ObjectTypeFilter.File)) {
							s3Objects.AddRange(response.S3Objects.Where(x => x.Size > 0));
						}
						if (typeFilter.HasFlag(S3ObjectTypeFilter.Folder)) {
							s3Objects.AddRange(response.S3Objects.Where(x => x.Size == 0));
						}
						if (response.IsTruncated) {
							request.Marker = response.NextMarker;
						} else {
							request = null;
						}
					} while (request != null);

					return s3Objects.Select(x => new S3File() {
						Key = x.Key,
						Name = GetName(x.Key, path),
						Extension = GetExtension(x.Key),
						Content = GetFile(bucketName, $"{x.Key}", false)
					});

					//var dir = new S3DirectoryInfo(client, bucketName, GetFilePath(path));
					//var files = dir.GetFileSystemInfos();
					//return files.Select(x => new S3File()
					//{
					//    Key = x.Name,
					//    Name = GetName(x.Name),
					//    Extension = x.Extension,
					//    FullName = x.FullName,
					//    IsFile = x.Type == FileSystemType.File,
					//    Content = GetFile(bucketName, $"{path}/{x.Name}", false)
					//});
				}
			} catch (Exception e) {
				Log.Error($"Error Getting files in bucket:{bucketName} with path:{path}", e, this);
				return null;
			}
		}
		/// <summary>
		/// S3 GetFiles uses backslash as folder separator.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private string GetFilePath(string path) {
			return path.Replace("/", "\\");
		}
		private string GetName(string fileKey, string path) {
			//TODO: Refactor this  to use regex.
			var extIndex = fileKey.LastIndexOf('.');
			return extIndex == -1
				? fileKey.Replace(path, string.Empty)
					.Replace("\\", string.Empty)
					.Replace("/", string.Empty)
				: fileKey.Substring(0, extIndex)
					.Replace(path, string.Empty)
					.Replace("\\", string.Empty)
					.Replace("/", string.Empty);
		}
		private string GetExtension(string fileKey) {
			//TODO: Refactor this  to use regex.
			var extIndex = fileKey.LastIndexOf('.') + 1;
			if (extIndex < fileKey.Length) {
				return extIndex == -1 ? string.Empty : fileKey.Substring(extIndex, fileKey.Length - extIndex);
			} else {
				return string.Empty;
			}
		}
	}
}