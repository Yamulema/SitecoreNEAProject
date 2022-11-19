using System;
using System.Globalization;
using System.Web;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Convert = System.Convert;


namespace Neambc.Neamb.Feature.GeneralContent.Personalization {
	/// <summary>
	/// This criteria must allow content authors to compare against the value on the "New member" attribute on an identified user profile
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RangeTimeRule<T> : StringOperatorCondition<T> where T : RuleContext {
		/// <summary>
		/// Gets or sets hour from
		/// </summary>
		public string HourFrom {
			get; set;
		}

        /// <summary>
        /// Gets or sets minute from
        /// </summary>
        public string MinuteFrom
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets hour to
        /// </summary>
        public string HourTo
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets minute to
        /// </summary>
        public string MinuteTo
        {
            get; set;
        }

        /// <summary>
		/// Main execute method.
		/// </summary>
		/// <param name="ruleContext">Rule context.</param>
		/// <returns>True or false.</returns>
		protected override bool Execute(T ruleContext) {
            try {
                int hourFromReferenced = 0;
                var selectedItemHourFrom = Context.Database.GetItem(HourFrom);
                if (selectedItemHourFrom == null) {
                    Log.Error("Hour From Value referenced is empty", this);
                    return false;
                } else {
                    hourFromReferenced = Convert.ToInt32(selectedItemHourFrom["Value"]);
                    Log.Debug("Hour From Value referenced " + hourFromReferenced, this);
                }

                int minuteFromReferenced = 0;
                var selectedItemMinuteFrom = Context.Database.GetItem(MinuteFrom);
                if (selectedItemMinuteFrom == null) {
                    Log.Error("Minute From Value referenced is empty", this);
                    return false;
                } else {
                    minuteFromReferenced = Convert.ToInt32(selectedItemMinuteFrom["Value"]);
                    Log.Debug("Minute From Value referenced " + minuteFromReferenced, this);
                }

                int hourToReferenced = 0;
                var selectedItemHourTo = Context.Database.GetItem(HourTo);
                if (selectedItemHourTo == null) {
                    Log.Error("Hour To Value referenced is empty", this);
                    return false;
                } else {
                    hourToReferenced = Convert.ToInt32(selectedItemHourTo["Value"]);
                    Log.Debug("Hour To Value referenced " + hourToReferenced, this);
                }

                int minuteToReferenced = 0;
                var selectedItemMinuteTo = Context.Database.GetItem(MinuteTo);
                if (selectedItemMinuteTo == null) {
                    Log.Error("Minute From Value reference is empty", this);
                    return false;
                } else {
                    minuteToReferenced = Convert.ToInt32(selectedItemMinuteTo["Value"]);
                    Log.Debug("Minute From Value referenced " + minuteToReferenced, this);
                }

                TimeSpan startTime = new TimeSpan(hourFromReferenced, minuteFromReferenced, 0);
                TimeSpan endTime = new TimeSpan(hourToReferenced, minuteToReferenced, 0);
                var globalConfigurationManager =
                    (IGlobalConfigurationManager) ServiceLocator.ServiceProvider.GetService(typeof(IGlobalConfigurationManager));

                var targetZone = TimeZoneInfo.FindSystemTimeZoneById(globalConfigurationManager.TimeZoneRangeTimePersonalizationRule);
                var targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);

                TimeSpan now = targetDateTime.TimeOfDay;
                if ((now >= startTime) && (now <= endTime)) {
                    return true;
                } else {
                    return false;
                }
            } catch (Exception e) {
                Log.Error("Error in Range Time Rule",e, this);
                return false;
            }
        }
	}
}