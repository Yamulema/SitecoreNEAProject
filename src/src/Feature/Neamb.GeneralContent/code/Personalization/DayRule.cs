using System;
using System.Collections;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;


namespace Neambc.Neamb.Feature.GeneralContent.Personalization {
	/// <summary>
	/// This criteria must allow content authors to compare against the value on the "New member" attribute on an identified user profile
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DayRule<T> : StringOperatorCondition<T> where T : RuleContext {
		/// <summary>
		/// Day list
		/// </summary>
		public string DaysList
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
                
                var globalConfigurationManager =
                    (IGlobalConfigurationManager) ServiceLocator.ServiceProvider.GetService(typeof(IGlobalConfigurationManager));

                var targetZone = TimeZoneInfo.FindSystemTimeZoneById(globalConfigurationManager.TimeZoneRangeTimePersonalizationRule);
                var targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
                var dayList = GetItemsDays();

                if (dayList.Contains(targetDateTime.DayOfWeek.ToString())) {
                    return true;
                } else {
                    return false;
                }
                
            } catch (Exception e) {
                Log.Error("Error in Day Rule",e, this);
                return false;
            }
        }
        public ID[] TargetIDs
        {
            get
            {
                ArrayList arrayList = new ArrayList();
                string str = this.DaysList;
                char[] chArray = new char[1] { '|' };
                foreach (string id in str.Split(chArray))
                {
                    if (id.Length > 0 && ID.IsID(id))
                        arrayList.Add((object)ID.Parse(id));
                }
                return arrayList.ToArray(typeof(ID)) as ID[];
            }
        }

        public List<string> GetItemsDays()
        {
            List<string> result = new List<string>();
            
            foreach (ID targetId in TargetIDs)
            {
                Item obj = Context.Database.GetItem(targetId);
                if (obj != null) {
                    result.Add(obj.Name);
                }
            }
            return result;
        }

    }
}