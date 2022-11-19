using System;
using NUnit.Framework;
using Oshyn.Framework.UITesting.Info;
using NU=Oshyn.Framework.UITesting.NUnit;

namespace Neambc.UITests.Common {
	public class NunitConfigurator : NU.Configurator {
		public override void AddNUnitOverrides(TestParameters nunitTestParameters, ISettings settings) {
			base.AddNUnitOverrides(nunitTestParameters, settings);
			var env = nunitTestParameters.Get("TargetEnvironment");
			env = env?.ToLower() ?? string.Empty;
			switch (env) {
				case "qa":
					settings.TargetUrl = settings.TargetUrl.Replace("https://www.", "https://qa.");
					break;
				case "prod":
					settings.TargetUrl = settings.TargetUrl.Replace("https://qa.", "https://www.");
					break;
				default:
					//throw new Exception("Nunit variable not set: 'TargetEnvironment'");
					break;
			}
		}
	}
}
