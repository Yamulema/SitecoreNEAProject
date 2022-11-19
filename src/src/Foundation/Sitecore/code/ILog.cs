using System;
using Sitecore.Caching;

// Maps to Type: Sitecore.Diagnostics.Log
// Assembly: Sitecore.Kernel, Version=10.0.0.0, Culture=neutral, PublicKeyToken=null
// Package: Sitecore.Kernel.NoReferences.8.2.170728\lib\NET452\Sitecore.Kernel.dll
namespace Neambc.Seiumb.Foundation.Sitecore {

	/// <summary>
	/// Allows for log injection to be mapped to different implementors
	/// </summary>
	public interface ILog {
		bool Enabled {
			get;
		}
		bool IsDebugEnabled {
			get;
		}
		ICache Singles {
			get;
		}
		void Audit(string message, object owner);
		void Audit(string message, string loggerName);
		void Audit(string message, Type ownerType);
		void Audit(Type ownerType, string format, params string[] parameters);
		void Audit(object owner, string format, params string[] parameters);
		void Debug(string message, object owner);
		void Debug(string message);
		void Debug(string message, string loggerName);
		void Error(string message, object owner);
		void Error(string message, Type ownerType);
		void Error(string message, Exception exception, object owner);
		void Error(string message, Exception exception, Type ownerType);
		void Error(string message, Exception exception, string loggerName);
		void Fatal(string message, object owner);
		void Fatal(string message, Type ownerType);
		void Fatal(string message, string loggerName);
		void Fatal(string message, Exception exception, object owner);
		void Fatal(string message, Exception exception, Type ownerType);
		void Info(string message, object owner);
		void Info(string message, string loggerName);
		void SingleError(string message, object owner);
		void SingleFatal(string message, Exception exception, object owner);
		void SingleFatal(string message, Exception exception, Type ownerType);
		void SingleWarn(string message, object owner);
		void Warn(string message, object owner);
		void Warn(string message, Exception exception, object owner);
		void Warn(string message, Exception exception, string loggerName);
	}
}