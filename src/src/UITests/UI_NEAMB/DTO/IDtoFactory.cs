using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_NEAMB.DTO {

		/// <summary>
		/// Implementations of this class are able to serialize objects to/from files
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public interface IDtoFactory<T> where T : class {

			/// <summary>
			/// The default root path to use for reading/writing files if one is not given in the arguments.
			/// </summary>
			string RootPath { get; }

			/// <summary>
			/// Retrieves
			/// </summary>
			/// <param name="filename">Required.  If path is rooted, it is used directly, if not it is appended to the <see cref="RootPath"/> property</param>
			/// <returns>A non-null object matching the T type</returns>
			T LoadFromFile(string filename);

			/// <summary>
			/// Serialized a given object to a file
			/// </summary>
			/// <param name="filename">Required.  If path is rooted, it is used directly, if not it is appended to the <see cref="RootPath"/> property</param>
			/// <param name="val">Required.  The object to serialize</param>
			/// <returns>The full path to the written file</returns>
			string SaveToFile(string filename, T val);
		}
	}
