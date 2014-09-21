﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.Text;
using EventHubSample.Shared.Contracts;

namespace EventHubSample.Listeners
{
    public class EventHubTraceListener : TraceListener
    {
        private readonly EventHubClient _client;
        private readonly string _siteName;
        private readonly string _instanceId;

        public EventHubTraceListener()
        {
            var settings = new MessagingFactorySettings()
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                    ConfigurationManager.AppSettings["ServiceBus.KeyName"], ConfigurationManager.AppSettings["ServiceBus.Key"]),
                TransportType = TransportType.Amqp
            };

            var factory = MessagingFactory.Create(
                    ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["ServiceBus.Namespace"], "")
                    , settings);

            _client = factory.CreateEventHubClient("logs");

            // event information
            _instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") ?? DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            _siteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") ?? "EventHubSample";
        }

        public override void Write(string message)
        {
            var eventData = new EventData(Encoding.Default.GetBytes(JsonConvert.SerializeObject(new LogMessageEvent()
            {
                InstanceId = _instanceId,
                MachineName = Environment.MachineName,
                SiteName = _siteName,
                Value = message
            })));

            eventData.PartitionKey = _instanceId;

            _client.Send(eventData);
        }

        public override void WriteLine(string message)
        {
            Write(message);
        }
    }
}