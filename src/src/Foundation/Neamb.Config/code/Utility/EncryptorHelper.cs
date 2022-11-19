using System;
using System.Text;


namespace Neambc.Neamb.Foundation.Configuration.Utility
{
	public static class EncryptorHelper
	{
		/// <summary>
		/// Method to encrypt the user id in Click and Save
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string Mbencode(string value)
		{
			var outstring = new StringBuilder();
			var password = "neambjy";
			int i, j;
			for (i = 0, j = 0; i < value.Length; i++, j++)
			{
				var c = value[i];
				if (j >= password.Length)
				{
					j = 0;
				}
				var tmp = c ^ password[j];
				var sT = tmp.ToString("X");
				if (sT.Length < 2)
				{
					outstring.Append('0');
				}
				outstring.Append(sT);
			}
			return outstring.ToString();
		}
	}
}