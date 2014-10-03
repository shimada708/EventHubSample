using EventHubSample.Shared.Contracts;
using EventHubSample.Web.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubSample.Web.EventProcessors
{
    class HandStateEventProcessor :IEventProcessor
    {
        private IHubConnectionContext<dynamic> Clients { get; set; }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            return Task.FromResult<object>(null);
        }

        public Task OpenAsync(PartitionContext context)
        {
            Clients = GlobalHost.ConnectionManager.GetHubContext<HandStateHub>().Clients;
            return Task.FromResult<object>(null);
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var message in messages)
            {
                var tempData = Newtonsoft.Json.JsonConvert.DeserializeObject<HandStateEvent>(Encoding.Default.GetString(message.GetBytes()));
                Clients.All.broadcastState(tempData.MachineName, tempData.HandState);
            }

            return Task.FromResult<object>(null);
        }
    }
}
