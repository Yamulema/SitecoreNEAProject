using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Feature.Forms.Extensions
{
    public static class ProfileExtensions
    {
        public static SeiuProfile ToProfile(this SeiuProfile profile, ProfileFormModel model) {
            profile.StreetAddress = string.IsNullOrEmpty(model.Address) ? string.Empty : model.Address;
            profile.FirstName = string.IsNullOrEmpty(model.FirstName) ? string.Empty : model.FirstName;
            profile.LastName = string.IsNullOrEmpty(model.LastName) ? string.Empty : model.LastName;
            profile.DateOfBirth = string.IsNullOrEmpty(Utilities.Utilities.FormatDate(model.DateOfBirth))
                ? string.Empty : Utilities.Utilities.FormatDate(model.DateOfBirth);
            profile.City = string.IsNullOrEmpty(model.City) ? string.Empty : model.City;
            profile.StateCode = string.IsNullOrEmpty(model.State) ? string.Empty : model.State;
            profile.ZipCode = string.IsNullOrEmpty(model.ZipCode) ? string.Empty : model.ZipCode;
            profile.Phone = string.IsNullOrEmpty(Utilities.Utilities.FormatPhone(model.Phone)) ? string.Empty : Utilities.Utilities.FormatPhone(model.Phone);
            profile.EmailPermission = Utilities.Utilities.FormatPermission(model.SendInformation);
            return profile;
        }

        public static void FillModel(this ProfileFormModel model, SeiuProfile profile)
        {
            model.FirstName = profile.FirstName;
            model.LastName = profile.LastName;
            model.Address = profile.StreetAddress;
            model.City = profile.City;
            model.State = profile.StateCode;
            model.ZipCode = profile.ZipCode;
            model.DateOfBirth = profile.DateOfBirth;
            model.Phone = profile.Phone;
        }
    }
}