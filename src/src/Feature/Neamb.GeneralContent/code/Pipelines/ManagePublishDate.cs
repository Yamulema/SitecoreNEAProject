using System;
using System.Linq;
using Neambc.Neamb.Feature.GeneralContent.Services;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Neambc.Neamb.Feature.GeneralContent.Pipelines {
	public class ManagePublishDate : PublishItemProcessor {

		#region Private Methods
        private bool SuccessfulPublishState(PublishItemContext context) {
			if (context == null || context.Result == null) {
				return false;
			}

			var op = context.Result.Operation;

			if (op != PublishOperation.Created && op != PublishOperation.Updated) {
				return false;
			}

			if (context.Action != PublishAction.PublishVersion) {
				return false;
			}

			if (context.VersionToPublish == null) {
				return false;
			}

			return true;
		}
        private bool FieldsExist(Item item) {
            if (item != null) {
                item.Fields.ReadAll();
                var result = item.Fields.FirstOrDefault(itemindex => itemindex.ID == Templates.StatisticsCustom.Fields.LastPublishDate);
                if (result == null) {
                    return false;
                }
            }
            return true;
		}
		private void SetDates(Item item, DateTime dt) {
			using (new Sitecore.SecurityModel.SecurityDisabler()) {
				using (new EditContext(item, false, false)) {
					var dtServer = DateUtil.ToServerTime(dt);
					var isoDt = DateUtil.ToIsoDate(dtServer);
					var utcDt = DateUtil.IsoDateToUtcIsoDate(isoDt);
					item[Templates.StatisticsCustom.Fields.LastPublishDate] = utcDt;
				}
			}
		}
		#endregion

		#region Public Methods
		public override void Process(PublishItemContext context) {
			if (!SuccessfulPublishState(context)) {
				return;
			}

            var itemId = context.ItemId;
			var sourceItem = context.PublishHelper.GetSourceItem(itemId);
			if (sourceItem.Name.Equals("__Standard Values")) {
				return;
			}
			var targetItem = context.PublishHelper.GetTargetItem(itemId);

			if (!FieldsExist(sourceItem) || !FieldsExist(targetItem)) {
				return;
			}

			var now = DateTime.Now;

			SetDates(sourceItem, now);
			SetDates(targetItem, now);
		}
		#endregion

	}
}