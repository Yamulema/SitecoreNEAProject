using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Contest.Interfaces;
using Neambc.Neamb.Feature.Contest.Model;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Contest.Services
{
	[Service(typeof(IContestSubmissionService))]
	public class ContestSubmissionService : IContestSubmissionService
	{
		#region Properties

		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IAmazonS3Repository _amazonS3Repository;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly IVoteService _voteService;

		#endregion

		#region Ctor

		public ContestSubmissionService(ISessionAuthenticationManager sessionAuthenticationManager,
			IAmazonS3Repository amazonS3Repository, IGlobalConfigurationManager globalConfigurationManager, IVoteService voteService)
		{
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_amazonS3Repository = amazonS3Repository;
			_globalConfigurationManager = globalConfigurationManager;
			_voteService = voteService;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get the model to be displayed in Get action
		/// </summary>
		/// <param name="model">Model to be returned to the view</param>
		public void FillModelContestSubmission(ContestSubmissionDto model)
		{
			model.Initialize(RenderingContext.Current.Rendering);
			model.MaxSize = GetMaxSizeSubmission(model.Item);
			model.AllowTypes = GetContentTypeAllowed(model.Item.Parent);
			var accountUser = _sessionAuthenticationManager.GetAccountMembership();
			model.UserStatus = accountUser.Status;
			model.PageNotAvailable = VerificationPageAvailable(model.Item);
			
		}

		/// <summary>
		/// Execute the action in submit saving the file in S3 
		/// </summary>
		/// <param name="model">data received in post action</param>
		/// <param name="viewData"></param>
		/// <param name="modelState"></param>
		public void ExecuteSubmission(ContestSubmissionDto model, ViewDataDictionary viewData,
			ModelStateDictionary modelState)
		{
			var destination = new MemoryStream();
			model.Initialize(RenderingContext.Current.Rendering);
			//Get the max size configured value
			model.MaxSize = GetMaxSizeSubmission(model.Item);
			//Get the content types allowed
			model.AllowTypes = GetContentTypeAllowed(model.Item);
			var accountUser = _sessionAuthenticationManager.GetAccountMembership();
			model.UserStatus = accountUser.Status;
			//Validation of the information submitted in the form
			ExecuteValidations(model, viewData);

			if (modelState.IsValid && model.UploadedFile != null)
			{
				model.UploadedFile.InputStream.CopyTo(destination);
				var keyFile = Guid.NewGuid();
				var extension = System.IO.Path.GetExtension(model.UploadedFile.FileName);
				//Build the file path to be saved in S3
				var filePath =
					$"{model.ParentId}/{_globalConfigurationManager.S3SubmissionFolder}/{keyFile}{extension}";

				try
				{
					var pathCurated = $"{model.ParentId}/{Configuration.S3VoteFolder}/";
					RequestS3 requestS3 = new RequestS3 {
						BucketName = _globalConfigurationManager.BucketNameContestImages,
						Key = pathCurated
					};
					var responseFolder= _amazonS3Repository.CreateObjectS3(requestS3);
					if(!responseFolder)
					{
						model.ProcessedSucessfully = false;
						model.HasGeneralError = true;
						return;
					}
				}
				catch (Exception ex)
				{
					Log.Error("Error creating curated folder", ex, this);
				}

				try
				{
					destination.Seek(0, SeekOrigin.Begin);
					RequestS3 requestS3 = new RequestS3
					{
						BucketName = _globalConfigurationManager.BucketNameContestImages,
						Key = filePath,
						ContentType= model.UploadedFile.ContentType,
						InputStream = destination
					};
					var responseCreationImage= _amazonS3Repository.CreateObjectS3(requestS3);
					if (!responseCreationImage)
					{
						model.ProcessedSucessfully = false;
						model.HasGeneralError = true;
						return;
					}
					//create/update the contest submission json file in S3
					var responseCreationJson =ProcessContestSubmissionJson(keyFile, model.ParentId, model.FileName,
						accountUser.Profile.Webuserid, accountUser.Mdsid);
					if (!responseCreationJson)
					{
						model.ProcessedSucessfully = false;
						model.HasGeneralError = true;
						return;
					}
					model.ProcessedSucessfully = true;
					//Call the logging in Oracle database
					var itemCode = model.Item[Templates.ConstestSubmission.Fields.ContestCode];
					var resultExecutionLogging= _voteService.ExecuteContestLoggingProcess(itemCode, accountUser.Mdsid);
					if (!resultExecutionLogging && !string.IsNullOrEmpty(itemCode))
					{
						Log.Error($"Error in contest submission logging process. Contest code: {itemCode}, mdsid: {accountUser.Mdsid}",this);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Error in contest submission page", ex, this);
					model.ProcessedSucessfully = false;
					model.HasGeneralError = true;
				}
			}
			else
			{
				model.ProcessedSucessfully = false;
			}
		}

	    public IEnumerable<string> GetAllowedTypes(Item contestPage)
	    {
	        return ((Sitecore.Data.Fields.MultilistField)contestPage.Fields[
	            Templates.ConstestSubmission.Fields.AllowType]).GetItems().Select(x=>x.Name);
        }


		#endregion

		#region Private Methods

		private bool VerificationPageAvailable(Item currentItem)
		{
			var resultVerification = false;
			var startSubmissionDate = currentItem[Templates.ConstestSubmission.Fields.StartSubmissionDate];
			if (!string.IsNullOrEmpty(startSubmissionDate))
			{
				var startDate = Sitecore.DateUtil.IsoDateToDateTime(startSubmissionDate);
				if (DateTime.Now < startDate)
				{
					resultVerification = true;
				}
			}


			var endSubmissionDate = currentItem[Templates.ConstestSubmission.Fields.EndSubmissionDate];

			if (!string.IsNullOrEmpty(endSubmissionDate))
			{
				var endDate = Sitecore.DateUtil.IsoDateToDateTime(endSubmissionDate);
				if (DateTime.Now > endDate)
				{
					resultVerification = true;
				}
			}

			return resultVerification;
		}

		/// <summary>
		/// Do the data validation in post action
		/// </summary>
		/// <param name="model">Data received in Post</param>
		/// <param name="viewData"></param>
		private void ExecuteValidations(ContestSubmissionDto model, ViewDataDictionary viewData)
		{
			model.ErrorsFileName =
				ValidationFieldHelper.SetErrorsField(viewData.ModelState[nameof(model.FileName)], true, true, true);
			if (model.UploadedFile == null)
			{
				model.ErrorsUploadFile.Add(ErrorStatusEnum.Required);
			}
		}

		/// <summary>
		/// Convert the max size configured in Sitecore in bytes
		/// </summary>
		/// <param name="currentItem">Contest submission page</param>
		private int GetMaxSizeSubmission(Item currentItem)
		{
			return int.TryParse(currentItem[Templates.ConstestSubmission.Fields.AllowSize], out var totalMb)
				? totalMb * 1024 * 1024
				: _globalConfigurationManager.MaxImageSizeAvatar;
		}

		/// <summary>
		/// Get the content types allowed in contest submission page
		/// </summary>
		/// <param name="currentItem">Contest submission page</param>
		/// <returns>string with extensions allowed to be upload</returns>
		private string GetContentTypeAllowed(Item currentItem)
		{
			var resultContentTypes = string.Empty;
		    var allowTypes = GetAllowedTypes(currentItem);

            var result = string.Empty;
			foreach (var allowTypesItem in allowTypes)
			{
				result = string.Concat(result, $"{allowTypesItem}|");
			}

			if (result.Length > 0)
			{
				resultContentTypes = result.Remove(result.Length - 1);
			} else {
                //try to put an extension that is not going to work (unexisting extension)
                resultContentTypes = "wi";
            }

			return resultContentTypes;
		}

		/// <summary>
		/// Save the json file that contains the file properties in S3
		/// </summary>
		/// <param name="key">File name guid</param>
		/// <param name="parentId">Sitecore id of the contest name</param>
		/// <param name="filename">File name provided by the user</param>
		/// <param name="webuserid">User identifier logged in the system</param>
		private bool ProcessContestSubmissionJson(Guid key, string parentId, string filename, string webuserid, string mdsid)
		{
			//Build the path of the json file pear file/image to be upload in S3
			var filePath = $"{parentId}/{_globalConfigurationManager.S3SubmissionFolder}/{key}.json";
			var contestFileItemTobeAdded = new ContestFileItem
				{Key = key, FileName = filename, Webuserid = webuserid, Mdsid = mdsid};
			//Get the json content to be saved in S3
			var resultJson = JsonConvert.SerializeObject(contestFileItemTobeAdded);
			RequestS3 requestS3 = new RequestS3
			{
				BucketName = _globalConfigurationManager.BucketNameContestImages,
				Key = filePath,
				ContentType = "text/plain",
				ContentBody = resultJson
			};
			return _amazonS3Repository.CreateObjectS3(requestS3);
		}

		#endregion
	}
}