using System;
using System.Linq;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Membership.Enums;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Account.Models {
	public static class BeneficiaryDTOExtensions {
		public static bool FillBeneficiaryFromDataSource(this BeneficiaryDTO beneficiary, Item item) {
			var ret = false;
			if (item != null) {
				ret = true;
				beneficiary.BackCta = new BackCTA() {
                    FriendlyUrl = ((LinkField)item.Fields[Templates.Beneficiary.Fields.Back])?.GetFriendlyUrl(),
                    Text = ((LinkField)item.Fields[Templates.Beneficiary.Fields.Back])?.Text
                };
				beneficiary.Type = Enum.TryParse<BeneficiaryType>(beneficiary.SelectedType, out var type)
					? type
					: BeneficiaryType.NamedIndividual;

				beneficiary.Item = item;
				beneficiary.Relationship = new MbcDbOption(
					item,
					Templates.Beneficiary.Fields.RelationshipLabel,
					Templates.Beneficiary.Fields.RelationshipValues,
					beneficiary.Relationship.SelectedValue
				);
			}
			return ret;
		}

		public static bool HasFieldErrors(this BeneficiaryDTO beneficiary) {
			return beneficiary.GetType().GetProperties()
				.Any(x =>
					(x.PropertyType == typeof(ErrorStatusEnum)) &&
					(((ErrorStatusEnum)x.GetValue(beneficiary)) != ErrorStatusEnum.None)
				);
		}
	}

}