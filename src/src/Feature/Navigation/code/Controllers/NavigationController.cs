using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Navigation.Repositories;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Navigation.Controllers {
	public class NavigationController : BaseController {
		private readonly INavigationRepository _navigationRepository;

		public NavigationController() : this(new NavigationRepository(RenderingContext.Current.ContextItem)) {
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="navigationRepository"></param>
		public NavigationController(INavigationRepository navigationRepository) {
			_navigationRepository = navigationRepository;
		}

		/// <summary>
		/// Get the top menu
		/// </summary>
		/// <returns>Return the top menu</returns>
		public ActionResult TopMenu() {
			var items = _navigationRepository.GetTopMenu();
			return View("/Views/Navigation/Renderings/TopMenu.cshtml", items);
		}

		/// <summary>
		/// Get the bottom menu
		/// </summary>
		/// <returns>Return the bottom menu</returns>
		public ActionResult BottomMenu() {
			var items = _navigationRepository.GetBottomMenu();
			return View("/Views/Navigation/Renderings/BottomMenu.cshtml", items);
		}

		/// <summary>
		/// Get the left menu
		/// </summary>
		/// <returns>Return the left menu</returns>
		public ActionResult LeftMenu() {
			var items = _navigationRepository.GetLeftMenu();
			return View("/Views/Navigation/Renderings/LeftNavigation.cshtml", items);
		}
	}
}