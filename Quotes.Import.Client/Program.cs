using Autofac;
using Microsoft.Extensions.Logging;
using Quotes.Import.Client.Ioc;
using Quotes.Import.Client.Models;
namespace Quotes.Import.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Start the App.
            Console.WriteLine("Started Quotes importer process");

            // Resolve dependencies
            var container = QuotesDependenciesResolver.RegisterDependencies().Build();
            var logger = container.Resolve<ILogger<Program>>();
            var quotesImportService = container.Resolve<IQuotesImportService>();

            /// Check if the argument length is lesser than 2 then return error
            if (args == null || args.Length < 2)
            {
                logger.LogError("Quotes importer has been started without input directory and/or storage directory");
                return;
            }

            try
            {
                // Process Quotes
                await quotesImportService.ProcessAsync(new QuotesImportRequest(args[0], args[1]));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error on processing the Quotes importer");
            }

            // Complete the app
            Console.WriteLine("Completed Quotes importer process");
        }
    }
}