using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public class SweepstakesController : BaseController
	{
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IOracleDatabase _oracleManager;
		private readonly IStepperService _stepperService;
        private readonly ISweepstakesRepository _sweepstakesRepository;
        private readonly ILoginHandlerPostAction _loginHandlerPostAction;

        public SweepstakesController(ISessionAuthenticationManager sessionAuthenticationManager, IOracleDatabase oracleManager,
			IStepperService stepperService,
            ISweepstakesRepository sweepstakesRepository,
            ILoginHandlerPostAction loginHandlerPostAction)
		{
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_oracleManager = oracleManager;
			_stepperService = stepperService;
			_sweepstakesRepository = sweepstakesRepository;
            _loginHandlerPostAction = loginHandlerPostAction;
        }

		/// <summary>
		/// Get method
		/// </summary>
		/// <returns></returns>
		public ActionResult Sweepstakes()
		{
			var model = new SweepstakesDTO();
			var rendering = RenderingContext.Current.Rendering;
			model.Initialize(rendering);
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            
            //Verify if there is a componentId in the query string to see if the component needs to execute the action
            var componentId = Request.QueryString["componentId"];
            var renderingComponentId = rendering.UniqueId.ToString("N");
            model.SweepstakesBase.HasResultAuthentication = _loginHandlerPostAction.VerifyExecutionPostAction(renderingComponentId, componentId, LoginHandlerEnum.Sweepstake);
            _sweepstakesRepository.SetComponentId(ref model, rendering.UniqueId);
            _sweepstakesRepository.SetPropertiesSweepstake(ref model, rendering.Item);
            //, rendering.UniqueId
            if (accountMembership.Status == StatusEnum.Unknown ||
                accountMembership.Status == StatusEnum.Cold ||
                accountMembership.Status == StatusEnum.WarmHot ||
                accountMembership.Status == StatusEnum.WarmCold)
            {
                _sweepstakesRepository.SetActionClickAuthentication(ref model);
            }
            model.ContextItem = RenderingContext.Current.Rendering.Item.ID.ToString();
            return View("/Views/Neamb.Product/Renderings/Sweepstakes.cshtml", model);
		}

		/// <summary>
		/// Call the EnrollInSweepstakes in Oracle db, send the email
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ExecuteSweepstakes(SweepstakesDTO model)
		{
			bool hasError = false;
			bool processedSucessfully = false;
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			//Case is user not hot
			if (accountMembership.Status != StatusEnum.Hot) {
				return Json(new
					{
						hasError = hasError,
						processedSucessfully = processedSucessfully,
						errorauthentication=true
					},
					JsonRequestBehavior.AllowGet);
			} else {
				if (!string.IsNullOrEmpty(accountMembership.Mdsid)) {
                    var contextItem = Sitecore.Context.Database.GetItem(model.ContextItem);

                    //Insert into Oracle database
                    var enrollmentOk = _oracleManager.EnrollInSweepstakes(accountMembership.Mdsid.PadLeft(9, '0'), model.SweepstakesId);

					//Send the email with exact target
                    var resultEmail = _sweepstakesRepository.SendEmail(contextItem, accountMembership);


                    if (!enrollmentOk || !resultEmail) {
						hasError = true;
					} else {
						_stepperService.Run(contextItem);
						processedSucessfully = true;
					}
				} else {
					hasError = true;
				}
				return Json(new {
						hasError = hasError,
						processedSucessfully = processedSucessfully,
						errorauthentication = false
				},
					JsonRequestBehavior.AllowGet);
			}
		}
	}
}