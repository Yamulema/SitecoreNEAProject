using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.MBCData.Enums;
using ServiceStack.Common.Extensions;

namespace Neambc.Neamb.Foundation.MBCData.Services
{
	public class TokenizationServiceBase {

		#region Fields
		private readonly Regex _tokenRegex;
		private readonly Tuple<Token, string>[] _allTokens;
        #endregion

        #region Constructor
        public TokenizationServiceBase()
			{
            // recompute the invariants
            _tokenRegex = new Regex(@"\[(.*?)\]", RegexOptions.Compiled);
			_allTokens = Enum.GetValues(typeof(Token)).ToList<Token>()
				.Select(x => new Tuple<Token, string>(x, x.GetDescription())).ToArray();
        }
		#endregion

		#region Public Methods
		public string DeTokenize(string rawText) {
			if (string.IsNullOrEmpty(rawText)) {
				return rawText;
			}
			var result = rawText;
			var tokens = GetTokens(rawText)
							.GroupBy(x => x.Item1)
							.Select(x => x.First());
			var mappedTokens = GetMappedTokens(tokens);
            if (mappedTokens != null) {
                foreach (var mappedToken in mappedTokens.Where(x => x.Item2 != Token.None)) {
                    result = result.Replace(mappedToken.Item1, mappedToken.Item3);
                }
            }
            return result;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns>Item 1: Raw token, Item 2: Typed Token</returns>
		private IEnumerable<Tuple<string, Token>> GetTokens(string text) {
			var result = new List<Tuple<string, Token>>();
			var matches = _tokenRegex.Matches(text);
			foreach (Match m in matches) {
				var rawToken = m.Value;
				var rawTokenName = m.Groups[1].Value;
                Tuple<Token, string> token = null;
                if (rawToken.Contains('|')) {
                    rawTokenName = rawToken.Split('|')[0].Remove(0, 1);
                }
                token = _allTokens.FirstOrDefault(x =>
                    x.Item2.Equals(rawTokenName, StringComparison.InvariantCultureIgnoreCase));

                if (token != null) {
					result.Add(new Tuple<string, Token>(rawToken, token.Item1));
				}
			}
			return result;
		}
		#endregion

		/// <summary>
		/// Get Mapped Tokens to be implemented in override method
		/// </summary>
		/// <param name="tokens"></param>
		/// <returns>Item 1: Raw token, Item 2: Typed Token, Item 3: Token value</returns>
		protected virtual IEnumerable<Tuple<string, Token, string>> GetMappedTokens(IEnumerable<Tuple<string, Token>> tokens) {
            return null;
        }
   }
}