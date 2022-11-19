using System;
using System.Collections.Generic;
using System.IO;
using Amazon.S3.IO;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
	public interface IAmazonS3Manager
	{
		void CreateFolder(string bucketName, string path);
		void WritingFile(string bucketName, string fileName, MemoryStream filecontent,string filecontesttext, string contenttype, bool encryptName = true);
		byte[] GetFile(string bucketName, string fileName, bool encryptName = true);
		void DeleteAnObject(string bucketName, string fileName);
		bool CopyAnObject(string bucketName, string sourcekey, string destinationkey);
	    void WriteFile(string bucketName, string path, string fileName, object content, S3ContentType contentType, bool encryptName = true);
        T GetFile<T>(string bucketName, string path, string fileName, bool encryptName = true);
	    IEnumerable<S3File> GetFiles(string bucketName, string path, S3ObjectTypeFilter typeFilter);
	}
}