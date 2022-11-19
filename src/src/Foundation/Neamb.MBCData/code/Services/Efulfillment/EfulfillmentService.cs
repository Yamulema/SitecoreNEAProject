using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.Efulfillment;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.Efulfillment
{
    [Service(typeof(IEfulfillmentService))]
    public class EfulfillmentService : IEfulfillmentService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IBase64Service _base64Service;
        private readonly IResponseHandler _responseHandler;

        public EfulfillmentService(IAccessTokenService accessTokenService, IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService,
            ILog logService, IBase64Service base64Service, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _base64Service = base64Service;
            _responseHandler = responseHandler;
        }

        public byte[] GetPdfFile(PdfRequest pdfRequest)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid");
            }

            if (pdfRequest == null ||
                pdfRequest.PdMdsid < 1)
            {
                throw new ArgumentException($"Parameters for GetPdfFile are incorrect");
            }
            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlEfulfillment,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new EfulfillmentRequest
                {
                    ProductItemId = pdfRequest.ProductIemId,
                    MdsId = pdfRequest.PdMdsid,
                    Email = pdfRequest.Email,
                    TransactionDate = pdfRequest.PdTransDate,
                    FirstName = pdfRequest.PdFirstName,
                    LastName = pdfRequest.PdLastName,
                    StreetAddress = pdfRequest.PdAddress,
                    City = pdfRequest.PdCity,
                    StateCode = pdfRequest.PdState,
                    ZipCode = pdfRequest.PdZip,
                    Dob = pdfRequest.PdDob,
                    MembershipType = pdfRequest.PdMemberType,
                    Custom1 = pdfRequest.Custom1,
                    Custom2=pdfRequest.Custom2,
                    Custom3= pdfRequest.Custom3,
                    Custom4=pdfRequest.Custom4,
                    Custom5 = pdfRequest.Custom5
                },
                IsBasicAuthentication = false
            };
            var response = _mbcRestfulService.Post<EfulfillmentResponse>(restRequestDto);
            if (response != null) {
                if (response.Success)
                {
                    if (response.Result != null && !response.Result.Success && response.Result.Error != null)
                    {
                        _responseHandler.LogErrorResponse(response.Result.Error, "EfulfillmentService", _logService);
                    } 
                }
                else
                {
                    _logService.Error($"Post Error in Efulfillment Service: {response.StatusCode}, {response.ExceptionDetail}", this);
                }
                if (response.Result != null && response.Result.Data!=null) {
                    return _base64Service.ConvertBytes(response.Result.Data.EncodedString);
                }
                return null;
            } else {
                _logService.Error($"Response is null in Efulfillment Service", this);
                return null;
            }

            
        }
    }
}