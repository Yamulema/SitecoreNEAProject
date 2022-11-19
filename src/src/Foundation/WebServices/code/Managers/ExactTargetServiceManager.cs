using System;
using System.ServiceModel;
using Neambc.Seiumb.Foundation.WebServices.ExactTargetServices;
using Sitecore.Diagnostics;
using ScSettings = Sitecore.Configuration.Settings;

namespace Neambc.Seiumb.Foundation.WebServices.Managers {
	public static class ExactTargetServiceManager {

		#region Fields
		private const string ServiceTimeoutConfig = "ServiceTimeOut";
		private const string UsernameSoapBinding = "UserNameSoapBinding";
		private static readonly int _configuredTimeout = int.Parse(ScSettings.GetSetting(ServiceTimeoutConfig));
		#endregion

		#region Public Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="firstname"></param>
		/// <param name="lastname"></param>
		/// <param name="cellcode"></param>
		/// <param name="campaignCd"></param>
		/// <param name="individualId"></param>
		/// <param name="datetimeAction"></param>
		/// <param name="sea"></param>
		/// <param name="state"></param>
		/// <param name="msgTopic"></param>
		/// <param name="mbrEmail"></param>
		/// <param name="message"></param>
		/// <param name="customerDefinition"></param>
		/// <param name="isforUser"></param>
		/// <returns></returns>
		public static bool SendExactTarget(string firstname, string lastname, string cellcode, string campaignCd, string individualId, string datetimeAction, string sea, string state, string msgTopic, string mbrEmail, string message, string customerDefinition, bool isforUser) {
			// Create the binding
			var binding = new BasicHttpBinding {
				Name = UsernameSoapBinding,
				Security = {
					Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
					Message = {ClientCredentialType = BasicHttpMessageCredentialType.UserName}
				},
				ReceiveTimeout = new TimeSpan(0, 0, _configuredTimeout),
				OpenTimeout = new TimeSpan(0, 0, _configuredTimeout),
				CloseTimeout = new TimeSpan(0, 0, _configuredTimeout),
				SendTimeout = new TimeSpan(0, 0, _configuredTimeout)
			};
			// Set the transport security to UsernameOverTransport for Plaintext usernames
			var endpoint = new EndpointAddress(ScSettings.GetSetting("ExacttargetEndPoint"));
			// Create the SOAP Client (and pass in the endpoint and the binding)
			var soapClient = new SoapClient(binding, endpoint) {
				ClientCredentials = {
					UserName = {
						UserName = ScSettings.GetSetting("ExacttargetUsernameSeiumb"),
						Password = ScSettings.GetSetting("ExacttargetPasswordSeiumb")
					}
				}
			};

			var subscriber = new Subscriber();
			if (isforUser) {
				subscriber.SubscriberKey = mbrEmail;
				subscriber.EmailAddress = mbrEmail;
			} else {
				subscriber.SubscriberKey = ScSettings.GetSetting("ExacttargetDefaultMail");
				subscriber.EmailAddress = ScSettings.GetSetting("ExacttargetDefaultMail");
			}

			//create a subscriber and fill with the same fields of dynament
			subscriber.Attributes = new ExactTargetServices.Attribute[12];

			subscriber.Attributes[0] = new ExactTargetServices.Attribute {
				Name = "FIRST_NAME",
				Value = firstname
			};

			subscriber.Attributes[1] = new ExactTargetServices.Attribute {
				Name = "LAST_NAME",
				Value = lastname
			};

			subscriber.Attributes[2] = new ExactTargetServices.Attribute {
				Name = "CELL_CODE",
				Value = cellcode
			};

			subscriber.Attributes[3] = new ExactTargetServices.Attribute {
				Name = "CAMPAIGN_CD",
				Value = campaignCd
			};

			subscriber.Attributes[4] = new ExactTargetServices.Attribute {
				Name = "INDIVIDUAL_ID",
				Value = individualId
			};

			subscriber.Attributes[5] = new ExactTargetServices.Attribute {
				Name = "DATE_TIME_ACTION",
				Value = datetimeAction
			};

			subscriber.Attributes[6] = new ExactTargetServices.Attribute {
				Name = "SEA",
				Value = sea
			};

			subscriber.Attributes[7] = new ExactTargetServices.Attribute {
				Name = "MBR_STATUS",
				Value = string.Empty
			};

			subscriber.Attributes[8] = new ExactTargetServices.Attribute {
				Name = "STATE",
				Value = state
			};

			subscriber.Attributes[9] = new ExactTargetServices.Attribute {
				Name = "MSG_TOPIC",
				Value = msgTopic
			};

			subscriber.Attributes[10] = new ExactTargetServices.Attribute {
				Name = "MBR_EMAIL",
				Value = mbrEmail
			};

			subscriber.Attributes[11] = new ExactTargetServices.Attribute {
				Name = "MESSAGE",
				Value = message
			};

			//createa a client object and set the client ID
			var ts = new TriggeredSend {
				TriggeredSendDefinition = new TriggeredSendDefinition()
			};
			ts.TriggeredSendDefinition.CustomerKey = customerDefinition;
			ts.TriggeredSendDefinition.Priority = "High";//This is an additional feature
			ts.Client = new ClientID {
				ID = int.Parse(ScSettings.GetSetting("ExacttargetClientIdSeiumb")),
				IDSpecified = true
			};

			var co = new CreateOptions {
				RequestType = RequestType.Synchronous,
				RequestTypeSpecified = true
			};

			var saveOption = new SaveOption {
				SaveAction = SaveAction.UpdateAdd,
				PropertyName = "*"
			};
			co.SaveOptions = new[] { saveOption };
			ts.Subscribers = new[] { subscriber };
			var tsStatus = string.Empty;
			try {
				//Create the request to add the subscriber
				soapClient.Create(co, new APIObject[] { ts }, out _, out tsStatus);
				//Check overallstatus received from the call
				if (!tsStatus.Equals("OK")) {
					Log.Error("Error SendExactTarget ContactUs Overallstatus no OK", nameof(ExactTargetServiceManager));
				}
				soapClient.Close();
			} catch (Exception exCreate) {
				var msg = "Error SendExactTarget ContactUs: " + exCreate.Message;
				Log.Error(msg, nameof(ExactTargetServiceManager));
			}

			return tsStatus.Equals("OK");
		}

