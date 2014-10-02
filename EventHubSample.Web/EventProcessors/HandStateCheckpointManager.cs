using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventHubSample.Web.EventProcessors
{
    class HandStateCheckpointManager: ICheckpointManager
    {
        public System.Threading.Tasks.Task CheckpointAsync(Lease lease, string offset)
        {
            throw new NotImplementedException();
        }
    }
}
