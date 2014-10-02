using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHubSample.Web.EventProcessors
{
    class HandStateEventProcessor :IEventProcessor
    {
        public System.Threading.Tasks.Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task OpenAsync(PartitionContext context)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            throw new NotImplementedException();
        }
    }
}
