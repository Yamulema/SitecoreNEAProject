using Neambc.Seiumb.Feature.ContactUs.Models;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Feature.ContactUs.Extensions
{
    public static class ContactUsExtensions
    {
        public static void FillModel(this ContactUsModel model, SeiuProfile profile)
        {
            model.FirstName = profile.FirstName;
            model.LastName = profile.LastName;
            model.Email = profile.Email;
            model.Phone = profile.Phone;
            model.LocalUnion = profile.LocalUnion;
            model.State = profile.StateCode;
        }
    }
}