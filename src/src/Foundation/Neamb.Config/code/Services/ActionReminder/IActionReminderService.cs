using System;

namespace Neambc.Neamb.Foundation.Configuration.Services.ActionReminder
{
    public interface IActionReminderService {
        void SetVisited(PageType pageType, string userName);
        bool WasVisited(PageType pageType, string userName);
        void RemoveVisited(PageType pageType, string userName);
        void SetAll(string username);
        void RemoveAll(string username);
        int Migrate();
        void RemoveVisited(PageType pageType);
    }
}
