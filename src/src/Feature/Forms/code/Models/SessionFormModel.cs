using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Managers;
using Sitecore.Mvc.Presentation;
using System.Web;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.DependencyInjection;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class SessionFormModel : IRenderingModel
    {
        public string SessionId { get; set; }
        public string BirthDate { get; set; }
        public string City { get; set; }
        public string DuplicateEmail { get; set; }
        public string Email { get; set; }
        public string EmailList { get; set; }
        public string EmailPermission { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MdsId { get; set; }
        public string MembershipCategoryCode { get; set; }
        public string MembershipType { get; set; }
        public string Phone { get; set; }
        public string Registered { get; set; }
        public string SeaName { get; set; }
        public string SeaNumber { get; set; }
        public string SeiuCurrentMember { get; set; }
        public string SeiuLocalName { get; set; }
        public string SeiuLocalNumber { get; set; }
        public string StateCode { get; set; }
        public string StreetAddress { get; set; }
        public string UserStatus { get; set; }
        public string WebUserId { get; set; }
        public string ZipCode { get; set; }
        public string IPCD { get; set; }
        public bool IsAuthenticated { get; set; }
        public string ProfileEmail { get; set; }
        public void Initialize(Rendering rendering)
        {
            var seiumbProfileManager = (ISeiumbProfileManager)ServiceLocator.ServiceProvider.GetService(typeof(ISeiumbProfileManager));
            var seiuProfile = seiumbProfileManager.GetProfile();

            SessionId = HttpContext.Current.Session.SessionID;
            BirthDate = seiuProfile.DateOfBirth;
            City = seiuProfile.City;
            DuplicateEmail = seiuProfile.DuplicateEmail;
            Email = seiuProfile.Email;
            EmailList = seiuProfile.Emails;
            EmailPermission = seiuProfile.EmailPermission;
            FirstName = seiuProfile.FirstName;
            LastName = seiuProfile.LastName;
            MdsId = seiuProfile.MdsId;
            MembershipCategoryCode = seiuProfile.MembershipCategoryCode;
            MembershipType = seiuProfile.MembershipType;
            Phone = seiuProfile.Phone;
            Registered = seiuProfile.Registered;
            SeaName = seiuProfile.SeaName;
            SeaNumber = seiuProfile.SeaNumber;
            SeiuCurrentMember = seiuProfile.SeiuCurrentMember;
            SeiuLocalName = seiuProfile.SeiuLocalName;
            SeiuLocalNumber = seiuProfile.SeiuLocalNumber;
            StateCode = seiuProfile.StateCode;
            StreetAddress = seiuProfile.StreetAddress;
            UserStatus = seiuProfile.Status;
            WebUserId = seiuProfile.Webuserid;
            ZipCode = seiuProfile.ZipCode;
            IPCD = HttpContext.Current != null && HttpContext.Current.Request != null ? HttpContext.Current.Request.UserHostAddress : string.Empty;
            IsAuthenticated = Sitecore.Context.User != null && Sitecore.Context.User.IsAuthenticated;
            ProfileEmail = Sitecore.Context.User != null ? Sitecore.Context.User.Profile.Email : string.Empty;
        }
    }
}