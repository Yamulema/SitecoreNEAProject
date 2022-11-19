using System;
using System.Collections.Specialized;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System.Linq;
using Neambc.Neamb.Foundation.MBCData.Services.SecurityManagement;
using Neambc.Neamb.Foundation.MBCData.Enums;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(ILinkActionTypeManager))]
	public class LinkActionTypeManager : ILinkActionTypeManager {
		private readonly IEligibilityManager _eligibilityManager;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IAccountServiceProxy _accountServiceProxy;
		private readonly ISecurityService _securityService;
        private readonly ITokenManager _tokenManager;

        public LinkActionTypeManager(IEligibilityManager eligibilityManager,
			ISessionAuthenticationManager sessionAuthenticationManager, IAccountServiceProxy accountServiceProxy,
			ISecurityService securityService, ITokenManager tokenManager) {
			_accountServiceProxy = accountServiceProxy;
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_eligibilityManager = eligibilityManager;
			_securityService = securityService;
            _securityService = securityService;
            _tokenManager = tokenManager;
        }
        /// <summary>
        /// this method will check to see if the value is not null. if null will remove all the params.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string RemoveAllEmptyParameters(string url)
        {
            //split param/value with question mark
            string[] separateURL = url.Split('?');
			string result = url;

			if (separateURL.Length == 2) {
				//All the parameters passing the Querystring part.
				NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[1]);
				//check if any value is null or empty
				foreach (var keyItem in queryString.AllKeys) {
					var valPar = queryString[keyItem];
					if (String.IsNullOrEmpty(valPar.Trim())) {
						queryString.Remove(keyItem);
					}
				}
				result = String.IsNullOrEmpty(queryString.ToString()) ? separateURL[0] : separateURL[0] + "?" + queryString.ToString();
			} 
			return result;
        }

        public OperationResult GetUrlLink(LinkModel linkModel) {
            
            var operationResult = new OperationResult{PostData = linkModel.PostData};

            var db = Sitecore.Context.Database;
			var contextItem = db.GetItem(new ID(linkModel.ContextItem));

			var eligibility = linkModel.EligibilityItemId!=ID.Null.ToString() && contextItem.Fields[new ID(linkModel.EligibilityItemId)].IsChecked();

			var resultEligibility = EligibilityResultEnum.None;

			if (eligibility) {
				resultEligibility = _eligibilityManager.IsMemberEligible(linkModel.AccountUser.Mdsid, linkModel.ProductCodeLink);
			}

			if ((eligibility && resultEligibility == EligibilityResultEnum.Eligible) || !eligibility) {
				var url = contextItem.LinkFieldUrl(new ID(linkModel.CtaLinkItemId));
				url = ReplaceToken(url, linkModel.AccountUser.Mdsid,linkModel.PassthrougData);
				operationResult.Url = url.Trim();
                Log.Debug($"Url link {linkModel.AccountUser.Mdsid} {operationResult.Url}",this);
			} else {
				operationResult.ResultUrl = ResultUrlEnum.UnForbidden;
			}

			return operationResult;
		}

		public string ReplaceToken(string url, string mdsid, PassthroughModel passthrougData) {
            string cellCode="";
            string campcode = "";
            string medium = "";
            string sob = "";
            string gclid = "";
            string utmTerm = "";

            //Process PassthroughModel or Session variables
            if (passthrougData != null) {
                cellCode = passthrougData.UtmSource;
                campcode = passthrougData.UtmCampaign;
                medium = passthrougData.UtmMedium;
                sob = passthrougData.Sob;
                gclid = passthrougData.Gclid;
                utmTerm = passthrougData.UtmTerm;
            } else {
                cellCode = _sessionAuthenticationManager.GetCellCode();
                campcode = _sessionAuthenticationManager.GetCampaignCode();
                medium = _sessionAuthenticationManager.GetMedium();
                sob = _sessionAuthenticationManager.GetSob();
                gclid = _sessionAuthenticationManager.GetGclid();
                utmTerm = _sessionAuthenticationManager.GetUtmTerm();
            }
			if (url.Contains("[mdsid]")) {
				Log.Debug($"Starting [mdsid] encryption value: {mdsid}", this);
				var result = _accountServiceProxy.EncryptPartner(mdsid, "mercer");
				url = url.Replace("[mdsid]", HttpUtility.UrlEncode(result));
			} else if (url.Contains("[mdsid_mercer]")) {
				Log.Debug($"Starting [mdsid_mercer] encryption value: {mdsid}", this);
				var result = _accountServiceProxy.EncryptPartner(mdsid, "mercer");
				url = url.Replace("[mdsid_mercer]", HttpUtility.UrlEncode(result));
			} else if (url.Contains("[materialid]")) {

				if (long.TryParse(mdsid, out var mdsidLong)) {
					Log.Debug($"Starting [materialid] encryption value: {mdsidLong}", this);
					url = url.Replace("[materialid]", mdsidLong.ToString());
				}
			} else if (url.Contains("[mdsid_clear]")) {

				if (long.TryParse(mdsid, out var mdsidLong)) {
					Log.Debug($"Starting [mdsid_clear] encryption value: {mdsidLong}", this);
					url = url.Replace("[mdsid_clear]", mdsidLong.ToString());
				}
			} else if (url.Contains("[mdsid_afinium]")) {
				Log.Debug($"Starting [mdsid_afinium] encryption value: {mdsid}", this);

				var result = _accountServiceProxy.EncryptPartner(mdsid, "afinium");
                Log.Debug($"Url encrypted {result}",this);
				url = url.Replace("[mdsid_afinium]", HttpUtility.UrlEncode(result));
			}
            else if (url.Contains("[mercer]"))
            {
                Log.Debug($"Starting [mercer] encryption value: {mdsid}", this);
                var result = _securityService.AesEncrypt(mdsid, Token.Mercer);
                Log.Debug($"Url encrypted {result}", this);
                url = url.Replace("[mercer]", result);
            }
            else if (url.Contains("[afinium]"))
            {
                Log.Debug($"Starting [afinium] encryption value: {mdsid}", this);
                var result =_securityService.AesEncrypt(mdsid, Token.Afinium);
                if (string.IsNullOrEmpty(result)) {
                    throw new ApplicationException($"Error with afinium encryption process Neamb");
                }
                Log.Debug($"Url encrypted {result}", this);
                url = url.Replace("[afinium]", result);
            }

            if (url.Contains("[cellcode]")) {
				url = url.Replace("[cellcode]", HttpUtility.UrlEncode(cellCode));
            }

			if (url.Contains("[campcode]")) {
				url = url.Replace("[campcode]", HttpUtility.UrlEncode(campcode));
            }
            if (url.Contains("[medium]"))
            {
                url = url.Replace("[medium]", HttpUtility.UrlEncode(medium));
            }
            var parametersUrl = GetQueryParameters(url);
            
            if (url.Contains("sob"))
            {
                if (!string.IsNullOrEmpty(sob) && url.Contains("[sob]")) {
                    url = url.Replace("[sob]", HttpUtility.UrlEncode(sob));
                } else {
                    if (url.StartsWith("[sob"))
                    {
                        if (!string.IsNullOrEmpty(sob))
                        {
                            url = url.Replace(url, HttpUtility.UrlEncode(sob));
                        } else {
                            var defaultvaluegclidDepured = FindDefaultValueParameter(url);

                            url = url.Replace(url, HttpUtility.UrlEncode(defaultvaluegclidDepured));
                        }
                    } else {
                        var defaultvaluesobRaw = FindParameter(parametersUrl, "[sob");
                        if (!string.IsNullOrEmpty(defaultvaluesobRaw)) {
                            if (!string.IsNullOrEmpty(sob)) {
                                url = url.Replace(defaultvaluesobRaw, HttpUtility.UrlEncode(sob));
                            } else {
                                var defaultvaluesobDepured = FindDefaultValueParameter(defaultvaluesobRaw);

                                url = url.Replace(defaultvaluesobRaw, HttpUtility.UrlEncode(defaultvaluesobDepured));
                            }
                            
                        }
                    }
                }
            }
            if (url.Contains("gclid"))
            {
                if (!string.IsNullOrEmpty(gclid) && url.Contains("[gclid]")) {
                    url = url.Replace("[gclid]", HttpUtility.UrlEncode(gclid));
                }
                else
                {
                    if (url.StartsWith("[gclid")) {
                        if (!string.IsNullOrEmpty(gclid))
                        {
                            url = url.Replace(url, HttpUtility.UrlEncode(gclid));
                        } else {
                            var defaultvaluegclidDepured = FindDefaultValueParameter(url);

                            url = url.Replace(url, HttpUtility.UrlEncode(defaultvaluegclidDepured));
                        }
                    } else {
                        var defaultvaluegclidRaw = FindParameter(parametersUrl, "[gclid");
                        if (!string.IsNullOrEmpty(defaultvaluegclidRaw)) {
                            if (!string.IsNullOrEmpty(gclid))
                            {
                                url = url.Replace(defaultvaluegclidRaw, HttpUtility.UrlEncode(gclid));
                            } else {
                                var defaultvaluegclidDepured = FindDefaultValueParameter(defaultvaluegclidRaw);

                                url = url.Replace(defaultvaluegclidRaw, HttpUtility.UrlEncode(defaultvaluegclidDepured));
                            }
                        }
                    }
                }
            }
            if (url.Contains("term"))
            {
                if (!string.IsNullOrEmpty(utmTerm) && url.Contains("[term]")) {
                    url = url.Replace("[term]", HttpUtility.UrlEncode(utmTerm));
                }
                else
                {
                    if (url.StartsWith("[term"))
                    {
                        if (!string.IsNullOrEmpty(utmTerm))
                        {
                            url = url.Replace(url, HttpUtility.UrlEncode(utmTerm));
                        } else {
                            var defaultvaluegclidDepured = FindDefaultValueParameter(url);

                            url = url.Replace(url, HttpUtility.UrlEncode(defaultvaluegclidDepured));
                        }
                    } else {
                        var defaultvaluetermRaw = FindParameter(parametersUrl, "[term");

                        if (!string.IsNullOrEmpty(defaultvaluetermRaw)) {
                            if (!string.IsNullOrEmpty(utmTerm))
                            {
                                url = url.Replace(defaultvaluetermRaw, HttpUtility.UrlEncode(utmTerm));
                            } else {
                                var defaultvaluetermDepured = FindDefaultValueParameter(defaultvaluetermRaw);

                                url = url.Replace(defaultvaluetermRaw, HttpUtility.UrlEncode(defaultvaluetermDepured));
                            }
                        }
                    }

                }
            }
            if (url.Contains("[NCESID]"))
            {
                url = url.Replace("[NCESID]", _tokenManager.GetNcesId());
            }
            url = RemoveAllEmptyParameters(url);

            return url;
		}
        private NameValueCollection GetQueryParameters(string dataWithQuery)
        {
            NameValueCollection result = new NameValueCollection();
            string[] parts = dataWithQuery.Split('?');
            if (parts.Length > 0)
            {
                string queryParameter = parts.Length > 1 ? parts[1] : parts[0];
                if (!string.IsNullOrEmpty(queryParameter))
                {
                    string[] p = queryParameter.Split('&');
                    foreach (string s in p)
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(temp[0], temp[1]);
                        }
                        else
                        {
                            result.Add(s, string.Empty);
                        }
                    }
                }
            }
            return result;
        }
        private string FindParameter(NameValueCollection allParameters, string parameterToFind) {
            var keys= allParameters.AllKeys;
            foreach (var key in keys) {
                var valueParameter = allParameters[key];
                if (valueParameter.StartsWith(parameterToFind)) {
                    return valueParameter;
                }
            }
            return "";
        }
        private string FindDefaultValueParameter(string parameter)
        {
            var valueParameter = parameter.Split('|');
            if (valueParameter.Length == 2)
            {
                return valueParameter[1].Remove(valueParameter[1].Length - 1); 
            }
            else
            {
                return "";
            }
        }
    }
}