		/// <summary>
		/// Sends email related to forget password process
		/// </summary>
		/// <param name="firstname"></param>
		/// <param name="resetUrl"></param>
		/// <param name="cancelUrl"></param>
		/// <param name="cellcode"></param>
		/// <param name="campaignCd"></param>
		/// <param name="mbrEmail"></param>
		/// <param name="customerDefinition"></param>
		public static void SendExactTargetForgetPassword(string firstname, string resetUrl, string cancelUrl, string cellcode, string campaignCd, string mbrEmail, string customerDefinition) {
			// Create the binding
			var binding = new BasicHttpBinding {
				Name = UsernameSoapBinding
			};
			binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
			binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
			binding.ReceiveTimeout = new TimeSpan(0, 0, _configuredTimeout);
			binding.OpenTimeout = new TimeSpan(0, 0, _configuredTimeout);
			binding.CloseTimeout = new TimeSpan(0, 0, _configuredTimeout);
			binding.SendTimeout = new TimeSpan(0, 0, _configuredTimeout);
			// Set the transport security to UsernameOverTransport for Plaintext usernames
			var endpoint = new EndpointAddress(ScSettings.GetSetting("ExacttargetEndPoint"));
			// Create the SOAP Client (and pass in the endpoint and the binding)
			var soapClient = new SoapClient(binding, endpoint);
			// Set the username and password
			if (soapClient.ClientCredentials?.UserName != null) {
				soapClient.ClientCredentials.UserName.UserName = ScSettings.GetSetting("ExacttargetUsername");
				soapClient.ClientCredentials.UserName.Password = ScSettings.GetSetting("ExacttargetPasswordSeiumb");
			}

			var subscriber = new Subscriber {
				SubscriberKey = mbrEmail,
				EmailAddress = mbrEmail,

				//create a subscriber and fill with the same fields of dynament
				Attributes = new ExactTargetServices.Attribute[5]
			};

			subscriber.Attributes[0] = new ExactTargetServices.Attribute {
				Name = "FIRST_NAME",
				Value = firstname
			};

			subscriber.Attributes[1] = new ExactTargetServices.Attribute {
				Name = "RESET_URL",
				Value = resetUrl
			};

			subscriber.Attributes[2] = new ExactTargetServices.Attribute {
				Name = "CANCEL_URL",
				Value = cancelUrl
			};

			subscriber.Attributes[3] = new ExactTargetServices.Attribute {
				Name = "CELL_CODE",
				Value = cellcode
			};

			subscriber.Attributes[4] = new ExactTargetServices.Attribute {
				Name = "CAMPAIGN_CD",
				Value = campaignCd
			};

			var ts = new TriggeredSend {
				TriggeredSendDefinition = new TriggeredSendDefinition()
			};
			ts.TriggeredSendDefinition.CustomerKey = customerDefinition;
			ts.TriggeredSendDefinition.Priority = "High";//This is an additional feature
			ts.Client = new ClientID {
				ID = int.Parse(ScSettings.GetSetting("ExacttargetClientIdSeiumb")),
				IDSpecified = true
			};

			var co = new CreateOptions {
				RequestType = RequestType.Synchronous,
				RequestTypeSpecified = true
			};

			var saveOption = new SaveOption {
				SaveAction = SaveAction.UpdateAdd,
				PropertyName = "*"
			};
			co.SaveOptions = new[] { saveOption };
			ts.Subscribers = new[] { subscriber };
			try {
				//Create the request to add the subscriber
				soapClient.Create(co, new APIObject[] { ts }, out _, out var tsStatus);
				//Check overallstatus received from the call
				if (!tsStatus.Equals("OK")) {
					Log.Error("Error SendExactTarget Forget Password Overallstatus no OK", nameof(ExactTargetServiceManager));
				}
				soapClient.Close();
			} catch (Exception exCreate) {
				var msg = "Error SendExactTarget Forget Password: " + exCreate.Message;
				Log.Error(msg, nameof(ExactTargetServiceManager));
			}
		}

