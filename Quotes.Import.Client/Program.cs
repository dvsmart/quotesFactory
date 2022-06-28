using Autofac;
using Microsoft.Extensions.Logging;
using Quotes.Import.Client.Ioc;

public class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Started Quotes importer process");
        var Container = QuotesDependenciesResolver.RegisterDependencies();
        var logger = Container.Resolve<ILogger<Program>>();
        var quotesImportManager = Container.Resolve<IQuotesImportService>();

        if (args == null || args.Length < 2)
        {
            logger.LogError("Quotes importer has been started without input directory and/or storage directory");
        }

        try
        {
            quotesImportManager.ProcessQuotes(args[0], args[1]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on processing the Quotes importer");
        }

        Console.WriteLine("Completed Quotes importer process");
    }
}