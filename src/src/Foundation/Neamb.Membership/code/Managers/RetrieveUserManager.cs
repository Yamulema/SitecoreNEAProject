using System;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser;
using Neambc.Neamb.Foundation.MBCData.Services.RetrieveUser;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
    [Service(typeof(IRetrieveUserManager))]
    public class RetrieveUserManager : IRetrieveUserManager
    {
        private readonly IRetrieveUserService _retrieveUserService;

        public RetrieveUserManager(IRetrieveUserService retrieveUserService) {
            _retrieveUserService = retrieveUserService;
        }

        public bool TryRetrieveUserDataSeiumb(string mdsid, out RetrieveUserModel userDataResponse)
        {
            var success = false;
            userDataResponse = new RetrieveUserModel();

            try
            {
                var parseMdsId = int.TryParse(mdsid, out int mdsIdInt);
                if (parseMdsId) {
                    userDataResponse = _retrieveUserService.RetrieveUserData(mdsIdInt, (int)Union.SEIU);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Log.Warn("TryRetrieveUserData : Unable to retrieve mdsid ", ex, "TryRetrieveUserData");
                success = false;
            }

            return success;
        }

        public RetrieveUserModel RetrieveUserNeamb(string mdsid)
        {
            var mdsIdInt = int.Parse(mdsid);
            return _retrieveUserService.RetrieveUserData(mdsIdInt, (int)Union.NEA);
        }

        public RetrieveUserModel RetrieveUserSeiumb(string mdsid)
        {
            var mdsIdInt = int.Parse(mdsid);
            return _retrieveUserService.RetrieveUserData(mdsIdInt, (int)Union.SEIU);
        }

        public Profile ToProfileModel(RetrieveUserModel model)
        {
            var profile = new Profile
            {
                FirstName = !string.IsNullOrEmpty(model.FirstName) ? model.FirstName : string.Empty,
                LastName = !string.IsNullOrEmpty(model.LastName) ? model.LastName : string.Empty,
                StreetAddress = !string.IsNullOrEmpty(model.StreetAddress) ? model.StreetAddress : string.Empty,
                City = !string.IsNullOrEmpty(model.City) ? model.City : string.Empty,
                DateOfBirth = !string.IsNullOrEmpty(model.Dob) ? model.Dob.Replace("-", "") : string.Empty,
                Email = !string.IsNullOrEmpty(model.Email) ? model.Email : string.Empty,
                EmailPermissionIndicator = !string.IsNullOrEmpty(model.EmailPermissionIndicator) ? model.EmailPermissionIndicator : string.Empty,
                IAId = !string.IsNullOrEmpty(model.IaId) ? model.IaId : string.Empty,
                MembershipCategoryCode = !string.IsNullOrEmpty(model.MembershipCategoryCode) ? model.MembershipCategoryCode : string.Empty,
                IsNeaCurrentMember = model.NeaCurrentMember,
                NeaMembershipType = !string.IsNullOrEmpty(model.NeaMembershipType) ? model.NeaMembershipType : string.Empty,
                NeaMembershipTypeName = !string.IsNullOrEmpty(model.NeaMembershipTypeName) ? model.NeaMembershipTypeName : string.Empty,
                Phone = !string.IsNullOrEmpty(model.Phone) ? model.Phone : string.Empty,
                IsRegistered = model.Registered,
                SeaName = !string.IsNullOrEmpty(model.SeaName) ? model.SeaName : string.Empty,
                SeaNumber = model.SeaNumber.ToString(),
                IsSeiuCurrentMember = model.SeiuCurrentMember,
                SeiuLocalName = !string.IsNullOrEmpty(model.SeiuLocalName) ? model.SeiuLocalName : string.Empty,
                SeiuLocalNumber = model.SeiuLocalNumber.ToString(),
                StateCode = !string.IsNullOrEmpty(model.StateCode) ? model.StateCode : string.Empty,
                UnionId = model.UnionId != null ? model.UnionId.ToString() : string.Empty,
                Webuserid = model.WebUserId != null ? model.WebUserId.ToString() : string.Empty,
                ZipCode = !string.IsNullOrEmpty(model.ZipCode) ? model.ZipCode : string.Empty,
                NewEnvInd = !string.IsNullOrEmpty(model.NewEnvironmentIndicator) ? model.NewEnvironmentIndicator : string.Empty,
                ComplifesignDate = !string.IsNullOrEmpty(model.CompIntroSignDate) ? model.CompIntroSignDate.Replace("-", "") : string.Empty,
                GenderCode = !string.IsNullOrEmpty(model.GenderCode) ? model.GenderCode : string.Empty,
                Introlifeenddate = !string.IsNullOrEmpty(model.CompIntroEndDate) ? model.CompIntroEndDate.Replace("-", "") : string.Empty,
                Newmembersegmentindicator = !string.IsNullOrEmpty(model.NewMemberSegmentIndicator) ? model.NewMemberSegmentIndicator : string.Empty,
                LeaName = !string.IsNullOrEmpty(model.LeaName) ? model.LeaName : string.Empty,
                LeaNumber = model.LeaNumber.ToString(),
                NeambPermissionMail = model.NeambPermissionMail,
                NcesId= model.NcesId
            };
            return profile;
        }
    }
}