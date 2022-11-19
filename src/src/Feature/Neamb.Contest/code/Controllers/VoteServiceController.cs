using System;
using System.Net;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Contest.Interfaces;
using Neambc.Neamb.Feature.Contest.Model;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;

namespace Neambc.Neamb.Feature.Contest.Controllers {
	public class VoteServiceController : BaseController {
		#region Properties
		private readonly IVoteService _voteService;
		#endregion

		#region Ctor
		public VoteServiceController(IVoteService voteService) {
			_voteService = voteService;
		}
		#endregion

		#region Public Methods
		[HttpPost]
		
		public ActionResult GetUserVote(string contestId) {
			Log.Info($"GetUserVote for contestId:{contestId}", this);
			try {
				var result = _voteService.GetUserVote(contestId);
				if (result == null) {
					return Json(new UserVote() {
						ContestId = contestId,
						SubmissionId = null
					}, JsonRequestBehavior.AllowGet);
				}
				return Json(result, JsonRequestBehavior.AllowGet);
			} catch (AccessDeniedException e) {
				Log.Warn("User not authorized to call GetUserVote:", e, this);
				return Json(new RouteError {
					ErrorCode = (int)HttpStatusCode.Unauthorized,
					Message = "Unauthorized"
				}, JsonRequestBehavior.AllowGet);
			} catch (Exception e) {
				Log.Error("Unhandled error in GetUserVote:", e, this);
				return Json(new RouteError {
					ErrorCode = (int)HttpStatusCode.InternalServerError,
					Message = "InternalServerError"
				}, JsonRequestBehavior.AllowGet);
			}
		}
		[HttpPost]
		
		public ActionResult AddUserVote(UserVote userVote) {
			Log.Info($"AddUserVote for SubmissionId:{userVote?.SubmissionId}", this);
			try {
				_voteService.AddUserVote(userVote);
				_voteService.PerformIntegritySync(userVote?.ContestId);
				var result = _voteService.GetVote(userVote.ContestId, userVote.SubmissionId);
				return Json(result, JsonRequestBehavior.AllowGet);
			} catch (AccessDeniedException e) {
				Log.Warn("User not authorized to call AddUserVote:", e, this);
				return Json(new RouteError {
					ErrorCode = (int)HttpStatusCode.Unauthorized,
					Message = "Unauthorized"
				}, JsonRequestBehavior.AllowGet);
			} catch (Exception e) {
				Log.Error("Unhandled error in AddUserVote:", e, this);
				return Json(new RouteError {
					ErrorCode = (int)HttpStatusCode.InternalServerError,
					Message = "InternalServerError"
				}, JsonRequestBehavior.AllowGet);
			}
		}
		[HttpPost]
		
		public ActionResult GetVotes(string contestId) {
			Log.Info($"GetVotes for contestId:{contestId}", this);
			try {
				_voteService.PerformIntegritySync(contestId);
				var result = _voteService.GetVotes(contestId);
				return Json(result, JsonRequestBehavior.AllowGet);
			} catch (Exception e) {
				Log.Error("Unhandled error in GetVotes:", e, this);
				return Json(new RouteError {
					ErrorCode = (int)HttpStatusCode.InternalServerError,
					Message = "InternalServerError"
				}, JsonRequestBehavior.AllowGet);
			}
		}
		#endregion
	}
}