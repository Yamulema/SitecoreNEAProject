using Sitecore.Cintel.Reporting;
using Sitecore.Cintel.Reporting.Processors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms.Pipelines
{
    public class ConstructCustomDataTable : ReportProcessorBase
    {
        public override void Process(ReportProcessorArgs args)
        {
			args.ResultTableForView = new DataTable();

			//FirstName
	        var customViewFieldFirstName = new ViewField<string>("FirstName");
	        args.ResultTableForView.Columns.Add(customViewFieldFirstName.ToColumn());
	        //LastName
	        var customViewFieldLastName = new ViewField<string>("LastName");
	        args.ResultTableForView.Columns.Add(customViewFieldLastName.ToColumn());
			//Email
	        var customViewFieldEmail = new ViewField<string>("Email");
	        args.ResultTableForView.Columns.Add(customViewFieldEmail.ToColumn());
			//Iaid
	        var customViewFieldIaid = new ViewField<string>("Iaid");
	        args.ResultTableForView.Columns.Add(customViewFieldIaid.ToColumn());
			//NeaCurrentMember
	        var customViewFieldNeaCurrentMember = new ViewField<string>("NeaCurrentMember");
	        args.ResultTableForView.Columns.Add(customViewFieldNeaCurrentMember.ToColumn());
			//UnionId
	        var customViewFieldUnionId = new ViewField<string>("UnionId");
	        args.ResultTableForView.Columns.Add(customViewFieldUnionId.ToColumn());
			//Mdsid
	        var viewField = new ViewField<string>("MdsId");
	        args.ResultTableForView.Columns.Add(viewField.ToColumn());
			//StreetAddress
	        var customViewField = new ViewField<string>("StreetAddress");
	        args.ResultTableForView.Columns.Add(customViewField.ToColumn());
			//DateOfBirth
			var customViewFieldDate = new ViewField<string>("DateOfBirth");
            args.ResultTableForView.Columns.Add(customViewFieldDate.ToColumn());
            //City
            var customViewFieldCity = new ViewField<string>("City");
            args.ResultTableForView.Columns.Add(customViewFieldCity.ToColumn());
            //StateCode
            var customViewFieldStateCode = new ViewField<string>("StateCode");
            args.ResultTableForView.Columns.Add(customViewFieldStateCode.ToColumn());
            //ZipCode
            var customViewFieldZipCode = new ViewField<string>("ZipCode");
            args.ResultTableForView.Columns.Add(customViewFieldZipCode.ToColumn());
            //Phone
            var customViewFieldPhone = new ViewField<string>("Phone");
            args.ResultTableForView.Columns.Add(customViewFieldPhone.ToColumn());
            //Registered
            var customViewFieldRegistered = new ViewField<string>("Registered");
            args.ResultTableForView.Columns.Add(customViewFieldRegistered.ToColumn());
            //MembershipCatCode
            var customViewFieldMembershipCatCode = new ViewField<string>("MembershipCatCode");
            args.ResultTableForView.Columns.Add(customViewFieldMembershipCatCode.ToColumn());
            //NeaMembershipType
            var customViewFieldMembershipType = new ViewField<string>("NeaMembershipType");
            args.ResultTableForView.Columns.Add(customViewFieldMembershipType.ToColumn());
            //SeaCurrentMember
            var customViewFieldSeaCurrentMember = new ViewField<string>("SeiuCurrentMember");
            args.ResultTableForView.Columns.Add(customViewFieldSeaCurrentMember.ToColumn());
            //SeaNumber
            var customViewFieldSeaNumber = new ViewField<string>("SeaNumber");
            args.ResultTableForView.Columns.Add(customViewFieldSeaNumber.ToColumn());
            //SeaName
            var customViewFieldSeaName = new ViewField<string>("SeaName");
            args.ResultTableForView.Columns.Add(customViewFieldSeaName.ToColumn());
            //WebUserId
            var customViewFieldWebUserId = new ViewField<string>("WebUserId");
            args.ResultTableForView.Columns.Add(customViewFieldWebUserId.ToColumn());
            //SeiuLocalName
            var customViewFieldSeiuLocalName = new ViewField<string>("SeiuLocalName");
            args.ResultTableForView.Columns.Add(customViewFieldSeiuLocalName.ToColumn());
            //SeiuLocalNumber
            var customViewFieldSeiuLocalNumber = new ViewField<string>("SeiuLocalNumber");
            args.ResultTableForView.Columns.Add(customViewFieldSeiuLocalNumber.ToColumn());
            //EmailPermission
            var customViewFieldEmailPermission = new ViewField<string>("EmailPermission");
            args.ResultTableForView.Columns.Add(customViewFieldEmailPermission.ToColumn());
			//NewEnvInd
			var customViewFieldNewEnvInd = new ViewField<string>("NewEnvInd");
	        args.ResultTableForView.Columns.Add(customViewFieldNewEnvInd.ToColumn());
	        //ComplifesignDate
	        var customViewFieldComplifesignDate = new ViewField<string>("ComplifesignDate");
	        args.ResultTableForView.Columns.Add(customViewFieldComplifesignDate.ToColumn());
	        //GenderCode
	        var customViewFieldGenderCode = new ViewField<string>("GenderCode");
	        args.ResultTableForView.Columns.Add(customViewFieldGenderCode.ToColumn());
	        //Introlifeenddate
	        var customViewFieldIntrolifeenddate = new ViewField<string>("Introlifeenddate");
	        args.ResultTableForView.Columns.Add(customViewFieldIntrolifeenddate.ToColumn());
			//Newmembersegmentindicator
			var customViewFieldNewmembersegmentindicator = new ViewField<string>("Newmembersegmentindicator");
	        args.ResultTableForView.Columns.Add(customViewFieldNewmembersegmentindicator.ToColumn());
            //LeaName
            var customViewFieldLeaName = new ViewField<string>("LeaName");
            args.ResultTableForView.Columns.Add(customViewFieldLeaName.ToColumn());
            //LeaNumber
            var customViewFieldLeaNumber = new ViewField<string>("LeaNumber");
            args.ResultTableForView.Columns.Add(customViewFieldLeaNumber.ToColumn());
		}
    }
}