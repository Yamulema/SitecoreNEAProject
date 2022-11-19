using System;
using System.Collections.Generic;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Caching;

//  Intended for promotion to the Oshyn.Framework.UnitTest project
namespace Neambc.UnitTesting.Base.Fakes {
	/// <summary>
	/// ILog Implementation for Unit Tests. Allows for all method parameters to be captured
	/// </summary>
	public class FakeLog : ILog {

		/// <summary>
		/// Bag of any value the ILog interface could record
		/// </summary>
		public class LogEntry {
			public string EntryType;
			public Exception LoggedException;
			public string Message;
			public object Owner;
			public Type OwnerType;
			public string LoggerName;
			public string FormatString;
			public string[] FormatStringParameters;
			public bool IsSingle;
		}

		#region Properties
		public const string AUDIT = "audit";
		public const string ERROR = "error";
		public const string FATAL = "fatal";
		public const string WARN = "warn";
		public const string INFO = "info";
		public const string DEBUG = "debug";
		public readonly List<LogEntry> Entries;
		#endregion

		#region Constructor
		public FakeLog() {
			Entries = new List<LogEntry>();
		}
		#endregion

		#region Public Methods : ILog

		public bool Enabled {
			get; set;
		}
		public bool IsDebugEnabled {
			get; set;
		}
		public ICache Singles {
			get; set;
		}

		public void Audit(string message, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = AUDIT,
				Message = message,
				Owner = owner
			});
		}

		public void Audit(string message, string loggerName) {
			Entries.Add(new LogEntry() {
				EntryType = AUDIT,
				Message = message,
				LoggerName = loggerName
			});
		}

		public void Audit(string message, Type ownerType) {
			Entries.Add(new LogEntry() {
				EntryType = AUDIT,
				Message = message,
				OwnerType = ownerType
			});
		}

		public void Audit(Type ownerType, string format, params string[] parameters) {
			Entries.Add(new LogEntry() {
				EntryType = AUDIT,
				FormatString = format,
				FormatStringParameters = parameters
			});
		}

		public void Audit(object owner, string format, params string[] parameters) {
			Entries.Add(new LogEntry() {
				EntryType = AUDIT,
				Owner = owner,
				FormatString = format,
				FormatStringParameters = parameters
			});
		}

		public void Debug(string message, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = DEBUG,
				Message = message,
				Owner = owner
			});
		}

		public void Debug(string message) {
			Entries.Add(new LogEntry() {
				EntryType = DEBUG,
				Message = message
			});
		}

		public void Debug(string message, string loggerName) {
			Entries.Add(new LogEntry() {
				EntryType = DEBUG,
				Message = message,
				LoggerName = loggerName
			});
		}

		public void Error(string message, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = ERROR,
				Message = message,
				Owner = owner
			});
		}

		public void Error(string message, Type ownerType) {
			Entries.Add(new LogEntry() {
				EntryType = ERROR,
				Message = message,
				OwnerType = ownerType
			});
		}

		public void Error(string message, Exception exception, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = ERROR,
				LoggedException = exception,
				Owner = owner
			});
		}

		public void Error(string message, Exception exception, Type ownerType) {
			Entries.Add(new LogEntry() {
				EntryType = ERROR,
				Message = message,
				LoggedException = exception,
				OwnerType = ownerType
			});
		}

		public void Error(string message, Exception exception, string loggerName) {
			Entries.Add(new LogEntry() {
				EntryType = ERROR,
				Message = message,
				LoggedException = exception,
				LoggerName = loggerName
			});
		}

		public void Fatal(string message, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = FATAL,
				Message = message,
				Owner = owner
			});
		}

		public void Fatal(string message, Type ownerType) {
			Entries.Add(new LogEntry() {
				EntryType = FATAL,
				Message = message,
				OwnerType = ownerType
			});
		}

		public void Fatal(string message, string loggerName) {
			Entries.Add(new LogEntry() {
				EntryType = FATAL,
				Message = message,
				LoggerName = loggerName
			});
		}

		public void Fatal(string message, Exception exception, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = FATAL,
				Message = message,
				LoggedException = exception,
				Owner = owner
			});
		}

		public void Fatal(string message, Exception exception, Type ownerType) {
			Entries.Add(new LogEntry() {
				EntryType = FATAL,
				Message = message,
				LoggedException = exception,
				OwnerType = ownerType
			});
		}

		public void Info(string message, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = INFO,
				Message = message,
				Owner = owner
			});
		}

		public void Info(string message, string loggerName) {
			Entries.Add(new LogEntry() {
				EntryType = INFO,
				Message = message,
				LoggerName = loggerName
			});
		}

		public void SingleError(string message, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = ERROR,
				Message = message,
				Owner = owner,
				IsSingle = true
			});
		}

		public void SingleFatal(string message, Exception exception, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = FATAL,
				Message = message,
				Owner = owner,
				IsSingle = true
			});
		}

		public void SingleFatal(string message, Exception exception, Type ownerType) {
			Entries.Add(new LogEntry() {
				EntryType = FATAL,
				Message = message,
				OwnerType = ownerType,
				IsSingle = true
			});
		}

		public void SingleWarn(string message, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = WARN,
				Message = message,
				Owner = owner,
				IsSingle = true
			});
		}

		public void Warn(string message, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = WARN,
				Message = message,
				Owner = owner
			});
		}

		public void Warn(string message, Exception exception, object owner) {
			Entries.Add(new LogEntry() {
				EntryType = WARN,
				Message = message,
				LoggedException = exception,
				Owner = owner
			});
		}

		public void Warn(string message, Exception exception, string loggerName) {
			Entries.Add(new LogEntry() {
				EntryType = WARN,
				Message = message,
				LoggedException = exception,
				LoggerName = loggerName
			});
		}
		#endregion
	}
}
