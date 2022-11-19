using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers {

	/// <summary>
	/// Response for transforming local business objects into the exact target requests
	/// </summary>
	[Service(typeof(IExactTargetClient))]
	public class ExactTargetClient : IExactTargetClient {

		#region Fields
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly IExactTargetProxy _proxy;
        private string[] _subscriptionProperties = new string[] {
			"ListID", "SubscriberKey", "Status", "Client.ID"
		};
        private readonly string subscriberKeyUpdated = "MB-";
		#endregion

		#region Constructor
		public ExactTargetClient(IGlobalConfigurationManager globalConfigurationManager, IExactTargetProxy proxy) {
			_globalConfigurationManager = globalConfigurationManager ?? throw new ArgumentNullException(nameof(globalConfigurationManager));
			_proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
        }
		#endregion

		#region Public Methods
		public bool SendExactTargetService(string customerDefinition, string username,
			List<KeyValuePair<string, string>> exactTargetParameters, string subscriberKey = null
		) {
            var key = UpdateSubscriberKey(!string.IsNullOrEmpty(subscriberKey) ? subscriberKey : username);
            var subscriber = new Subscriber {
				SubscriberKey = key,
				EmailAddress = username,
				Attributes = exactTargetParameters?.Select(x => new ExactTargetService.Attribute {
					Name = x.Key,
					Value = x.Value
				})
					.ToArray() ?? new ExactTargetService.Attribute[0]
			};
            return _proxy.Send(customerDefinition, subscriber);
		}

		public IEnumerable<APIObject> RetrieveAllSubscriptions(string subscriberKey) {
			IEnumerable<APIObject> ret;
			// Build RetrieveRequest

            var key = UpdateSubscriberKey(subscriberKey);
			var retrieveRequest = new RetrieveRequest {
				ClientIDs = new[] {
						new ClientID() {
							ID = _globalConfigurationManager.ExacttargetClientId,
							IDSpecified = true
						}
					},
				ObjectType = "ListSubscriber",
				Properties = _subscriptionProperties,
				Filter = new SimpleFilterPart() {
					Property = "SubscriberKey",
					SimpleOperator = SimpleOperators.@equals,
					Value = new[] {
                        key
                    }
				}
			};
			ret = _proxy.Retrieve(retrieveRequest);
			return ret ?? new List<APIObject>();
		}

		public bool UpdateSubscriberList(string subscriberKey, int listId, SubscriberStatus newStatus) {
            // Build UpdateRequest
            var key = UpdateSubscriberKey(subscriberKey);
            var updateRequest = new UpdateRequest {
				Options = new UpdateOptions(),
				Objects = new APIObject[] {
						new Subscriber {
							Client = new ClientID {
								ID = _globalConfigurationManager.ExacttargetClientId,
								IDSpecified = true
							},
							SubscriberKey = key,
							Lists = new [] {
								new SubscriberList {
									ID = listId,
									IDSpecified = true,
									Status = newStatus,
									StatusSpecified = true,
									Action = "update"
								}
							}
						}
					}
			};
			return _proxy.Update(updateRequest);
		}

		public bool AddUpdateSubscriberList(string subscriberKey, int listId, string email, SubscriberStatus newStatus) {
            var key = UpdateSubscriberKey(subscriberKey);
            var createRequest = new CreateRequest {
				Options = new CreateOptions {
					SaveOptions = new [] {
							new SaveOption {
								SaveAction = SaveAction.UpdateAdd,
								PropertyName = "*"
							}
						}
				},
				Objects = new APIObject[] {
						new Subscriber {
							Client = new ClientID {
								ID = _globalConfigurationManager.ExacttargetClientId,
								IDSpecified = true
							},
							SubscriberKey = key,
							Lists = new [] {
								new SubscriberList {
									ID = listId,
									IDSpecified = true,
									Status = newStatus,
									StatusSpecified = true
								}
							},
							EmailAddress = email
						}
					}
			};

            var response = _proxy.CreateCall(createRequest).FirstOrDefault();
            return response != null && (response.ErrorCode == 0 || response.ErrorCode == 13000 && newStatus == SubscriberStatus.Unsubscribed);
		}

		public bool AddUpdateDataExtension(string customerKey, Dictionary<string, string> properties) {
            var updateRequest = new UpdateRequest {
				Options = new UpdateOptions {
					SaveOptions = new [] {
							new SaveOption {
								SaveAction = SaveAction.UpdateAdd,
								PropertyName = "*"
							}
						}
				},
				Objects = new APIObject[] {
					new DataExtensionObject {
						CustomerKey = customerKey,
							Properties = properties.Select(x => new APIProperty {
								Name = x.Key,
								Value = x.Value
							}).ToArray()
						}
					}
			};
			return _proxy.Update(updateRequest);
		}

		/// <summary>
		/// Unsubscribe a email list in Exact target
		/// </summary>
		/// <param name="subscriberKey">Mdsid</param>
		/// <param name="listId">Subscription list id</param>
		/// <returns></returns>
		public bool UnsubscribeListMail(string subscriberKey, int listId) {
            var key = UpdateSubscriberKey(subscriberKey);
            var updateRequest = new UpdateRequest {
				Options = new UpdateOptions {
					RequestType = RequestType.Synchronous,
					RequestTypeSpecified = true
				},
				Objects = new APIObject[] {
						new Subscriber {
							SubscriberKey = key,
							Client = new ClientID {
								ID = _globalConfigurationManager.ExacttargetClientId,
								IDSpecified = true
							},
							Lists = new[] {
								new SubscriberList {
									ID = listId,
									IDSpecified = true,
									Status = SubscriberStatus.Unsubscribed,
									Action = "update",
									StatusSpecified = true
								}
							}
						}
					}
			};
			return _proxy.Update(updateRequest);
		}

		public async Task<bool> TriggeredSendAsync(ExactTargetEmail exactTargetEmail) {
            var key = UpdateSubscriberKey(exactTargetEmail.SubscriberKey);
            var createRequest = new CreateRequest {
				Objects = new APIObject[] {
					new TriggeredSend {
						Client = new ClientID {
							ID = _globalConfigurationManager.ExacttargetClientId
						},
						TriggeredSendDefinition = new TriggeredSendDefinition() {
							CustomerKey = exactTargetEmail.CustomerKey
						},
						Subscribers = new [] {
							new Subscriber {
								EmailAddress = exactTargetEmail.EmailTo,
								SubscriberKey = key
                            }
						},
						Attributes = exactTargetEmail.Attributes.Select(x => new ExactTargetService.Attribute() {
								Name = x.Key,
								Value = x.Value
							})
							.ToArray()
					}
				},
				Options = new CreateOptions {
					RequestType = RequestType.Asynchronous,
					RequestTypeSpecified = true
				}
			};
			return await _proxy.CreateAsync(createRequest);
		}

		public bool TriggeredSend(ExactTargetEmail exactTargetEmail) {
            var key = UpdateSubscriberKey(exactTargetEmail.SubscriberKey);
            var createRequest = new CreateRequest {
				Options = new CreateOptions {
					RequestType = RequestType.Synchronous,
					RequestTypeSpecified = true
				},
				Objects = new APIObject[] {
						new TriggeredSend {
							Client = new ClientID {
								ID = _globalConfigurationManager.ExacttargetClientId
							},
							TriggeredSendDefinition = new TriggeredSendDefinition() {
								CustomerKey = exactTargetEmail.CustomerKey
							},
							Subscribers = new [] {
								new Subscriber {
									EmailAddress = exactTargetEmail.EmailTo,
									SubscriberKey = key
								}
							},
							Attributes = exactTargetEmail.Attributes.Select(x => new ExactTargetService.Attribute {
									Name = x.Key,
									Value = x.Value
								})
								.ToArray()
						}
					}
			};
			return _proxy.Create(createRequest);
		}

        public IEnumerable<Subscriber> RetrieveSubscriber(string subscriberKey)
        {
            // Build RetrieveRequest
            var key = UpdateSubscriberKey(subscriberKey);
            var retrieveRequest = new RetrieveRequest()
            {
                ClientIDs = new[] {
                    new ClientID() {
                        ID = _globalConfigurationManager.ExacttargetClientId,
                        IDSpecified = true
                    }
                },
                ObjectType = "Subscriber",
                Properties = new[] {
                    "EmailAddress", "SubscriberKey", "Status"
                },
                Filter = new SimpleFilterPart()
                {
                    Property = "SubscriberKey",
                    SimpleOperator = SimpleOperators.@equals,
                    Value = new[] {
                        key
                    }
                }
            };
            var retrieve = _proxy.Retrieve(retrieveRequest);
            return retrieve.Select(x => (Subscriber)x);
        }
        #endregion

        #region Private Methods

        private string UpdateSubscriberKey(string key) {
            if (_globalConfigurationManager.ExactTargetUpdateSubscriberKey) {
                //basic email validation
                var isEmail = key.Contains("@");

                if (!isEmail) {
                    var mdsIds = new List<string> { "000000991", "000000992", "991", "992" };

                    if (mdsIds.Any(mds => string.Equals(key, mds))) {
                        var keyNoZeros = key.TrimStart('0');
                        return subscriberKeyUpdated + keyNoZeros;
                    }
                }
            }
            return key;
        }
        #endregion
    }
}