		/// <summary>
		/// Sends email related change username process
		/// </summary>
		/// <param name="mdsid"></param>
		/// <param name="newUsername"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="cellCode"></param>
		/// <param name="oldUsername"></param>
		/// <param name="msrName"></param>
		/// <param name="campaignCode"></param>
		/// <param name="customerKey"></param>
		/// <param name="isNew"></param>
		/// <returns></returns>
		public static string SendExactTargetChangeUsername(
			string mdsid, 
			string newUsername, 
			string firstName, 
			string lastName,
			string cellCode, 
			string oldUsername, 
			string msrName, 
			string campaignCode, 
			string customerKey, 
			bool isNew
		) {
			// Create the binding
			var binding = new BasicHttpBinding {
				Name = UsernameSoapBinding,
				Security = {
					Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
					Message = {
						ClientCredentialType = BasicHttpMessageCredentialType.UserName
					}
				},
				ReceiveTimeout = new TimeSpan(0, 0, _configuredTimeout),
				OpenTimeout = new TimeSpan(0, 0, _configuredTimeout),
				CloseTimeout = new TimeSpan(0, 0, _configuredTimeout),
				SendTimeout = new TimeSpan(0, 0, _configuredTimeout),
			};
			// Set the transport security to UsernameOverTransport for Plaintext usernames
			var endpoint = new EndpointAddress(ScSettings.GetSetting("ExacttargetEndPoint"));
			// Create the SOAP Client (and pass in the endpoint and the binding)
			var soapClient = new SoapClient(binding, endpoint);
			// Set the username and password
			if (soapClient.ClientCredentials?.UserName != null) {
				soapClient.ClientCredentials.UserName.UserName = ScSettings.GetSetting("ExacttargetUsername");
				soapClient.ClientCredentials.UserName.Password = ScSettings.GetSetting("ExacttargetPasswordSeiumb");
			}

			var subscriber = new Subscriber {
				SubscriberKey = mdsid,
				EmailAddress = isNew ? newUsername : oldUsername
			};

			var targetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
			var targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);

