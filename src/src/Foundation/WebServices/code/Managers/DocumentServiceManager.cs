using System;
using System.ServiceModel;
using Neambc.Seiumb.Foundation.WebServices.Model;

namespace Neambc.Seiumb.Foundation.WebServices.Managers {
	public static class DocumentServiceManager {
		private static readonly int SERVICE_TIMEOUT = int.Parse(Sitecore.Configuration.Settings.GetSetting("ServiceTimeOut"));

		public static byte[] GetPdf(PdfRequest pdfRequest) {
			byte[] response = null;
			var client = new PdfFactoryService.PdfFactoryPortTypeClient();
			try {
				client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, SERVICE_TIMEOUT);
				client.Endpoint.Address = new EndpointAddress(Sitecore.Configuration.Settings.GetSetting("PDFFactoryEndPoint"));
				response = client.getPdf(pdfRequest.ProductIemId, pdfRequest.PdDescription, pdfRequest.PdTransDate, pdfRequest.PdFirstName,
					pdfRequest.PdLastName, pdfRequest.PdDob, pdfRequest.PdMdsid, pdfRequest.PdAddress, pdfRequest.PdCity, pdfRequest.PdState,
					pdfRequest.PdZip, pdfRequest.PdMemberType, pdfRequest.Custom1, pdfRequest.Custom2, pdfRequest.Custom3, pdfRequest.Custom4,
					pdfRequest.Custom5);
				client.Close();
			} catch (TimeoutException ex) {
				Sitecore.Diagnostics.Log.Error(ex.Message, nameof(DocumentServiceManager));
				client.Abort();
			} catch (CommunicationException ex) {
				Sitecore.Diagnostics.Log.Error(ex.Message, nameof(DocumentServiceManager));
				client.Abort();
			}
			return response;
		}

	}
}