using System;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Caching;
using SD = Sitecore.Diagnostics;

namespace Neambc.Seiumb.Foundation.Sitecore {
	/// <summary>
	/// Implementor for ILog that directs to Sitecore.Diagnostics
	/// </summary>
	[Service(typeof(ILog))]
	public class Log : ILog {
		public bool Enabled => SD.Log.Enabled;
		public bool IsDebugEnabled => SD.Log.IsDebugEnabled;
		public ICache Singles => SD.Log.Singles;
		public void Audit(string message, object owner) {
			SD.Log.Audit(message, owner);
		}

		public void Audit(string message, string loggerName) {
			SD.Log.Audit(message, loggerName);
		}

		public void Audit(string message, Type ownerType) {
			SD.Log.Audit(message, ownerType);
		}

		public void Audit(Type ownerType, string format, params string[] parameters) {
			SD.Log.Audit(ownerType, format, parameters);
		}

		public void Audit(object owner, string format, params string[] parameters) {
			SD.Log.Audit(owner, format, parameters);
		}

		public void Debug(string message, object owner) {
			SD.Log.Debug(message, owner);
		}

		public void Debug(string message) {
			SD.Log.Debug(message);
		}

		public void Debug(string message, string loggerName) {
			SD.Log.Debug(message, loggerName);
		}

		public void Error(string message, object owner) {
			SD.Log.Error(message, owner);
		}

		public void Error(string message, Type ownerType) {
			SD.Log.Error(message, ownerType);
		}

		public void Error(string message, Exception exception, object owner) {
			SD.Log.Error(message, exception, owner);
		}

		public void Error(string message, Exception exception, Type ownerType) {
			SD.Log.Error(message, exception, ownerType);
		}

		public void Error(string message, Exception exception, string loggerName) {
			SD.Log.Error(message, exception, loggerName);
		}

		public void Fatal(string message, object owner) {
			SD.Log.Fatal(message, owner);
		}

		public void Fatal(string message, Type ownerType) {
			SD.Log.Fatal(message, ownerType);
		}

		public void Fatal(string message, string loggerName) {
			SD.Log.Fatal(message, loggerName);
		}

		public void Fatal(string message, Exception exception, object owner) {
			SD.Log.Fatal(message, exception, owner);
		}

		public void Fatal(string message, Exception exception, Type ownerType) {
			SD.Log.Fatal(message, exception, ownerType);
		}

		public void Info(string message, object owner) {
			SD.Log.Info(message, owner);
		}

		public void Info(string message, string loggerName) {
			SD.Log.Info(message, loggerName);
		}

		public void SingleError(string message, object owner) {
			SD.Log.SingleError(message, owner);
		}

		public void SingleFatal(string message, Exception exception, object owner) {
			SD.Log.SingleFatal(message, exception, owner);
		}

		public void SingleFatal(string message, Exception exception, Type ownerType) {
			SD.Log.SingleFatal(message, exception, ownerType);
		}

		public void SingleWarn(string message, object owner) {
			SD.Log.SingleWarn(message, owner);
		}

		public void Warn(string message, object owner) {
			SD.Log.Warn(message, owner);
		}

		public void Warn(string message, Exception exception, object owner) {
			SD.Log.Warn(message, exception, owner);
		}

		public void Warn(string message, Exception exception, string loggerName) {
			SD.Log.Warn(message, exception, loggerName);
		}
	}
}