using System;
using System.IO;
using System.Xml.Serialization;

namespace UI_NEAMB.DTO {
		public class DtoFactory<T> : IDtoFactory<T> where T : class {

			#region Fields
			private readonly Type _type;
			private readonly XmlSerializer _serializer;
			#endregion

			#region Properties
			/// <summary>
			/// See <see cref="IDtoFactory{T}.RootPath"/>
			/// </summary>
			public string RootPath { get; }
			#endregion

			#region Constructors
			public DtoFactory(string rootPath = null) {
				_type = typeof(T);
				RootPath = rootPath ?? GetRootPath();
				_serializer = new XmlSerializer(_type);
			}
			#endregion

			#region Private Methods
			private string GetRootPath() {
				return Path.GetFullPath(Path.GetDirectoryName(_type.Assembly.Location) ?? string.Empty);
			}
			private string GetFullPath(string filename) {
				filename = filename ?? throw new ArgumentNullException(nameof(filename));
				return Path.IsPathRooted(filename)
					? filename
					: Path.GetFullPath(Path.Combine(RootPath, filename));
			}
			#endregion

			#region Public Methods
			/// <summary>
			/// See <see cref="IDtoFactory{T}.SaveToFile(string, T)"/>
			/// </summary>
			public T LoadFromFile(string filename) {
				using (var reader = new FileStream(GetFullPath(filename), FileMode.Open)) {
					return _serializer.Deserialize(reader) as T;
				}
			}
			/// <summary>
			/// See <see cref="IDtoFactory{T}.SaveToFile(string, T)"/>
			/// </summary>
			public string SaveToFile(string filename, T val) {
				val = val ?? throw new ArgumentNullException(nameof(val));
				var ret = GetFullPath(filename);
				using (var writer = new StreamWriter(ret)) {
					_serializer.Serialize(writer, val);
					writer.Flush();
					writer.Close();
				}
				return ret;
			}
			#endregion

		}
	}
