using Neambc.Seiumb.Feature.Reminder.Repositories;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Reminder.Models
{
    public class ReminderModel : IRenderingModel {
		public string ReminderId {
			get; set;
		}

		public Rendering Rendering {
			get; set;
		}
		public Item Item {
			get; set;
		}
		public Item PageItem {
			get; set;
		}

		public bool HaveRequested {
			get; set;
		}

		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			ReminderId = Item["Code"];
			HaveRequested = GetRequestStatus();
		}

		public bool GetRequestStatus() {
            var seiumbProfileManager = (ISeiumbProfileManager)ServiceLocator.ServiceProvider.GetService(typeof(ISeiumbProfileManager));
            var userRepository = (IUserRepository)ServiceLocator.ServiceProvider.GetService(typeof(IUserRepository));
            var seiuProfile = seiumbProfileManager.GetProfile();
            var ret = false;
            if (userRepository.GetUserStatus().Equals(UserStatusCons.COLD)) return ret;
            //call Oracle DB: query all reminders by reminderId and mdsIndvId
            var result = ReminderRepository.Instance.GetAllReminderlogById(ReminderId, seiuProfile.MdsId);
            int.TryParse(result, out var reminders);
            ret = reminders > 0;
            return ret;
		}

	}
}