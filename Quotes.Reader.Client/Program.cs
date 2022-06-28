using Autofac;
using Microsoft.Extensions.Logging;
using Quotes.Import.Client.Ioc;
using Quotes.Reader.Client;

public class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Started Quotes Group Reader process");
        var Container = QuotesDependenciesResolver.RegisterDependencies();
        var logger = Container.Resolve<ILogger<Program>>();
        var quotesGroupReaderService = Container.Resolve<IQuotesGroupReaderService>();

        if (args == null || args.Length < 4)
        {
            logger.LogError("Quotes Group Reader requires storageDirectory,output directory, date and group");
            Console.WriteLine("please pass the required input in the following format: storageDirectory outputdirectory date(MM/dd/YYYY) group");
        }


        if (!DateTime.TryParse(args[2], out DateTime expectedDate))
        {
            logger.LogError("Quotes Group Reader requires date in MM/dd/YYYY format");
            Console.WriteLine("please provide date in MM/dd/YYYY format");

        }


        try
        {
            quotesGroupReaderService.ReadGroupQuotes(args[0], args[1], expectedDate, args[3]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on processing the Quotes Group Reader");
        }

        Console.WriteLine("Completed Quotes Group Reader process");
    }
}