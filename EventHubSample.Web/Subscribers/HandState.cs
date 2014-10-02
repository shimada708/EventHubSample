using EventHubSample.Web.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System.Configuration;
using EventHubSample.Web.EventProcessors;


namespace EventHubSample.Web.Subscribers
{
    public class HandState
    {
        private readonly static Lazy<HandState> _instance = new Lazy<HandState>(() => {
            return new HandState(GlobalHost.ConnectionManager.GetHubContext<HandStateHub>().Clients);
        });
        
        private IHubConnectionContext<dynamic> Clients { get; set; }

        private EventHubClient eventHubClient { get; set; }
        
        private HandState(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            eventHubClient = EventHubClient.CreateFromConnectionString(GetAmqpConnectionString(), ConfigurationManager.AppSettings["ServiceBus.Path"]);
            var defaultConsumerGroup = eventHubClient.GetDefaultConsumerGroup();
            EventHubDescription eventHub = NamespaceManager.CreateFromConnectionString(GetAmqpConnectionString()).GetEventHub(ConfigurationManager.AppSettings["ServiceBus.Path"]);
            foreach (var partitionId in eventHub.PartitionIds)
            {
                defaultConsumerGroup.RegisterProcessor<HandStateEventProcessor>(new Lease() { PartitionId = partitionId }, new HandStateCheckpointManager());
            }
        }

        static string GetAmqpConnectionString() 
        { 
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"]; 
            if (string.IsNullOrEmpty(connectionString)) 
            { 
                Console.WriteLine("Did not find Service Bus connections string in appsettings (app.config)"); 
                return string.Empty; 
            } 
 
            ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(connectionString); 
            builder.TransportType = TransportType.Amqp; 
            return builder.ToString(); 
        } 

        public static HandState Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}