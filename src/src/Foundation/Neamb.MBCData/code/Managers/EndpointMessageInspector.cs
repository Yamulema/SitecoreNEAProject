using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    /// <summary>
    /// Used for SOAP calls debugging.
    /// </summary>
    public class EndpointMessageInspector : IClientMessageInspector
    {
		public object BeforeSendRequest(ref Message request, IClientChannel channel)
	    {
		    var envelope = request.ToString();
			Log.Debug("BeforeSendRequest start",this);
		    Log.Debug(envelope,this);
		    Log.Debug("BeforeSendRequest end", this);
		    return Guid.NewGuid();
	    }

	    public void AfterReceiveReply(ref Message reply, object correlationState)
	    {
		    var envelope = reply.ToString();
			Log.Debug("AfterReceiveReply start", this);
		    Log.Debug(envelope,this);
		    Log.Debug("AfterReceiveReply end", this);
		}
	}
}