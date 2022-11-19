using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Exceptions;
using Neambc.Seiumb.Foundation.Sitecore;
using Newtonsoft.Json;

namespace Neambc.Neamb.Foundation.MBCData.Managers {

	/// <summary>
	/// Responsible for communications with ExactTarget 
	/// </summary>
	[Service(typeof(IExactTargetProxy))]
	public class ExactTargetProxy : IExactTargetProxy {

		#region Fields
		private readonly ILog _log;
        private readonly IExactTargetLogger _exactTargetLog;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly IExactTargetSoapClientFactory _clientFactory;
		private CreateOptions _sendOptions = new CreateOptions {
			RequestType = RequestType.Synchronous,
			RequestTypeSpecified = true,
			SaveOptions = new [] {
				new SaveOption {
					SaveAction = SaveAction.UpdateAdd,
					PropertyName = "*"
				}
			}
		};

		#endregion

		#region Constructor
		public ExactTargetProxy(
			ILog log,
			IGlobalConfigurationManager globalConfigurationManager,
			IExactTargetSoapClientFactory factory,
            IExactTargetLogger exactTargetLog) {
            _log = log ?? throw new ArgumentNullException(nameof(log));
			_globalConfigurationManager = globalConfigurationManager ?? throw new ArgumentNullException(nameof(globalConfigurationManager));
			_clientFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            _exactTargetLog = exactTargetLog;

        }
		#endregion
		 
		#region Protected Methods
		protected bool CheckStatus(string status, Result result) {
            _exactTargetLog.Info("CheckStatus - Response: " + JsonConvert.SerializeObject(result));

            var ret = false;
			switch (status) {
				case "OK":
					ret = true;
					break;
				default:
					status = status ?? "(null)";
					var resultMsg = result?.StatusMessage ?? "(null)";
					_log.Error($"No handle for status {status} after preforming a ExactTarget SOAP request.", this);
					_log.Error($"ExactTarget status message: {resultMsg}", this);
                    _exactTargetLog.Error($"No handle for status {status} after preforming a ExactTarget SOAP request.");
                    _exactTargetLog.Error($"ExactTarget status message: {resultMsg}");
                    break;
			}
			return ret;
		}
		#endregion

		#region Public Methods
		public bool Send(string customerKey, Subscriber subscriber) {
			var createObjects = new APIObject[] {
				new TriggeredSend {
					TriggeredSendDefinition = new TriggeredSendDefinition {
						CustomerKey = customerKey,
						Priority = "High",
						Client = new ClientID {
							ID = _globalConfigurationManager.ExacttargetClientId,
							IDSpecified = true
						},
					},
					Subscribers = new[] {
						subscriber
					}
				}
			};
			CreateResult[] results = null;
			string status = null;
			try {
				using (var soapClient = _clientFactory.CreateClient()) {
					results = soapClient.Create(_sendOptions, createObjects, out _, out status);
                    _exactTargetLog.Info("Method: Send - Call: " + JsonConvert.SerializeObject(createObjects));
				}
			} catch (Exception ex) {
                var e = new NeambExactTargetException("Send", ex);
                _log.Fatal(e.Message, e, this);
			}
			return CheckStatus(status, results?.FirstOrDefault());
		}

		public IEnumerable<APIObject> Retrieve(RetrieveRequest retrieveRequest) {
			IEnumerable<APIObject> ret = null;
			string status = null;
			APIObject[] results = null;
			try {
				using (var soapClient = _clientFactory.CreateClient()) {
					status = soapClient.Retrieve(retrieveRequest, out _, out results);
                    _exactTargetLog.Info("Method: Retrieve - Call: " + JsonConvert.SerializeObject(retrieveRequest));
                }
			} catch (Exception ex) {
                var e = new NeambExactTargetException("Retrieve", ex);
                _log.Fatal(e.Message, e, this);
			}
			if (CheckStatus(status, null)) {
				ret = results;
			}
			return ret ?? new List<APIObject>();
		}

		public bool Update(UpdateRequest updateRequest) {
			UpdateResult[] results = null;
			string status = null;
			try {
				using (var soapClient = _clientFactory.CreateClient()) {
					results = soapClient.Update(updateRequest.Options, updateRequest.Objects, out _, out status);
                    _exactTargetLog.Info("Method: Update - Call: " + JsonConvert.SerializeObject(updateRequest.Objects));
                }
			} catch (Exception ex) {
                var e = new NeambExactTargetException("Update", ex);
                _log.Fatal(e.Message, e, this);
			}
			return CheckStatus(status, results?.FirstOrDefault());
		}

		public bool Create(CreateRequest createRequest) {
			string status = null;
			CreateResult[] results = null;
			try {
				using (var soapClient = _clientFactory.CreateClient()) {
					results = soapClient.Create(createRequest.Options, createRequest.Objects, out _, out status);
                    _exactTargetLog.Info("Method: Create - Call: " + JsonConvert.SerializeObject(createRequest.Objects));
                }
			} catch (Exception ex) {
                var e = new NeambExactTargetException("Create", ex);
                _log.Fatal(e.Message, e, this);
			}
			return CheckStatus(status, results?.FirstOrDefault());
		}

        public CreateResult[] CreateCall(CreateRequest createRequest)
        {
            CreateResult[] results = null;
            try
            {
                using (var soapClient = _clientFactory.CreateClient())
                {
                    results = soapClient.Create(createRequest.Options, createRequest.Objects, out _, out var status);
                    _exactTargetLog.Info("Method: CreateCall - Call: " + JsonConvert.SerializeObject(createRequest.Objects));
                }
            }
            catch (Exception ex)
            {
                var e = new NeambExactTargetException("CreateCall", ex);
                _log.Fatal(e.Message, e, this);
            }
            return results;
        }

        public async Task<bool> CreateAsync(CreateRequest createRequest) {
			CreateResponse response = null;
			try {
				using (var soapClient = _clientFactory.CreateClient()) {
					response = await soapClient.CreateAsync(createRequest);
                    _exactTargetLog.Info("Method: CreateAsync - Call: " + JsonConvert.SerializeObject(createRequest.Objects));
                }
            } catch (Exception ex) {
                var e = new NeambExactTargetException("Create", ex);
                _log.Fatal(e.Message, e, this);
			}
			return CheckStatus(response?.OverallStatus, response?.Results.FirstOrDefault());
		}
		#endregion
    }
}