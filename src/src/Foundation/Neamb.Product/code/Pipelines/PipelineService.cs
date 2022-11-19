using System;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Pipelines;

namespace Neambc.Neamb.Foundation.Product.Pipelines
{
	[Service(typeof(IPipelineService))]
	public class PipelineService : IPipelineService
	{
		private readonly IGlobalConfigurationManager _globalConfigurationManager;

		public PipelineService(IGlobalConfigurationManager globalConfigurationManager)
		{
			_globalConfigurationManager = globalConfigurationManager;
		}
		/// <summary>
		/// Run the pipelines to get the primary and secondary cta links
		/// </summary>
		/// <param name="productCode">Product code</param>
		/// <param name="accountUser">User membership data</param>
		/// <returns>True when aborted</returns>
		public ResultPipeline RunProcessPipelines(string productCode, AccountUserBase accountUser, string pipelineName,
            string returnUrl = null)
        {
			var args = new ProductPipelineArgs {
				ProductCode = productCode
			};
			var resultPipeline = new ResultPipeline();

			args.MdsId = accountUser.Mdsid;
			args.GivenName = accountUser.Profile.FirstName;
			args.FamilyName = accountUser.Profile.LastName;
			args.EmailAddress = accountUser.Username;
			args.AddressLine1 = accountUser.Profile.StreetAddress;
			args.City = accountUser.Profile.City;
			args.State = accountUser.Profile.StateCode;
			args.PostalCode = accountUser.Profile.ZipCode;
			args.Telephone = accountUser.Profile.Phone;
			args.DateBirth = accountUser.Profile.DateOfBirth;
            args.ReturnUrl = returnUrl ?? string.Empty;
			args.WebserviceUrl = _globalConfigurationManager.UrlJeepZag;
            args.ReferrerId = _globalConfigurationManager.ReferrerIdJeepZag;
            args.HeaderTrueCarToken = _globalConfigurationManager.HeaderTrueCarToken;

            CorePipeline.Run(pipelineName, args);
			if (!string.IsNullOrEmpty(args.Result))
			{
				resultPipeline.ActionPrimary=args.Result;
			}
			

			if (!string.IsNullOrEmpty(args.SecondaryUrl))
			{
				resultPipeline.ActionSecondary = args.SecondaryUrl;
			}

			return resultPipeline;
		}
	}
}