			//create a subscriber and fill with the same fields of dynament
			subscriber.Attributes = new [] {
				new ExactTargetServices.Attribute {
					Name = "FIRST_NAME",
					Value = firstName
				},
				new ExactTargetServices.Attribute {
					Name = "LAST_NAME",
					Value = lastName
				},
				new ExactTargetServices.Attribute {
					Name = "CELL_CODE",
					Value = !string.IsNullOrEmpty(cellCode) ? cellCode : "TG001443"
				},
				new ExactTargetServices.Attribute {
					Name = "CAMPAIGN_CD",
					Value = campaignCode
				},
				new ExactTargetServices.Attribute {
					Name = "INDIVIDUAL_ID",
					Value = mdsid
				},
				new ExactTargetServices.Attribute {
					Name = "DATE_TIME_ACTION",
					Value = $"{targetDateTime:MM/dd/yyyy} at {targetDateTime:h:mm tt} (CST)"
				},
				new ExactTargetServices.Attribute {
					Name = "NEW_USERNAME",
					Value = newUsername
				},
				new ExactTargetServices.Attribute {
					Name = "OLD_USERNAME",
					Value = oldUsername
				},
				new ExactTargetServices.Attribute {
					Name = "MSR_NAME",
					Value = string.Empty //didn't find in dynaments
				}
			};


			var ts = new TriggeredSend {
				TriggeredSendDefinition = new TriggeredSendDefinition()
			};
			ts.TriggeredSendDefinition.CustomerKey = customerKey;
			ts.TriggeredSendDefinition.Priority = "High";

			//createa a client object and set the client ID
			ts.Client = new ClientID {
				ID = int.Parse(ScSettings.GetSetting("ExacttargetClientIdSeiumb")),
				IDSpecified = true
			};

			var co = new CreateOptions {
				RequestType = RequestType.Synchronous,
				RequestTypeSpecified = true
			};

			var saveOption = new SaveOption {
				SaveAction = SaveAction.UpdateAdd,
				PropertyName = "*"
			};
			co.SaveOptions = new [] { saveOption };
			ts.Subscribers = new [] { subscriber };

			var tsStatus = string.Empty;
			try {
				//Create the request to add the subscriber
				soapClient.Create(co, new APIObject[] { ts }, out _, out tsStatus);
				soapClient.Close();
			} catch (Exception exCreate) {
				var msg = "Error SendExactTarget ChangeUsername: " + exCreate.Message;
				Log.Error(msg, nameof(ExactTargetServiceManager));
			}

			return tsStatus;
		}

