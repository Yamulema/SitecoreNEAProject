using System;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	[Service(typeof(ICaptchaManager))]
	public class CaptchaManager : ICaptchaManager {
		private readonly IGlobalConfigurationManager _globalConfigurationManager;

		public CaptchaManager(IGlobalConfigurationManager globalConfigurationManager) {
			_globalConfigurationManager = globalConfigurationManager;
		}


		public bool ExecutePostRecaptcha(string captchaVerification, string captchaSecret) {
			var initialurl = _globalConfigurationManager.CatpchaUrl;
			Log.Info($"Captcha url: {initialurl} ", this);

			var parameters = $"secret={captchaSecret}&response={captchaVerification}";
			Log.Info($"Captcha parameters: {parameters} ", this);
			var response = new CaptchaResponse();
			try {
				var myRequest = new WebRequestHelper(initialurl, "POST", parameters);
				var responseJson = myRequest.GetResponse();
				response = JsonConvert.DeserializeObject<CaptchaResponse>(responseJson);
				if (response != null) {
					Log.Info($"Captcha response retrieved successfully: {response.success} ", this);
				} else {
					Log.Info($"Captcha response null ", this);
				}
			} catch (Exception ex) {
				Log.Error(this + "Error calling post recaptcha", ex, this);
			}
			return response.success;
		}
	}
}