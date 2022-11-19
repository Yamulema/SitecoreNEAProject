using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Seiumb.Feature.Navigation.Extensions;
using Neambc.Seiumb.Feature.Navigation.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM;
using Neambc.Seiumb.Foundation.Analytics.GTM.Processors;
using Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Utility;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Seiumb.Feature.Navigation.Repositories {
	public class NavigationRepository : INavigationRepository {
        IGTMServiceSeiumb GTMService { get; set; }
        IHtmlProcessor htmlProcessor = new HtmlProcessor();

        /// <summary>
        /// Reference to the current item that is being displayed in the browser
        /// </summary>
        public Item ContextItem {
			get;
		}
		/// <summary>
		/// Reference to the root item (Home)
		/// </summary>
		public Item NavigationRoot {
			get;
		}
		/// <summary>
		/// Menu items to be displayed in the screen
		/// </summary>
		private List<NavigationItem> _navigationItems;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="contextItem"></param>
		public NavigationRepository(Item contextItem) {
			ContextItem = contextItem;
            GTMService = new GTMServiceSeiumb(htmlProcessor);

            NavigationRoot = GetNavigationRoot(ContextItem);
			if (NavigationRoot == null) {
				throw new InvalidOperationException($"Cannot determine navigation root from '{ContextItem.Paths.FullPath}'");
			}
		}

		/// <summary>
		/// Get the Navigation root item (Home). That is the one that has the NavigationRoot added in the template
		/// </summary>
		/// <param name="contextItem"></param>
		/// <returns></returns>
		public Item GetNavigationRoot(Item contextItem) {
			return contextItem.GetAncestorOrSelfOfTemplate(Templates.NavigationRoot.ID) ?? Context.Site.GetContextItem(Templates.NavigationRoot.ID);
		}

		/// <summary>
		/// Get the top menu with the items marked with the menu position
		/// </summary>
		/// <returns>Menu items</returns>
		public NavigationItems GetTopMenu() {
			_navigationItems = new List<NavigationItem>();
			GetNavigationItems(NavigationRoot, 0, 1, NavigationPosition.Top);
			var navItems = new NavigationItems {
				Items = _navigationItems
			};
			return navItems;
		}

		/// <summary>
		/// Get the bottom menu with the items marked with the menu position
		/// </summary>
		/// <returns>Menu items</returns>
		public NavigationItems GetBottomMenu() {
			_navigationItems = new List<NavigationItem>();
			GetNavigationItems(NavigationRoot, 0, 1, NavigationPosition.Bottom);
			var navItems = new NavigationItems {
				Items = _navigationItems
			};
			AddRootToPrimaryMenu(navItems, NavigationPosition.Bottom);
			return navItems;
		}

		/// <summary>
		/// Add the Root Item to the menu
		/// </summary>
		/// <param name="navItems"></param>
		private void AddRootToPrimaryMenu(NavigationItems navItems, Enum position) {
			var navigationItem = CreateNavigationItem(NavigationRoot, position);
			navItems?.Items?.Insert(0, navigationItem);
		}

		/// <summary>
		/// Get the menu items recursivelly depending of the menu item levels 
		/// </summary>
		/// <param name="currentItemTree">Item to start to get the children items</param>
		private void GetNavigationItems(Item currentItemTree, int level, int maxLevel, NavigationPosition navigationPosition) {
			if (level <= maxLevel) {
				if (currentItemTree.IsDerived(Templates.Navigable.ID) && VerifyShowInNavigation(navigationPosition, currentItemTree)) {
					_navigationItems.Add(CreateNavigationItem(currentItemTree, navigationPosition));
				}

				if (level != maxLevel) {
					foreach (var childItem in currentItemTree.Children.ToList()) {
						var result = childItem.HasAContextLanguage();
						if (result) {
							GetNavigationItems(childItem, level + 1, maxLevel, navigationPosition);
						}
					}
				}
			}
		}

		/// <summary>
		/// Verify if the item in Sitecore has been marked to be displayed in the menu position
		/// </summary>
		/// <param name="navigationPosition">Top, left, bottom menu position</param>
		/// <param name="item">Item to be evaluated</param>
		/// <returns>True the item has been marked in that menu position; otherwise false</returns>
		private bool VerifyShowInNavigation(NavigationPosition navigationPosition, Item item) {
			var items = ((Sitecore.Data.Fields.MultilistField)item.Fields[Templates.Navigable.Fields.ShowInNavigationMenu]).GetItems();
			if (items != null) {
				var result = items.FirstOrDefault(i => i.Name == navigationPosition.ToString());
				return result != null;
			} else {
				return false;
			}
		}
		/// <summary>
		/// Create the menu item with the information neccesary to be processed in the view
		/// </summary>
		/// <param name="item">Item to be evaluated</param>
		/// <returns></returns>
		private NavigationItem CreateNavigationItem(Item item, Enum position, bool findChildren = false) {

			var navigationItem = new NavigationItem {
				Item = item,
				Selected = item.ID == ContextItem.ID,
				Url = item.Url(),
                OnClickEventContent = GTMService.GetOnClickEvent(new Foundation.Analytics.GTM.Models.NavigationSeiumb()
                {
                    Event = "navigation",
                    NavType = position.GetEnumStringValue(),
                    NavText = item.Fields[Templates.Navigable.Fields.ShortTitle]?.Value ?? string.Empty
                })
            };

            if (findChildren) {
				var navigationChildItems = new NavigationItems {
					Items = new List<NavigationItem>()
				};
				foreach (var childItem in item.Children.ToList())
                    if (childItem.IsDerived(Templates.Navigable.ID) && VerifyShowInNavigation(NavigationPosition.Left, childItem)) {
                        var result = childItem.HasAContextLanguage();
                        if (result) navigationChildItems.Items.Add(CreateNavigationItem(childItem, NavigationPosition.Left));
                    }
                navigationItem.Children = navigationChildItems;
			}
			return navigationItem;
		}
		/// <summary>
		/// Get the left menu items
		/// </summary>
		/// <returns></returns>
		public NavigationItems GetLeftMenu() {
			var navigationItemsLocal = new List<NavigationItem>();
			var firstItem = GetChildHome(ContextItem);

			GetLeftNavigationItems(firstItem, navigationItemsLocal);
			var navItems = new NavigationItems {
				Items = navigationItemsLocal
			};
			return navItems;
		}

		/// <summary>
		/// Get the menu items recursivelly depending of the menu item levels 
		/// </summary>
		/// <param name="firstItem"></param>
		/// <param name="navigationItemsArg"></param>
		private void GetLeftNavigationItems(Item firstItem, List<NavigationItem> navigationItemsArg) {
			if (firstItem.IsDerived(Templates.Navigable.ID) && VerifyShowInNavigation(NavigationPosition.Left, firstItem)) {
				navigationItemsArg.Add(CreateNavigationItem(firstItem, NavigationPosition.Left, true));
			}
		}

		/// <summary>
		/// Get the first parent left item that is a child of Home
		/// </summary>
		/// <param name="currentItem"></param>
		/// <returns></returns>
		private Item GetChildHome(Item currentItem) {
            if (currentItem.Parent.ID == NavigationRoot.ID) return currentItem;
            return GetChildHome(currentItem.Parent);
        }

	}
}