using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubSample.Shared.Contracts
{
    public class LogMessageEvent
    {
        public string MachineName { get; set; }
        public string SiteName { get; set; }
        public string InstanceId { get; set; }
        public string Value { get; set; }
    }
}
