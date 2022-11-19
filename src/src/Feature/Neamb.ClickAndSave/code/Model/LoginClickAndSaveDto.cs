using System.ComponentModel.DataAnnotations;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.ClickAndSave.Model
{
    public class LoginClickAndSaveDto : IRenderingModel
    {
		[Required] public string Password { get; set; }
	    public Rendering Rendering { get; set; }
	    public Item Item { get; set; }
	    public Item PageItem { get; set; }
	    public bool HasErrorInvalidCredentials { get; set; }
	    public bool HasAlreadyLockedTokenValidError { get; set; }
		//public bool HasAlreadyLockedTokenExpiredError { get; set; }
	    public bool HasLockedError { get; set; }
	    public bool HasErrorPassword { get; set; }
	    public bool HasErrorTimeout { get; set; }
	    public bool IsValid { get; set; }
	    public string Mdsid { get; set; }
	    public string RedirectUrl { get; set; }
	    public string UserName { get; set; }
	    public bool HasErrorEligible { get; set; }

		public void Initialize(Rendering rendering)
	    {
		    IsValid = false;
		    Rendering = rendering;
		    Item = rendering.Item;
		    PageItem = PageContext.Current.Item;
		    HasErrorInvalidCredentials = false;
			HasAlreadyLockedTokenValidError = false;
			//HasAlreadyLockedTokenExpiredError = false;
		    HasLockedError = false;
		    HasErrorTimeout = false;
		    HasErrorEligible = false;
	    }
    }
}