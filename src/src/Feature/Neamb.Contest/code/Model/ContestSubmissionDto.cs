using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Contest.Model
{
    public class ContestSubmissionDto : IRenderingModel
    {
		public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public StatusEnum UserStatus { get; set; }
	    public bool ProcessedSucessfully { get; set; }
	    public bool HasGeneralError { get; set; }
	    public HttpPostedFileBase UploadedFile { get; set; }
	    public string ParentId { get; set; }
	    [Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
	    [RegularExpression(@"[A-Za-z0-9\-., \']+", ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
	    [MaxLength(30, ErrorMessage = ConstantsNeamb.ValidationLength)]
	    [MinLength(2, ErrorMessage = ConstantsNeamb.ValidationMinLength)]
	    public string FileName { get; set; }
	    public int MaxSize { get; set; }
	    public string AllowTypes { get; set; }
	    public List<ErrorStatusEnum> ErrorsFileName { get; set; }
	    public List<ErrorStatusEnum> ErrorsUploadFile { get; set; }
	    public string UrlForRedirection { get; set; }
	    public bool PageNotAvailable { get; set; }

		public void Initialize(Rendering rendering)
	    {
		    Rendering = rendering;
		    Item = rendering.Item;
		    PageItem = PageContext.Current.Item;
		    ParentId = PageContext.Current.Item.ParentID.ToString();
		    UserStatus = StatusEnum.Cold;
		    ProcessedSucessfully = false;
		    HasGeneralError = false;
		    ErrorsFileName = new List<ErrorStatusEnum>();
		    ErrorsUploadFile = new List<ErrorStatusEnum>();
		    LinkField destinationLinkField = rendering.Item.Fields[Templates.ConstestSubmission.Fields.DestinationLink];
		    UrlForRedirection = destinationLinkField.TargetItem!=null?LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(destinationLinkField.TargetItem.ID)):LinkManager.GetItemUrl(rendering.Item);
		    PageNotAvailable = false;
	    }
    }
}