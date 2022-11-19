using System;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(ILoginHandlerPostAction))]
    public class LoginHandlerPostAction: ILoginHandlerPostAction
    {
        private readonly ISessionManager _sessionManager;

        public LoginHandlerPostAction(ISessionManager sessionManager) {
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// Verify if there is a session value with the component id and the rendering id match with the query string parameter
        /// </summary>
        /// <param name="currentRenderingComponentId">Rendering id</param>
        /// <param name="componentIdQueryParameter">Query parameter</param>
        /// <param name="loginHandlerFeature">Feature enum</param>
        /// <returns></returns>
        public bool VerifyExecutionPostAction(string currentRenderingComponentId, string componentIdQueryParameter, LoginHandlerEnum loginHandlerFeature) {
            var hasExecutePostAction = false;
            string keyRedisComponentId = GetKeyRedisComponentId(loginHandlerFeature);
            var componentIdFeature=_sessionManager.RetrieveFromSession<string>(keyRedisComponentId);
            if (!string.IsNullOrEmpty(componentIdFeature) &&
                currentRenderingComponentId.Equals(componentIdQueryParameter)) {
                _sessionManager.Remove(keyRedisComponentId);
                _sessionManager.Remove(ConstantsNeamb.LoginHandlerFeatureType);
                hasExecutePostAction = true;
            }
            return hasExecutePostAction;
        }

        /// <summary>
        /// Get the page to be redirected after login comparing the component id in session
        /// </summary>
        /// <param name="loginPageRedirection">Url to be redirected</param>
        /// <returns></returns>
        public string GetPageToRedirection(string loginPageRedirection) {
            var resultPageRedirection = "";
            //Get the feature type
            var resultFeatureType = _sessionManager.RetrieveFromSession<LoginHandlerEnum>(ConstantsNeamb.LoginHandlerFeatureType);
            resultPageRedirection = GetPageRedirectionByFeature(loginPageRedirection, resultFeatureType);
            return resultPageRedirection;
        }

        private string GetPageRedirectionByFeature(string loginPageRedirection, LoginHandlerEnum loginHandlerEnum) {
            string resultPageRedirection = loginPageRedirection;

            string keyRedisComponentId = GetKeyRedisComponentId(loginHandlerEnum);

            if (!string.IsNullOrEmpty(keyRedisComponentId)) {
                var componentIdAuthentication = _sessionManager.RetrieveFromSession<string>(keyRedisComponentId);
                if (!string.IsNullOrEmpty(componentIdAuthentication))
                {
                    resultPageRedirection = loginPageRedirection.Contains("?")
                        ? $"{loginPageRedirection}&componentId={componentIdAuthentication}"
                        : $"{loginPageRedirection}?componentId={componentIdAuthentication}";
                }
            }
            return resultPageRedirection;
        }
        /// <summary>
        /// Save in session the component id
        /// </summary>
        /// <param name="loginHandlerFeature">Feature enum</param>
        /// <param name="componentIdAuthentication">Component id to be saved in session</param>
        public void SaveTrackingPageToRedirection(LoginHandlerEnum loginHandlerFeature, string componentIdAuthentication) {
            if (loginHandlerFeature == LoginHandlerEnum.None || string.IsNullOrEmpty(componentIdAuthentication)) {
                throw new ArgumentException($"Parameters for  SaveTrackingPageToRedirection are incorrect");
            }
            string keyRedisComponentId = GetKeyRedisComponentId(loginHandlerFeature);
            _sessionManager.StoreInSession(keyRedisComponentId, componentIdAuthentication);
            _sessionManager.StoreInSession(ConstantsNeamb.LoginHandlerFeatureType,loginHandlerFeature);
        }

        /// <summary>
        /// Get the key in redis to save the component id according the feature
        /// </summary>
        /// <param name="loginHandlerFeature">Feature enum</param>
        /// <returns></returns>
        private string GetKeyRedisComponentId(LoginHandlerEnum loginHandlerFeature) {
            string redisKeyComponentId = "";
            switch (loginHandlerFeature) {
                case LoginHandlerEnum.Sweepstake:
                    redisKeyComponentId = ConstantsNeamb.ComponentIdSweepstakesCallAuthentication;
                    break;
                case LoginHandlerEnum.Seminar:
                    redisKeyComponentId = ConstantsNeamb.ComponentIdSeminarCallAuthentication;
                    break;
                case LoginHandlerEnum.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(loginHandlerFeature), loginHandlerFeature, null);
            }
            return redisKeyComponentId;
        }
    }
}