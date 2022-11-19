using System;

using System.IO;

using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Services.Efulfillment;
using Neambc.Neamb.Foundation.Product.Interfaces;


namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(IPdfManager))]
    public class PdfManager: IPdfManager
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICacheManagerSeiumb _cacheManagerSeiumb;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IAccountServiceProxy _serviceManager;
        private readonly IAmazonS3Repository _amazonS3Repository;
        private readonly IEfulfillmentService _efulfillmentService;
        public PdfManager(ICacheManager cacheManager, IGlobalConfigurationManager globalConfigurationManager,
            IAccountServiceProxy serviceManager, IAmazonS3Repository amazonS3Repository,
            ICacheManagerSeiumb cacheManagerSeiumb,
            IEfulfillmentService efulfillmentService) {
            _cacheManager = cacheManager;
            _globalConfigurationManager = globalConfigurationManager;
            _serviceManager = serviceManager;
            _amazonS3Repository = amazonS3Repository;
            _cacheManagerSeiumb = cacheManagerSeiumb;
            _efulfillmentService = efulfillmentService;
        }
        /// <summary>
        /// Get the PDF that is download in Efulfillment option from the webservice
        /// </summary>
        /// <param name="materialId">Material Id</param>
        /// <param name="accountUser"></param>
        /// <param name="custom1"></param>
        /// <param name="custom2"></param>
        /// <param name="uniqueName"></param>
        /// <returns>PDF as byte array</returns>
        public byte[] GetPdfFile(
            string materialId,
            string uniqueName,
            PdfRequest pdfRequest,
            string bucketName,
            string custom1 = "",
            string custom2 = "",
            bool isNeamb = true
        )
        {
            byte[] result;
            result = _efulfillmentService.GetPdfFile(pdfRequest);
            if (result != null && result.Length > 0)
            {
                //Generate a unique identifier for the pdf name
                Guid newIdentifier = Guid.NewGuid();
                string nameFilePdf = $"{newIdentifier.ToString("N")}.pdf";
                //Expiration in hours entry in redis
                var expiration = _globalConfigurationManager.ExpirationRedisPdf;
                if (isNeamb) {
                    _cacheManager.StoreInCache<string>(uniqueName, nameFilePdf, DateTime.Now.AddHours(expiration));
                } else {
                    _cacheManagerSeiumb.StoreInCache<string>(uniqueName, nameFilePdf, DateTime.Now.AddHours(expiration));
                }
                Stream stream = new MemoryStream(result);
                var resultSave = SavePdfS3(nameFilePdf, stream,bucketName);
                if (!resultSave)
                {
                    return null;
                }
            }
            return result;

        }
        /// <summary>
        /// Save in S3 the pdf file as stream
        /// </summary>
        /// <param name="identifier">name of the file to be saved in S3</param>
        /// <param name="inputStream">pdf stream</param>
        /// <param name="bucketName">Bucket name in a config file</param>
        /// <returns></returns>
        private bool SavePdfS3(string identifier, Stream inputStream, string bucketName)
        {
            RequestS3 requestS3 = new RequestS3
            {
                BucketName = bucketName,
                Key = identifier,
                ContentType = "application/pdf",
                InputStream = inputStream,
                IsEncrypted = false
            };
            return _amazonS3Repository.CreateObjectS3(requestS3);
        }
        /// <summary>
        /// Return the byte array from S3
        /// </summary>
        /// <param name="uniqueName">key to retrieve the file name from Redis</param>
        /// <param name="bucketName">Bucket name in a config file</param>
        /// <param name="isNeamb">Is for Neamb or Seiumb</param>
        /// <returns></returns>
        public byte[] VerifyExistencePdfFile(string uniqueName,string bucketName, bool isNeamb=true) {
            string fileName = "";
            if (isNeamb) {
                fileName = _cacheManager.RetrieveFromCache<string>(uniqueName);
            } else {
                fileName = _cacheManagerSeiumb.RetrieveFromCache<string>(uniqueName);
            }
            byte[] result = null;
            if (!string.IsNullOrEmpty(fileName))
            {

                BaseRequestS3 baseRequest = new BaseRequestS3
                {
                    BucketName =bucketName,
                    Key = fileName,
                    IsEncrypted = false
                };
                result = _amazonS3Repository.GetObjectS3<byte[]>(baseRequest);
            }
            return result;
        }

        /// <summary>
        /// Build the path to be displayed in the browser for the PDF file
        /// </summary>
        /// <param name="uniqueName">Key to retrieve from redis</param>
        /// <returns>Path pdf file</returns>
        public string GetPdfUrl(string uniqueName, bool isNeamb=true) {
            string fileName = "";
            if (isNeamb) {
                fileName = _cacheManager.RetrieveFromCache<string>(uniqueName);
            } else {
                fileName = _cacheManagerSeiumb.RetrieveFromCache<string>(uniqueName);
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                if (isNeamb) {
                    return $"{_globalConfigurationManager.UrlEfulfillmentS3External}{fileName}";
                } else {
                    return $"{_globalConfigurationManager.UrlEfulfillmentS3SeiumbExternal}{fileName}";
                }
            }
            else
            {
                return null;
            }
        }
    }
}