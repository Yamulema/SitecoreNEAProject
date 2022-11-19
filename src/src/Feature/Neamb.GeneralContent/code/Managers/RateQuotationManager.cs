using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.GeneralContent.Enums;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;

namespace Neambc.Neamb.Feature.GeneralContent.Managers
{
    [Service(typeof(IRateQuotationManager))]
    public class RateQuotationManager : IRateQuotationManager
    {

        #region Fields
        private readonly IOracleDatabase _oracleManager;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Properties
        protected string CacheKeyGroup => "RateQuotation";
        #endregion

        #region Constructors
        public RateQuotationManager(IOracleDatabase oracleManager, ICacheManager cacheManager)
        {
            _oracleManager = oracleManager;
            _cacheManager = cacheManager;
        }
        #endregion

        #region Public Methods
        public List<Plan> GetPlanQuotes(string state, string zip, string age)
        {
            List<Plan> ret;
            var key = $"{CacheKeyGroup}:{state}_{zip}_{age}";

            if (_cacheManager.ExistInCache(key))
            {
                ret = _cacheManager.RetrieveFromCache<List<Plan>>(key);
            }
            else
            {
                var result = new List<Plan>();
                var ages = age.Split(',').Select(x => int.TryParse(x.Trim(), out var ageOptions) ? ageOptions : 0).ToArray();

                foreach (var option in Configuration.PlanOptions)
                {
                    var rates = _oracleManager.SelectRates(state, zip, ages, option);
                    if (rates != null) {
                        var optionName = option;
                        if (state == "MN") optionName = ReplaceOptionName(option);

                        result.Add(new Plan
                        {
                            Name = optionName,
                            Info = $"{Configuration.PlanInfoPath}/NEAMB_RetireeHealth_Detail_{optionName}.pdf",
                            Quotes = rates.Where(y => !string.IsNullOrEmpty(y)).Select((x, i) => new Quote
                            {
                                Age = AddPlusToAge(ages[i]),
                                Price = x
                            }).ToList()
                        });
                    }
                }

                // Checks if there are any valid quotes.
                if (!result.Any(x => x.Quotes.Any(y => !string.IsNullOrEmpty(y.Price))))
                {
                    result = new List<Plan>();
                }

                // Removes entries with 0 values
                result.RemoveAll(x => x.Quotes.All(y => string.IsNullOrEmpty(y.Price) || y.Price.Contains("$.00")));

                _cacheManager.StoreInCache(key, result, DateTime.Now.AddDays(Configuration.RateQuotationCacheDuration));
                ret = result;
            }
            return ret;
        }

        public QuoteStatus Validate(string state, string zip)
        {
            if (!string.IsNullOrEmpty(zip) && (!int.TryParse(zip, out var _zip) || zip.Length != 5))
            {
                return QuoteStatus.NoData;
            }

            if (!string.IsNullOrEmpty(state) && state.Length == 2)
            {
                return QuoteStatus.Ok;
            }

            return QuoteStatus.NoData;
        }
        public StateStatus Validate(string state)
        {
            var result = StateStatus.None;

            if (string.IsNullOrEmpty(state))
            {
                return StateStatus.Invalid;
            }

            if (state.Length != 2)
            {
                return StateStatus.Invalid;
            }

            if (state.Equals(Configuration.FloridaStateCode, StringComparison.InvariantCultureIgnoreCase))
            {
                result |= StateStatus.SpecifyZip;
                result |= StateStatus.SpecifyAge;
            }
            else
            {
                result &= ~StateStatus.SpecifyAge;
            }

            var zipCodeCount = _oracleManager.SelectZipCodeCount(state);

            if (zipCodeCount > 1)
            {
                result |= StateStatus.SpecifyZip;
            }
            else
            {
                if (state.Equals(Configuration.MissouriStateCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    result |= StateStatus.SpecifyAge;
                    result &= ~StateStatus.SpecifyZip;
                }
                else
                {
                    result &= ~StateStatus.SpecifyAge;
                    result &= ~StateStatus.SpecifyZip;
                }
            }

            return result;
        }
        #endregion

        private string ReplaceOptionName(string optionName)
        {
            switch (optionName)
            {
                case "A":
                    return "ME8";
                case "B":
                    return "ME9";
                case "F":
                    return "MF1";
                case "G":
                    return "MF2";
            }
            return optionName;
        }

        private string AddPlusToAge(int age) {
            return age == 80 ? "80+" : age.ToString();
        }
    }
}