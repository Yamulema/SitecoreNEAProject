using System;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Neambc.Neamb.Foundation.MBCData.ExactTargetService {
	public interface IExactTargetSoapClient : IDisposable {

		#region Synchronous
		CreateResult[] Create(CreateOptions Options, APIObject[] Objects, out string RequestID, out string OverallStatus);
		string Retrieve(RetrieveRequest RetrieveRequest, out string RequestID, out APIObject[] Results);
		UpdateResult[] Update(UpdateOptions Options, APIObject[] Objects, out string RequestID, out string OverallStatus);
		DeleteResult[] Delete(DeleteOptions Options, APIObject[] Objects, out string RequestID, out string OverallStatus);
		string Query(QueryRequest QueryRequest, out string RequestID, out APIObject[] Results);
		ObjectDefinition[] Describe(ObjectDefinitionRequest[] DescribeRequests, out string RequestID);
		string Execute(ExecuteRequest[] Requests, out string RequestID, out ExecuteResponse[] Results);
		PerformResult[] Perform(PerformOptions Options, string Action, APIObject[] Definitions, out string OverallStatus, out string OverallStatusMessage, out string RequestID);
		ConfigureResult[] Configure(ConfigureOptions Options, string Action, APIObject[] Configurations, out string OverallStatus, out string OverallStatusMessage, out string RequestID);
		ScheduleResult[] Schedule(ScheduleOptions Options, string Action, ScheduleDefinition Schedule1, APIObject[] Interactions, out string OverallStatus, out string OverallStatusMessage, out string RequestID);
		VersionInfoResponse VersionInfo(bool IncludeVersionHistory, out string RequestID);
		string Extract(ExtractRequest[] Requests, out string RequestID, out ExtractResult[] Results);
		SystemStatusResult[] GetSystemStatus(SystemStatusOptions Options, out string OverallStatus, out string OverallStatusMessage, out string RequestID);
		#endregion

		#region Asynchronous
		Task<CreateResponse> CreateAsync(CreateRequest request);
		Task<RetrieveResponse> RetrieveAsync(RetrieveRequest1 request);
		Task<UpdateResponse> UpdateAsync(UpdateRequest request);
		Task<DeleteResponse> DeleteAsync(DeleteRequest request);
		Task<QueryResponse> QueryAsync(QueryRequest1 request);
		Task<DescribeResponse> DescribeAsync(DescribeRequest request);
		Task<ExecuteResponse1> ExecuteAsync(ExecuteRequest1 request);
		Task<PerformResponse> PerformAsync(PerformRequest request);
		Task<ConfigureResponse> ConfigureAsync(ConfigureRequest request);
		Task<ScheduleResponse> ScheduleAsync(ScheduleRequest request);
		Task<VersionInfoResponse1> VersionInfoAsync(VersionInfoRequest request);
		Task<ExtractResponse> ExtractAsync(ExtractRequest1 request);
		Task<GetSystemStatusResponse> GetSystemStatusAsync(GetSystemStatusRequest request);
		#endregion

		#region Core
		void Open();
		void Abort();
		void Close();
		void DisplayInitializationUI();
		#endregion

		#region Properties
		ChannelFactory<Soap> ChannelFactory { get; }
		ClientCredentials ClientCredentials { get; }
		CommunicationState State { get; }
		IClientChannel InnerChannel { get; }
		ServiceEndpoint Endpoint { get; }
		#endregion

	}
}