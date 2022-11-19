using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Neambc.Neamb.Foundation.Configuration.Utility
{
	public static class StringExtension
	{
		public static string GetName(this string str, string path)
		{
			var extIndex = str.LastIndexOf('.');
			return extIndex == -1
				? str.Replace(path, string.Empty)
					.Replace("\\", string.Empty)
					.Replace("/", string.Empty)
				: str.Substring(0, extIndex)
					.Replace(path, string.Empty)
					.Replace("\\", string.Empty)
					.Replace("/", string.Empty);
		}

		public static string GetExtension(this string str)
		{
			var extIndex = str.LastIndexOf('.') + 1;
			if (extIndex < str.Length)
			{
				return extIndex == -1 ? string.Empty : str.Substring(extIndex, str.Length - extIndex);
			}
			else
			{
				return string.Empty;
			}
		}

		public static string CalculateMD5Hash(this string str)
		{
			// step 1, calculate MD5 hash from input

			var md5 = MD5.Create();
			var inputBytes = Encoding.ASCII.GetBytes(str);
			var hash = md5.ComputeHash(inputBytes);
			// step 2, convert byte array to hex string
			var sb = new StringBuilder();
			for (var i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("x2"));
			}
			return sb.ToString();
		}
	}
}