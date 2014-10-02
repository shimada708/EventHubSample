using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubSample.Shared.Contracts
{
    public class HandStateEvent
    {
        public string HandState { get; set; }
        public string MachineName { get; set; }
    }
}
