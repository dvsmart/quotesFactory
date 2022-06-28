using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quotes.Reader.Client;

namespace Quotes.Import.Client.Ioc
{
    public class QuotesDependenciesResolver
    {
        public static IContainer RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder()
           .AddJsonFile("appSettings.json");
            var Configuration = configurationBuilder.Build();

            // The Microsoft.Extensions.Logging package provides this one-liner
            // to add logging services.
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<IConfiguration>(Configuration);
            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);
            builder.RegisterType<QuotesGroupReaderService>()
           .As<IQuotesGroupReaderService>();

            return builder.Build();
        }
    }
}
