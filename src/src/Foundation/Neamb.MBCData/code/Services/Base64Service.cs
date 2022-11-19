using System;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Sitecore;
using Log = Sitecore.Diagnostics.Log;

namespace Neambc.Neamb.Foundation.MBCData.Services
{
	/// <summary>
	/// Redirects all exceptions raised by System.Convert To/From base64 to an ILog implementation
	/// </summary>
	[Service(typeof(IBase64Service))]
	public class Base64Service : IBase64Service {

		#region Fields
		protected readonly ILog _log;
		#endregion

		#region Constructors
		public Base64Service(ILog log) {
			_log = log ?? throw new ArgumentNullException(nameof(log));
		}
		#endregion

		#region Public Methods
		public string Encode(string plain) {
			try {
				var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plain);
				return Convert.ToBase64String(plainTextBytes);
			} catch (Exception e) {
				Log.Error($"Error while encoding:{e}", this);
				return string.Empty;
			}
		}

		public string Decode(string encoded) {
			try {
				var base64EncodedBytes = Convert.FromBase64String(encoded);
				return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
			} catch (Exception e) {
                Log.Error($"Error while decoding", this);
                Log.Error($"{e}", this);
				return string.Empty;
			}
		}
        /// <summary>
        /// Convert image in byte[] to a string in base 64.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        public string EncodeImage(byte[] inputFile)
        {
            try
            {
                //Build the image url
                return $"data:image/png;base64,{Convert.ToBase64String(inputFile)}";
            }
            catch (Exception e)
            {
                _log.Error($"Error while encoding:{e}", e, this);
                return null;
            }
        }

        public byte[] ConvertBytes(string encoded) {
            try
            {
                var base64 = encoded.Substring(encoded.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            return Convert.FromBase64String(base64);
            }
            catch (Exception e)
            {
                _log.Error($"Error converting to bytes in Base64Service:{e}", e, this);
                return null;
            }
        }
        #endregion
    }
}