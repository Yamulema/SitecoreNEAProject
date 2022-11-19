using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Neambc.Seiumb.Feature.Account.Personalization
{
    /// <summary>
    /// This criteria must allow content authors to compare against the value on session
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SessionRule<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string NameSession
        {
            get; set;
        }

        public string ValueSession
        {
            get; set;
        }

        protected override bool Execute(T ruleContext)
        {
            var sessionManager =
                (ISessionManager)ServiceLocator.ServiceProvider.GetService(typeof(ISessionManager));
            var log =
                (ILog)ServiceLocator.ServiceProvider.GetService(typeof(ILog));

            var ret = false;
            //Verify input data
            if (string.IsNullOrEmpty(NameSession) || string.IsNullOrEmpty(ValueSession))
            {
                //case no input data
                log.Debug("Session Name/Value is empty", this);
            }
            else
            {
                //Get the sitecore item in Global about the session variables 
                var selectedItem = Context.Database.GetItem(NameSession);
                if (selectedItem == null)
                {
                    log.Debug("Session Name item is empty", this);
                }
                else
                {
                    //Get the value of the Sitecore item
                    var valueItemReferenced = selectedItem[Templates.NameValueItem.Fields.ItemValue];
                    var valueRetrievedSession = sessionManager.RetrieveFromSession<string>(valueItemReferenced);
                    if (!string.IsNullOrEmpty(valueRetrievedSession)) {
                        ret = Compare(valueRetrievedSession, ValueSession);
                    }
                }
            }
            return ret;
        }
    }
}