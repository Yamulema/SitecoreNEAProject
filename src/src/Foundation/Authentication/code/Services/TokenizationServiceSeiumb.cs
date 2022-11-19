using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.Pipelines.RenderField;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neambc.Seiumb.Foundation.Authentication.Services
{
    [Service(typeof(ITokenizationServiceSeiumb))]
    public class TokenizationServiceSeiumb : TokenizationServiceBase, ITokenizationServiceSeiumb
    {
        private readonly ISeiumbProfileManager _seiumbProfileManager;

        public TokenizationServiceSeiumb(ISeiumbProfileManager seiumbProfileManager)
        {
            _seiumbProfileManager = seiumbProfileManager;
        }

        #region Protected Methods
        /// <summary>
        /// Available tokens for Seiumb
        /// </summary>
        /// <param name="tokens">Tokens to be replaced in the string</param>
        /// <returns>Item 1: Raw token, Item 2: Typed Token, Item 3: Token value</returns>
        protected override IEnumerable<Tuple<string, Token, string>> GetMappedTokens(IEnumerable<Tuple<string, Token>> tokens)
        {
            return tokens.Select(x =>
            {
                var mappedValue = string.Empty;
                switch (x.Item2)
                {
                    case Token.FirstName:
                        mappedValue = GetName();
                        break;
                    case Token.LastName:
                        mappedValue = GetLastName();
                        break;
                    case Token.Phone:
                        mappedValue = GetPhone();
                        break;
                    case Token.Email:
                        mappedValue = GetEmail();
                        break;
                    case Token.StateCode:
                        mappedValue = GetStateCode();
                        break;
                    case Token.mdsid_clear:
                        mappedValue = GetMdsidClear();
                        break;
                    case Token.mdsid_clear_int:
                        mappedValue = GetMdsidClearInt();
                        break;
                    default:
                        mappedValue = string.Empty;
                        break;
                }
                return new Tuple<string, Token, string>(x.Item1, x.Item2, mappedValue);
            });
        }

        /// <summary>
        /// Replace token [FirstName]
        /// </summary>
        /// <returns>String replaced</returns>
		private string GetName()
        {
            var seiumbProfile = _seiumbProfileManager.GetProfile();
            return seiumbProfile?.FirstName ?? string.Empty;
        }

        /// <summary>
        /// Replace token LastName
        /// </summary>
        /// <returns></returns>
        private string GetLastName()
        {
            var seiumbProfile = _seiumbProfileManager.GetProfile();
            return seiumbProfile?.LastName ?? string.Empty;
        }


        /// <summary>
        /// Replace token Phone
        /// </summary>
        /// <returns></returns>
        private string GetPhone()
        {
            var seiumbProfile = _seiumbProfileManager.GetProfile();
            return seiumbProfile?.Phone ?? string.Empty;
        }

        /// <summary>
        /// Replace token Email
        /// </summary>
        /// <returns></returns>
        private string GetEmail()
        {
            var seiumbProfile = _seiumbProfileManager.GetProfile();
            return seiumbProfile?.Email ?? string.Empty;
        }

        /// <summary>
        /// Replace token StateCode
        /// </summary>
        /// <returns></returns>
        private string GetStateCode()
        {
            var seiumbProfile = _seiumbProfileManager.GetProfile();
            return seiumbProfile?.StateCode ?? string.Empty;
        }

        /// <summary>
        /// Replace token [mdsid_clear]
        /// </summary>
        /// <returns>String replaced</returns>
        private string GetMdsidClear()
        {
            var seiumbProfile = _seiumbProfileManager.GetProfile();
            return seiumbProfile?.MdsId ?? string.Empty;
        }

        /// <summary>
        /// Replace token mdsid_clear_int
        /// </summary>
        /// <returns></returns>
        private string GetMdsidClearInt()
        {
            var seiumbProfile = _seiumbProfileManager.GetProfile();
            return seiumbProfile?.MdsIdInt.ToString() ?? string.Empty;
        }
        #endregion

        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            return DeTokenize(input);
        }
    }
}