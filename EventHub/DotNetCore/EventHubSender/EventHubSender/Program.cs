using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Azure.Identity;


namespace EventHubSender
{
    class Program
    {
        // number of events to be sent to the event hub
        private const int numOfEvents = 100;

        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        static EventHubProducerClient producerClient;
        
        static async Task Main()
        {
            await ClientCredentialsScenarioAsync();


            /*
            // Create a producer client that you can use to send events to an event hub
            producerClient = new EventHubProducerClient(connectionString, eventHubName);

            // Create a batch of events 
            EventDataBatch eventBatch;

            for (int i = 1; i <= numOfEvents; i++)
            {
                eventBatch = await producerClient.CreateBatchAsync();
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}")));

                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"A Message {i} has been sent");

            }

            */
        }

        //输入TenantId
        static readonly string TenantId = "";
        //输入ClientID
        static readonly string ClientId = "";
        //输入App Key
        static readonly string ClientSecret = "";
        //Event Hub Namespace
        static readonly string EventHubNamespace = "leieventhub01.servicebus.chinacloudapi.cn";
        //Event Hub Name
        static readonly string EventHubName = "event01";
        static async Task ClientCredentialsScenarioAsync()
        {
            var options = new ClientCertificateCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzureChina };
            TokenCredential credential = new ClientSecretCredential(TenantId, ClientId, ClientSecret, options);


            // Create a producer client that you can use to send events to an event hub
            producerClient = new EventHubProducerClient(EventHubNamespace, EventHubName, credential);

            // Create a batch of events 
            EventDataBatch eventBatch;

            for (int i = 1; i <= numOfEvents; i++)
            {
                eventBatch = await producerClient.CreateBatchAsync();
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}")));

                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"A Message {i} has been sent");

            }
        }

    }
}
