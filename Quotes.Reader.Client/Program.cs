using Autofac;
using Microsoft.Extensions.Logging;
using Quotes.Reader.Client.Ioc;
using Quotes.Reader.Client.Models;
using Quotes.Reader.Client.Services;

public class Program
{ 
    static void Main(string[] args)
    {
        Console.WriteLine("Started Quotes Group Reader process");
        var Container = QuotesDependenciesResolver.RegisterDependencies().Build();
        var logger = Container.Resolve<ILogger<Program>>();
        var quotesGroupReaderService = Container.Resolve<IQuotesGroupReaderService>();

        // Return if number of argument is lesser than 4
        if (args == null || args.Length < 4)
        {
            logger.LogError("Quotes Group Reader requires storageDirectory,output directory, date and group");
            Console.WriteLine("please pass the required input in the following format: storageDirectory outputdirectory date(MM/dd/YYYY) group");
            return;
        }
        try
        {
            var request = new QuotesGroupReaderRequest(args[0], args[1], args[2], args[3]);
            // Read group quotes for the given group name.
            quotesGroupReaderService.ProcessAsync(request).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on processing the Quotes Group Reader");
            return;
        }

        Console.WriteLine("Completed Quotes Group Reader process");
    }
}