using Neambc.Seiumb.Feature.Navigation.Models;


namespace Neambc.Seiumb.Feature.Navigation.Repositories
{
    /// <summary>
    /// Actions in menu
    /// </summary>
    public interface INavigationRepository
    {
        NavigationItems GetTopMenu();
        NavigationItems GetBottomMenu();
        NavigationItems GetLeftMenu();
    }
}