		/// <summary>
		/// Sends email related to duplicate registration
		/// </summary>
		/// <param name="mdsId"></param>
		/// <param name="selectedUsername"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="cellCode"></param>
		/// <param name="campaignCd"></param>
		/// <param name="removedUserNames"></param>
		/// <param name="customerKey"></param>
		/// <returns></returns>
		public static string SendExactTargetDuplicateRegistration(
			string mdsId, 
			string selectedUsername, 
			string firstName, 
			string lastName,
			string cellCode, 
			string campaignCd, 
			string removedUserNames, 
			string customerKey
		) {
			// Create the binding
			var binding = new BasicHttpBinding {
				Name = UsernameSoapBinding,
				Security = {
					Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
					Message = {
						ClientCredentialType = BasicHttpMessageCredentialType.UserName
					}
				},
				ReceiveTimeout = new TimeSpan(0, 0, _configuredTimeout),
				OpenTimeout = new TimeSpan(0, 0, _configuredTimeout),
				CloseTimeout = new TimeSpan(0, 0, _configuredTimeout),
				SendTimeout = new TimeSpan(0, 0, _configuredTimeout)
			};
			// Set the transport security to UsernameOverTransport for Plaintext usernames
			var endpoint = new EndpointAddress(ScSettings.GetSetting("ExacttargetEndPoint"));
			// Create the SOAP Client (and pass in the endpoint and the binding)
			var soapClient = new SoapClient(binding, endpoint);
			// Set the username and password
			if (soapClient.ClientCredentials?.UserName != null) {
				soapClient.ClientCredentials.UserName.UserName = ScSettings.GetSetting("ExacttargetUsername");
				soapClient.ClientCredentials.UserName.Password = ScSettings.GetSetting("ExacttargetPasswordSeiumb");
			}

			var subscriber = new Subscriber {
				SubscriberKey = mdsId,
				EmailAddress = selectedUsername,

				//create a subscriber and fill with the same fields of dynament
				Attributes = new ExactTargetServices.Attribute[9]
			};

			subscriber.Attributes[0] = new ExactTargetServices.Attribute {
				Name = "FIRST_NAME",
				Value = firstName
			};

			subscriber.Attributes[1] = new ExactTargetServices.Attribute {
				Name = "LAST_NAME",
				Value = lastName
			};

			subscriber.Attributes[2] = new ExactTargetServices.Attribute {
				Name = "CELL_CODE",
				Value = !string.IsNullOrEmpty(cellCode) ? cellCode : "TG001445"
			};

			subscriber.Attributes[3] = new ExactTargetServices.Attribute {
				Name = "CAMPAIGN_CD",
				Value = !string.IsNullOrEmpty(campaignCd) ? campaignCd : "TGS01131"
			};

			subscriber.Attributes[4] = new ExactTargetServices.Attribute {
				Name = "INDIVIDUAL_ID",
				Value = mdsId
			};

			var targetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
			var newDt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);

			subscriber.Attributes[5] = new ExactTargetServices.Attribute {
				Name = "DATE_TIME_ACTION",
				Value = $"{newDt:MM/dd/yyyy} at {newDt:h:mm tt} (CST)"
			};

			subscriber.Attributes[6] = new ExactTargetServices.Attribute {
				Name = "REMOVED_USERNAMES",
				Value = removedUserNames
			};

			var ts = new TriggeredSend {
				TriggeredSendDefinition = new TriggeredSendDefinition {
					CustomerKey = customerKey,
					Priority = "High"
				},

				//create a client object and set the client ID
				Client = new ClientID {
					ID = int.Parse(ScSettings.GetSetting("ExacttargetClientIdSeiumb")),
					IDSpecified = true
				}
			};

			var co = new CreateOptions {
				RequestType = RequestType.Synchronous,
				RequestTypeSpecified = true
			};

			var saveOption = new SaveOption {
				SaveAction = SaveAction.UpdateAdd,
				PropertyName = "*"
			};
			co.SaveOptions = new[] { saveOption };
			ts.Subscribers = new[] { subscriber };

			var tsStatus = string.Empty;
			try {
				//Create the request to add the subscriber
				soapClient.Create(co, new APIObject[] { ts }, out _, out tsStatus);
				soapClient.Close();
			} catch (Exception exCreate) {
				var msg = "Error SendExactTarget DuplicateRegistration: " + exCreate.Message;
				Log.Error(msg, nameof(ExactTargetServiceManager));
			}

