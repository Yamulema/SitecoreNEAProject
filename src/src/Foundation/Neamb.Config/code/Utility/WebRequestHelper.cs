using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace Neambc.Neamb.Foundation.Config.Utility {
	public class WebRequestHelper {

		#region Fields
		private readonly WebRequest _request;
		private Stream _dataStream;
		#endregion

		#region Properties
		public string Status { get; set; }
		#endregion

		#region Constructor
		public WebRequestHelper(string url) {
			// Create a request using a URL that can receive a post.

			_request = WebRequest.Create(url);
		}

		public WebRequestHelper(string url, string method)
			: this(url) {

			if (method.Equals("GET") || method.Equals("POST")) {
				// Set the Method property of the request to POST.
				_request.Method = method;
			} else {
				throw new Exception("Invalid Method Type");
			}
		}

		public WebRequestHelper(string url, string method, string data, NameValueCollection nameValueCollection=null)
			: this(url, method) {
			// Create POST data and convert it to a byte array.
			var postData = data;
			var byteArray = Encoding.UTF8.GetBytes(postData);

			ServicePointManager.Expect100Continue = true;

			ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3
				| System.Net.SecurityProtocolType.Tls12
				| SecurityProtocolType.Tls11
				| SecurityProtocolType.Tls;

			// Set the ContentType property of the WebRequest.
			_request.ContentType = "application/x-www-form-urlencoded";

            if (nameValueCollection != null) {
                _request.Headers.Add(nameValueCollection);
            }
            // Set the ContentLength property of the WebRequest.
			_request.ContentLength = byteArray.Length;

			// Get the request stream.
			_dataStream = _request.GetRequestStream();

			// Write the data to the request stream.
			_dataStream.Write(byteArray, 0, byteArray.Length);

			// Close the Stream object.
			_dataStream.Close();

		}
		#endregion

		#region Public Methods

		public string GetResponse() {
			// Get the original response.
			var response = _request.GetResponse();

			Status = ((HttpWebResponse)response).StatusDescription;

			// Get the stream containing all content returned by the requested server.
			_dataStream = response.GetResponseStream();

			// Open the stream using a StreamReader for easy access.
			var reader = new StreamReader(_dataStream);

			// Read the content fully up to the end.
			var responseFromServer = reader.ReadToEnd();

			// Clean up the streams.
			reader.Close();
			_dataStream.Close();
			response.Close();

			return responseFromServer;
		}
		#endregion

	}
}
