using EventHubSample.Shared.Contracts;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHubSample.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            for (int i = 0; i <= 15; i++)
            {
                Task.Factory.StartNew((state) =>
                {
                    System.Console.WriteLine("Starting worker to process partition: {0}", state);

                    var factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["ServiceBus.Namespace"], ""), new MessagingFactorySettings()
                    {
                        TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ConfigurationManager.AppSettings["ServiceBus.KeyName"], ConfigurationManager.AppSettings["ServiceBus.Key"]),
                        TransportType = TransportType.Amqp
                    });

                    var client = factory.CreateEventHubClient("logs");
                    var group = client.GetDefaultConsumerGroup();

                    System.Console.WriteLine("Group: {0}", group.GroupName);

                    var receiver = group.CreateReceiver(state.ToString(), DateTime.UtcNow);


                    while (true)
                    {
                        if (cts.IsCancellationRequested)
                        {
                            receiver.Close();
                            break;
                        }

                        // Receive could fail, I would need a retry policy etc...
                        var messages = receiver.Receive(10);
                        foreach (var message in messages)
                        {
                            var logMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<LogMessageEvent>(Encoding.Default.GetString(message.GetBytes()));

                            System.Console.WriteLine("{0} [{6}] {2}/{3}: {5}", DateTime.Now, message.PartitionKey, logMessage.MachineName, logMessage.SiteName, logMessage.InstanceId, logMessage.Value, state);
                            System.Console.WriteLine(" > Instance/PartitionKey: {0}", message.PartitionKey);


                        }
                    }
                }, i);
            }

            System.Console.ReadLine();
            cts.Cancel();
        }
    }
}
