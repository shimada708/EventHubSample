using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using EventHubSample.Web.Subscribers;

namespace EventHubSample.Web.Hubs
{
    public class HandStateHub : Hub
    {
        private readonly HandState _handState;

        public HandStateHub() : this(HandState.Instance) { }

        public HandStateHub(HandState handState)
        {
            _handState = handState;
        }
    }
}