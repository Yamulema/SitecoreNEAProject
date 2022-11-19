using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Managers
{
	[Service(typeof(IContactUsManager))]
	public class ContactUsManager: IContactUsManager
	{
        #region Fields
		private readonly IGtmService _gtmService;
		#endregion
		#region Constructors
		public ContactUsManager(IGtmService gtmService)
		{
			_gtmService = gtmService;
		}
		#endregion

		/// <summary>
		/// Get the GTM action when the contact us form was sucessfully submitted
		/// </summary>
		/// <param name="contextItem">Contact us item</param>
		/// <returns>GTM action</returns>
		public string GetGtmAction(Item contextItem)
		{
			string accountAction = contextItem[Templates.ContactUsForm.Fields.Title];
			string buttonText = contextItem[Templates.ContactUsForm.Fields.Submit];
			ContactUs profileUpdate = new ContactUs
			{
				AccountAction = accountAction,
				CtaText = buttonText
			};
			return _gtmService.GetGtmEvent(profileUpdate);
		}
	}
}