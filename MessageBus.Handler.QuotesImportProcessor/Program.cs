using System;
using System.Configuration;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MessageBus.Handler.QuotesImportProcessor;
using Topshelf;

namespace SubscriptionReceiver
{
    public class Program
    {
        static async Task Main()
        {
            var consumer = HostFactory.Run( x=>
            {
                var connectionString = "Endpoint=sb://servicebus-quotes-import.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JExvllXk3uhBYuPLf77wAJg63P8N2vv6B5kNwGbyaP8=";// ConfigurationManager.AppSettings["Azure.ServiceBus.ConnectionString"];
                var topicName = "";
                var subscriptionName = "";
                x.Service<QuotesImportService>( s => {
                    s.ConstructUsing(n => new QuotesImportService());
                    s.WhenStarted(n => n.Start());
                    s.WhenStopped(n => n.Stop());
                });

                var client = new ServiceBusClient(connectionString);

                // create a processor that we can use to process the messages
                var processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

                try
                {
                    // add handler to process messages
                    processor.ProcessMessageAsync += QuotesImportedEventHandler;

                    // add handler to process any errors
                    processor.ProcessErrorAsync += ErrorHandler;

                    // start processing 
                    await processor.StartProcessingAsync();

                    Console.WriteLine("Wait for a minute and then press any key to end the processing");
                    Console.ReadKey();

                    // stop processing 
                    Console.WriteLine("\nStopping the receiver...");
                    await processor.StopProcessingAsync();
                    Console.WriteLine("Stopped receiving messages");
                }
                finally
                {
                    // Calling DisposeAsync on client types is required to ensure that network
                    // resources and other unmanaged objects are properly cleaned up.
                    await processor.DisposeAsync();
                    await client.DisposeAsync();
                }
                x.SetServiceName("ServiceBus-Quotes-Import");
                x.SetDisplayName("ServiceBus-Quotes-Import");
                x.SetDescription("ServiceBus-Quotes-Import");

            });

            var existCode = (int)Convert.ChangeType(consumer, consumer.GetTypeCode());
            Environment.ExitCode = existCode;
        }
    }
}