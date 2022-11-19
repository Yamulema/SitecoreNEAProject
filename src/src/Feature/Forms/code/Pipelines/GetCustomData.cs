using Sitecore.Cintel.ContactService;
using Sitecore.Cintel.Reporting;
using Sitecore.Cintel.Reporting.Processors;
using Sitecore.Cintel.Reporting.ReportingServerDatasource;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using System;
using System.Data;
using System.Globalization;
using Neambc.Neamb.Foundation.MBCData.Utilities;

namespace Neambc.Seiumb.Feature.Forms.Pipelines {
    public class GetCustomData : ReportProcessorBase {
        public override void Process(ReportProcessorArgs args) {
            var table = CreateTableWithSchema();
            GetTableFromContact(table, args.ReportParameters.ContactId);
            args.QueryResult = table;
        }

        private static DataTable CreateTableWithSchema() {
            var dataTable = new DataTable() {
                Locale = CultureInfo.InvariantCulture
            };
            dataTable.Columns.AddRange(new[] {
                new DataColumn(XConnectFields.Contact.Id, typeof(Guid)),
                new DataColumn("City", typeof(string)),
                new DataColumn("ComplifesignDate", typeof(string)),
                new DataColumn("DateOfBirth", typeof(string)),
                new DataColumn("Email", typeof(string)),
                new DataColumn("EmailPermission", typeof(string)),
                new DataColumn("FirstName", typeof(string)),
                new DataColumn("Gendercode", typeof(string)),
                new DataColumn("Iaid", typeof(string)),
                new DataColumn("Introlifeenddate", typeof(string)),
                new DataColumn("LastName", typeof(string)),
                new DataColumn("MdsId", typeof(string)),
                new DataColumn("MembershipCatCode", typeof(string)),
                new DataColumn("NeaCurrentMember", typeof(string)),
                new DataColumn("NeaMembershipType", typeof(string)),
                new DataColumn("NewEnvInd", typeof(string)),
                new DataColumn("Newmembersegmentindicator", typeof(string)),
                new DataColumn("Phone", typeof(string)),
                new DataColumn("Registered", typeof(string)),
                new DataColumn("SeaName", typeof(string)),
                new DataColumn("SeaNumber", typeof(string)),
                new DataColumn("SeiuCurrentMember", typeof(string)),
                new DataColumn("SeiuLocalName", typeof(string)),
                new DataColumn("SeiuLocalNumber", typeof(string)),
                new DataColumn("StateCode", typeof(string)),
                new DataColumn("StreetAddress", typeof(string)),
                new DataColumn("UnionId", typeof(string)),
                new DataColumn("WebUserId", typeof(string)),
                new DataColumn("ZipCode", typeof(string)),
                new DataColumn("LeaName", typeof(string)),
                new DataColumn("LeaNumber", typeof(string)),
            });

            return dataTable;
        }

        private static void GetTableFromContact(DataTable rawTable, Guid contactId) {
            string[] facets = {
                CustomFacetInformation.DefaultFacetKey
            };
            var contact = GetContact(contactId, facets);
            var row = rawTable.NewRow();
            row[XConnectFields.Contact.Id] = contactId;

            if (contact.Facets.TryGetValue(CustomFacetInformation.DefaultFacetKey, out var customFacet)) {
                row["City"] = ((CustomFacetInformation) customFacet)?.City;
                row["ComplifesignDate"] = ((CustomFacetInformation) customFacet)?.ComplifesignDate;
                row["DateOfBirth"] = ((CustomFacetInformation) customFacet)?.DateOfBirth;
                row["Email"] = ((CustomFacetInformation) customFacet)?.Email;
                row["EmailPermission"] = ((CustomFacetInformation) customFacet)?.EmailPermission;
                row["FirstName"] = ((CustomFacetInformation) customFacet)?.FirstName;
                row["Gendercode"] = ((CustomFacetInformation) customFacet)?.Gendercode;
                row["Iaid"] = ((CustomFacetInformation) customFacet)?.Iaid;
                row["Introlifeenddate"] = ((CustomFacetInformation) customFacet)?.Introlifeenddate;
                row["LastName"] = ((CustomFacetInformation) customFacet)?.LastName;
                row["MdsId"] = ((CustomFacetInformation) customFacet)?.MdsId;
                row["MembershipCatCode"] = ((CustomFacetInformation) customFacet)?.MembershipCatCode;
                row["NeaCurrentMember"] = ((CustomFacetInformation) customFacet)?.NeaCurrentMember;
                row["NeaMembershipType"] = ((CustomFacetInformation) customFacet)?.NeaMembershipType;
                row["NewEnvInd"] = ((CustomFacetInformation) customFacet)?.NewEnvInd;
                row["Newmembersegmentindicator"] = ((CustomFacetInformation) customFacet)?.Newmembersegmentindicator;
                row["Phone"] = ((CustomFacetInformation) customFacet)?.Phone;
                row["Registered"] = ((CustomFacetInformation) customFacet)?.Registered;
                row["SeaName"] = ((CustomFacetInformation) customFacet)?.SeaName;
                row["SeaNumber"] = ((CustomFacetInformation) customFacet)?.SeaNumber;
                row["SeiuCurrentMember"] = ((CustomFacetInformation) customFacet)?.SeiuCurrentMember;
                row["SeiuLocalName"] = ((CustomFacetInformation) customFacet)?.SeiuLocalName;
                row["SeiuLocalNumber"] = ((CustomFacetInformation) customFacet)?.SeiuLocalNumber;
                row["StateCode"] = ((CustomFacetInformation) customFacet)?.StateCode;
                row["StreetAddress"] = ((CustomFacetInformation) customFacet)?.StreetAddress;
                row["UnionId"] = ((CustomFacetInformation) customFacet)?.UnionId;
                row["WebUserId"] = ((CustomFacetInformation) customFacet)?.WebUserId;
                row["ZipCode"] = ((CustomFacetInformation) customFacet)?.ZipCode;
                row["LeaName"] = ((CustomFacetInformation)customFacet)?.LeaName;
                row["LeaNumber"] = ((CustomFacetInformation)customFacet)?.LeaNumber;
            }

            rawTable.Rows.Add(row);
        }

        private static Contact GetContact(Guid contactId, string[] facets) {
            using (var client = SitecoreXConnectClientConfiguration.GetClient()) {
                var contactReference = new ContactReference(contactId);
                var contact = facets == null || facets.Length == 0
                    ? client.Get(contactReference, new ContactExpandOptions(Array.Empty<string>()))
                    : client.Get(contactReference, new ContactExpandOptions(facets));
                if (contact == null) {
                    throw new ContactNotFoundException(FormattableString.Invariant($"No Contact with id [{contactId}] found"));
                }

                return contact;
            }
        }
    }
}