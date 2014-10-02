using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace EventHubSample.Web.Hubs
{
    public class HandStateHub : Hub
    {
        public void Send()
        {
            Clients.All.broadcastState("HandType", "HandState");
        }
    }
}