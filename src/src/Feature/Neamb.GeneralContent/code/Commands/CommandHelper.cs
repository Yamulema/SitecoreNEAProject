using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Neambc.Neamb.Feature.GeneralContent.Commands
{
	public static class CommandHelper
	{
		public static String GetFullPathWithoutExtension(String path, bool isRedirect) {
			var filename = Path.GetFileNameWithoutExtension(path);
			var parcialPath = path.Replace(Path.GetFileName(path), "");
			if (!isRedirect) {
				parcialPath = parcialPath.Replace("-", " ");
			}
			return (parcialPath + filename);
		}
	}
}