			return tsStatus;
		}

		public static string SendExactTargetRegistration(
			string loginUserId, 
			string mdsInvId, 
			string firstName, 
			string lastName,
			string cellCode, 
			string campaignCd, 
			string emailOptOut, 
			string customerKey
		) {
			// Create the binding
			var binding = new BasicHttpBinding {
				Name = UsernameSoapBinding,
				Security = {
					Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
					Message = {ClientCredentialType = BasicHttpMessageCredentialType.UserName}
				},
				ReceiveTimeout = new TimeSpan(0, 0, _configuredTimeout),
				OpenTimeout = new TimeSpan(0, 0, _configuredTimeout),
				CloseTimeout = new TimeSpan(0, 0, _configuredTimeout),
				SendTimeout = new TimeSpan(0, 0, _configuredTimeout)
			};
			// Set the transport security to UsernameOverTransport for Plaintext usernames
			var endpoint = new EndpointAddress(Sitecore.Configuration.Settings.GetSetting("ExacttargetEndPoint"));
			// Create the SOAP Client (and pass in the endpoint and the binding)
			var soapClient = new SoapClient(binding, endpoint) {
				ClientCredentials = {
					UserName = {
						UserName = ScSettings.GetSetting("ExacttargetUsername"),
						Password = ScSettings.GetSetting("ExacttargetPasswordSeiumb")
					}
				}
			};

			var subscriber = new Subscriber {
				SubscriberKey = loginUserId,
				EmailAddress = loginUserId,

				//create a subscriber and fill with the same fields of dynament
				Attributes = new ExactTargetServices.Attribute[8]
			};

			subscriber.Attributes[0] = new ExactTargetServices.Attribute {
				Name = "INDIVIDUAL_ID",
				Value = mdsInvId
			};

			subscriber.Attributes[1] = new ExactTargetServices.Attribute {
				Name = "FIRST_NAME",
				Value = firstName
			};

			subscriber.Attributes[2] = new ExactTargetServices.Attribute {
				Name = "LAST_NAME",
				Value = lastName
			};

			subscriber.Attributes[3] = new ExactTargetServices.Attribute {
				Name = "COMP_LIFE",
				Value = "0"
			};

			subscriber.Attributes[4] = new ExactTargetServices.Attribute {
				Name = "INTRO_LIFE",
				Value = "0"
			};

			subscriber.Attributes[5] = new ExactTargetServices.Attribute {
				Name = "EMAIL_OPT_OUT",
				Value = emailOptOut
			};

			subscriber.Attributes[6] = new ExactTargetServices.Attribute {
				Name = "CELL_CODE",
				Value = cellCode
			};

			subscriber.Attributes[7] = new ExactTargetServices.Attribute {
				Name = "CAMPAIGN_CD",
				Value = campaignCd
			};

			var ts = new TriggeredSend {
				TriggeredSendDefinition = new TriggeredSendDefinition {
					CustomerKey = customerKey,
					Priority = "High"
				},

				//create a client object and set the client ID
				Client = new ClientID {
					ID = int.Parse(ScSettings.GetSetting("ExacttargetClientIdSeiumb")),
					IDSpecified = true
				}
			};

			var co = new CreateOptions {
				RequestType = RequestType.Synchronous,
				RequestTypeSpecified = true
			};

			var saveOption = new SaveOption {
				SaveAction = SaveAction.UpdateAdd,
				PropertyName = "*"
			};
			co.SaveOptions = new [] { saveOption };
			ts.Subscribers = new [] { subscriber };

			var tsStatus = string.Empty;
			try {
				//Create the request to add the subscriber
				soapClient.Create(co, new APIObject[] { ts }, out _, out tsStatus);
				soapClient.Close();
			} catch (Exception exCreate) {
				Log.Error("Error SendExactTarget Registration", exCreate, nameof(ExactTargetServiceManager));
			}

			return tsStatus;
		}
		/// <summary>
		/// Unsubscribe an email from a email list
		/// </summary>
		/// <param name="subscriberKey">mdsid</param>
		/// <param name="listId">mail list</param>
		/// <returns>true when successful</returns>
		public static bool UnsubscribeListMail(string subscriberKey, int listId) {
			var ret = false;
			try {
				UnsubscribeListMailBase(subscriberKey, listId, out _, out var overallStatus);
				ret = overallStatus.Equals("OK");
			} catch (Exception ex) {
				Log.Error("UnsubscribeListMail", ex, typeof(ExactTargetServiceManager));
			}
			return ret;
		}
		public static UpdateResult[] UnsubscribeListMailBase(string subscriberkey, int listid, out string requestId, out string overallStatus) {
			//TODO : initialize once and then use from memory
			var endpoint = Sitecore.Configuration.Settings.GetSetting("ExacttargetEndPoint");
			var username = Sitecore.Configuration.Settings.GetSetting("ExacttargetUsername");
			var password = Sitecore.Configuration.Settings.GetSetting("ExacttargetPasswordSeiumb");
			var clientId = int.Parse(Sitecore.Configuration.Settings.GetSetting("ExacttargetClientIdSeiumb"));
			UpdateResult[] updateWsResponse;
			requestId = string.Empty;
			overallStatus = string.Empty;

			var soapClient = ConfiguredSoapClient(endpoint, username, password);

			var subscriber = new Subscriber {
				SubscriberKey = subscriberkey,
				Client = new ClientID {
					ID = clientId,
					IDSpecified = true
				},
				Lists = new[]
				{
					new SubscriberList
					{
						ID = listid,
						IDSpecified = true,
						Status = SubscriberStatus.Unsubscribed,
						Action = "update",
						StatusSpecified = true
					}
				}
			};

			var updateOptions = new UpdateOptions {
				RequestType = RequestType.Synchronous,
				RequestTypeSpecified = true
			};

			try {
				//Create the request to add the subscriber
				updateWsResponse = soapClient.Update(updateOptions, new APIObject[] { subscriber }, out requestId,
					out overallStatus);
			} finally {
				if (soapClient.State != CommunicationState.Closed) {
					soapClient.Close();
				}
			}

			return updateWsResponse;
		}
		#endregion

		#region Private Methods
		private static BasicHttpBinding ServiceBinding() {
			//TODO : transform this in global use
			return new BasicHttpBinding {
				Name = UsernameSoapBinding,
				Security =
				{
					Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
					Message = {ClientCredentialType = BasicHttpMessageCredentialType.UserName}
				},
				ReceiveTimeout = new TimeSpan(0, 0, _configuredTimeout),
				OpenTimeout = new TimeSpan(0, 0, _configuredTimeout),
				CloseTimeout = new TimeSpan(0, 0, _configuredTimeout),
				SendTimeout = new TimeSpan(0, 0, _configuredTimeout)
			};
		}
		private static SoapClient ConfiguredSoapClient(string endpoint, string username, string password) {
			Assert.ArgumentNotNullOrEmpty(endpoint, "endpoint");
			Assert.ArgumentNotNullOrEmpty(username, "username");
			Assert.ArgumentNotNullOrEmpty(password, "password");

			// Create the binding
			var binding = ServiceBinding();
			// Set the transport security to UsernameOverTransport for Plaintext usernames
			var endpointAddress = new EndpointAddress(endpoint);
			// Create the SOAP Client (and pass in the endpoint and the binding)
			var soapClient = new SoapClient(binding, endpointAddress);
			// Set the username and password
			if (soapClient.ClientCredentials?.UserName != null) {
				soapClient.ClientCredentials.UserName.UserName = username;
				soapClient.ClientCredentials.UserName.Password = password;
			}
			return soapClient;
		}
		#endregion
	}
}